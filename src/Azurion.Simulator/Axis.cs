namespace Azurion.Simulator;

/// <summary>
/// A single motorised axis. Holds its physical limits, its current and target
/// position, and how fast it travels. The <see cref="AzurionSystem"/> advances it
/// toward its target a little on every tick — so a move takes real wall-clock time,
/// exactly like a physical motor. That latency is the root cause of the "flaky"
/// behaviour test scripts must cope with.
/// </summary>
public sealed class Axis
{
    public AxisId Id { get; }

    /// <summary>"deg" or "cm" — for display only.</summary>
    public string Unit { get; }

    public double Min { get; }
    public double Max { get; }

    /// <summary>Position the axis rests at after <c>Home()</c>.</summary>
    public double HomePosition { get; }

    /// <summary>Nominal travel speed in units per second.</summary>
    public double NominalSpeed { get; }

    /// <summary>Current speed (units/sec). Can be overridden per axis.</summary>
    public double Speed { get; set; }

    /// <summary>Where the axis is right now.</summary>
    public double Position { get; set; }

    /// <summary>Where the axis has been commanded to go.</summary>
    public double Target { get; set; }

    /// <summary>True while the axis is actively driving toward its target.</summary>
    public bool IsMoving { get; set; }

    /// <summary>
    /// True once the axis has reached its target AND any flakiness-induced settle
    /// delay has elapsed. Robust test scripts wait for this before asserting.
    /// </summary>
    public bool IsSettled { get; set; } = true;

    /// <summary>Remaining settle-delay timer (seconds) injected by flakiness.</summary>
    public double SettleTimer { get; set; }

    public Axis(AxisId id, string unit, double min, double max, double home, double nominalSpeed)
    {
        Id = id;
        Unit = unit;
        Min = min;
        Max = max;
        HomePosition = home;
        NominalSpeed = nominalSpeed;
        Speed = nominalSpeed;
        Position = home;
        Target = home;
    }

    public bool InRange(double value) => value >= Min && value <= Max;

    /// <summary>The canonical set of axes for the positioner, at their home pose.</summary>
    public static IReadOnlyList<Axis> CreateDefaultSet() => new[]
    {
        new Axis(AxisId.StandRotation,     "deg", -120, 120,   0, 25),
        new Axis(AxisId.StandAngulation,   "deg",  -90,  90,   0, 20),
        new Axis(AxisId.StandRoll,         "deg", -180, 180,   0, 30),
        new Axis(AxisId.Sid,               "cm",    90, 120, 100, 10),
        new Axis(AxisId.TableLongitudinal, "cm",  -100, 100,   0, 15),
        new Axis(AxisId.TableLateral,      "cm",   -25,  25,   0, 10),
        new Axis(AxisId.TableHeight,       "cm",    70, 120,  95,  8),
        new Axis(AxisId.TableTilt,         "deg",  -15,  15,   0,  5),
        new Axis(AxisId.TableCradle,       "deg",  -15,  15,   0,  5),
    };
}
