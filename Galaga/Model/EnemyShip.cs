using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Galaga.View.Sprites;

namespace Galaga.Model
{
    /// <summary>
    ///     Abstract class for enemy ships.
    /// </summary>
    /// <seealso cref="Galaga.Model.GameObject" />
    public abstract class EnemyShip : GameObject
    {
        #region Data members

        private const int SpeedXDirection = 3;
        private const int SpeedYDirection = 0;

        /// <summary>
        ///     The primary sprite/
        /// </summary>
        public BaseSprite PrimarySprite;

        /// <summary>
        ///     The secondary sprite
        /// </summary>
        //public EnemyLevel1SpriteAlternate SecondarySprite;
        public BaseSprite SecondarySprite;

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
        public virtual int ScoreValue { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyShip" /> class.
        /// </summary>
        protected EnemyShip(BaseSprite mainSprite, BaseSprite alternateSprite, Canvas canvas)
        {
            Sprite = mainSprite;
            this.canvas = canvas;
            SetSpeed(SpeedXDirection, SpeedYDirection);
            this.sprites = new[] { mainSprite, alternateSprite };
            this.PrimarySprite = mainSprite;
            this.SecondarySprite = alternateSprite;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Fires the missile.
        /// </summary>
        /// <returns></returns>
        public virtual EnemyMissile FireMissile()
        {
            var missile = new EnemyMissile();
            missile.X = X + Width / 2.0 - missile.Width / 2.0;
            missile.Y = Y + Height;
            return missile;
        }

        /// <summary>
        ///     Adds the enemy sprites to the canvas and sets it initial visibility.
        /// </summary>
        public virtual void addEnemyToCanvas()
        {
            this.canvas.Children.Add(this.sprites[0]);
            this.sprites[0].Visibility = Visibility.Visible;

            this.canvas.Children.Add(this.sprites[1]);
            this.sprites[1].Visibility = Visibility.Collapsed;
        }

        /// <summary>
        ///     Swaps the sprites.
        /// </summary>
        public virtual void SwapSprites()
        {
        }

        #endregion
    }
}