using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage;

namespace Galaga.Model
{
    public class Score
    {
        private static readonly string ScoreFilePath =
            Path.Combine(ApplicationData.Current.LocalFolder.Path, "highscores.txt");
        private const int MaxScores = 10;

        public string PlayerName { get; set; }
        public int PlayerScore { get; set; }
        public int LevelCompleted { get; set; }

        public Score(string playerName, int playerScore, int levelCompleted)
        {
            PlayerName = playerName;
            PlayerScore = playerScore;
            LevelCompleted = levelCompleted;
        }

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
                    {
                        highScores.Add(new Score(parts[0], score, level));
                    }
                }
            }

            return highScores.OrderByDescending(s => s.PlayerScore)
                             .ThenBy(s => s.PlayerName)
                             .ThenByDescending(s => s.LevelCompleted)
                             .Take(MaxScores)
                             .ToList();
        }

        public static void SaveHighScores(List<Score> highScores)
        {
            var lines = highScores.Select(s => $"{s.PlayerName},{s.PlayerScore},{s.LevelCompleted}");
            File.WriteAllLines(ScoreFilePath, lines);
        }

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
    }
}
