namespace Countdown.Common.GameData
{
    public class GameAlphabet
    {
        private List<char> _vowels;
        private List<char> _consonants;

        private string _allLetters = "";
        private int _indexVowel;
        private int _indexConsonants;

        public GameAlphabet()
        {
            int countVowels = 67;
            int countConsonants = 74;

            _vowels = new List<char>(countVowels);
            _consonants = new List<char>(countConsonants);

            InitAlphabet();
            Update();
        }

        private void InitAlphabet()
        {
            List<(char, int)> vowels = new List<(char, int)>
            {
               ('a', 15),
               ('e', 21),
               ('i', 13),
               ('o', 13),
               ('u', 5)
            };

            List<(char, int)> consonants = new List<(char, int)>
            {
               ('b', 2), ('c', 3),
               ('d', 6), ('f', 2),
               ('g', 3), ('h', 2),
               ('j', 1), ('k', 1),
               ('l', 5), ('m', 4),
               ('n', 8), ('p', 4),
               ('q', 1), ('r', 9),
               ('s', 9), ('t', 9),
               ('v', 1), ('w', 1),
               ('x', 1), ('y', 1), ('z', 1),
            };

            Fill(vowels, _vowels);
            Fill(consonants, _consonants);
        }

        private void Fill(List<(char, int)> source, List<char> container)
        {
            foreach (var (letter, count) in source)
            {
                for (int i = 0; i < count; i++)
                {
                    container.Add(letter);
                }
            }
        }

        private List<T> Shuffle<T>(List<T> collection)
        {
            return collection.OrderBy(x => Guid.NewGuid()).ToList();
        }

        public void Update()
        {
            _vowels = Shuffle(_vowels);
            _consonants = Shuffle(_consonants);

            _indexVowel = 0;
            _indexConsonants = 0;
            _allLetters = "";
        }

        public char NextVowel()
        {
            _allLetters += _vowels[_indexVowel];
            return _vowels[_indexVowel++];
        }

        public char NextConsonant()
        {
            _allLetters += _consonants[_indexConsonants];
            return _consonants[_indexConsonants++];
        }

        public bool IsValidLetters(string word)
        {
            string availableLetters = _allLetters;

            foreach (char letter in word)
            {
                int index = availableLetters.IndexOf(letter);

                if (index == -1)
                {
                    return false;
                }

                availableLetters = availableLetters.Remove(index, 1);
            }

            return true;
        }
    }
}