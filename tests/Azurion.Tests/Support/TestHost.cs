using Microsoft.AspNetCore.Mvc.Testing;

namespace Azurion.Tests.Support;

/// <summary>
/// Decides what the test suite talks to, once per run:
///
///  • <b>External mode</b> — if a simulator is already running (default
///    <c>http://localhost:5023</c>, or wherever <c>AZURION_BASE_URL</c> points), the
///    tests drive that instance. Open the dashboard in a browser and you literally
///    watch the C-arm move as scenarios execute. This is the intended way to learn.
///
///  • <b>In-process mode</b> — if nothing is reachable, the suite boots the web app
///    itself via <see cref="WebApplicationFactory{TEntryPoint}"/>. Fully headless, so
///    <c>dotnet test</c> always works (CI, first run) with no manual setup.
/// </summary>
public static class TestHost
{
    private static readonly object Gate = new();
    private static HttpClient? _client;
    private static WebApplicationFactory<Program>? _factory;

    /// <summary>Human-readable description of the resolved mode, for diagnostics.</summary>
    public static string Mode { get; private set; } = "(unresolved)";

    public static HttpClient Client
    {
        get
        {
            lock (Gate)
            {
                return _client ??= Resolve();
            }
        }
    }

    private static HttpClient Resolve()
    {
        var candidate = Environment.GetEnvironmentVariable("AZURION_BASE_URL") ?? "http://localhost:5023";

        if (IsReachable(candidate))
        {
            Mode = $"external — {candidate} (watch the dashboard!)";
            return new HttpClient { BaseAddress = new Uri(candidate) };
        }

        _factory = new WebApplicationFactory<Program>();
        Mode = "in-process WebApplicationFactory (headless)";
        return _factory.CreateClient();
    }

    private static bool IsReachable(string url)
    {
        try
        {
            using var probe = new HttpClient { BaseAddress = new Uri(url), Timeout = TimeSpan.FromMilliseconds(800) };
            return probe.GetAsync("/api/system/state").GetAwaiter().GetResult().IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public static void Shutdown()
    {
        lock (Gate)
        {
            _client?.Dispose();
            _factory?.Dispose();
            _client = null;
            _factory = null;
        }
    }
}
