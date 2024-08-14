namespace Countdown.Common
{
    public class GameAlphabet
    {
        private List<char> _vowels;
        private List<char> _consonants;

        private int indexVowel;
        private int indexConsonants;

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

        private void Shuffle<T>(List<T> collection)
        {
            collection = collection.OrderBy(x => Guid.NewGuid()).ToList();
        }

        public void Update()
        {
            Shuffle(_vowels);
            Shuffle(_consonants);

            indexVowel = 0;
            indexConsonants = 0;
        }

        public char NextVowel()
        {
            return _vowels[indexVowel++];
        }

        public char NextConsonant()
        {
            return _consonants[indexConsonants++];
        }
    }
}