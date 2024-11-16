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

        //TODO: Change names of these constants to something more descriptive
        private const int EnemyMovementStepOne = 10;
        private const int EnemyMovementStepTwo = 30;
        private const int EnemyMovementStepThree = 40;

        private readonly Canvas canvas;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyManager" /> class.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        public EnemyManager(Canvas canvas)
        {
            this.canvas = canvas;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates and places the enemy ships.
        /// </summary>
        /// <returns></returns>
        public List<EnemyShip> CreateAndPlaceEnemyShip()
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
                    switch (rowIndex)
                    {
                        case 0:
                            enemyShip = new NonFiringEnemy(new EnemyLevel1Sprite(), new EnemyLevel1SpriteAlternate(),
                                this.canvas);
                            enemyShip.ScoreValue = 1;
                            break;
                        case 1:
                            enemyShip = new NonFiringEnemy(new EnemyLevel2Sprite(), new EnemyLevel2SpriteAlternate(),
                                this.canvas);
                            enemyShip.ScoreValue = 2;
                            break;
                        case 2:
                            enemyShip = new FiringEnemy(new EnemyLevel3Sprite(), new EnemyLevel3SpriteAlternate(),
                                this.canvas);
                            enemyShip.ScoreValue = 3;
                            break;
                        default:
                            enemyShip = new FiringEnemy(new EnemyLevel3Sprite(), new EnemyLevel3SpriteAlternate(),
                                this.canvas);
                            enemyShip.ScoreValue = 4;
                            break;
                    }

                    enemyShip.addEnemyToCanvas();
                    enemyShips.Add(enemyShip);

                    var xPosition = (i + 1) * spacing - enemyShip.Width / 2.0;
                    enemyShip.X = xPosition;
                    enemyShip.Y = startY - rowIndex * rowSpacing;
                }
            }

            return enemyShips;
        }

        /// <summary>
        ///     Moves the enemy ships.
        /// </summary>
        /// <param name="enemyShips">The enemy ships.</param>
        /// <param name="tickCounter">The tick counter.</param>
        public void MoveEnemyShips(List<EnemyShip> enemyShips, int tickCounter)
        {
            foreach (var ship in enemyShips)
            {
                if (ship != null)
                {
                    if (tickCounter < EnemyMovementStepOne)
                    {
                        ship.MoveLeft();
                    }
                    else if (tickCounter < EnemyMovementStepTwo)
                    {
                        ship.MoveRight();
                    }
                    else if (tickCounter < EnemyMovementStepThree)
                    {
                        ship.MoveLeft();
                    }
                }
            }
        }

        /// <summary>
        ///     Swaps the sprites every tick.
        /// </summary>
        public void swapSpritesAnimation(List<EnemyShip> enemyShips)
        {
            foreach (var enemyShip in enemyShips)
            {
                enemyShip.SwapSprites();
            }
        }

        #endregion
    }
}