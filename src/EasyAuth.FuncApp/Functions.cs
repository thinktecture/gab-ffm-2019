using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace EasyAuth.FuncApp
{
    public static class Functions
    {
        [FunctionName("Function1")]
        public static IActionResult RunFunction1(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ClaimsPrincipal principal)
        {
            return principal.IfInRole("GabEasyAuthFunc1", () =>
            {
                var name = $"{principal.Claims.FirstOrDefault(c => c.Type == "name")?.Value} ({principal.Identity.Name})";
                return new OkObjectResult($"Hello, {name} from Function 1.");
            });
        }

        [FunctionName("Function2")]
        public static IActionResult RunFunction2(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ClaimsPrincipal principal)
        {
            return principal.IfInRole("GabEasyAuthFunc2", () =>
            {
                var name = $"{principal.Claims.FirstOrDefault(c => c.Type == "name")?.Value} ({principal.Identity.Name})";
                return new OkObjectResult($"Hello, {name} from Function 2.");
            });
        }
    }

    public static class ClaimsPrincipalExtensions
    {
        public static IActionResult IfInRole(this ClaimsPrincipal principal, string role, Func<IActionResult> action)
        {
            // principal.IsInRole is not working with AAD Application Roles: https://github.com/Azure/azure-functions-host/issues/3898
            if (principal.FindAll("roles").Any(c => c.Value == role))
            {
                return action();
            }

            return new UnauthorizedResult();
        }
    }
}
