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
            InitializeComponent();
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GameCanvas));
        }

        private void ViewHighScoresButton_Click(object sender, RoutedEventArgs e)
        {
            var highScoresPage = new ContentDialog
            {
                Title = "High Scores",
                Content = this.createHighScoreListView(),
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
