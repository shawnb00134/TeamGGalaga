using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage;

namespace Galaga.Model
{
    /// <summary>
    ///    Score class.
    /// </summary>
    public class Score
    {
        private const int MaxScores = 10;

        private static readonly string ScoreFilePath =
            Path.Combine(ApplicationData.Current.LocalFolder.Path, "highscores.txt");

        /// <summary>
        ///    Initializes a new instance of the <see cref="Score" /> class.
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="playerScore"></param>
        /// <param name="levelCompleted"></param>
        public Score(string playerName, int playerScore, int levelCompleted)
        {
            PlayerName = playerName;
            PlayerScore = playerScore;
            LevelCompleted = levelCompleted;
        }

        /// <summary>
        ///     Gets or sets the player name.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        ///     Gets or sets the player score.
        /// </summary>
        public int PlayerScore { get; set; }

        /// <summary>
        ///     Level completed.
        /// </summary>
        public int LevelCompleted { get; set; }

        /// <summary>
        ///    Load high scores.
        /// </summary>
        /// <returns></returns>
        public static List<Score> LoadHighScores()
        {
            var highScores = new List<Score>();

            if (File.Exists(ScoreFilePath))
            {
                var lines = File.ReadAllLines(ScoreFilePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 3 &&
                        int.TryParse(parts[1], out var score) &&
                        int.TryParse(parts[2], out var level))
                        highScores.Add(new Score(parts[0], score, level));
                }
            }

            return highScores.OrderByDescending(s => s.PlayerScore)
                .ThenBy(s => s.PlayerName)
                .ThenByDescending(s => s.LevelCompleted)
                .Take(MaxScores)
                .ToList();
        }

        /// <summary>
        ///   Save high scores.
        /// </summary>
        /// <param name="highScores"></param>
        public static void SaveHighScores(List<Score> highScores)
        {
            var lines = highScores.Select(s => $"{s.PlayerName},{s.PlayerScore},{s.LevelCompleted}");
            File.WriteAllLines(ScoreFilePath, lines);
        }

        /// <summary>
        ///     Add new score.
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="playerScore"></param>
        /// <param name="levelCompleted"></param>
        public static void AddNewScore(string playerName, int playerScore, int levelCompleted)
        {
            var highScores = LoadHighScores();

            highScores.Add(new Score(playerName, playerScore, levelCompleted));

            highScores = highScores.OrderByDescending(s => s.PlayerScore)
                .ThenBy(s => s.PlayerName)
                .ThenByDescending(s => s.LevelCompleted)
                .Take(MaxScores)
                .ToList();

            SaveHighScores(highScores);
        }

        /// <summary>
        ///     Resets the high scores.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void ResetHighScores()
        {
            try
            {
                File.WriteAllText(ScoreFilePath, string.Empty);
            }
            catch (Exception ex)
            {
                throw new Exception("Error resetting high scores: " + ex.Message);
            }
        }
    }
}