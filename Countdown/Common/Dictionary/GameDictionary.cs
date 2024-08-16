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
            _dictionary = _dictLoader.GetDictionaryJson() ?? _dictLoader.GetDictionaryTxt();
        }
        
        public bool HasWord(string word)
        {
            return _dictionary[word.First()].Contains(word);
        }
    }
}
