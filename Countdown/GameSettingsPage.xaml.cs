using Countdown.Common.Dictionary;
using Countdown.Common.GameData;
namespace Countdown;

public partial class GameSettingsPage : ContentPage
{
	public GameSettingsPage()
	{
		InitializeComponent();
	}


    private async void OnPlayButtonClicked(object sender, EventArgs e)
	{
        EnableButton(false);
        EnableIndicator(true);
        await Task.Delay(1);

        string firstPlayer = FirstPlayer.Text;
        string secondPlayer = SecondPlayer.Text;
        string numberOfRounds = CountRounds.Text;
        string roundTime = (string)RoundTime.SelectedItem;
        string firstTurn = (string)FirstTurn.SelectedItem;

        GameSettings gameSettings = new GameSettings(firstPlayer, 
                                                     secondPlayer,
                                                     numberOfRounds, 
                                                     roundTime, firstTurn);

        GameDictionary gameDictionary = new GameDictionary();

        await gameDictionary.LoadDictionary();

        EnableIndicator(false);
        EnableButton(true);

        await Navigation.PushAsync(new GamePage(gameSettings, 
                                                gameDictionary,
                                                new GameAlphabet())
                                   );
    }

   

    private void EnableButton(bool enable)
    {
        ButtonStart.IsVisible = enable;
        ButtonStart.IsEnabled = enable;
    }

    private void EnableIndicator(bool flag)
    {
        Indicator.IsEnabled = flag;
        Indicator.IsVisible = flag;
        Indicator.IsRunning = flag;
        ContainerIndicator.IsEnabled = flag;
        ContainerIndicator.IsVisible = flag;
    }
}