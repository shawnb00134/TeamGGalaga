using Galaga.View.Sprites;

namespace Galaga.Model
{
    /// <summary>
    ///     Missile class.
    /// </summary>
    public class Missile : GameObject
    {
        private const int SpeedXDirection = 0;

        /// <summary>
        ///    Initializes a new instance of the <see cref="Missile" /> class.
        /// </summary>
        /// <param name="speedY"></param>
        /// <param name="sprite"></param>
        public Missile(int speedY, BaseSprite sprite)
        {
            SetSpeed(SpeedXDirection, speedY);
            Sprite = sprite;
        }
    }
}
