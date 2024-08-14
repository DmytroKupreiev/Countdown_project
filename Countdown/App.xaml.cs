using Countdown.Common.Dictionary;
using Countdown.Common;
namespace Countdown
{
    public partial class App : Application
    {
        public static GameDictionary? GameDictionary { get; private set; }
        public static GameAlphabet? GameAlphabet { get; private set; }

        public App()
        {
            InitializeComponent();

            GameDictionary = new GameDictionary();
            GameAlphabet = new GameAlphabet();
            MainPage = new NavigationPage(new MainPage());

        }
    }
}
