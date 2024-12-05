using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using Newtonsoft.Json;

namespace Galaga.Model
{
    /// <summary>
    /// Manages high score logic and score-related functionality.
    /// </summary>
    public class Score
    {
        private const string HighScoreFileName = "HighScores.json";

        /// <summary>
        ///     Gets the current score.
        /// </summary>
        /// <value>
        ///     The current score.
        /// </value>
        public int CurrentScore { get; private set; }

        /// <summary>
        ///     Gets the high scores.
        /// </summary>
        /// <value>
        ///     The high scores.
        /// </value>
        public List<HighScore> HighScores { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Score"/> class.
        /// Loads existing high scores from storage.
        /// </summary>
        public Score()
        {
            this.CurrentScore = 0;
            this.HighScores = this.loadHighScores();
        }

        /// <summary>
        /// Increases the current score by the specified value.
        /// </summary>
        /// <param name="points">The number of points to add.</param>
        public void AddPoints(int points)
        {
            this.CurrentScore += points;
        }

        /// <summary>
        /// Resets the current score to zero.
        /// </summary>
        public void ResetScore()
        {
            this.CurrentScore = 0;
        }

        /// <summary>
        /// Updates the high score list with the current score if it qualifies.
        /// </summary>
        /// <param name="playerName">The player's name.</param>
        /// <param name="levelCompleted">The level completed by the player.</param>
        public void UpdateHighScores(string playerName, int levelCompleted)
        {
            this.HighScores.Add(new HighScore(playerName, this.CurrentScore, levelCompleted));
            this.HighScores = this.HighScores
                .OrderByDescending(h => h.Score)
                .ThenBy(h => h.PlayerName)
                .ThenByDescending(h => h.Level)
                .Take(10)
                .ToList();
            this.saveHighScores();
        }

        /// <summary>
        /// Loads high scores from the storage file.
        /// </summary>
        /// <returns>A list of high scores.</returns>
        private List<HighScore> loadHighScores()
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
        private void saveHighScores()
        {
            try
            {
                var json = JsonConvert.SerializeObject(this.HighScores);
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
        /// <summary>
        ///     Gets or sets the name of the player.
        /// </summary>
        /// <value>
        ///     The name of the player.
        /// </value>
        public string PlayerName { get; set; }

        /// <summary>
        ///     Gets or sets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public int Score { get; set; }

        /// <summary>
        ///     Gets or sets the level.
        /// </summary>
        /// <value>
        ///     The level.
        /// </value>
        public int Level { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HighScore"/> class.
        /// </summary>
        /// <param name="playerName">Name of the player.</param>
        /// <param name="score">The score.</param>
        /// <param name="level">The level.</param>
        public HighScore(string playerName, int score, int level)
        {
            this.PlayerName = playerName;
            this.Score = score;
            this.Level = level;
        }
    }
}
