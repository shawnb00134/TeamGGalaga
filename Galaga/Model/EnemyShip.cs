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
        ///    Initializes a new instance of the <see cref="EnemyShip" /> class.
        /// </summary>
        /// <param name="mainSprite"></param>
        /// <param name="speed"></param>
        /// <param name="canvas"></param>
        protected EnemyShip(BaseSprite mainSprite, int speed, Canvas canvas)
        {
            this.canvas = canvas;
            SetSpeed(speed, SpeedYDirection);
            this.sprites = new[] { mainSprite };
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyShip" /> class.
        /// </summary>
        protected EnemyShip(BaseSprite mainSprite, BaseSprite alternateSprite, int levelMultiplier, Canvas canvas)
        {
            this.canvas = canvas;
            SetSpeed(SpeedXDirection * levelMultiplier, SpeedYDirection);
            this.sprites = new[] { mainSprite, alternateSprite };
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds the enemy sprites to the canvas and sets it initial visibility.
        /// </summary>
        public virtual void AddEnemyToCanvas()
        {
            this.canvas.Children.Add(this.sprites[0]);
            this.sprites[0].Visibility = Visibility.Visible;

            this.canvas.Children.Add(this.sprites[1]);
            this.sprites[1].Visibility = Visibility.Collapsed;
        }

        /// <summary>
        ///    Special method to add the bonus ship to canvas.
        /// </summary>
        public virtual void AddBonusShipToCanvas()
        {
            this.canvas.Children.Add(this.sprites[0]);
        }

        /// <summary>
        ///     Swaps the sprites.
        /// </summary>
        public virtual void SwapSprites()
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