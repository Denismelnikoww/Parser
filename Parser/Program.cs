using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
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

            string? VKaccessToken = configuration["VK_API"];
            await TestVK(VKaccessToken);

        }


        static async Task TestVK(string? accessToken)
        {
            string userId = "denis_bondarenko_dev";
            string apiUrl = $"https://api.vk.com/method/wall.get?domain={userId}&count=10&access_token={accessToken}&v=5.131";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string response = await client.GetStringAsync(apiUrl);
                    JObject json = JObject.Parse(response);


                    var posts = json["response"]["items"];
                    foreach (var post in posts)
                    {
                        Console.WriteLine($"Текст: {post["text"]}");
                        Console.WriteLine("---");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }
    }
}




