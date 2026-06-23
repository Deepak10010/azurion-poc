using Azurion.Simulator;
using Azurion.Tests.Support;
using Reqnroll;

namespace Azurion.Tests.Steps;

/// <summary>
/// Step definitions for the positioning scenarios. Reqnroll creates one instance per
/// scenario, so the fields below safely carry state between a scenario's steps (e.g.
/// the snapshot captured by a "read immediately" step).
/// </summary>
[Binding]
public sealed class PositioningSteps
{
    private readonly AzurionClient _client = new(TestHost.Client);
    private SystemSnapshot? _immediate; // state captured right after a command, on purpose

    private static AxisId Axis(string name) =>
        Enum.Parse<AxisId>(name, ignoreCase: true);

    // ---- Given ----------------------------------------------------------------

    [Given(@"the system is at its home pose")]
    [Given(@"the system is reset")]
    public async Task GivenReset() => await _client.ResetAsync();

    [Given(@"the flakiness profile is ""(.*)""")]
    public async Task GivenFlakiness(string profile) => await _client.SetFlakinessAsync(profile);

    // ---- When -----------------------------------------------------------------

    [When(@"I command (\w+) to (-?\d+(?:\.\d+)?)(?: degrees| centimetres| cm| deg)?")]
    public async Task WhenCommandTo(string axis, double value) =>
        await _client.MoveAsync(Axis(axis), value);

    [When(@"I command (\w+) by (-?\d+(?:\.\d+)?)(?: degrees| centimetres| cm| deg)?")]
    public async Task WhenCommandBy(string axis, double delta) =>
        await _client.MoveByAsync(Axis(axis), delta);

    [When(@"I send the system home")]
    public async Task WhenHome() => await _client.HomeAsync();

    [When(@"I stop all motion")]
    public async Task WhenStop() => await _client.StopAsync();

    [When(@"I trigger an emergency stop")]
    public async Task WhenEstop() => await _client.EmergencyStopAsync();

    [When(@"I wait for the system to settle")]
    public async Task WhenWaitSettle() => await Wait.UntilSettledAsync(_client);

    [When(@"I read the state immediately")]
    public async Task WhenReadImmediately() => _immediate = await _client.GetStateAsync();

    [When(@"I drive into the collision envelope")]
    public async Task WhenDriveIntoCollision()
    {
        // Steep angulation + raised + laterally-offset table → the danger envelope.
        await _client.MoveAsync(AxisId.StandAngulation, 70);
        await _client.MoveAsync(AxisId.TableHeight, 118);
        await _client.MoveAsync(AxisId.TableLateral, 24);
    }

    // ---- Then -----------------------------------------------------------------

    [Then(@"(\w+) is at (-?\d+(?:\.\d+)?) within (\d+(?:\.\d+)?)")]
    public async Task ThenAxisAtWithin(string axis, double value, double tolerance)
    {
        var snap = await _client.GetStateAsync();
        var a = snap.Axis(Axis(axis));
        Assert.That(a.Position, Is.EqualTo(value).Within(tolerance),
            $"{axis} should rest at {value} (±{tolerance}) but was {a.Position}");
    }

    [Then(@"(\w+) has not arrived at (-?\d+(?:\.\d+)?) yet")]
    public void ThenNotArrivedYet(string axis, double value)
    {
        Assert.That(_immediate, Is.Not.Null, "call 'I read the state immediately' first");
        var a = _immediate!.Axis(Axis(axis));
        // The teaching point: right after commanding, the axis is still en route. A script
        // that asserts the target here is asserting mid-flight — the classic flaky mistake.
        Assert.That(a.Position, Is.Not.EqualTo(value).Within(0.001),
            $"{axis} unexpectedly arrived instantly — real hardware never does");
    }

    [Then(@"the system is settled")]
    public async Task ThenSettled()
    {
        var snap = await _client.GetStateAsync();
        Assert.That(snap.IsSettled, Is.True);
    }

    [Then(@"the system is not moving")]
    public async Task ThenNotMoving()
    {
        var snap = await _client.GetStateAsync();
        Assert.That(snap.IsMoving, Is.False);
    }

    [Then(@"a(?:n)? (\w+) fault is raised")]
    public async Task ThenFaultRaised(string kind)
    {
        var snap = await Wait.UntilFaultAsync(_client, kind, TimeSpan.FromSeconds(5));
        Assert.That(snap.HasFault(kind), Is.True,
            $"expected a '{kind}' fault; faults were: {string.Join(", ", snap.Faults.Select(f => f.Kind))}");
    }

    [Then(@"no fault is raised")]
    public async Task ThenNoFault()
    {
        var snap = await _client.GetStateAsync();
        Assert.That(snap.Faults, Is.Empty,
            $"expected no faults but saw: {string.Join(", ", snap.Faults.Select(f => f.Kind))}");
    }

    [Then(@"(\w+) stays at (-?\d+(?:\.\d+)?) within (\d+(?:\.\d+)?)")]
    public async Task ThenStaysAt(string axis, double value, double tolerance)
    {
        var snap = await _client.GetStateAsync();
        var a = snap.Axis(Axis(axis));
        Assert.That(a.Position, Is.EqualTo(value).Within(tolerance));
    }
}
