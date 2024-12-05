using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Galaga.Model
{
    /// <summary>
    /// Manages high score logic and score-related functionality.
    /// </summary>
    public class Score
    {
        private const string HighScoreFileName = "HighScores.json";

        public int CurrentScore { get; private set; }
        public List<HighScore> HighScores { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Score"/> class.
        /// Loads existing high scores from storage.
        /// </summary>
        public Score()
        {
            CurrentScore = 0;
            HighScores = LoadHighScores();
        }

        /// <summary>
        /// Increases the current score by the specified value.
        /// </summary>
        /// <param name="points">The number of points to add.</param>
        public void AddPoints(int points)
        {
            CurrentScore += points;
        }

        /// <summary>
        /// Resets the current score to zero.
        /// </summary>
        public void ResetScore()
        {
            CurrentScore = 0;
        }

        /// <summary>
        /// Updates the high score list with the current score if it qualifies.
        /// </summary>
        /// <param name="playerName">The player's name.</param>
        /// <param name="levelCompleted">The level completed by the player.</param>
        public void UpdateHighScores(string playerName, int levelCompleted)
        {
            HighScores.Add(new HighScore(playerName, CurrentScore, levelCompleted));
            HighScores = HighScores
                .OrderByDescending(h => h.Score)
                .ThenBy(h => h.PlayerName)
                .ThenByDescending(h => h.Level)
                .Take(10)
                .ToList();
            SaveHighScores();
        }

        /// <summary>
        /// Loads high scores from the storage file.
        /// </summary>
        /// <returns>A list of high scores.</returns>
        private List<HighScore> LoadHighScores()
        {
            try
            {
                if (File.Exists(HighScoreFileName))
                {
                    var json = File.ReadAllText(HighScoreFileName);
                    return JsonConvert.DeserializeObject<List<HighScore>>(json) ?? new List<HighScore>();
                }
            }
            catch (Exception)
            {
            }
            return new List<HighScore>();
        }

        /// <summary>
        /// Saves high scores to the storage file.
        /// </summary>
        private void SaveHighScores()
        {
            try
            {
                var json = JsonConvert.SerializeObject(HighScores);
                File.WriteAllText(HighScoreFileName, json);
            }
            catch (Exception)
            {
            }
        }
    }

    /// <summary>
    /// Represents a high score entry.
    /// </summary>
    public class HighScore
    {
        public string PlayerName { get; set; }
        public int Score { get; set; }
        public int Level { get; set; }

        public HighScore(string playerName, int score, int level)
        {
            PlayerName = playerName;
            Score = score;
            Level = level;
        }
    }
}
