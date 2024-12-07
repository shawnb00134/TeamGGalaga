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
            shipFactory = new ShipFactory();

            BonusShipBounce = 4;
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
            var canvasWidth = canvas.Width;
            var startY = canvas.Height / 2;
            int[] enemiesPerRow = { 2, 3, 4, 5 };

            for (var row = 0; row < enemiesPerRow.Length; row++)
            {
                rowDirections[row] = 1;
                rowSpeeds[row] = DefaultSpeedX * levelMultiplier;
            }

            if (levelMultiplier >= 2)
            {
                rowDirections[0] = 1;
                rowDirections[1] = 1;
                rowDirections[2] = -1;
                rowDirections[3] = -1;
            }

            if (levelMultiplier >= 3)
            {
                rowDirections[1] = -1;
                rowDirections[3] = -1;

                rowSpeeds[1] *= 2;
                rowSpeeds[3] *= 2;
            }

            for (var rowIndex = 0; rowIndex < enemiesPerRow.Length; rowIndex++)
            {
                var enemyCount = enemiesPerRow[rowIndex];
                var spacing = canvasWidth / (enemyCount + 1);

                for (var i = 0; i < enemyCount; i++)
                {
                    var enemyShip = shipFactory.CreateEnemyShip(rowIndex, levelMultiplier, canvas);
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
            bonusShip = shipFactory.CreateSpecialShip(canvas);
            bonusShip.AddBonusShipToCanvas();
            return bonusShip;
        }

        /// <summary>
        ///     Moves the enemy ships.
        /// </summary>
        /// <param name="enemyShips">The enemy ships.</param>
        /// <param name="tickCounter">The tick counter.</param>
        public void MoveEnemyShips(IList<EnemyShip> enemyShips, int tickCounter)
        {
            var shouldChangeDirection = false;

            for (var rowIndex = 0; rowIndex < rowDirections.Count; rowIndex++)
            {
                var direction = rowDirections[rowIndex];
                var speed = rowSpeeds[rowIndex];

                foreach (var ship in enemyShips)
                    if (ship.Sprite != null && ship.Sprite.GetType() != typeof(EnemySpecialSprite) &&
                        ship.Y == canvas.Height / 2 - rowIndex * 100)
                    {
                        ship.X += direction * speed;

                        if ((direction == 1 && ship.X + ship.Width >= canvas.Width) ||
                            (direction == -1 && ship.X <= 0))
                            shouldChangeDirection = true;
                    }
            }

            if (shouldChangeDirection)
                for (var rowIndex = 0; rowIndex < rowDirections.Count; rowIndex++)
                    rowDirections[rowIndex] *= -1;
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
                ship.X += ship.SpeedX * speedMultiplier;
            else
                ship.X -= ship.SpeedX * speedMultiplier;
            Canvas.SetLeft(ship.Sprite, ship.X);
        }

        /// <summary>
        ///     Determines the row index of a ship based on its Y position.
        /// </summary>
        private int getRowIndex(double yPosition)
        {
            const double rowSpacing = 100;
            var startY = canvas.Height / 2;
            return (int)((startY - yPosition) / rowSpacing);
        }

        /// <summary>
        ///     Moves the bonus ship.
        /// </summary>
        public void MoveBonusShip()
        {
            CheckBounceCounter();

            this.checkBonusShipPosition();

            if (bonusShip != null)
            {
                if (movingRightBonusShip)
                {
                    bonusShip.MoveRight();

                    if (bonusShip.X + bonusShip.Width >= canvas.Width) movingRightBonusShip = false;
                }
                else
                {
                    bonusShip.MoveLeft();

                    if (bonusShip.X <= 0) movingRightBonusShip = true;
                }
            }
        }

        /// <summary>
        ///     Lets the GameManager know when to remove the bonus ship from play.
        /// </summary>
        /// <returns></returns>
        public bool CheckBounceCounter()
        {
            if (BonusShipBounce == 0)
            {
                this.resetBonusShipBoundCounter();
                return true;
            }

            return false;
        }

        private void resetBonusShipBoundCounter()
        {
            BonusShipBounce = 4;
        }

        private void checkBonusShipPosition()
        {
            if (bonusShip != null)
            {
                if (bonusShip.X == 0)
                {
                    BonusShipBounce--;
                    movingRightBonusShip = true;
                }

                if (Convert.ToInt32(bonusShip.X) == Convert.ToInt32(canvas.Width - bonusShip.Width))
                {
                    BonusShipBounce--;
                    movingRightBonusShip = false;
                }
            }
        }

        /// <summary>
        ///     Swaps the Sprites every tick.
        /// </summary>
        public void SwapSpritesAnimation(IList<EnemyShip> enemyShips)
        {
            foreach (var enemyShip in enemyShips)
                if (enemyShip.Sprite.GetType() != typeof(EnemySpecialSprite))
                    enemyShip.SwapSprites();
        }

        /// <summary>
        ///     Plays the explosion.
        /// </summary>
        /// <param name="ship">The ship.</param>
        public void PlayExplosion(GameObject ship)
        {
            var explosion = new Explosion(ship, canvas);
            _ = explosion.PlayExplosion();
        }

        #endregion
    }
}