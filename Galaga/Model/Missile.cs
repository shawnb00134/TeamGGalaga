using Galaga.View.Sprites;

namespace Galaga.Model
{
    public class Missile : GameObject
    {
        private const int SpeedXDirection = 0;

        public Missile(int speedY, BaseSprite sprite)
        {
            SetSpeed(SpeedXDirection, speedY);
            Sprite = sprite;
        }
    }
}
