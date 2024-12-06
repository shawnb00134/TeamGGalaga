using System;
using Galaga.View.Sprites;

namespace Galaga.Model
{
    /// <summary>
    ///     Missile class.
    /// </summary>
    public class Missile : GameObject
    {
        #region Data members

        private const int SpeedXDirection = 0;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Missile" /> class.
        /// </summary>
        /// <param name="speedY"></param>
        /// <param name="sprite"></param>
        public Missile(int speedY, BaseSprite sprite)
        {
            SetSpeed(SpeedXDirection, speedY);
            Sprite = sprite;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Missile" /> class.
        /// </summary>
        /// <param name="speedX">The speed x.</param>
        /// <param name="speedY">The speed y.</param>
        /// <param name="sprite">The sprite.</param>
        public Missile(double speedX, double speedY, BaseSprite sprite)
        {
            SetSpeed(Convert.ToInt32(speedX), Convert.ToInt32(speedY));
            Sprite = sprite;
        }

        #endregion
    }
}