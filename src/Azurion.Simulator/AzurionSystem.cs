namespace Azurion.Simulator;

/// <summary>
/// The simulated positioner — the "system under test". Holds every axis, accepts
/// movement commands, and advances the axes toward their targets on each
/// <see cref="Tick"/>. All public members are thread-safe: the hosted movement loop
/// ticks on one thread while API requests command it from others.
///
/// A command (e.g. <see cref="MoveTo"/>) returns immediately — it only sets a target.
/// The motion itself plays out over subsequent ticks, which is what makes a test that
/// asserts position right after commanding it inherently flaky.
/// </summary>
public sealed class AzurionSystem
{
    private readonly object _gate = new();
    private readonly Dictionary<AxisId, Axis> _axes;
    private readonly List<SystemFault> _faults = new();
    private FlakinessProfile _flakiness = FlakinessProfile.Off;
    private Random _rng = new(FlakinessProfile.Off.Seed);
    private bool _emergencyStopped;

    public AzurionSystem()
    {
        _axes = Axis.CreateDefaultSet().ToDictionary(a => a.Id);
    }

    // ---- Commands -------------------------------------------------------------

    /// <summary>Command an axis to an absolute position. Returns at once; motion follows.</summary>
    public void MoveTo(AxisId id, double value)
    {
        lock (_gate)
        {
            if (_emergencyStopped)
            {
                AddFault("Blocked", id, "Emergency stop is active; reset required.");
                return;
            }

            var axis = _axes[id];
            if (!axis.InRange(value))
            {
                AddFault("OutOfRange", id,
                    $"{id} target {value:0.##}{axis.Unit} is outside [{axis.Min:0.##}, {axis.Max:0.##}]{axis.Unit}.");
                return;
            }

            axis.Target = value;
            // Pick a per-move speed (flakiness may vary it from nominal).
            axis.Speed = NextSpeed(axis);
            axis.IsMoving = !NearlyEqual(axis.Position, value, ArrivalEpsilon);
            axis.IsSettled = !axis.IsMoving;
            axis.SettleTimer = 0;
        }
    }

    /// <summary>Command an axis to move by a relative delta.</summary>
    public void MoveBy(AxisId id, double delta)
    {
        double target;
        lock (_gate) { target = _axes[id].Position + delta; }
        MoveTo(id, target);
    }

    /// <summary>Override an axis's travel speed (units/second).</summary>
    public void SetSpeed(AxisId id, double speed)
    {
        lock (_gate) { _axes[id].Speed = Math.Max(0.1, speed); }
    }

    /// <summary>Halt all motion, leaving axes where they are.</summary>
    public void Stop()
    {
        lock (_gate)
        {
            foreach (var axis in _axes.Values) StopAxisLocked(axis);
        }
    }

    public void StopAxis(AxisId id)
    {
        lock (_gate) { StopAxisLocked(_axes[id]); }
    }

    /// <summary>Drive every axis back to its home/baseline pose.</summary>
    public void Home()
    {
        foreach (var id in _axes.Keys.ToArray())
            MoveTo(id, _axes[id].HomePosition);
    }

    /// <summary>Latch an emergency stop — all motion halts and further commands are blocked until reset.</summary>
    public void EmergencyStop()
    {
        lock (_gate)
        {
            _emergencyStopped = true;
            foreach (var axis in _axes.Values) StopAxisLocked(axis);
            AddFault("EmergencyStop", null, "Emergency stop engaged.");
        }
    }

    /// <summary>Set the active flakiness profile (Off / Mild / Harsh / custom).</summary>
    public void SetFlakiness(FlakinessProfile profile)
    {
        lock (_gate)
        {
            _flakiness = profile;
            _rng = new Random(profile.Seed);
        }
    }

    /// <summary>Restore a known baseline: home pose, no faults, no e-stop. Flakiness is preserved.</summary>
    public void Reset()
    {
        lock (_gate)
        {
            _emergencyStopped = false;
            _faults.Clear();
            foreach (var axis in _axes.Values)
            {
                axis.Position = axis.HomePosition;
                axis.Target = axis.HomePosition;
                axis.Speed = axis.NominalSpeed;
                axis.IsMoving = false;
                axis.IsSettled = true;
                axis.SettleTimer = 0;
            }
            _rng = new Random(_flakiness.Seed);
        }
    }

