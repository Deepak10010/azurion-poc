using System.Net.Http.Json;
using System.Text.Json;
using Azurion.Simulator;

namespace Azurion.Tests.Support;

/// <summary>
/// The test suite's view of the system under test — a typed wrapper over the REST API.
/// Think of it as the "page object" for hardware: scenarios speak in terms of moves and
/// state, never raw HTTP. Every command returns the resulting <see cref="SystemSnapshot"/>.
/// </summary>
public sealed class AzurionClient
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    private readonly HttpClient _http;

    public AzurionClient(HttpClient http) => _http = http;

    public Task<SystemSnapshot> GetStateAsync() => GetAsync("/api/system/state");

    public Task<SystemSnapshot> MoveAsync(AxisId axis, double value) =>
        PostAsync("/api/system/move", new { axis = axis.ToString(), value });

    public Task<SystemSnapshot> MoveByAsync(AxisId axis, double delta) =>
        PostAsync("/api/system/moveby", new { axis = axis.ToString(), delta });

    public Task<SystemSnapshot> SetSpeedAsync(AxisId axis, double speed) =>
        PostAsync("/api/system/speed", new { axis = axis.ToString(), speed });

    public Task<SystemSnapshot> StopAsync() => PostAsync("/api/system/stop", null);
    public Task<SystemSnapshot> HomeAsync() => PostAsync("/api/system/home", null);
    public Task<SystemSnapshot> EmergencyStopAsync() => PostAsync("/api/system/emergency-stop", null);
    public Task<SystemSnapshot> ResetAsync() => PostAsync("/api/system/reset", null);
    public Task<SystemSnapshot> ClearFaultsAsync() => PostAsync("/api/system/clear-faults", null);

    public Task<SystemSnapshot> SetFlakinessAsync(string profile) =>
        PostAsync("/api/system/flakiness", new { profile });

    // ---- plumbing -------------------------------------------------------------

    private async Task<SystemSnapshot> GetAsync(string path)
    {
        var resp = await _http.GetAsync(path);
        resp.EnsureSuccessStatusCode();
        return (await resp.Content.ReadFromJsonAsync<SystemSnapshot>(Json))!;
    }

    private async Task<SystemSnapshot> PostAsync(string path, object? body)
    {
        var resp = await _http.PostAsJsonAsync(path, body, Json);
        resp.EnsureSuccessStatusCode();
        return (await resp.Content.ReadFromJsonAsync<SystemSnapshot>(Json))!;
    }
}

/// <summary>Convenience extensions for reading a single axis out of a snapshot.</summary>
public static class SnapshotExtensions
{
    public static AxisSnapshot Axis(this SystemSnapshot snap, AxisId id) =>
        snap.Axes.Single(a => a.Axis == id.ToString());

    public static bool HasFault(this SystemSnapshot snap, string kind) =>
        snap.Faults.Any(f => f.Kind.Equals(kind, StringComparison.OrdinalIgnoreCase));
}
