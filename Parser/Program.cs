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
        static async Task Main(string[] args)
        {
            string? userId;

            if (args.Length == 0)
            {
                Console.WriteLine("Нет айди пользователя!");
                return;
            }
            else if (args.Length == 1)
            {
                userId = args[0];
            }
            else {
                Console.WriteLine("Слишком много параметров!");
                return;
            }


            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            string vkAccessToken = configuration["VK_API"];
            string vkAccessToken = "c3f0ff24c3f0ff24c3f0ff249bc0c0f6cecc3f0c3f0ff24abf9cb1eb20fc25cf246849a";

            

            using (var httpClient = new HttpClient())
            {
                var vkService = new VkApiService(httpClient, vkAccessToken);
                await vkService.ParseUser(userId);
            }

            Console.Read();
        }
    }
}