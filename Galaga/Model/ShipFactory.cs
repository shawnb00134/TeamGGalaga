using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Galaga.View.Sprites;

namespace Galaga.Model
{
    /// <summary>
    ///     Factory class for creating ships.
    /// </summary>
    public class ShipFactory
    {
        #region Data members

        private const int SpecialEnemySpeed = 0;

        #endregion

        #region Methods

        /// <summary>
        ///     Creates the enemy ship.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="levelMultiplier"></param>
        /// <param name="canvas"></param>
        /// <returns></returns>
        public EnemyShip CreateEnemyShip(int level, int levelMultiplier, Canvas canvas)
        {
            EnemyShip enemyShip;
            switch (level)
            {
                case 0:
                    enemyShip = new NonFiringEnemy(new EnemyLevel1Sprite(), new EnemyLevel1SpriteAlternate(),
                        levelMultiplier, canvas);
                    enemyShip.ScoreValue = 1 * levelMultiplier;
                    break;
                case 1:
                    enemyShip = new NonFiringEnemy(new EnemyLevel2Sprite(), new EnemyLevel2SpriteAlternate(),
                        levelMultiplier, canvas);
                    enemyShip.ScoreValue = 2 * levelMultiplier;
                    break;
                case 2:
                    enemyShip = new FiringEnemy(new EnemyLevel3Sprite(), new EnemyLevel3SpriteAlternate(),
                        levelMultiplier, canvas);
                    enemyShip.ScoreValue = 3 * levelMultiplier;
                    break;
                default:
                    enemyShip = new FiringEnemy(new EnemyLevel4Sprite(), new EnemyLevel4SpriteAlternate(),
                        levelMultiplier, canvas);
                    enemyShip.ScoreValue = 4 * levelMultiplier;
                    break;
            }

            return enemyShip;
        }

        /// <summary>
        ///     Creates the special ship.
        /// </summary>
        /// <returns></returns>
        public EnemyShip CreateSpecialShip(Canvas canvas)
        {
            EnemyShip specialEnemyShip = new FiringEnemy(new EnemySpecialSprite(), SpecialEnemySpeed, canvas);
            specialEnemyShip.ScoreValue = 0;

            specialEnemyShip.X = 0;
            specialEnemyShip.Y = 0;

            return specialEnemyShip;
        }

        #endregion
    }
}