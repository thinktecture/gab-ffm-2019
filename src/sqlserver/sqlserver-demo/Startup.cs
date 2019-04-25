using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Serverless.Azure.WebJobs.Extensions.SqlServer;

[assembly: WebJobsStartup(typeof(Serverless.Startup))]
namespace Serverless
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddSqlServer();
        }
    }
}