namespace Azurion.Simulator;

/// <summary>
/// The motorised axes of an Azurion-like interventional X-ray positioner.
///
/// The C-arm <i>stand</i> carries the X-ray tube and detector and can rotate around
/// the patient (RAO/LAO), angulate head-to-foot (CRAN/CAUD) and roll (propeller),
/// and extend the source-image distance (SID). The patient <i>table</i> translates
/// in three directions and tilts/cradles. Together they let the clinician keep a
/// region of interest in view from almost any angle.
/// </summary>
public enum AxisId
{
    // C-arm stand
    StandRotation,    // RAO/LAO  – degrees, rotation around the patient's long axis
    StandAngulation,  // CRAN/CAUD – degrees, head-to-foot angulation
    StandRoll,        // propeller – degrees
    Sid,              // source-image distance – centimetres (detector in/out)

    // Patient table
    TableLongitudinal, // head-foot travel – centimetres
    TableLateral,      // left-right travel – centimetres
    TableHeight,       // up-down travel   – centimetres
    TableTilt,         // degrees
    TableCradle,       // side-to-side roll – degrees
}
