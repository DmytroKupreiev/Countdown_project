using CommunityToolkit.Maui.Views;
using Countdown.Common.Dictionary;
using Countdown.Common.GameData;

namespace Countdown;

public partial class GamePage : ContentPage
{
    private GameModel _model;
    private GameView _view;
    private CountdownTimer _timer;
    private SoundController _soundController;

    public GamePage(GameSettings settings,
                    GameDictionary dictionary,
                    GameAlphabet alphabet)
	{
		InitializeComponent();

        _model = new GameModel(settings, dictionary, alphabet);
        _view = new GameView(this, settings);
        _timer = new CountdownTimer(settings.RoundTime);
        _soundController = new SoundController(GetAudioByTime(settings.RoundTime));

        _model.OnStartTimer += _timer.Start;
        _model.OnStartTimer += _view.DisableAllButtons;
        _model.OnStartTimer += _soundController.Start;

        _timer.OnTimeChanged += _view.OnChangeTimer;
        _timer.OnTimerStarted += _view.EndRoundButton;
        _timer.OnTimerFinished += RoundResultEvaluate;
        _timer.OnTimerFinished += _soundController.Stop;

        _view.OnGameEntered();
    }

    private void RoundResultEvaluate()
    {   
        string firstWord = FirstPlayerInput.Text ?? "_";
        string secondWord = SecondPlayerInput.Text ?? "_";

        string message = _model.EvaluateWinner(firstWord, secondWord);
        _view.ShowResult(message);

        if (_model.IsLastRound())
        {
            _view.ChangeGameButton("Restart", "#F3C15C");
            _model.SaveResult();
        }
        else
        {
            _view.ChangeGameButton("Start", "#04C4AE");
        }
    }

    private void OnStartRoundEvent(object sender, EventArgs e)
    {
        if (_timer.IsActive)
        {
            _timer.IsActive = false;
            return;
        }

        _view.StartRound();
        _model.StartRound();

        if (_model.IsEndGame())
        {
            _model.Restart();
            _view.Restart();
        }

        _view.RoundLabelUpdate(_model.CurrentRound);
    }

    private void OnVowelChoose(object sender, EventArgs e)
    {
        if (_model.IsMaxCountLetters()) return;

        (char Letter, int Index) letterInfo = _model.GetLetter("vowel");
        _view.ChangeLetter(letterInfo.Letter, letterInfo.Index);
    }

    private void OnConsonantChoose(object sender, EventArgs e)
    {
        if (_model.IsMaxCountLetters()) return;

        (char Letter, int Index) letterInfo = _model.GetLetter("consonant");
        _view.ChangeLetter(letterInfo.Letter, letterInfo.Index);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _view.EnableFirstPlayerButtons(false);
        _view.EnableSecondPlayerButtons(false);
        _view.ChangeGameButton("Start", "#04C4AE");
    }

    private MediaElement GetAudioByTime(int time)
    {
        return time switch
        {
            30 => Audio30,
            60 => Audio60,
            90 => Audio90,
            _ => Audio30
        };
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        double blocksDivider = 3.5;
        int countBlockLetter = 11;

        if (SquaresGrid != null)
        {
            double blockSize = width / countBlockLetter;
            SquaresGrid.HeightRequest = blockSize;

            foreach (var child in SquaresGrid.Children)
            {
                if (child is Frame frame)
                {
                    frame.WidthRequest = blockSize;
                    frame.HeightRequest = blockSize - countBlockLetter;
                }
            }
        }
        
        if (ControllerBlock != null) 
        {
            double blockSize = width / blocksDivider;
            ControllerBlock.HeightRequest = blockSize;

            foreach (var child in ControllerBlock.Children)
            {
                if (child is Frame frame)
                {
                    frame.WidthRequest = blockSize;
                    frame.HeightRequest = blockSize - blocksDivider;
                }
            }
        }
    }
}