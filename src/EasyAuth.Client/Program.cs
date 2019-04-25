using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EasyAuth.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string aadInstance = "https://login.windows.net/{0}";
            const string tenant = "thinktecture.com";
            const string serviceResourceId = "https://gab-easy-auth-test.azurewebsites.net";
            const string clientId = "fb50c490-9ecf-4a1a-88d6-5ba9759de599";

            var authContext = new AuthenticationContext(string.Format(CultureInfo.InvariantCulture, aadInstance, tenant));
            var deviceCodeResult = await authContext.AcquireDeviceCodeAsync(serviceResourceId, clientId);
            Console.WriteLine(deviceCodeResult.Message);

            var result = await authContext.AcquireTokenByDeviceCodeAsync(deviceCodeResult);
            Console.WriteLine($"Token acquired. ");
            Console.WriteLine("--------------------------------------------");

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            Console.WriteLine("Calling Function 1");
            var response = await httpClient.GetAsync($"https://gab-easy-auth-test.azurewebsites.net/api/Function1");
            Console.WriteLine($"Status {response.StatusCode} Content: {await response.Content.ReadAsStringAsync()}");
            Console.WriteLine("--------------------------------------------");

            Console.WriteLine("Calling Function 2");
            response = await httpClient.GetAsync($"https://gab-easy-auth-test.azurewebsites.net/api/Function2");
            Console.WriteLine($"Status {response.StatusCode} Content: {await response.Content.ReadAsStringAsync()}");
        }
    }
}
