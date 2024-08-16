using Countdown.Common.Dictionary;
using Countdown.Common.GameData;

namespace Countdown;

public partial class GamePage : ContentPage
{
    private GameModel _model;
    private GameView _view;

	public GamePage(GameSettings settings,
                    GameDictionary dictionary,
                    GameAlphabet alphabet)
	{
		InitializeComponent();

        _model = new GameModel(settings, dictionary, alphabet);
        _view = new GameView(this, settings);

        _view.OnGameEntered();
    }

    private async void OnStartRoundEvent(object sender, EventArgs e)
    {
        _view.ChangePlayer();
        
        string f = FirstPlayerInput.Text;
        string s = SecondPlayerInput.Text;

        string message = _model.EvaluateWinner(f, s);
        await DisplayAlert("Title", message, "OK");

        _model.UpdateAlphabet();
        _model.NextTurn();
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