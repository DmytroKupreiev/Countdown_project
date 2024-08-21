using Countdown.Common.GameData.History;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace Countdown.Common.GameHistory
{
    public class CollectionViewBuilder
    {
        private CollectionView _collectionView;
        private List<GameRecord> _source;

        public CollectionViewBuilder()
        {
            _collectionView = new CollectionView();
        }

        public CollectionView Build()
        {
            return _collectionView;
        }

        public bool IsEmpty()
        {
            return _source is null || _source.Count == 0;
        }

        public CollectionViewBuilder SetSource(List<GameRecord> source)
        {
            _source = source;
            _collectionView.ItemsSource = source;
            return this;
        }

        public CollectionViewBuilder SetDataTemplate()
        {
            _collectionView.ItemTemplate = new DataTemplate(() =>
            {
                var FirstPlayerName = new Label { FontSize = 20, Padding = 5 };
                FirstPlayerName.SetBinding(Label.TextProperty, new Binding { Path = "FirstPlayerName", StringFormat = "First Player: {0}" });

                var FirstPlayerPoints = new Label { FontSize = 20, Padding = 5 };
                FirstPlayerPoints.SetBinding(Label.TextProperty, new Binding { Path = "FirstPlayerPoints", StringFormat = "Points: {0}" });

                var SecondPlayerName = new Label { FontSize = 20, Padding = 5 };
                SecondPlayerName.SetBinding(Label.TextProperty, new Binding { Path = "SecondPlayerName", StringFormat = "Second player: {0}" });

                var SecondPlayerPoints = new Label { FontSize = 20, Padding = 5 };
                SecondPlayerPoints.SetBinding(Label.TextProperty, new Binding { Path = "SecondPlayerPoints", StringFormat = "Points: {0}" });

                var RoundCount = new Label { FontSize = 20, Padding = 5 };
                RoundCount.SetBinding(Label.TextProperty, new Binding { Path = "RoundCount", StringFormat = "Number of rounds: {0}" });

                var RoundTime = new Label { FontSize = 20, Padding = 5 };
                RoundTime.SetBinding(Label.TextProperty, new Binding { Path = "RoundTime", StringFormat = "Round time: {0}" });

                var EndGameDate = new Label { FontSize = 20, Padding = 5 };
                EndGameDate.SetBinding(Label.TextProperty, new Binding { Path = "EndGameDate ", StringFormat = "Date: {0}" });

                var EndGameTime = new Label { FontSize = 20, Padding = 5 };
                EndGameTime.SetBinding(Label.TextProperty, new Binding { Path = "EndGameTime", StringFormat = "Time: {0}" });

                var containerPlayer1 = new FlexLayout
                {
                    Direction = FlexDirection.Row,
                    JustifyContent = FlexJustify.SpaceEvenly,
                    Children = { FirstPlayerName, FirstPlayerPoints }
                };

                var containerPlayer2 = new FlexLayout
                {
                    Direction = FlexDirection.Row,
                    JustifyContent = FlexJustify.SpaceEvenly,
                    Children = { SecondPlayerName, SecondPlayerPoints }
                };

                var GameInfo = new FlexLayout
                {
                    Direction = FlexDirection.Row,
                    JustifyContent = FlexJustify.SpaceEvenly,
                    Children = { RoundCount, RoundTime }
                };

                var DateInfo = new FlexLayout
                {
                    Direction = FlexDirection.Row,
                    JustifyContent = FlexJustify.SpaceEvenly,
                    Children = { EndGameDate, EndGameTime }
                };

                var container = new StackLayout
                {
                    Children = { containerPlayer1, containerPlayer2, GameInfo, DateInfo },
                };

                var border = new Border
                {
                    Content = container,
                    Padding = 10,
                    Margin = new Thickness(10, 5, 10, 5),
                    BackgroundColor = Color.FromRgba("#F5F5F5"),
                    Stroke = Color.FromRgba("#F5F5F5"),
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(20) },

                };

                return new StackLayout
                {
                    Children = { border },
                    Margin = new Thickness(15, 10)
                };
            });

            return this;
        }
    }
}
