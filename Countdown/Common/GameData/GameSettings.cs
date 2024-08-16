namespace Countdown.Common.GameData
{
    public class GameSettings
    {
        public Player FirstPlayer { get; private set; }
        public Player SecondPlayer { get; private set; }

        private int _numberOfRounds;
        public int NumberOfRounds => _numberOfRounds;

        private int _roundTime;
        public int RoundTime => _roundTime;

        public Turn CurrentTurn { get; private set; }

        public GameSettings(string firstName, string secondName,
                            string numberOfRounds, string roundTime,
                            string firstTurn)
        {
            FirstPlayer = new Player(firstName);
            SecondPlayer = new Player(secondName);
            CurrentTurn = GetTurn(firstTurn);

            if (!int.TryParse(numberOfRounds, out _numberOfRounds))
            {
                _numberOfRounds = 6;
            }

            if (!int.TryParse(roundTime, out _roundTime))
            {
                _roundTime = 30;
            }
        }

        private Turn GetTurn(string turn) => turn switch
        {
            "First player" => Turn.FirstPlayer,
            "Second player" => Turn.SecondPlayer,
            "Random" => GetRandom(),
            _ => GetRandom()
        };

        private Turn GetRandom()
        {
            return new Random().Next(0, 2) == 0 ? Turn.FirstPlayer
                                                : Turn.SecondPlayer;
        }

        public void NextTurn()
        {
            CurrentTurn = CurrentTurn == Turn.FirstPlayer ? Turn.SecondPlayer
                                                          : Turn.FirstPlayer;
        }
    }
}
