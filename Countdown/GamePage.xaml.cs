using Countdown.Common.Dictionary;
using Countdown.Common.GameData;

namespace Countdown;

public partial class GamePage : ContentPage
{
    private GameModel _model;
    private GameView _view;
    private CountdownTimer _timer;

	public GamePage(GameSettings settings,
                    GameDictionary dictionary,
                    GameAlphabet alphabet)
	{
		InitializeComponent();

        _model = new GameModel(settings, dictionary, alphabet);
        _view = new GameView(this, settings);
        _timer = new CountdownTimer(settings.RoundTime);

        _model.OnStartTimer += _timer.Start;
        _model.OnStartTimer += _view.DisableAllButtons;
        _timer.OnTimeChanged += _view.OnChangeTimer;
        _timer.OnTimerFinished += RoundResultEvaluate;

        _view.OnGameEntered();
    }

    private void RoundResultEvaluate()
    {   
        string firstWord = FirstPlayerInput.Text ?? "_";
        string secondWord = SecondPlayerInput.Text ?? "_";

        string message = _model.EvaluateWinner(firstWord, secondWord);
        _view.ShowResult(message);
        _view.EnableStartRoundButton(true);
    }

    private void OnStartRoundEvent(object sender, EventArgs e)
    {
        _view.StartRound();
        _model.StartRound();

        if (_model.IsLastRound())
        {
            _view.RestartButton();
        }

        if (_model.IsEndGame())
        {
            _model.Restart();
            _view.Restart();
        }

        if (_model.IsFirstRound())
        {
            _view.FirstRound();
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