    public void ClearFaults()
    {
        lock (_gate) { _faults.Clear(); }
    }

    // ---- Movement engine ------------------------------------------------------

    private const double ArrivalEpsilon = 0.01;

    /// <summary>
    /// Advance the simulation by <paramref name="elapsedSeconds"/>. Called repeatedly
    /// by the hosted loop. Each moving axis steps toward its target, may overshoot,
    /// then settles after a (possibly flaky) delay. Collisions halt all motion.
    /// </summary>
    public void Tick(double elapsedSeconds)
    {
        if (elapsedSeconds <= 0) return;

        lock (_gate)
        {
            foreach (var axis in _axes.Values)
                StepAxisLocked(axis, elapsedSeconds);

            // Continuous safety check: entering the danger envelope halts everything.
            if (CollisionDetector.IsInCollisionEnvelope(_axes)
                && !_faults.Any(f => f.Kind == "Collision"))
            {
                AddFault("Collision", null,
                    "Collision risk: steep angulation with a raised, off-centre table. Motion halted.");
                foreach (var axis in _axes.Values) StopAxisLocked(axis);
            }
        }
    }

    private void StepAxisLocked(Axis axis, double dt)
    {
        if (!axis.IsMoving)
        {
            // Already arrived; run down any injected settle delay.
            if (!axis.IsSettled)
            {
                axis.SettleTimer -= dt;
                if (axis.SettleTimer <= 0)
                {
                    axis.SettleTimer = 0;
                    axis.IsSettled = true;
                }
            }
            return;
        }

        double remaining = axis.Target - axis.Position;
        double step = axis.Speed * dt;

        if (Math.Abs(remaining) <= step)
        {
            // Reaches (or passes) target this tick.
            axis.Position = axis.Target + FinalJitter();
            axis.IsMoving = false;
            axis.SettleTimer = NextSettleDelay();
            axis.IsSettled = axis.SettleTimer <= 0;
        }
        else
        {
            axis.Position += Math.Sign(remaining) * step;
        }
    }

    private void StopAxisLocked(Axis axis)
    {
        axis.Target = axis.Position;
        axis.IsMoving = false;
        axis.IsSettled = true;
        axis.SettleTimer = 0;
    }

    // ---- Flakiness helpers (all under _gate) ----------------------------------

    private double NextSpeed(Axis axis)
    {
        if (_flakiness.SpeedVariance <= 0) return axis.NominalSpeed;
        double factor = 1 + (_rng.NextDouble() * 2 - 1) * _flakiness.SpeedVariance;
        return Math.Max(0.1, axis.NominalSpeed * factor);
    }

    private double NextSettleDelay() =>
        _flakiness.MaxSettleDelaySeconds <= 0 ? 0 : _rng.NextDouble() * _flakiness.MaxSettleDelaySeconds;

    private double FinalJitter() =>
        _flakiness.JitterAmplitude <= 0 ? 0 : (_rng.NextDouble() * 2 - 1) * _flakiness.JitterAmplitude;

    private static bool NearlyEqual(double a, double b, double eps) => Math.Abs(a - b) <= eps;

    private void AddFault(string kind, AxisId? axis, string message)
    {
        // Caller holds _gate.
        _faults.Add(new SystemFault(kind, axis?.ToString(), message));
    }

    // ---- Read state -----------------------------------------------------------

    public SystemSnapshot GetSnapshot()
    {
        lock (_gate)
        {
            var axes = _axes.Values
                .OrderBy(a => (int)a.Id)
                .Select(a => new AxisSnapshot(
                    a.Id.ToString(), a.Unit, Round(a.Position), Round(a.Target),
                    a.Min, a.Max, Round(a.Speed), a.IsMoving, a.IsSettled))
                .ToList();

            return new SystemSnapshot(
                axes,
                IsMoving: _axes.Values.Any(a => a.IsMoving),
                IsSettled: _axes.Values.All(a => a.IsSettled),
                EmergencyStopped: _emergencyStopped,
                FlakinessProfile: _flakiness.Name,
                Faults: _faults.ToList());
        }
    }

    private static double Round(double v) => Math.Round(v, 3);
}
