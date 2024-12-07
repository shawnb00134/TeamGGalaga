using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Galaga.Model
{
    /// <summary>
    ///     High School Manager class.
    /// </summary>
    public static class HighScoreManager
    {
        #region Methods

        /// <summary>
        ///     High Score Manager class.
        /// </summary>
        /// <param name="highScoreListView"></param>
        /// <param name="sortBy"></param>
        public static void DisplayHighScores(ListView highScoreListView, string sortBy = "Sort by Score/Name/Level")
        {
            var highScores = Score.LoadHighScores();

            if (highScores.Count == 0)
            {
                highScoreListView.ItemsSource = new[] { "No scores yet!" };
                return;
            }

            switch (sortBy)
            {
                case "Sort by Score/Name/Level":
                    highScores = highScores
                        .OrderByDescending(s => s.PlayerScore)
                        .ThenBy(s => s.PlayerName)
                        .ThenByDescending(s => s.LevelCompleted)
                        .ToList();
                    break;
                case "Sort by Name/Score/Level":
                    highScores = highScores
                        .OrderBy(s => s.PlayerName)
                        .ThenByDescending(s => s.PlayerScore)
                        .ThenByDescending(s => s.LevelCompleted)
                        .ToList();
                    break;
                case "Sort by Level/Score/Name":
                    highScores = highScores
                        .OrderByDescending(s => s.LevelCompleted)
                        .ThenByDescending(s => s.PlayerScore)
                        .ThenBy(s => s.PlayerName)
                        .ToList();
                    break;
            }

            highScoreListView.ItemsSource =
                highScores.Select(s => $"{s.PlayerName} - {s.PlayerScore} - Level {s.LevelCompleted}");
        }

        #endregion
    }
}