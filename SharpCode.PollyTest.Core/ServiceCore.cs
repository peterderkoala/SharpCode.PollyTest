using Microsoft.Extensions.Logging;

namespace SharpCode.PollyTest.Core
{
    public class ServiceCore
    {
        private readonly ILogger<ServiceCore> _logger;

        public ServiceCore(ILogger<ServiceCore> logger)
        {
            _logger = logger;
        }

        public async Task DoStuff()
        {
            _logger.LogDebug($"{nameof(DoStuff)} :: DoStuff");
            throw new NotImplementedException();
        }
    }
}