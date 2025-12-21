using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.External
{
    public static class PollyResiliencePolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ApiClientConfiguration config)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                // Tu profe usaba BadRequest; yo te sugiero cambiarlo a TooManyRequests (429)
                // pero si querés copiar literal, dejalo como BadRequest.
                .OrResult(msg => (int)msg.StatusCode == 429)
                .WaitAndRetryAsync(
                    config.RetryCount,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt) * config.RetryAttemptInSeconds)
                );
        }

        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(ApiClientConfiguration config)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => (int)msg.StatusCode == 429)
                .CircuitBreakerAsync(
                    config.HandledEventsAllowedBeforeBreaking,
                    TimeSpan.FromSeconds(config.DurationOfBreakInSeconds)
                );
        }
    }
}
