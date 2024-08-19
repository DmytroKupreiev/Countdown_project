using Countdown.Common.Dictionary;
using Countdown.Common.GameData;
using Countdown.Common.GameData.History;

public class GameModel
{
    private GameSettings _settings;
    private GameDictionary _gameDictionary;
    private GameAlphabet _alphabet;
    private GameHistoryRepository _gameHistoryRepository;

    public event Action? OnStartTimer;

    private int _currentRound = 0;
    public int CurrentRound => _currentRound;

    private int _countLetter = 0;
    private int CountLetter
    {
        set
        {
            if (value >= 9)
            {
                OnStartTimer?.Invoke();
            }

            _countLetter = value;
        }

        get => _countLetter;
    }

    public GameModel(GameSettings settings, 
                     GameDictionary gameDictionary,
                     GameAlphabet alphabet)
    {
        _settings = settings;
        _gameDictionary = gameDictionary;
        _alphabet = alphabet;
        _gameHistoryRepository = new GameHistoryRepository();
    }

    public (char, int) GetLetter(string type)
    {
        return type switch
        {
            "vowel" => (_alphabet.NextVowel(), CountLetter++),
            "consonant" => (_alphabet.NextConsonant(), CountLetter++),
            _ => throw new ArgumentException()
        };
    }

    public bool IsMaxCountLetters()
    {
        return _countLetter >= 9;
    }

    public bool IsLastRound()
    {
        return _currentRound == _settings.NumberOfRounds;
    }

    public bool IsEndGame()
    {
        return _currentRound > _settings.NumberOfRounds;
    }

    public bool IsFirstRound()
    {
        return _currentRound == 1;
    }

    public void StartRound()
    {
        CountLetter = 0;
        _currentRound++;
        _alphabet.Update();
        _settings.NextTurn();
    }

    public void Restart()
    {
        _currentRound = 1;
        _settings.FirstPlayer.ResetPoints();
        _settings.SecondPlayer.ResetPoints();
    }

    public string EvaluateWinner(string firstPlayerWord, string secondPlayerWord)
    {
        firstPlayerWord = firstPlayerWord.Trim().ToLower() ?? "_";
        secondPlayerWord = secondPlayerWord.Trim().ToLower() ?? "_";

        bool isValidLetters_f = _alphabet.IsValidLetters(firstPlayerWord);
        bool isValidLetters_s = _alphabet.IsValidLetters(secondPlayerWord);

        bool wordFound_f = _gameDictionary.HasWord(firstPlayerWord);
        bool wordFound_s = _gameDictionary.HasWord(secondPlayerWord);

        bool isValidWordFirst = isValidLetters_f && wordFound_f;
        bool isValidWordSecond = isValidLetters_s && wordFound_s;

        int firstWordLength = firstPlayerWord.Length;
        int secondWordLength = secondPlayerWord.Length;

        string outputMessage;

        if ((firstWordLength == secondWordLength) && isValidWordFirst && isValidWordSecond)
        {
            _settings.FirstPlayer.UpdatePoints(firstPlayerWord);
            _settings.SecondPlayer.UpdatePoints(secondPlayerWord);

            outputMessage = $"{_settings.FirstPlayer.Name} and {_settings.SecondPlayer.Name} won";
        }
        else if ((firstWordLength >= secondWordLength) && isValidWordFirst)
        {
            _settings.FirstPlayer.UpdatePoints(firstPlayerWord);
            outputMessage = $"{_settings.FirstPlayer.Name} won";
        }
        else if ((firstWordLength <= secondWordLength) && isValidWordSecond)
        {
            _settings.SecondPlayer.UpdatePoints(secondPlayerWord);
            outputMessage = $"{_settings.SecondPlayer.Name} won";
        }
        else if ((firstWordLength > secondWordLength) && !isValidWordFirst && isValidWordSecond)
        {
            _settings.SecondPlayer.UpdatePoints(secondPlayerWord);
            outputMessage = $"{_settings.SecondPlayer.Name} won";
        }
        else if ((firstWordLength < secondWordLength) && isValidWordFirst && !isValidWordSecond)
        {
            _settings.FirstPlayer.UpdatePoints(firstPlayerWord);
            outputMessage = $"{_settings.FirstPlayer.Name} won";
        }
        else
        {
            outputMessage = "Nobody won!";
        }

        return outputMessage;
    }

    public void SaveResult()
    {
        _gameHistoryRepository.AddRecord(
                new GameRecord
                {
                    FirstPlayerName = _settings.FirstPlayer.Name,
                    SecondPlayerName = _settings.SecondPlayer.Name,
                    FirstPlayerPoints = _settings.FirstPlayer.Points,
                    SecondPlayerPoints = _settings.SecondPlayer.Points,
                    RoundTime = _settings.RoundTime,
                    RoundCount = _settings.NumberOfRounds,
                    EndGameDate = DateTimeOffset.Now.Date.ToShortDateString(),
                    EndGameTime = DateTime.Now.ToString("HH:mm:ss"),
                }
            );
    }
}