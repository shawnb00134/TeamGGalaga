using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Galaga.View.Sprites;

namespace Galaga.Model
{
    /// <summary>
    ///     EnemyManager class.
    /// </summary>
    public class EnemyManager
    {
        #region Data members

        private bool movingRightBonusShip = true;

        /// <summary>
        ///     Checks the bounces the special ship has made
        /// </summary>
        public int BonusShipBounce { get; private set; }

        private EnemyShip bonusShip;

        private readonly Canvas canvas;

        private readonly ShipFactory shipFactory;

        private readonly Dictionary<int, int> rowDirections = new Dictionary<int, int>();
        private readonly Dictionary<int, int> rowSpeeds = new Dictionary<int, int>();
        private const int DefaultSpeedX = 3;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyManager" /> class.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        public EnemyManager(Canvas canvas)
        {
            this.canvas = canvas;
            this.shipFactory = new ShipFactory();

            this.BonusShipBounce = 4;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates and places the enemy ships.
        /// </summary>
        /// <returns></returns>
        public List<EnemyShip> CreateAndPlaceEnemyShip(int levelMultiplier)
        {
            var enemyShips = new List<EnemyShip>();

            const double rowSpacing = 100;
            var canvasWidth = this.canvas.Width;
            var startY = this.canvas.Height / 2;
            int[] enemiesPerRow = { 2, 3, 4, 5 };

            for (var row = 0; row < enemiesPerRow.Length; row++)
            {
                this.rowDirections[row] = 1;
                this.rowSpeeds[row] = DefaultSpeedX * levelMultiplier;
            }

            if (levelMultiplier >= 2)
            {
                this.rowDirections[0] = 1;
                this.rowDirections[1] = 1;
                this.rowDirections[2] = -1;
                this.rowDirections[3] = -1;
            }

            if (levelMultiplier >= 3)
            {
                this.rowDirections[1] = -1;
                this.rowDirections[3] = -1;

                this.rowSpeeds[1] *= 2;
                this.rowSpeeds[3] *= 2;
            }

            for (var rowIndex = 0; rowIndex < enemiesPerRow.Length; rowIndex++)
            {
                var enemyCount = enemiesPerRow[rowIndex];
                var spacing = canvasWidth / (enemyCount + 1);

                for (var i = 0; i < enemyCount; i++)
                {
                    var enemyShip = this.shipFactory.CreateEnemyShip(rowIndex, levelMultiplier, this.canvas);
                    enemyShip.AddEnemyToCanvas();
                    enemyShips.Add(enemyShip);

                    var xPosition = (i + 1) * spacing - enemyShip.Width / 2.0;
                    enemyShip.X = xPosition;
                    enemyShip.Y = startY - rowIndex * rowSpacing;
                }
            }

            return enemyShips;
        }

        /// <summary>
        ///     Creates the special/bonus enemy ship
        /// </summary>
        /// <returns></returns>
        public EnemyShip CreateSpecialShip()
        {
            this.bonusShip = this.shipFactory.CreateSpecialShip(this.canvas);
            this.bonusShip.AddBonusShipToCanvas();
            return this.bonusShip;
        }

        /// <summary>
        ///     Moves the enemy ships.
        /// </summary>
        /// <param name="enemyShips">The enemy ships.</param>
        /// <param name="tickCounter">The tick counter.</param>
        public void MoveEnemyShips(IList<EnemyShip> enemyShips, int tickCounter)
        {
            var shouldChangeDirection = false;

            for (var rowIndex = 0; rowIndex < this.rowDirections.Count; rowIndex++)
            {
                var direction = this.rowDirections[rowIndex];
                var speed = this.rowSpeeds[rowIndex];

                foreach (var ship in enemyShips)
                {
                    if (ship.Sprite != null && ship.Sprite.GetType() != typeof(EnemySpecialSprite) &&
                        ship.Y == this.canvas.Height / 2 - rowIndex * 100)
                    {
                        ship.X += direction * speed;

                        if ((direction == 1 && ship.X + ship.Width >= this.canvas.Width) ||
                            (direction == -1 && ship.X <= 0))
                        {
                            shouldChangeDirection = true;
                        }
                    }
                }
            }

            if (shouldChangeDirection)
            {
                for (var rowIndex = 0; rowIndex < this.rowDirections.Count; rowIndex++)
                {
                    this.rowDirections[rowIndex] *= -1;
                }
            }
        }

        /// <summary>
        ///     Moves an individual ship based on direction and speed.
        /// </summary>
        /// <summary>
        ///     Moves an individual ship based on direction and speed.
        /// </summary>
        private void moveShip(EnemyShip ship, bool moveRight, int speedMultiplier = 1)
        {
            if (moveRight)
            {
                ship.X += ship.SpeedX * speedMultiplier;
            }
            else
            {
                ship.X -= ship.SpeedX * speedMultiplier;
            }

            Canvas.SetLeft(ship.Sprite, ship.X);
        }

        /// <summary>
        ///     Determines the row index of a ship based on its Y position.
        /// </summary>
        private int getRowIndex(double yPosition)
        {
            const double rowSpacing = 100;
            var startY = this.canvas.Height / 2;
            return (int)((startY - yPosition) / rowSpacing);
        }

        /// <summary>
        ///     Moves the bonus ship.
        /// </summary>
        public void MoveBonusShip()
        {
            this.CheckBounceCounter();

            this.checkBonusShipPosition();

            if (this.bonusShip != null)
            {
                if (this.movingRightBonusShip)
                {
                    this.bonusShip.MoveRight();

                    if (this.bonusShip.X + this.bonusShip.Width >= this.canvas.Width)
                    {
                        this.movingRightBonusShip = false;
                    }
                }
                else
                {
                    this.bonusShip.MoveLeft();

                    if (this.bonusShip.X <= 0)
                    {
                        this.movingRightBonusShip = true;
                    }
                }
            }
        }

        /// <summary>
        ///     Lets the GameManager know when to remove the bonus ship from play.
        /// </summary>
        /// <returns></returns>
        public bool CheckBounceCounter()
        {
            if (this.BonusShipBounce == 0)
            {
                this.resetBonusShipBoundCounter();
                return true;
            }

            return false;
        }

        private void resetBonusShipBoundCounter()
        {
            this.BonusShipBounce = 4;
        }

        private void checkBonusShipPosition()
        {
            if (this.bonusShip != null)
            {
                if (this.bonusShip.X == 0)
                {
                    this.BonusShipBounce--;
                    this.movingRightBonusShip = true;
                }

                if (Convert.ToInt32(this.bonusShip.X) == Convert.ToInt32(this.canvas.Width - this.bonusShip.Width))
                {
                    this.BonusShipBounce--;
                    this.movingRightBonusShip = false;
                }
            }
        }

        /// <summary>
        ///     Swaps the Sprites every tick.
        /// </summary>
        public void SwapSpritesAnimation(IList<EnemyShip> enemyShips)
        {
            foreach (var enemyShip in enemyShips)
            {
                if (enemyShip.Sprite.GetType() != typeof(EnemySpecialSprite))
                {
                    enemyShip.SwapSprites();
                }
            }
        }

        /// <summary>
        ///     Plays the explosion.
        /// </summary>
        /// <param name="ship">The ship.</param>
        public void PlayExplosion(GameObject ship)
        {
            var explosion = new Explosion(ship, this.canvas);
            _ = explosion.PlayExplosion();
        }

        #endregion
    }
}