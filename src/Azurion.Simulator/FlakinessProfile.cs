namespace Azurion.Simulator;

/// <summary>
/// Controls how "unreliable" the simulated hardware behaves. Real motorised axes
/// don't stop instantly on a clean number: they take a variable time to settle,
/// rest a hair off target, sometimes overshoot, and don't always travel at exactly
/// the rated speed. This profile dials those imperfections up or down so you can
/// author tests against a perfectly clean system first, then turn on the noise and
/// see why naïve assertions break — and how waiting/tolerance fixes them.
///
/// Randomness is seeded so a given profile reproduces the same run, which keeps
/// "flaky" scenarios debuggable.
/// </summary>
public sealed class FlakinessProfile
{
    /// <summary>Human-readable name (Off / Mild / Harsh / custom).</summary>
    public string Name { get; init; } = "Off";

    /// <summary>Max extra seconds an axis stays un-settled after reaching target.</summary>
    public double MaxSettleDelaySeconds { get; init; }

    /// <summary>Axis may rest up to ±this many units away from the exact target.</summary>
    public double JitterAmplitude { get; init; }

    /// <summary>Probability [0..1] a move overshoots its target before returning.</summary>
    public double OvershootChance { get; init; }

    /// <summary>How far (in units) an overshoot travels past target.</summary>
    public double OvershootAmplitude { get; init; }

    /// <summary>Fractional random variation in travel speed (0.2 = ±20%).</summary>
    public double SpeedVariance { get; init; }

    /// <summary>Seed for reproducible runs.</summary>
    public int Seed { get; init; } = 1;

    public bool IsClean =>
        MaxSettleDelaySeconds == 0 && JitterAmplitude == 0 &&
        OvershootChance == 0 && SpeedVariance == 0;

    public static FlakinessProfile Off => new() { Name = "Off" };

    public static FlakinessProfile Mild => new()
    {
        Name = "Mild",
        MaxSettleDelaySeconds = 0.4,
        JitterAmplitude = 0.2,
        OvershootChance = 0.15,
        OvershootAmplitude = 0.6,
        SpeedVariance = 0.10,
        Seed = 7,
    };

    public static FlakinessProfile Harsh => new()
    {
        Name = "Harsh",
        MaxSettleDelaySeconds = 1.6,
        JitterAmplitude = 0.9,
        OvershootChance = 0.5,
        OvershootAmplitude = 3.0,
        SpeedVariance = 0.4,
        Seed = 13,
    };

    public static FlakinessProfile FromName(string? name) => (name ?? "off").Trim().ToLowerInvariant() switch
    {
        "mild" => Mild,
        "harsh" => Harsh,
        _ => Off,
    };
}
