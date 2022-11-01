using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace AzScaleFun
{
    public static class MyScaleTest
    {
        [FunctionName("MyScaleTest")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var lStateTime = DateTime.UtcNow.Ticks;

            await DoSomeHeavyWork();

            var lEndTime = DateTime.UtcNow.Ticks;

            var lTicks = lEndTime - lStateTime;

            string responseMessage = $"Ticks taken: {lTicks}";

            log.LogMetric("TicksTaken", lTicks);

            return new OkObjectResult(responseMessage);
        }

        private static Task<int> DoSomeHeavyWork()
        {
            var lIterations = 1000;

            for (int i = 0; i < lIterations; i++)
            {
                FindPrimeNumber(1000);
            }

            return Task.FromResult(0);
        }

        private static long FindPrimeNumber(int n)
        {
            int count = 0;
            long a = 2;
            while (count < n)
            {
                long b = 2;
                int prime = 1;// to check if found a prime
                while (b * b <= a)
                {
                    if (a % b == 0)
                    {
                        prime = 0;
                        break;
                    }
                    b++;
                }
                if (prime > 0)
                {
                    count++;
                }
                a++;
            }
            return (--a);

        }
    }
}
