namespace Azurion.Simulator;

/// <summary>Immutable view of one axis at an instant. Shared by the REST API and dashboard.</summary>
public sealed record AxisSnapshot(
    string Axis,
    string Unit,
    double Position,
    double Target,
    double Min,
    double Max,
    double Speed,
    bool IsMoving,
    bool IsSettled);

/// <summary>A fault raised by the system (range violation, collision, etc.).</summary>
public sealed record SystemFault(
    string Kind,
    string? Axis,
    string Message);

/// <summary>Complete state of the positioner at an instant.</summary>
public sealed record SystemSnapshot(
    IReadOnlyList<AxisSnapshot> Axes,
    bool IsMoving,
    bool IsSettled,
    bool EmergencyStopped,
    string FlakinessProfile,
    IReadOnlyList<SystemFault> Faults);
