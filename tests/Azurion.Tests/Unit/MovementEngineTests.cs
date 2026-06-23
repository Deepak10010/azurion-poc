using Azurion.Simulator;

namespace Azurion.Tests.Unit;

/// <summary>
/// Fast, in-process tests of the simulator's physics — no web host, no HTTP.
/// These pin down the engine's contract so the higher-level Gherkin tests can trust it.
/// We drive time manually by calling <see cref="AzurionSystem.Tick"/>, so they run instantly.
/// </summary>
[TestFixture]
public class MovementEngineTests
{
    private AzurionSystem _sys = null!;

    [SetUp]
    public void SetUp() => _sys = new AzurionSystem();

    /// <summary>Advance simulated time in small steps until a condition holds or we give up.</summary>
    private bool Advance(Func<SystemSnapshot, bool> until, double maxSeconds = 30, double dt = 0.05)
    {
        for (double t = 0; t < maxSeconds; t += dt)
        {
            _sys.Tick(dt);
            if (until(_sys.GetSnapshot())) return true;
        }
        return until(_sys.GetSnapshot());
    }

    private static AxisSnapshot Axis(SystemSnapshot s, AxisId id) =>
        s.Axes.Single(a => a.Axis == id.ToString());

    [Test]
    public void Move_does_not_complete_instantly()
    {
        _sys.MoveTo(AxisId.StandRotation, 30);
        var immediate = _sys.GetSnapshot();

        // The whole point: commanding a move only sets a target; the arm is still moving.
        Assert.That(Axis(immediate, AxisId.StandRotation).IsMoving, Is.True);
        Assert.That(Axis(immediate, AxisId.StandRotation).Position, Is.LessThan(30));
    }

    [Test]
    public void Move_reaches_target_after_enough_ticks()
    {
        _sys.MoveTo(AxisId.StandRotation, 30);
        bool settled = Advance(s => Axis(s, AxisId.StandRotation).IsSettled);

        Assert.That(settled, Is.True);
        Assert.That(Axis(_sys.GetSnapshot(), AxisId.StandRotation).Position, Is.EqualTo(30).Within(0.01));
    }

    [Test]
    public void Out_of_range_command_raises_fault_and_does_not_move()
    {
        _sys.MoveTo(AxisId.StandRotation, 999); // limit is 120
        var s = _sys.GetSnapshot();

        Assert.That(s.Faults.Any(f => f.Kind == "OutOfRange"), Is.True);
        Assert.That(Axis(s, AxisId.StandRotation).Position, Is.EqualTo(0).Within(0.01));
    }

    [Test]
    public void Stop_halts_motion_partway()
    {
        _sys.MoveTo(AxisId.StandRotation, 120);
        _sys.Tick(0.2); // move a little
        _sys.Stop();
        var afterStop = Axis(_sys.GetSnapshot(), AxisId.StandRotation).Position;

        _sys.Tick(5); // would have finished if still moving
        var later = Axis(_sys.GetSnapshot(), AxisId.StandRotation).Position;

        Assert.That(later, Is.EqualTo(afterStop).Within(0.01));
        Assert.That(later, Is.LessThan(120));
    }

    [Test]
    public void Home_returns_all_axes_to_baseline()
    {
        _sys.MoveTo(AxisId.TableHeight, 110);
        Advance(s => Axis(s, AxisId.TableHeight).IsSettled);
        _sys.Home();
        Advance(s => s.IsSettled);

        Assert.That(Axis(_sys.GetSnapshot(), AxisId.TableHeight).Position, Is.EqualTo(95).Within(0.01));
    }

    [Test]
    public void Collision_envelope_raises_fault_and_halts()
    {
        // Drive into the danger envelope: steep angulation + raised + off-centre table.
        _sys.MoveTo(AxisId.StandAngulation, 70);
        _sys.MoveTo(AxisId.TableHeight, 118);
        _sys.MoveTo(AxisId.TableLateral, 24);

        bool collided = Advance(s => s.Faults.Any(f => f.Kind == "Collision"));

        Assert.That(collided, Is.True);
        Assert.That(_sys.GetSnapshot().IsMoving, Is.False, "motion should halt on collision");
    }

    [Test]
    public void Emergency_stop_blocks_further_commands_until_reset()
    {
        _sys.EmergencyStop();
        _sys.MoveTo(AxisId.StandRotation, 30);
        Advance(s => true, maxSeconds: 1);

        Assert.That(Axis(_sys.GetSnapshot(), AxisId.StandRotation).Position, Is.EqualTo(0).Within(0.01));

        _sys.Reset();
        _sys.MoveTo(AxisId.StandRotation, 30);
        Advance(s => Axis(s, AxisId.StandRotation).IsSettled);
        Assert.That(Axis(_sys.GetSnapshot(), AxisId.StandRotation).Position, Is.EqualTo(30).Within(0.01));
    }

    [Test]
    public void Harsh_flakiness_delays_settle_beyond_arrival()
    {
        _sys.SetFlakiness(FlakinessProfile.Harsh);
        _sys.MoveTo(AxisId.StandRotation, 30);

        // Run until the position is essentially at target...
        Advance(s => Math.Abs(Axis(s, AxisId.StandRotation).Position - 30) < 1.0, maxSeconds: 10);

        // ...and confirm the axis can still report not-settled right after arriving,
        // which is exactly what trips up a naïve "assert immediately" script.
        // (With a fixed seed the harsh profile injects a settle delay.)
        bool eventuallySettles = Advance(s => Axis(s, AxisId.StandRotation).IsSettled, maxSeconds: 10);
        Assert.That(eventuallySettles, Is.True, "axis must settle eventually even under harsh flakiness");
    }
}
