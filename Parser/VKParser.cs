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

        public VkApiService(HttpClient httpClient, string accessToken)
        {
            _httpClient = httpClient;
            _accessToken = accessToken;
        }

        public async Task ParseUserWall(string userId)
        {
            try
            {
                //КОЛИЧЕСТВО ПОСТОВ
                string metaApiUrl = $"https://api.vk.com/method/wall.get?domain={userId}&count=1&access_token={_accessToken}&v=5.131";

                string response = await _httpClient.GetStringAsync(metaApiUrl);
                JObject data = JObject.Parse(response);

                int totalPosts = (int)data["response"]["count"];
                Console.WriteLine($"Всего постов на стене: {totalPosts}");

                
                int batchSize = 0;

                while (totalPosts > 0)
                {
                    string apiUrl = $"https://api.vk.com/method/wall.get?domain={userId}&count=10&access_token={_accessToken}&v=5.131&extended=1";

                    response = await _httpClient.GetStringAsync(apiUrl);
                    JObject json = JObject.Parse(response);
                    
                    //File.WriteAllText("D:\\ИИ ПРОЕКТ\\test.json", json.ToString());

                    var texts = new List<string>();
                    JArray posts = (JArray)json["response"]["items"];

                    batchSize = posts.Count;
                    totalPosts -= batchSize;

                    foreach (var post in posts)
                    {

                        string text = post["text"]?.ToString();
                        if (!string.IsNullOrEmpty(text))
                        {
                            texts.Add(text);
                        }

                        await ParsePhoto(post, userId);
                    }
                    await FileManager.SavePostsToJson(userId, texts);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при парсинге стены: {ex.Message}");
            }
        }


        static long photoId;
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

        private string GetMaxSizePhotoUrl(JToken photo)
        {
            string[] sizePriority = { "photo_2560", "photo_1280", "photo_807", "photo_604", "photo_130", "photo_75" };

            foreach (var size in sizePriority)
            {
                if (photo[size] != null)
                    return photo[size].ToString();
            }

            return null;
        }
    }

}
