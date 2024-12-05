using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Galaga.View;
using Galaga.View.Sprites;

namespace Galaga.Model
{
    /// <summary>
    ///     Manages the Galaga game play.
    /// </summary>
    public class GameManager
    {
        #region Data members

        private const int TickTimer = 50;
        private const int TickCounterReset = 40;
        private const int LevelCap = 3;

        private readonly Canvas canvas;
        private readonly GameCanvas gameCanvas;
        private readonly PlayerManager playerManager;
        private readonly MissileManager missileManager;
        private readonly SoundManager soundManager;

        private readonly DispatcherTimer timer;
        private int tickCounter;

        private int score;
        private int currentLevel;

        private IList<EnemyShip> enemyShips;
        private readonly IList<GameObject> listOfShips;
        private readonly IList<GameObject> missiles;
        private readonly Physics physics;
        private readonly EnemyManager enemyManager;

        private readonly Score scoreManager;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameManager" /> class.
        /// </summary>
        public GameManager(Canvas canvas, GameCanvas gameCanvas)
        {
            this.canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
            this.gameCanvas = gameCanvas ?? throw new ArgumentNullException(nameof(gameCanvas));
            this.soundManager = new SoundManager();

            this.playerManager = new PlayerManager(this.canvas);
            this.missileManager = new MissileManager();
            this.enemyShips = new List<EnemyShip>();
            this.listOfShips = new List<GameObject>();
            this.missiles = new List<GameObject>();
            this.enemyManager = new EnemyManager(this.canvas);
            this.physics = new Physics();
            this.canvas = canvas;

            this.tickCounter = 0;
            this.score = 0;
            this.currentLevel = 1;

            this.timer = new DispatcherTimer();
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, TickTimer);
            this.timer.Tick += this.timer_Tick;

            this.scoreManager = new Score();

            this.initializeGame();
            this.timer.Start();
            this.updateScoreUI();
        }

        #endregion

        #region Methods

        private void timer_Tick(object sender, object e)
        {
            if (this.gameCanvas.IsMovingLeft())
            {
                this.playerManager.MovePlayerLeft();
            }

            if (this.gameCanvas.IsMovingRight())
            {
                this.playerManager.MovePlayerRight();
            }

            this.enemyManager.MoveEnemyShips(this.enemyShips, this.tickCounter);
            //this.enemyManager.MoveBonusShip(this.enemyShips);
            this.enemyFireMissiles();
            this.moveMissiles();
            this.tickCounter++;

            if (this.tickCounter >= TickCounterReset)
            {
                this.tickCounter = 0;
            }

            this.missileManager.UpdateDelayTick();
            this.enemyManager.SwapSpritesAnimation(this.enemyShips);
            this.checkForMissileOutOfBounds();
            this.checkForCollisions();
        }

        private void initializeGame()
        {
            this.playerManager.CreateAndPlacePlayer(this.listOfShips);
            this.initializeLevel();
        }

        private void initializeLevel()
        {
            this.updatePlayerLives();
            this.createEnemyShips();
        }

        private void createEnemyShips()
        {
            this.enemyShips = this.enemyManager.CreateAndPlaceEnemyShip(this.currentLevel);

            foreach (var enemyShip in this.enemyShips)
            {
                this.listOfShips.Add(enemyShip);
            }

            //TODO: Fix Special Ship
            //this.enemyShips.Add(this.enemyManager.CreateSpecialShip());
            //this.enemyManager.CreateSpecialShip();
        }

        /// <summary>
        ///     Moves the player left.
        /// </summary>
        public void MovePlayerLeft()
        {
            this.playerManager.MovePlayerLeft();
        }

        /// <summary>
        ///     Moves the player right.
        /// </summary>
        public void MovePlayerRight()
        {
            this.playerManager.MovePlayerRight();
        }

        /// <summary>
        ///     Fires the missile.
        /// </summary>
        public void FireMissile()
        {
            this.missiles.Add(this.missileManager.FireMissile(this.playerManager.GetPlayer(), this.canvas));
        }

        private void moveMissiles()
        {
            this.missileManager.MoveMissiles(this.missiles);
        }

