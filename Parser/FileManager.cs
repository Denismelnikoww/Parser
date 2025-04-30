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

        static async Task Mai()
        {
            string imageUrl = "https://sun2-19.userapi.com//s//v1//if2//sucAULkYHTrOA90D2wgW8dZFIFJ_hICNxgzDagxNfreGshoL2F16sY4oti71hsxWVfBd3K0ocQ5LBfAw2oHvF4Dj.jpg?quality=95&as=32x24,48x36,72x54,108x81,160x120,240x180,360x270,480x360,540x405,640x480,720x540,1080x810,1204x903&from=bu";
            string savePath = "downloaded_image.jpg"; // Путь для сохранения изображения

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Отправляем GET-запрос для получения изображения
                    HttpResponseMessage response = await client.GetAsync(imageUrl);
                    response.EnsureSuccessStatusCode(); // Проверяем успешность запроса

                    // Читаем содержимое ответа как поток байтов
                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                    // Сохраняем изображение на диск
                    await File.WriteAllBytesAsync(savePath, imageBytes);

                    Console.WriteLine($"Изображение успешно скачано и сохранено как: {savePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при скачивании изображения: {ex.Message}");
            }
        }

        public static async Task DownloadPhoto(HttpClient client, string url, string userId, string photoId)
        {
            try
            {
                string userDirectory = Path.Combine(BaseDirectory, userId, "photos");
                Console.WriteLine(userDirectory);
                Directory.CreateDirectory(userDirectory);

                string extension = Path.GetExtension(url.Split('?')[0]) ?? ".jpg";
                string fileName = $"{photoId}.jpg";
                string fullPath = Path.Combine(userDirectory, fileName);

                byte[] imageBytes = await client.GetByteArrayAsync(url);
                await File.WriteAllBytesAsync(fullPath, imageBytes);
                Console.WriteLine($"Фото сохранено: {fullPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при скачивании фото: {ex.Message}");
            }
        }
    }

}


