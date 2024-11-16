using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Galaga.View;

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

        private readonly Canvas canvas;
        private readonly GameCanvas gameCanvas;
        private readonly PlayerManager playerManager;
        private readonly MissileManager missileManager;

        private readonly DispatcherTimer timer;
        private int tickCounter;

        private int score;

        private List<EnemyShip> enemyShips;
        private readonly List<GameObject> listOfShips;
        private readonly List<GameObject> missiles;
        private readonly Physics physics;
        private readonly EnemyManager enemyManager;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameManager" /> class.
        /// </summary>
        public GameManager(Canvas canvas, GameCanvas gameCanvas)
        {
            this.canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
            this.gameCanvas = gameCanvas ?? throw new ArgumentNullException(nameof(gameCanvas));

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

            this.timer = new DispatcherTimer();
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, TickTimer);
            this.timer.Tick += this.timer_Tick;

            this.initializeGame();
            this.timer.Start();
            this.updateScore(this.score);
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

            this.enemyFireMissiles();

            this.enemyManager.MoveEnemyShips(this.enemyShips, this.tickCounter);
            this.moveMissiles();
            this.tickCounter++;

            if (this.tickCounter >= TickCounterReset)
            {
                this.tickCounter = 0;
            }

            this.missileManager.UpdateDelayTick();
            this.enemyManager.swapSpritesAnimation(this.enemyShips);
            this.checkForMissileOutOfBounds();
            this.checkForCollisions();
        }

        private void initializeGame()
        {
            this.playerManager.CreateAndPlacePlayer(this.listOfShips);
            this.updatePlayerLives();
            this.createEnemyShips();
        }

        private void createEnemyShips()
        {
            this.enemyShips = this.enemyManager.CreateAndPlaceEnemyShip();

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
            //this.missiles.Add(this.missileManager.FireMissile(GameObject player, this.canvas));
            //this.missiles.Add(this.playerManager.FirePlayerMissile());
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

        private void removeObjectsFromCanvas(List<GameObject> objectsToRemove)
        {
            foreach (var obj in objectsToRemove)
            {
                switch (obj)
                {
                    case Player _:
                        this.playerManager.CheckPlayerLives(obj, this.listOfShips);
                        this.updatePlayerLives();
                        break;
                    case EnemyShip enemyShip:
                        this.updateScore(enemyShip.ScoreValue);
                        this.enemyShips.Remove(enemyShip);
                        this.listOfShips.Remove(enemyShip);
                        this.canvas.Children.Remove(obj.Sprite);
                        break;
                    case EnemyMissile _:
                        this.missiles.Remove(obj);
                        this.canvas.Children.Remove(obj.Sprite);
                        break;
                    case PlayerMissile _:
                        this.missiles.Remove(obj);
                        this.missileManager.DecrementPlayerMissileCount();
                        this.canvas.Children.Remove(obj.Sprite);
                        break;
                }

                this.checkForEndGame();
            }
        }

        private void updateScore(int scoreValue)
        {
            this.score += scoreValue;
            this.gameCanvas.updateScoreBoard("Score: " + this.score);
        }

        private void checkForEndGame()
        {
            //if (!this.listOfShips.Contains(Player))
            if (!this.listOfShips.Any(ship => ship is Player))
            {
                this.timer.Stop();
                this.gameCanvas.DisplayYouLoseText();
            }

            if (!this.enemyShips.Any())
            {
                this.timer.Stop();
                this.gameCanvas.DisplayYouWinText();
            }
        }

        private void updatePlayerLives()
        {
            this.gameCanvas.updatePlayerLivesBoard("Lives: " + this.playerManager.GetPlayerLivesCount());
        }

        #endregion
    }
}