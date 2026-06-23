using Reqnroll;

namespace Azurion.Tests.Support;

/// <summary>
/// Reqnroll lifecycle hooks. Every scenario starts from a known baseline — clean
/// flakiness, home pose, no faults — so scenarios are independent and repeatable.
/// </summary>
[Binding]
public sealed class Hooks
{
    [BeforeTestRun]
    public static void AnnounceMode()
    {
        // Touch the client so the mode is resolved and printed once up front.
        _ = TestHost.Client;
        TestContext.Progress.WriteLine($"[Azurion] Test target: {TestHost.Mode}");
    }

    [BeforeScenario(Order = 0)]
    public async Task ResetToBaseline()
    {
        var client = new AzurionClient(TestHost.Client);
        await client.SetFlakinessAsync("off"); // reset preserves flakiness, so clear it first
        await client.ResetAsync();
    }

    [AfterTestRun]
    public static void Teardown() => TestHost.Shutdown();
}
