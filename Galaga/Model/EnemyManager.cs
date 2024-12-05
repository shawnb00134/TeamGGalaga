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
        private int bonusShipBounce;

        private readonly Canvas canvas;
        private readonly Random random;

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
            this.random = new Random();
            this.shipFactory = new ShipFactory();

            this.bonusShipBounce = 3;
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
            var bonusEnemyShip = this.shipFactory.CreateSpecialShip(this.canvas, this.random.Next(0, 2));

            bonusEnemyShip.AddBonusShipToCanvas();

            return bonusEnemyShip;
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
        /// <param name="enemyShips">The enemy ships.</param>
        public void MoveBonusShip(IList<EnemyShip> enemyShips)
        {
            this.checkBoundCounter();

            EnemyShip bonusShip = null;

            foreach (var ship in enemyShips)
            {
                if (ship.Sprite != null && ship.Sprite.GetType() != typeof(EnemySpecialSprite))
                {
                    bonusShip = ship;
                }
            }

            this.checkBonusShipPosition(bonusShip);

            if (bonusShip != null)
            {
                if (this.bonusShipBounce == 3 || this.bonusShipBounce == 1)
                {
                    bonusShip.MoveRight();
                }
                else
                {
                    bonusShip.MoveLeft();
                }
            }
        }

        private void checkBoundCounter()
        {
            if (this.bonusShipBounce == 0)
            {
                this.resetBonusShipBoundCounter();
            }
        }

        private void resetBonusShipBoundCounter()
        {
            this.bonusShipBounce = 3;
        }

        private void checkBonusShipPosition(EnemyShip bonusShip)
        {
            const double tolerance = 0.01;

            if (Math.Abs(bonusShip.X - (this.canvas.Width - bonusShip.Width)) < tolerance)
            {
                this.bonusShipBounce--;
            }
            else if (Math.Abs(bonusShip.X) < tolerance)
            {
                this.bonusShipBounce--;
            }

            //if (bonusShip.X == this.canvas.Width - bonusShip.Width)
            //{
            //    this.bonusShipBounce--;
            //}
            //else if (bonusShip.X == 0)
            //{
            //    this.bonusShipBounce--;
            //}
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
        public void playExplosion(EnemyShip ship)
        {
            var explosion = new Explosion(ship, this.canvas);
            _ = explosion.playExplosion();
        }

        #endregion
    }
}