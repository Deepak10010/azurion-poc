using Azurion.Simulator;

namespace Azurion.Web;

// Request bodies for the REST API. Axis names are parsed case-insensitively against AxisId.

public sealed record MoveRequest(string Axis, double Value);
public sealed record MoveByRequest(string Axis, double Delta);
public sealed record SpeedRequest(string Axis, double Speed);
public sealed record FlakinessRequest(string Profile);

internal static class AxisParsing
{
    /// <summary>Parse an axis name (case-insensitive) into an <see cref="AxisId"/>.</summary>
    public static bool TryParse(string? name, out AxisId id) =>
        Enum.TryParse(name, ignoreCase: true, out id);
}
