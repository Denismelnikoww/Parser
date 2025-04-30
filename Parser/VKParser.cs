using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class VkApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _accessToken;
        static long photoId = 0;
        static long postId = 0;

        public VkApiService(HttpClient httpClient, string accessToken)
        {
            _httpClient = httpClient;
            _accessToken = accessToken;
        }

        public async Task ParseUserWall(string userId)
        {
            try
            {

                string metaApiUrl = $"https://api.vk.com/method/wall.get?domain={userId}&count=1&access_token={_accessToken}&v=5.131";
                string response = await _httpClient.GetStringAsync(metaApiUrl);
                JObject data = JObject.Parse(response);

                int totalPosts = (int)data["response"]["count"];
                Console.WriteLine($"Всего постов на стене: {totalPosts}");


                int batchSize = 10;
                int offset = 0;

                while (offset < totalPosts)
                {
                    string apiUrl = $"https://api.vk.com/method/wall.get?domain={userId}&count={batchSize}&offset={offset}&access_token={_accessToken}&v=5.131&extended=1";
                    response = await _httpClient.GetStringAsync(apiUrl);
                    JObject json = JObject.Parse(response);

                    var texts = new List<string>();
                    JArray posts = (JArray)json["response"]["items"];

                    foreach (var post in posts)
                    {
                        postId++;
                        string text = post["text"]?.ToString();
                        if (!string.IsNullOrEmpty(text))
                        {
                            await FileManager.SaveSinglePostToTxt(userId, text, postId);
                        }
                        await ParsePhoto(post, userId);
                    }
                    offset += batchSize;

                    await Task.Delay(500);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при парсинге стены: {ex.Message}");
            }
        }

        private async Task ParsePhoto(JToken post, string userId)
        {
            if (post["attachments"] == null)
            {
                return;
            }
            foreach (var attachment in post["attachments"])
            {
                if (attachment["type"]?.ToString() == "photo")
                {
                    photoId++;
                    var photoElement = attachment["photo"];
                    var href = photoElement["orig_photo"]["url"].ToString();
                    await FileManager.DownloadPhoto(_httpClient, href, userId, photoId.ToString());
                }
            }
        }

    }

}
