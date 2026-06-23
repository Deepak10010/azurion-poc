using Azurion.Simulator;
using Azurion.Web;

var builder = WebApplication.CreateBuilder(args);

// One shared simulator for the whole process: the dashboard watches it and the
// test suite drives it — the same instance, so tests visibly move the dashboard.
builder.Services.AddSingleton<AzurionSystem>();
builder.Services.AddHostedService<MovementLoop>();

var app = builder.Build();

app.UseDefaultFiles();   // serve wwwroot/index.html at "/"
app.UseStaticFiles();

var api = app.MapGroup("/api/system");

api.MapGet("/state", (AzurionSystem sys) => Results.Ok(sys.GetSnapshot()));

api.MapPost("/move", (MoveRequest req, AzurionSystem sys) =>
{
    if (!AxisParsing.TryParse(req.Axis, out var id))
        return Results.BadRequest($"Unknown axis '{req.Axis}'.");
    sys.MoveTo(id, req.Value);
    return Results.Ok(sys.GetSnapshot());
});

api.MapPost("/moveby", (MoveByRequest req, AzurionSystem sys) =>
{
    if (!AxisParsing.TryParse(req.Axis, out var id))
        return Results.BadRequest($"Unknown axis '{req.Axis}'.");
    sys.MoveBy(id, req.Delta);
    return Results.Ok(sys.GetSnapshot());
});

api.MapPost("/speed", (SpeedRequest req, AzurionSystem sys) =>
{
    if (!AxisParsing.TryParse(req.Axis, out var id))
        return Results.BadRequest($"Unknown axis '{req.Axis}'.");
    sys.SetSpeed(id, req.Speed);
    return Results.Ok(sys.GetSnapshot());
});

api.MapPost("/stop", (AzurionSystem sys) => { sys.Stop(); return Results.Ok(sys.GetSnapshot()); });
api.MapPost("/home", (AzurionSystem sys) => { sys.Home(); return Results.Ok(sys.GetSnapshot()); });
api.MapPost("/emergency-stop", (AzurionSystem sys) => { sys.EmergencyStop(); return Results.Ok(sys.GetSnapshot()); });
api.MapPost("/reset", (AzurionSystem sys) => { sys.Reset(); return Results.Ok(sys.GetSnapshot()); });
api.MapPost("/clear-faults", (AzurionSystem sys) => { sys.ClearFaults(); return Results.Ok(sys.GetSnapshot()); });

api.MapPost("/flakiness", (FlakinessRequest req, AzurionSystem sys) =>
{
    sys.SetFlakiness(FlakinessProfile.FromName(req.Profile));
    return Results.Ok(sys.GetSnapshot());
});

app.Run();

// Exposed so the test project can spin up the host in-process via WebApplicationFactory.
public partial class Program { }
