using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.External
{
    public class ApiClientConfiguration
    {
        public int RetryCount { get; set; }
        public int RetryAttemptInSeconds { get; set; }

        public int DurationOfBreakInSeconds { get; set; }
        public int HandledEventsAllowedBeforeBreaking { get; set; }
    }
}
