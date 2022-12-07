using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace SharpCode.PollyTest.Service.Policies
{
    internal class ServicePolicy
    {
        // Adding new retry prop
        public AsyncRetryPolicy WaitOnErrorRetry { get; }
        public AsyncRetryPolicy WaitOnErrorIncrementalRetry { get; }

        private readonly ILogger<ServicePolicy> _logger;

        public ServicePolicy(ILogger<ServicePolicy> logger)
        {
            _logger = logger;

            // Defining Handler
            WaitOnErrorRetry = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                5,
                attempt => TimeSpan.FromSeconds(5),
                (exception, ts, count, context) =>
                {
                    _logger.LogError(exception, "Error Try {count} Wait {ts}", count, ts);
                });


            // In this case will wait for
            //  2 ^ 1 = 2 seconds then
            //  2 ^ 2 = 4 seconds then
            //  2 ^ 3 = 8 seconds then
            //  2 ^ 4 = 16 seconds then
            //  2 ^ 5 = 32 seconds
            WaitOnErrorIncrementalRetry = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                5,
                attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                (exception, ts, count, context) =>
                {
                    _logger.LogError(exception, "Error Try {count} Wait {ts}", count, ts);
                });
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

        }
    }
}
