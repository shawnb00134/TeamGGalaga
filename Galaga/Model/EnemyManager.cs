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

        private bool movingRight = true;
        private bool movingRightBonusShip = true;
        
        /// <summary>
        ///     Checks the bounces the special ship has made
        /// </summary>
        public int bonusShipBounce { get; private set; }

        private EnemyShip bonusShip;

        private readonly Canvas canvas;

        private readonly ShipFactory shipFactory;

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

            this.bonusShipBounce = 4;
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

            for (var rowIndex = 0; rowIndex < enemiesPerRow.Length; rowIndex++)
            {
                var enemyCount = enemiesPerRow[rowIndex];
                var spacing = canvasWidth / (enemyCount + 1);

                EnemyShip enemyShip;
                for (var i = 0; i < enemyCount; i++)
                {
                    enemyShip = this.shipFactory.CreateEnemyShip(rowIndex, levelMultiplier, this.canvas);
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
            foreach (var ship in enemyShips)
            {
                if (ship.Sprite != null && ship.Sprite.GetType() != typeof(EnemySpecialSprite))
                {
                    if (this.movingRight)
                    {
                        ship.MoveRight();

                        if (ship.X + ship.Width >= this.canvas.Width)
                        {
                            this.movingRight = false;
                        }
                    }
                    else
                    {
                        ship.MoveLeft();

                        if (ship.X <= 0)
                        {
                            this.movingRight = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Moves the bonus ship.
        /// </summary>
        public void MoveBonusShip()
        {
            this.checkBounceCounter();

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
        public bool checkBounceCounter()
        {
            if (this.bonusShipBounce == 0)
            {
                this.resetBonusShipBoundCounter();
                return true;
            }
            return false;
        }

        private void resetBonusShipBoundCounter()
        {
            this.bonusShipBounce = 4;
        }

        private void checkBonusShipPosition()
        {
            if (this.bonusShip != null)
            {
                if (this.bonusShip.X == 0)
                {
                    this.bonusShipBounce--;
                    this.movingRightBonusShip = true;
                }

                if (Convert.ToInt32(this.bonusShip.X) == Convert.ToInt32(this.canvas.Width - this.bonusShip.Width))
                {
                    this.bonusShipBounce--;
                    this.movingRightBonusShip = false;
                }
            }
        }

        /// <summary>
        ///     Swaps the sprites every tick.
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
        public void playExplosion(GameObject ship)
        {
            var explosion = new Explosion(ship, this.canvas);
            _ = explosion.playExplosion();
        }

        #endregion
    }
}