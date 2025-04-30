using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Parser
{
    
    
    internal class Program
    {
        static async Task Main()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            string vkAccessToken = configuration["VK_API"];

           
            string userId = "durov";

            using (var httpClient = new HttpClient())
            {
                var vkService = new VkApiService(httpClient, vkAccessToken);
                await vkService.ParseUserWall(userId);
            }
        }
    }
}