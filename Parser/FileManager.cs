using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{

    public static class FileManager
    {
        private static readonly string BaseDirectory = "D:\\ИИ ПРОЕКТ\\parse\\";

        public static async Task SaveSinglePostToTxt(string userId, string postText, long postIndex)
        {
            try
            {
                string userDirectory = Path.Combine(BaseDirectory, userId, "posts");
                Directory.CreateDirectory(userDirectory);

                string filePath = Path.Combine(userDirectory, $"post_{postIndex}.txt");

                await File.WriteAllTextAsync(filePath, postText, Encoding.UTF8);

                Console.WriteLine($"Пост #{postIndex} сохранён: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении поста #{postIndex}: {ex.Message}");
            }
        }
        public static async Task SavePostsToJson(string userId, List<string> posts)
        {
            try
            {
                string userDirectory = Path.Combine(BaseDirectory, userId);
                Directory.CreateDirectory(userDirectory);

                string json = JsonConvert.SerializeObject(posts, Formatting.Indented);
                string filePath = Path.Combine(userDirectory, "posts.json");
                await File.WriteAllTextAsync(filePath, json);

                Console.WriteLine($"Посты сохранены в {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении постов: {ex.Message}");
            }
        }

        public static async Task DownloadPhoto(HttpClient client, string url, string userId, string photoId)
        {
            try
            {
                string userDirectory = Path.Combine(BaseDirectory, userId, "photos");
                Directory.CreateDirectory(userDirectory);

                string extension = Path.GetExtension(url.Split('?')[0]) ?? ".jpg";
                string fileName = $"{photoId}.jpg";
                string fullPath = Path.Combine(userDirectory, fileName);

                byte[] imageBytes = await client.GetByteArrayAsync(url);
                await File.WriteAllBytesAsync(fullPath, imageBytes);
                Console.WriteLine($"Фото #{photoId} сохранёно: {fullPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при скачивании фото: {ex.Message}");
            }
        }
    }

}


