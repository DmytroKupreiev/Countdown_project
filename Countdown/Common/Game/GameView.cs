using Countdown;
using Countdown.Common.GameData;

public class GameView
{
    private GamePage _page;
    private GameSettings _settings;

    public GameView(GamePage page, 
                    GameSettings settings)
    {
        _page = page;
        _settings = settings;
    }

    public void OnGameEntered()
    {   
        _page.FirstPlayerName.Text = _settings.FirstPlayer.Name;
        _page.SecondPlayerName.Text = _settings.SecondPlayer.Name;
    }

    public void OnChangeTimer(int seconds)
    {
        _page.TimerLabel.Text = seconds.ToString();
    }

    public void ChangeLetter(char letter, int index)
    {
        _page.FindByName<Label>($"Letter_{index + 1}").Text = letter.ToString()
                                                                    .ToUpper();
    }
    public void CleareLetters()
    {
        for (int i = 0; i < 9; i++)
        {
            ChangeLetter(' ', i);
        }
    }

    public void ChangePlayer()
    {
        if (_settings.CurrentTurn == Turn.FirstPlayer)
        {
            EnableFirstPlayerButtons(true);
            EnableFirstPlayerButtons(false);
        }
        else
        {
            EnableFirstPlayerButtons(false);
            EnableSecondPlayerButtons(true);
        }
    }

    public void DisableAllButtons()
    {
        EnableFirstPlayerButtons(false);
        EnableSecondPlayerButtons(false);
        EnableStartRoundButton(false);
    }

    private void EnableFirstPlayerButtons(bool enable)
    {
        _page.FirstPlayerVowel.IsEnabled = enable;
        _page.FirstPlayerConsonant.IsEnabled = enable;
    }

    private void EnableSecondPlayerButtons(bool enable)
    {
        _page.SecondPlayerVowel.IsEnabled = enable;
        _page.SecondPlayerConsonant.IsEnabled = enable;
    }

    private void EnableStartRoundButton(bool enable)
    {
        _page.StartRoundButton.IsEnabled = enable;
    }
}

