using Azurion.Simulator;

namespace Azurion.Web;

/// <summary>
/// The heartbeat of the simulator. A hosted background service that ticks the shared
/// <see cref="AzurionSystem"/> on a fixed cadence (advancing every axis toward its
/// target). This is the single place wall-clock time is fed into the otherwise pure
/// simulator. The dashboard reads the resulting state by polling the REST API.
/// </summary>
public sealed class MovementLoop : BackgroundService
{
    private static readonly TimeSpan TickInterval = TimeSpan.FromMilliseconds(50); // 20 Hz

    private readonly AzurionSystem _system;

    public MovementLoop(AzurionSystem system) => _system = system;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TickInterval);
        double dt = TickInterval.TotalSeconds;

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
                _system.Tick(dt);
        }
        catch (OperationCanceledException)
        {
            // Normal shutdown.
        }
    }
}
