using Azurion.Simulator;

namespace Azurion.Tests.Support;

/// <summary>
/// The single most important idea in hardware test automation: never assert a moving
/// system's state — <i>poll until it settles</i>, then assert with a tolerance. These
/// helpers do exactly that. A naïve script that reads state right after commanding a
/// move is reading mid-flight, which is why such scripts are flaky. (The wall-clock
/// timing here is the test harness's own; it has nothing to do with the simulator's
/// deterministic physics.)
/// </summary>
public static class Wait
{
    public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(25);
    private static readonly TimeSpan PollInterval = TimeSpan.FromMilliseconds(50);

    /// <summary>Poll until the whole system reports settled (or time out).</summary>
    public static Task<SystemSnapshot> UntilSettledAsync(AzurionClient client, TimeSpan? timeout = null) =>
        UntilAsync(client, s => s.IsSettled, timeout);

    /// <summary>Poll until a specific axis is within tolerance of a value AND settled.</summary>
    public static Task<SystemSnapshot> UntilAxisAtAsync(
        AzurionClient client, AxisId axis, double value, double tolerance, TimeSpan? timeout = null) =>
        UntilAsync(client, s =>
        {
            var a = s.Axis(axis);
            return !a.IsMoving && a.IsSettled && Math.Abs(a.Position - value) <= tolerance;
        }, timeout);

    /// <summary>Poll until a fault of the given kind appears.</summary>
    public static Task<SystemSnapshot> UntilFaultAsync(AzurionClient client, string kind, TimeSpan? timeout = null) =>
        UntilAsync(client, s => s.HasFault(kind), timeout);

    /// <summary>
    /// Generic poll. Returns as soon as <paramref name="predicate"/> holds; otherwise
    /// returns the last snapshot when the timeout elapses, leaving the caller to assert.
    /// </summary>
    public static async Task<SystemSnapshot> UntilAsync(
        AzurionClient client, Func<SystemSnapshot, bool> predicate, TimeSpan? timeout = null)
    {
        var deadline = DateTime.UtcNow + (timeout ?? DefaultTimeout);
        SystemSnapshot snap = await client.GetStateAsync();
        while (!predicate(snap) && DateTime.UtcNow < deadline)
        {
            await Task.Delay(PollInterval);
            snap = await client.GetStateAsync();
        }
        return snap;
    }
}
