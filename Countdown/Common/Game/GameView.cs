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

    public void Clear()
    {
        for (int i = 0; i < 9; i++)
        {
            ChangeLetter(' ', i);
        }

        _page.FirstPlayerInput.Text = "";
        _page.SecondPlayerInput.Text = "";
    }

    public void ChangePlayer()
    {
        if (_settings.CurrentTurn == Turn.FirstPlayer)
        {
            EnableFirstPlayerButtons(true);
            EnableSecondPlayerButtons(false);
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

    public void EnableFirstPlayerButtons(bool enable)
    {
        _page.FirstPlayerVowel.IsEnabled = enable;
        _page.FirstPlayerConsonant.IsEnabled = enable;
    }

    public void EnableSecondPlayerButtons(bool enable)
    {
        _page.SecondPlayerVowel.IsEnabled = enable;
        _page.SecondPlayerConsonant.IsEnabled = enable;
    }

    public void EnableStartRoundButton(bool enable)
    {
        _page.StartRoundButton.IsEnabled = enable;
    }

    public async void ShowResult(string message)
    {
        _page.FirstPlayerPoints.Text = _settings.FirstPlayer.Points.ToString();
        _page.SecondPlayerPoints.Text = _settings.SecondPlayer.Points.ToString();

        await _page.DisplayAlert("Game result", message, "OK");
    }
}

