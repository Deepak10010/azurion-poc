namespace Azurion.Simulator;

/// <summary>
/// A deliberately simple geometric guard. On a real Azurion the C-arc wraps around
/// the patient, so a steep angulation combined with a raised, off-centre table can
/// drive the detector into the table or patient. We approximate that danger envelope
/// with one readable rule so tests can reliably provoke (and assert) the safety halt.
///
/// Rule: the system is in the collision envelope when the stand is steeply angulated
/// AND the table is raised into the arc's sweep AND pushed laterally toward the arc.
/// </summary>
public static class CollisionDetector
{
    public const double AngulationThreshold = 50.0; // deg
    public const double TableHeightThreshold = 112.0; // cm
    public const double LateralThreshold = 18.0; // cm (magnitude)

    /// <summary>True if the given pose would put the arc and table in conflict.</summary>
    public static bool IsInCollisionEnvelope(
        double standAngulation, double tableHeight, double tableLateral)
    {
        return Math.Abs(standAngulation) > AngulationThreshold
            && tableHeight > TableHeightThreshold
            && Math.Abs(tableLateral) > LateralThreshold;
    }

    public static bool IsInCollisionEnvelope(IReadOnlyDictionary<AxisId, Axis> axes) =>
        IsInCollisionEnvelope(
            axes[AxisId.StandAngulation].Position,
            axes[AxisId.TableHeight].Position,
            axes[AxisId.TableLateral].Position);
}
