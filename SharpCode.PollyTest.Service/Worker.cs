using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharpCode.PollyTest.Core;
using SharpCode.PollyTest.Service.Policies;

namespace SharpCode.PollyTest.Service
{
    internal class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ServicePolicy _servicePolicy;
        private readonly ServiceCore _serviceCore;

        public Worker(ILogger<Worker> logger, ServicePolicy servicePolicy, ServiceCore serviceCore)
        {
            _logger = logger;
            _servicePolicy = servicePolicy;
            _serviceCore = serviceCore;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Wrapping Core method in service policy ruleset
            await _servicePolicy.WaitOnErrorRetry.ExecuteAsync(
                () => _serviceCore.DoStuff());

            await _servicePolicy.WaitOnErrorIncrementalRetry.ExecuteAsync(
                () => _serviceCore.DoStuff());
        }
    }
}
