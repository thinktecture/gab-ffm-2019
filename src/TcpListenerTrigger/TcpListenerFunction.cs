using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Logging;
using System;
using TcpListenerTrigger;

[assembly: WebJobsStartup(typeof(Startup))]
namespace TcpListenerTrigger
{
    public static class TcpListenerFunction
    {
        [FunctionName("TcpListenerFunction")]
        public static void Run(
            [TcpListenerTrigger()] string message,
            ILogger log)
        {
            log.LogInformation("TcpListener trigger function processed a Message {message}", message);
            Console.WriteLine($"TcpListener trigger function processed a Message '{message}'");
        }
    }
}
