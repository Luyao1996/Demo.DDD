using Microsoft.Extensions.Hosting;

namespace DDD.Demo.Application.BackgroundJobs;

public class PrintNowTimeBackgroundJob:BackgroundService
{
    public override async Task StartAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("PrintNowTimeBackgroundJob Starting...");
        base.StartAsync(stoppingToken);
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("PrintNowTimeBackgroundJob Stopping...");
        base.StopAsync(stoppingToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            try
            {
                Console.WriteLine($"Now time is {DateTime.Now}");
            }
            finally
            {
                await Task.Delay(10 * 1000);
            }
        }
    }
}