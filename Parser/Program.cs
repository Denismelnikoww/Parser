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

            //string href = $"https://sun2-19.userapi.com//s//v1//if2//sucAULkYHTrOA90D2wgW8dZFIFJ_hICNxgzDagxNfreGshoL2F16sY4oti71hsxWVfBd3K0ocQ5LBfAw2oHvF4Dj.jpg?quality=95&as=32x24,48x36,72x54,108x81,160x120,240x180,360x270,480x360,540x405,640x480,720x540,1080x810,1204x903&from=bu";

            string userId = "durov";

            using (var httpClient = new HttpClient())
            {
                var vkService = new VkApiService(httpClient, vkAccessToken);
                await vkService.ParseUserWall(userId);
                //await FileManager.DownloadPhoto(httpClient,href,userId,"1");
            }
        }
    }
}