using System.Net;

namespace Countdown.Common.Dictionary
{
    public class GameDictionary
    {
        private DictionaryLoader _dictLoader;
        private Dictionary<char, HashSet<string>> _dictionary;

        public GameDictionary()
        {
            _dictLoader = new DictionaryLoader();
        }
        
        public async Task LoadDictionary()
        {
            await _dictLoader.InitializeAsync();
            _dictionary = _dictLoader.GetDictionaryJson() ?? _dictLoader.GetDictionaryTxt();
        }

        public bool HasWord(string word)
        {
            if (word.Length == 0) return false;
            if (!_dictionary.ContainsKey(word.First())) return false;

            return _dictionary[word.First()].Contains(word);
        }
    }
}
