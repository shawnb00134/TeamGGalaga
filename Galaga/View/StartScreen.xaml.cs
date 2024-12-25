using Galaga.Model;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Galaga.View
{
    /// <summary>
    ///     Start Screen class.
    /// </summary>
    public sealed partial class StartScreen
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="StartScreen" /> class.
        /// </summary>
        public StartScreen()
        {
            this.InitializeComponent();
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GameCanvas));
        }

        private void ViewHighScoresButton_Click(object sender, RoutedEventArgs e)
        {
            var highScorePanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Width = 450,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var sortingOptions = new ComboBox
            {
                Width = 400,
                ItemsSource = new List<string>
                {
                    "Sort by Score/Name/Level",
                    "Sort by Name/Score/Level",
                    "Sort by Level/Score/Name"
                },
                SelectedIndex = 0
            };

            var highScoreListView = this.createHighScoreListView();

            sortingOptions.SelectionChanged += (s, args) =>
            {
                if (sortingOptions.SelectedItem is string sortBy)
                {
                    HighScoreManager.DisplayHighScores(highScoreListView, sortBy);
                }
            };

            highScorePanel.Children.Add(new TextBlock
            {
                Text = "High Scores",
                FontSize = 24,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            });

            highScorePanel.Children.Add(sortingOptions);
            highScorePanel.Children.Add(highScoreListView);

            var highScoresPage = new ContentDialog
            {
                Title = "Scoreboard",
                Content = highScorePanel,
                CloseButtonText = "Back to Start Screen"
            };
            _ = highScoresPage.ShowAsync();
        }

        private async void ResetHighScoresButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Reset High Scores",
                Content = "Are you sure you want to reset the high scores? This action cannot be undone.",
                PrimaryButtonText = "Reset",
                CloseButtonText = "Cancel"
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    Score.ResetHighScores();

                    var highScoreListView = this.createHighScoreListView();
                    highScoreListView.ItemsSource = new List<string> { "No scores yet!" };

                    await new ContentDialog
                    {
                        Title = "High Scores Reset",
                        Content = "High scores have been reset.",
                        CloseButtonText = "OK"
                    }.ShowAsync();
                }
                catch (Exception ex)
                {
                    await new ContentDialog
                    {
                        Title = "Error",
                        Content = $"An error occurred while resetting the high scores: {ex.Message}",
                        CloseButtonText = "OK"
                    }.ShowAsync();
                }
            }
        }

        private ListView createHighScoreListView()
        {
            var highScoreListView = new ListView
            {
                Width = 400,
                Height = 300
            };
            HighScoreManager.DisplayHighScores(highScoreListView);
            return highScoreListView;
        }

    }
}
