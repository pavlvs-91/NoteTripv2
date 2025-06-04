using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;

namespace EndpointTester
{
    class Program
    {
        public async static void Test()
        {
            var client = new HttpClient(new HttpClientHandler() { AllowAutoRedirect = false });
            var values = new Dictionary<string, string>
            {
                { "login", "admin" },
                { "password", "admin" }
            };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("http://localhost:5091/Login/Form", content);

            Console.WriteLine("code" + response.StatusCode);
            Console.WriteLine(await response.Content.ReadAsStringAsync());

        }
    }
}
