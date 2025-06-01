# 📚 VK Parser (Проект для учебной практикии)

Простой парсер данных пользователя ВКонтакте.

**Основной функционал:**
- Парсинг постов со стены пользователя
- Скачивание основной фотографии профиля
- Сохранение фотографий из постов
- Экспорт данных в TXT и JSON

## 🛠 Технологии
- **.NET** (Console Application)
- **VK API** (версия 5.131)
- **Newtonsoft.Json** для работы с JSON
- **HttpClient** для асинхронных запросов
- **Microsoft.Extensions.Configuration** для хранения токена

## ⚙️ Настройка и запуск
1. Получите access token VK API (требуются права: `wall,photos`)
2. Добавьте токен в secrets.json:
   ```json
   {
     "VK_API": "ваш_токен"
   }
   ```
3. Запустите с параметром (ID пользователя или короткое имя):
```bash
dotnet run username
```
## 📂 Структура сохранения данных
Все файлы сохраняются в
``C:\Users\<User>\Downloads\parse\:``
```
user_id/
├── posts/
│   ├── post_1.txt
│   └── posts.json
└── photos/
    ├── main_photo.jpg
    └── 1.jpg
```
