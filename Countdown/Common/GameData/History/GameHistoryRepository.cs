using System.Text.Json;

namespace Countdown.Common.GameData.History
{
    public class GameHistoryRepository
    {
        private const string FOLDER_NAME = "Resources/History";
        private const string JSON_FILE_NAME = "history.json";

        private static readonly string CURRENT_APP_FOLDER = FileSystem.Current.AppDataDirectory;
        private static readonly string FOLDER_PATH = Path.Combine(CURRENT_APP_FOLDER, FOLDER_NAME);
        private static readonly string JSON_FILE_PATH = Path.Combine(FOLDER_PATH, JSON_FILE_NAME);

        private List<GameRecord> _history;

        public GameHistoryRepository()
        {
            TryDeserializeJSON();
        }

        public List<GameRecord> GetHistory()
        {
            return _history;
        }

        public bool IsEmptyHistory()
        {
            return _history is null || _history.Count == 0; 
        }

        public void AddRecord(GameRecord record)
        {
            _history.Add(record);
            SerializeJSON();
        }

        public void Clean()
        {
            _history = new List<GameRecord>();
            SerializeJSON();
        }

        private void TryDeserializeJSON()
        {
            if (!Directory.Exists(FOLDER_PATH))
            {
                Directory.CreateDirectory(FOLDER_PATH);
            }

            if (!File.Exists(JSON_FILE_PATH))
            {
                Clean();
                return;
            }

            string jsonString = File.ReadAllText(JSON_FILE_PATH);
            var jsonDoc = JsonDocument.Parse(jsonString);

            if (jsonDoc.RootElement.TryGetProperty("records", out var recordsElement))
            {
                _history = JsonSerializer.Deserialize<List<GameRecord>>(recordsElement.GetRawText()) 
                                                                           ?? new List<GameRecord>();
            }
            else
            {
                _history = new List<GameRecord>();
            }
        }

        private void SerializeJSON()
        {
            var updatedData = new { records = _history };
            var options = new JsonSerializerOptions { WriteIndented = true };

            string updatedJsonString = JsonSerializer.Serialize(updatedData, options);
            File.WriteAllText(JSON_FILE_PATH, updatedJsonString);
        }
    }
}
