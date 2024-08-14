using System.Text.Json;

namespace Countdown.Common.Dictionary
{
    public class DictionaryLoader
    {
        private const string NET_FILE_PATH = "https://raw.githubusercontent.com/DonH-ITS/jsonfiles/main/cdwords.txt";
        private const string FOLDER_NAME = "Resources/Dictionary";
        private const string TXT_FILE_NAME = "cdwords.txt";
        private const string JSON_FILE_NAME = "cdwords.json";

        private static readonly string CURRENT_APP_FOLDER = FileSystem.Current.AppDataDirectory;
        private static readonly string FOLDER_PATH = Path.Combine(CURRENT_APP_FOLDER, FOLDER_NAME);
        private static readonly string TXT_FILE_PATH = Path.Combine(FOLDER_PATH, TXT_FILE_NAME);
        private static readonly string JSON_FILE_PATH = Path.Combine(FOLDER_PATH, JSON_FILE_NAME);

        public DictionaryLoader()
        {
            InitializeAsync().GetAwaiter().GetResult();
        }

        public async Task InitializeAsync()
        {
            if (!Directory.Exists(FOLDER_PATH))
            {
                Directory.CreateDirectory(FOLDER_PATH);
            }

            if (!File.Exists(TXT_FILE_PATH))
            {
               await DownloadDictionaryAsync();
            }
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
            using (HttpClient client = new())
            {
                string content = await client.GetStringAsync(NET_FILE_PATH);
                await File.WriteAllTextAsync(TXT_FILE_PATH, content);
            }
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
