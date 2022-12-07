using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace SharpCode.PollyTest.Service.Policies
{
    internal class ServicePolicy
    {
        // Adding new retry prop
        public AsyncRetryPolicy WaitOnErrorRetry { get; }

        private readonly ILogger<ServicePolicy> _logger;

        public ServicePolicy(ILogger<ServicePolicy> logger)
        {

            // Defining Handler
            WaitOnErrorRetry = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(5, attempt => TimeSpan.FromSeconds(5), (ex, t, i, c) => LogStuff(ex, t, i, c));

            _logger = logger;
        }

        /// <summary>
        /// Custom Logger for each RetryWait
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="s">TimeSpan</param>
        /// <param name="i">RetryCounter</param>
        /// <param name="c">PolicyContext</param>
        private void LogStuff(Exception ex, TimeSpan s, int i, Context c)
        {
            _logger.LogError(ex, "Error Try {i} Wait {s}", i, s);
        }
    }
}
