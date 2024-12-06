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

        private const int SpecialEnemySpeed = 5;
        private const int Level1ScoreValue = 1;
        private const int Level2ScoreValue = 2;
        private const int Level3ScoreValue = 3;
        private const int Level4ScoreValue = 4;
        private const int SpecialEnemyScoreValue = 0;

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
                    enemyShip.ScoreValue = Level1ScoreValue * levelMultiplier;
                    break;
                case 1:
                    enemyShip = new NonFiringEnemy(new EnemyLevel2Sprite(), new EnemyLevel2SpriteAlternate(),
                        levelMultiplier, canvas);
                    enemyShip.ScoreValue = Level2ScoreValue * levelMultiplier;
                    break;
                case 2:
                    enemyShip = new FiringEnemy(new EnemyLevel3Sprite(), new EnemyLevel3SpriteAlternate(),
                        levelMultiplier, canvas);
                    enemyShip.ScoreValue = Level3ScoreValue * levelMultiplier;
                    break;
                default:
                    enemyShip = new FiringEnemy(new EnemyLevel4Sprite(), new EnemyLevel4SpriteAlternate(),
                        levelMultiplier, canvas);
                    enemyShip.ScoreValue = Level4ScoreValue * levelMultiplier;
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
            specialEnemyShip.ScoreValue = SpecialEnemyScoreValue;

            specialEnemyShip.X = 0;
            specialEnemyShip.Y = 0;

            return specialEnemyShip;
        }

        #endregion
    }
}