        private void enemyFireMissiles()
        {
            this.missiles.Add(this.missileManager.FireEnemyMissiles(this.enemyShips, this.canvas));
        }

        private void checkForCollisions()
        {
            var objectsToRemove = this.physics.CheckCollisions(this.listOfShips, this.missiles);
            this.removeObjectsFromCanvas(objectsToRemove);
        }

        private void checkForMissileOutOfBounds()
        {
            var objectsToRemove = new List<GameObject>();

            foreach (var missile in this.missiles)
            {
                if (this.physics.CheckMissileBoundary(missile, this.canvas))
                {
                    objectsToRemove.Add(missile);
                }
            }

            this.removeObjectsFromCanvas(objectsToRemove);
        }

        private void removeObjectsFromCanvas(IList<GameObject> objectsToRemove)
        {
            foreach (var obj in objectsToRemove)
            {
                switch (obj)
                {
                    case Player _:
                        this.playerManager.CheckPlayerLives(obj, this.listOfShips);
                        this.updatePlayerLives();
                        this.soundManager.playPlayerDestroyed();
                        break;
                    case EnemyShip enemyShip:
                        //this.updateScore(enemyShip.ScoreValue);
                        //this.enemyShips.Remove(enemyShip);
                        this.removeEnemyShip(enemyShip);
                        //this.listOfShips.Remove(enemyShip);
                        break;
                    case Missile _:
                        this.missileManager.checkForPlayerMissile(obj);
                        this.missiles.Remove(obj);
                        this.canvas.Children.Remove(obj.Sprite);
                        break;
                }

                this.checkForEndGame();
            }
        }

        private void removeEnemyShip(EnemyShip enemyShip)
        {
            if (enemyShip.Equals(typeof(EnemySpecialSprite)))
            {
                this.playerManager.AddPlayerLife();
            }

            this.enemyShips.Remove(enemyShip);
            this.listOfShips.Remove(enemyShip);
            this.canvas.Children.Remove(enemyShip.Sprite);
            this.enemyManager.playExplosion(enemyShip);
            this.soundManager.playEnemyDestroyed();

            this.scoreManager.AddPoints(enemyShip.ScoreValue);
            this.updateScoreUI();
        }

        // Keeping this here temporarily just in case.

        //private void updateScore(int scoreValue)
        //{
        //    this.score += scoreValue;
        //    this.gameCanvas.updateScoreBoard("Score: " + this.score);
        //}

        private void updateScoreUI()
        {
            this.gameCanvas.updateScoreBoard("Score: " + this.scoreManager.CurrentScore);
        }

        private void checkForEndGame()
        {
            if (!this.listOfShips.Any(ship => ship is Player))
            {
                this.timer.Stop();
                this.gameCanvas.DisplayYouLoseText();
                this.PromptForPlayerName();
            }

            if (!this.enemyShips.Any())
            {
                if (this.checkForLevel())
                {
                    this.currentLevel++;
                    this.initializeLevel();
                }
                else
                {
                    this.timer.Stop();
                    this.gameCanvas.DisplayYouWinText();
                    this.PromptForPlayerName();
                }
            }
        }

        private bool checkForLevel()
        {
            return this.currentLevel < LevelCap;
        }

        private void updatePlayerLives()
        {
            this.gameCanvas.updatePlayerLivesBoard("Lives: " + this.playerManager.GetPlayerLivesCount());
        }

        private void PromptForPlayerName()
        {
            TextBox playerNameInput = new TextBox
            {
                PlaceholderText = "Enter your name",
                Width = 200
            };
            Button submitButton = new Button
            {
                Content = "Submit"
            };

            submitButton.Click += (sender, args) =>
            {
                if (!string.IsNullOrWhiteSpace(playerNameInput.Text))
                {
                    this.scoreManager.UpdateHighScores(playerNameInput.Text, this.currentLevel);
                }
            };

            Canvas.SetLeft(playerNameInput, 500);
            Canvas.SetTop(playerNameInput, 300);
            canvas.Children.Add(playerNameInput);

            Canvas.SetLeft(submitButton, 500);
            Canvas.SetTop(submitButton, 350);
            canvas.Children.Add(submitButton);
        }

        #endregion
    }
}