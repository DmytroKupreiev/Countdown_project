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

        await Navigation.PushAsync(new GamePage(gameSettings, 
                                                gameDictionary,
                                                new GameAlphabet())
                                   );
    }
}