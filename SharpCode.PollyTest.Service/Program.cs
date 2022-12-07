using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Hosting;
using NLog.Extensions.Logging;
using SharpCode.PollyTest.Core;
using SharpCode.PollyTest.Service;
using SharpCode.PollyTest.Service.Policies;

var host = Host.CreateDefaultBuilder()
    .ConfigureLogging((context, logging) =>
    {
        logging.ClearProviders();
        logging.SetMinimumLevel(LogLevel.Trace);
        logging.AddNLog();
    })
    .ConfigureServices((context, service) =>
    {
        service.AddSingleton<ServicePolicy>();
        service.AddSingleton<ServiceCore>();
        service.AddHostedService<Worker>()
        .Configure<HostOptions>(options =>
        {
            options.ShutdownTimeout = TimeSpan.FromSeconds(1);
        });
    })
    .UseNLog()
    .Build();

await host.RunAsync();
