using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Galaga.View.Sprites;

namespace Galaga.Model
{
    /// <summary>
    ///     A simple physics class to check for collisions.
    /// </summary>
    public class Physics
    {
        #region Methods

        /// <summary>
        ///     Checks the collisions.
        /// </summary>
        /// <param name="listOfShips">The enemy ships.</param>
        /// <param name="missiles">The missiles.</param>
        public List<GameObject> CheckCollisions(IList<GameObject> listOfShips, IList<GameObject> missiles)
        {
            var objectsToRemove = new List<GameObject>();

            foreach (var ship in listOfShips)
            foreach (var missile in missiles)
                if (missile != null && !(ship is EnemyShip && missile.Sprite is EnemyMissileSprite))
                    if (isColliding(ship, missile))
                    {
                        objectsToRemove.Add(ship);
                        objectsToRemove.Add(missile);
                    }

            return objectsToRemove;
        }

        private bool isColliding(GameObject ship, GameObject missile)
        {
            return missile.X < ship.X + ship.Width &&
                   missile.X + missile.Width > ship.X &&
                   missile.Y < ship.Y + ship.Height &&
                   missile.Y + missile.Height > ship.Y;
        }

        /// <summary>
        ///     Checks the missile boundary.
        /// </summary>
        /// <param name="missile">The missiles.</param>
        /// <param name="canvas">The canvas.</param>
        /// <returns></returns>
        public bool CheckMissileBoundary(GameObject missile, Canvas canvas)
        {
            if (missile != null)
            {
                if (missile.Sprite is EnemyMissileSprite && missile.Y > canvas.Height) return true;

                if (missile.Sprite.GetType() == typeof(PlayerMissileSprite) && missile.Y < 0) return true;
            }

            return false;
        }

        #endregion
    }
}