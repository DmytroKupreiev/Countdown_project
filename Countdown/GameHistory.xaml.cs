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

        _container.Content = _collectionViewBuilder.SetSource(_gameHistory.GetHistory())
												   .SetDataTemplate()
												   .Build();
    }
}