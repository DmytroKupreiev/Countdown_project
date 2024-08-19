namespace Countdown.Common.GameData.History
{
    public class GameRecord
    {
        public string FirstPlayerName { get; set; } = "";
        public string SecondPlayerName { get; set; } = "";
        public int FirstPlayerPoints { get; set; }
        public int SecondPlayerPoints { get; set; }
        public DateTimeOffset EndGameTime { get; set; }
        public int RoundCount { get; set; }
        public int RoundTime { get; set; }
    }
}
