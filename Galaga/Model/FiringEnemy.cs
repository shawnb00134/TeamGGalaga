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
        #region Data members

        private const int SpeedXDirection = 3;
        private const int SpeedYDirection = 0;

        /// <summary>
        ///     The primary sprite/
        /// </summary>
        public new BaseSprite PrimarySprite;

        /// <summary>
        ///     The secondary sprite
        /// </summary>
        public new BaseSprite SecondarySprite;

        private readonly Canvas canvas;

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
        public FiringEnemy(BaseSprite mainSprite, BaseSprite alternateSprite, Canvas canvas) : base(mainSprite,
            alternateSprite, canvas)
        {
            Sprite = mainSprite;
            this.canvas = canvas;
            SetSpeed(SpeedXDirection, SpeedYDirection);
            this.sprites = new[] { mainSprite, alternateSprite };
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Fires the missile.
        /// </summary>
        public override EnemyMissile FireMissile()
        {
            var missile = new EnemyMissile();
            missile.X = X + Width / 2.0 - missile.Width / 2.0;
            missile.Y = Y + Height;
            return missile;
        }

        /// <summary>
        ///     Adds the enemy sprites to the canvas and sets it initial visibility.
        /// </summary>
        public override void addEnemyToCanvas()
        {
            this.canvas.Children.Add(this.sprites[0]);
            this.sprites[0].Visibility = Visibility.Visible;

            this.canvas.Children.Add(this.sprites[1]);
            this.sprites[1].Visibility = Visibility.Collapsed;
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