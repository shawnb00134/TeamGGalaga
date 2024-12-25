using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Galaga.View.Sprites;

namespace Galaga.Model
{
    /// <summary>
    ///     Class for a Level 3 Enemy.
    /// </summary>
    /// <seealso cref="Galaga.Model.EnemyShip" />
    public class FiringEnemy : EnemyShip
    {
        #region Properties

        /// <summary>
        ///     Gets the score value.
        /// </summary>
        /// <value>
        ///     The score value.
        /// </value>
        public override int ScoreValue { get; set; }

        #endregion

        #region Data members

        private const int SpeedXDirection = 3;
        private const int SpeedYDirection = 0;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="FiringEnemy" /> class.
        /// </summary>
        /// <param name="mainSprite"></param>
        /// <param name="speed"></param>
        /// <param name="canvas"></param>
        public FiringEnemy(BaseSprite mainSprite, int speed, Canvas canvas) : base(mainSprite, speed, canvas)
        {
            SetSpeed(speed, SpeedYDirection);

            if (mainSprite == null)
            {
                Debug.WriteLine("Single Sprite is null");
            }

            Sprite = mainSprite;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FiringEnemy" /> class.
        /// </summary>
        public FiringEnemy(BaseSprite mainSprite, BaseSprite alternateSprite, int levelMultiplier, Canvas canvas) :
            base(mainSprite,
                alternateSprite, levelMultiplier, canvas)
        {
            SetSpeed(SpeedXDirection * levelMultiplier, SpeedYDirection);
            Sprite = mainSprite;
            Sprites = new[] { mainSprite, alternateSprite };
        }

        /// <summary>
        ///     Swaps the Sprites.
        /// </summary>
        public override void SwapSprites()
        {
            if (Sprites[0].Visibility == Visibility.Visible)
            {
                Sprites[0].Visibility = Visibility.Collapsed;
                Sprites[1].Visibility = Visibility.Visible;
                Sprite = Sprites[1];
            }
            else
            {
                Sprites[0].Visibility = Visibility.Visible;
                Sprites[1].Visibility = Visibility.Collapsed;
                Sprite = Sprites[0];
            }
        }

        #endregion
    }
}