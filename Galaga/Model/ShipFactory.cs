using Windows.UI.Xaml.Controls;
using Galaga.View.Sprites;

namespace Galaga.Model
{
    /// <summary>
    ///    Factory class for creating ships.
    /// </summary>
    public class ShipFactory
    {
        /// <summary>
        ///    Creates the enemy ship.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public EnemyShip CreateEnemyShip(int level, Canvas canvas)
        {
            EnemyShip enemyShip;
            switch (level)
            {
                case 0:
                    enemyShip = new NonFiringEnemy(new EnemyLevel1Sprite(), new EnemyLevel1SpriteAlternate(), canvas);
                    enemyShip.ScoreValue = 1;
                    break;
                case 1:
                    enemyShip = new NonFiringEnemy(new EnemyLevel2Sprite(), new EnemyLevel2SpriteAlternate(), canvas);
                    enemyShip.ScoreValue = 2;
                    break;
                case 2:
                    enemyShip = new FiringEnemy(new EnemyLevel3Sprite(), new EnemyLevel3SpriteAlternate(), canvas);
                    enemyShip.ScoreValue = 3;
                    break;
                default:
                    enemyShip = new FiringEnemy(new EnemyLevel3Sprite(), new EnemyLevel3SpriteAlternate(), canvas);
                    enemyShip.ScoreValue = 4;
                    break;
            }

            return enemyShip;
        }

        /// <summary>
        ///   Creates the special ship.
        /// </summary>
        /// <returns></returns>
        public EnemyShip CreateSpecialShip(Canvas canvas)
        {
            //EnemyShip specialEnemyShip = new NonFiringEnemy(new XXXXX, new XXXXX, canvas);
            
            return null;
        }

        /// <summary>
        ///   Creates the player ship.
        /// </summary>
        /// <returns></returns>
        public Player CreatePlayerShip()
        {
            //return new PlayerShip(new PlayerSprite());
            return null;
        }
    }
}
