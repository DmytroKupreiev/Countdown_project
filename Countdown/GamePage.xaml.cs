namespace Countdown;

public partial class GamePage : ContentPage
{
	public GamePage()
	{
		InitializeComponent();
	}

	private async void OnHomeButtonClicked(object sender, EventArgs e)
	{
        await Navigation.PopAsync();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ForceLayout();
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