namespace Countdown
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await this.FadeTo(1, 250);
        }

        private async void OnPlayButtonClicked(object sender, EventArgs e)
        {
            await this.FadeTo(0, 250);
            await Navigation.PushAsync(new GameSettingsPage());
        }

        private void OnSettingsButtonClicked(object sender, EventArgs e)
        {

        }
    }   
}
