﻿using System.Text.Json;

namespace Countdown.Common.Dictionary
{
    public class DictionaryLoader
    {
        private const string NET_FILE_PATH = "https://raw.githubusercontent.com/DonH-ITS/jsonfiles/main/cdwords.txt";
        private const string FOLDER_NAME = "Resources/Dictionary";
        private const string TXT_FILE_NAME = "cdwords.txt";
        private const string JSON_FILE_NAME = "cdwords.json";
        private const string WORD_BACKUP_PATH = "Backup/words_backup.txt";

        private static readonly string CURRENT_APP_FOLDER = FileSystem.Current.AppDataDirectory;
        private static readonly string FOLDER_PATH = Path.Combine(CURRENT_APP_FOLDER, FOLDER_NAME);
        private static readonly string TXT_FILE_PATH = Path.Combine(FOLDER_PATH, TXT_FILE_NAME);
        private static readonly string JSON_FILE_PATH = Path.Combine(FOLDER_PATH, JSON_FILE_NAME);

        public bool IsDownloading { get; private set; }

        public async Task InitializeAsync()
        {
            IsDownloading = true;

            if (!Directory.Exists(FOLDER_PATH))
            {
                Directory.CreateDirectory(FOLDER_PATH);
            }

            if (!File.Exists(TXT_FILE_PATH))
            {
               await DownloadDictionaryAsync();
            }

            IsDownloading = false;
        }

        public Dictionary<char, HashSet<string>>? GetDictionaryJson()
        {
            if (!File.Exists(JSON_FILE_PATH))
            {
                JSONParse();
            }

            return JSONLoad();
        }

        public Dictionary<char, HashSet<string>> GetDictionaryTxt()
        {
            List<string> words = File.ReadAllLines(TXT_FILE_PATH)
                                     .Where(line => !string.IsNullOrWhiteSpace(line)) 
                                     .Select(word => word.Trim())
                                     .ToList();

            var wordDictionary = words.GroupBy(word => word.ToLower().First())
                                      .ToDictionary(group => group.Key, 
                                                    group => new HashSet<string>(group, StringComparer.OrdinalIgnoreCase));

            return wordDictionary;
        }

        private void JSONParse()
        {
            var dictionary = GetDictionaryTxt();
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(dictionary, jsonOptions);
            File.WriteAllText(JSON_FILE_PATH, jsonString);
        }

        private async Task DownloadDictionaryAsync()
        {
            string content;
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (accessType == NetworkAccess.Internet)
            {
                using HttpClient client = new();
                content = await client.GetStringAsync(NET_FILE_PATH);
            }
            else
            {
                using Stream inputStream = await FileSystem.Current.OpenAppPackageFileAsync(WORD_BACKUP_PATH);
                using StreamReader reader = new(inputStream);
                content = await reader.ReadToEndAsync();
            }
           
            await File.WriteAllTextAsync(TXT_FILE_PATH, content);
        }

        private Dictionary<char, HashSet<string>>? JSONLoad()
        {
            string jsonData = File.ReadAllText(JSON_FILE_PATH);
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<Dictionary<char, HashSet<string>>>(jsonData, jsonOptions);
        }
    }
}
