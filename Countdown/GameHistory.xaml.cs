using Countdown.Common.GameData.History;
using Countdown.Common.GameHistory;

namespace Countdown;

public partial class GameHistory : ContentPage
{
	private GameHistoryRepository _gameHistory;
	private CollectionViewBuilder _collectionViewBuilder;
    private Border _container;

	public GameHistory()
	{
		InitializeComponent();
        _container = CollectionViewContainer;
		_gameHistory = new GameHistoryRepository();
        _collectionViewBuilder = new CollectionViewBuilder();

        VisualiseHistory();
    }

    private async void OnButtonDeleteEvent(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Clean history?", "After deleting, all history will disappear.", "Yes", "No");

        if (answer)
        {
            _gameHistory.Clean();
            VisualiseHistory();
        }

    }

    private void VisualiseHistory()
    {
        if (!_gameHistory.IsEmptyHistory())
        {
            _container.Content = _collectionViewBuilder.SetSource(_gameHistory.GetHistory())
                                                       .SetDataTemplate()
                                                       .Build();
        }
        else
        {
            _container.Content = new Label { FontSize = 25, 
                                             Text = "History empty.", 
                                             HorizontalOptions = LayoutOptions.Center };
        }
    }
}