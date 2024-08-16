namespace Countdown.Common.GameData
{
    public class Player
    {
        public string Name { get; private set; }
        public int Points { get; private set; }

        public Player(string name)
        {
            Name = name;
            Points = 0;
        }

        public void UpdatePoints(string word)
        {
            Points += word.Length;
        }
    }
}
