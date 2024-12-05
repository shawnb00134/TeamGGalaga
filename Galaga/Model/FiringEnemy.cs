using Windows.UI.Xaml.Controls;
using Galaga.View.Sprites;
using Windows.UI.Xaml;

namespace Galaga.Model
{
    /// <summary>
    ///     Class for a Level 3 Enemy.
    /// </summary>
    /// <seealso cref="Galaga.Model.EnemyShip" />
    public class FiringEnemy : EnemyShip
    {
        #region Data members

        private const int SpeedXDirection = 3;
        private const int SpeedYDirection = 0;
        
        private readonly BaseSprite[] sprites;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the score value.
        /// </summary>
        /// <value>
        ///     The score value.
        /// </value>
        public override int ScoreValue { get; set; }

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
                System.Diagnostics.Debug.WriteLine("Single Sprite is null");
            }

            this.sprites = new[] { mainSprite };
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FiringEnemy" /> class.
        /// </summary>
        public FiringEnemy(BaseSprite mainSprite, BaseSprite alternateSprite, int levelMultiplier, Canvas canvas) :
            base(mainSprite,
                alternateSprite, levelMultiplier, canvas)
        {
            SetSpeed(SpeedXDirection * levelMultiplier, SpeedYDirection);

            if (mainSprite == null || alternateSprite == null)
            {
                System.Diagnostics.Debug.WriteLine("Double Sprite is null");
            }

            this.sprites = new[] { mainSprite, alternateSprite };
        }

        /// <summary>
        ///     Swaps the sprites.
        /// </summary>
        public override void SwapSprites()
        {
            if (this.sprites[0].Visibility == Visibility.Visible)
            {
                this.sprites[0].Visibility = Visibility.Collapsed;
                this.sprites[1].Visibility = Visibility.Visible;
                Sprite = this.sprites[1];
            }
            else
            {
                this.sprites[0].Visibility = Visibility.Visible;
                this.sprites[1].Visibility = Visibility.Collapsed;
                Sprite = this.sprites[0];
            }
        }

        #endregion
    }
}