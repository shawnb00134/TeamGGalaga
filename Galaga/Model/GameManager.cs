using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Galaga.View;
using Galaga.View.Sprites;
using System.Threading.Tasks;

namespace Galaga.Model
{
    /// <summary>
    ///     Manages the Galaga game play.
    /// </summary>
    public class GameManager
    {
        #region Data members

        private const int TickTimer = 50;
        //private const int TickCounterReset = 40;
        private const int LevelCap = 3;

        private readonly Canvas canvas;
        private readonly GameCanvas gameCanvas;
        private readonly PlayerManager playerManager;
        private readonly MissileManager missileManager;
        private readonly SoundManager soundManager;
        private Random random;

        private readonly DispatcherTimer timer;
        private int tickCounter;

        private int score;
        private int currentLevel;
        private bool specialShipHasSpawned;

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
            this.random = new Random();
            this.canvas = canvas;

            this.tickCounter = 0;
            this.score = 0;
            this.currentLevel = 1;
            this.specialShipHasSpawned = false;

            this.timer = new DispatcherTimer();
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, TickTimer);
            this.timer.Tick += this.timer_Tick;

            this.scoreManager = new Score();

            this.initializeGame();
            this.timer.Start();
            this.updateScoreUi();
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
            this.enemyManager.MoveBonusShip();
            this.enemyFireMissiles();
            this.moveMissiles();
            this.tickCounter++;

            this.missileManager.UpdateDelayTick();
            this.enemyManager.SwapSpritesAnimation(this.enemyShips);
            this.checkForMissileOutOfBounds();
            this.checkForCollisions();

            if (this.getRandomNumber() % 101 == 1 && this.specialShipHasSpawned == false)
            {
                this.enemyShips.Add(this.enemyManager.CreateSpecialShip());
                this.listOfShips.Add(this.enemyShips.Last());
                this.soundManager.playBonusShipCreation();
                this.specialShipHasSpawned = true;
            }

            this.checkWhenToRemoveSpecialShip();
        }

        private int getRandomNumber()
        {
            return this.random.Next(0, 1000001);
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
            this.missileManager.MoveMissiles(this.missiles, this.canvas);
        }

        private void enemyFireMissiles()
        {
            this.missiles.Add(this.missileManager.FireEnemyMissiles(this.enemyShips, this.playerManager.GetPlayer(), this.canvas));
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
                    case Missile _:
                        this.missileManager.checkForPlayerMissile(obj);
                        this.missiles.Remove(obj);
                        this.canvas.Children.Remove(obj.Sprite);
                        break;
                    case Player _:
                        this.destroyPlayerLife(obj);
                        break;
                    case EnemyShip enemyShip:
                        this.removeEnemyShip(enemyShip);
                        break;
                }

                this.checkForEndGame();
            }
        }

        private void checkWhenToRemoveSpecialShip()
        {
            if (this.enemyManager.checkBounceCounter())
            {
                foreach (var enemyShip in this.enemyShips)
                {
                    if (enemyShip.Sprite is EnemySpecialSprite)
                    {
                        this.listOfShips.Remove(enemyShip);
                        this.enemyShips.Remove(enemyShip);
                        this.canvas.Children.Remove(enemyShip.Sprite);
                        break;
                    }
                }
            }
        }

        private void destroyPlayerLife(GameObject ship)
        {
            this.playerManager.CheckPlayerLives(ship, this.listOfShips);
            this.enemyManager.playExplosion(ship);
            this.missileManager.ResetPlayerLimits();
            this.updatePlayerLives();
            this.soundManager.playPlayerDestroyed();
        }

        private void removeEnemyShip(EnemyShip enemyShip)
        {
            if (enemyShip.Sprite is EnemySpecialSprite)
            {
                this.destroySpecialEnemy();
                this.updatePlayerLives();
            }

            this.enemyShips.Remove(enemyShip);
            this.listOfShips.Remove(enemyShip);
            this.canvas.Children.Remove(enemyShip.Sprite);
            this.enemyManager.playExplosion(enemyShip);
            this.soundManager.playEnemyDestroyed();

            this.scoreManager.AddPoints(enemyShip.ScoreValue);
            this.updateScoreUi();
        }

        private void destroySpecialEnemy()
        {
            if (this.currentLevel == 1)
            {
                this.playerManager.AddPlayerLife();
            }

            if (this.currentLevel == 2)
            {
                this.playerManager.AddPlayerLife();
                this.missileManager.PowerUpPlayer();
            }
            if (this.currentLevel == 3)
            {
                this.missileManager.EnableNuke();
            }
        }

        // Keeping this here temporarily just in case.

        //private void updateScore(int scoreValue)
        //{
        //    this.score += scoreValue;
        //    this.gameCanvas.updateScoreBoard("Score: " + this.score);
        //}

        private void updateScoreUi()
        {
            this.gameCanvas.updateScoreBoard("Score: " + this.scoreManager.CurrentScore);
        }

        private void checkForEndGame()
        {
            if (!this.listOfShips.Any(ship => ship is Player))
            {
                this.timer.Stop();
                this.gameCanvas.DisplayYouLoseText();
                this.promptForPlayerName();
            }

            if (!this.enemyShips.Any())
            {
                if (this.checkForLevel())
                {
                    this.currentLevel++;
                    this.specialShipHasSpawned = false;
                    this.initializeLevel();
                }
                else
                {
                    this.timer.Stop();
                    this.gameCanvas.DisplayYouWinText();
                    this.promptForPlayerName();
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

        private void promptForPlayerName()
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
            this.canvas.Children.Add(playerNameInput);

            Canvas.SetLeft(submitButton, 500);
            Canvas.SetTop(submitButton, 350);
            this.canvas.Children.Add(submitButton);
        }

        /// <summary>
        ///    Fires the Nuke for the Wow Factor
        /// </summary>
        public void FireNuke()
        {
            if (this.currentLevel == LevelCap && this.missileManager.NukeEnabled)
            {
                this.missiles.Add(this.missileManager.FireNuke());

            }
        }

        #endregion
    }
}