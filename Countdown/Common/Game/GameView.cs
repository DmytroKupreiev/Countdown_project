using Countdown;
using Countdown.Common.GameData;
using System.Reflection.Metadata;


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

    public void StartRound()
    {
        ChangeGameButton("Start", "#8D99AE", false);
        ChangePlayer();
        Clear();
    }

    public void RoundLabelUpdate(int round)
    {
        _page.RoundLabel.Text = $"Round {round.ToString()}";
    }

    public void EndRoundButton()
    {
        ChangeGameButton("End round", "#DD6D51");
    }

    public void ChangeGameButton(string text, string colorHEX, bool isEnable = true)
    {
        EnableStartRoundButton(isEnable);
        var button = _page.StartRoundButton;
        button.Text = text;
        button.BackgroundColor = Color.FromRgba(colorHEX);
    }

    public void Restart()
    {
        UpdatePoints();
    }

    public void OnChangeTimer(int seconds)
    {
        _page.TimerLabel.Text = seconds.ToString();
    }

    public void ChangeLetter(char letter, int index)
    {   
        var label = _page.FindByName<Label>($"Letter_{index + 1}");
        label.Text = letter.ToString().ToUpper();

        

        var animation = new Animation
        {
            { 0, 0.2, new Animation(v => label.Scale = v, 1, 0.5, Easing.CubicOut) },
            { 0.2, 0.7, new Animation(v => label.Scale = v, 0.5, 1.2, Easing.CubicInOut) },
            { 0.7, 1, new Animation(v => label.Scale = v, 1.2, 1, Easing.SpringOut) }
        };


        animation.Commit(_page, "BounceAnimation", 16, 400);
    }  

    public void Clear()
    {
        for (int i = 0; i < 9; i++)
        {
            ChangeLetter(' ', i);
        }

        _page.FirstPlayerInput.Text = "";
        _page.SecondPlayerInput.Text = "";
        _page.TimerLabel.Text = _settings.RoundTime.ToString();
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
        Button vowel = _page.FirstPlayerVowel;
        Button consonant = _page.FirstPlayerConsonant;

        vowel.IsEnabled = enable;
        consonant.IsEnabled = enable;

        UpdateColor(vowel, consonant, enable);
    }
    public void EnableSecondPlayerButtons(bool enable)
    {
        Button vowel = _page.SecondPlayerVowel;
        Button consonant = _page.SecondPlayerConsonant;

        vowel.IsEnabled = enable;
        consonant.IsEnabled = enable;

        UpdateColor(vowel, consonant, enable);
    }

    private void UpdateColor(Button vowel, Button consonant, bool enable)
    {
        Color activeColor = Color.FromRgba("#EEE8BE");
        Color disableColor = Color.FromRgba("#8D99AE");

        if (enable)
        {
            vowel.BackgroundColor = activeColor;
            consonant.BackgroundColor = activeColor;
        }
        else
        {
            VisualStateManager.GoToState(vowel, "Normal");
            vowel.BackgroundColor = disableColor;
            VisualStateManager.GoToState(consonant, "Normal");
            consonant.BackgroundColor = disableColor;
        }
    }

    public void EnableStartRoundButton(bool enable)
    {   
        Button button = _page.StartRoundButton;

        if(!enable)
        {
            VisualStateManager.GoToState(button, "Normal");
        }

        button.IsEnabled = enable;
    }

    public async void ShowResult(string message)
    {
        UpdatePoints();
        await _page.DisplayAlert("Game result", message, "OK");
    }

    private void UpdatePoints()
    {
        _page.FirstPlayerPoints.Text = _settings.FirstPlayer.Points.ToString();
        _page.SecondPlayerPoints.Text = _settings.SecondPlayer.Points.ToString();
    }
}

