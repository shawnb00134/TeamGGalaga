using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Galaga.View.Sprites;
using System;

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

        private ShipFactory shipFactory;

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
                    enemyShip = shipFactory.CreateEnemyShip(rowIndex, levelMultiplier, canvas);
                    enemyShip.AddEnemyToCanvas();
                    enemyShips.Add(enemyShip);

                    var xPosition = (i + 1) * spacing - enemyShip.Width / 2.0;
                    enemyShip.X = xPosition;
                    enemyShip.Y = startY - rowIndex * rowSpacing;
                }
            }

            return enemyShips;
        }

        public EnemyShip CreateSpecialShip()
        {
            var bonusEnemyShip = shipFactory.CreateSpecialShip(canvas);

            if (this.random.Next(0, 1) == 0)
            {
                bonusEnemyShip.X = 0 - bonusEnemyShip.Width;
                bonusEnemyShip.Y = 0;
            }
            else
            {
                bonusEnemyShip.X = canvas.Width;
                bonusEnemyShip.Y = 0;
            }

            if (bonusEnemyShip == null)
            {
                System.Diagnostics.Debug.WriteLine("Bonus ship is null");
            }
            //this.canvas.Children.Add(bonusEnemyShip.Sprite);

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

        private void moveBonusShip()
        {
            
        }

        /// <summary>
        ///     Swaps the sprites every tick.
        /// </summary>
        public void SwapSpritesAnimation(IList<EnemyShip> enemyShips)
        {
            foreach (var enemyShip in enemyShips)
            {
                enemyShip.SwapSprites();
            }
        }

        #endregion
    }
}