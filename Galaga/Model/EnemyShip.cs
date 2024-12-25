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
        #region Properties

        /// <summary>
        ///     Gets the score value.
        /// </summary>
        /// <value>
        ///     The score value.
        /// </value>
        public virtual int ScoreValue { get; set; }

        #endregion

        #region Data members

        private const int SpeedXDirection = 3;
        private const int SpeedYDirection = 0;

        private readonly Canvas canvas;

        /// <summary>
        ///     An array of Sprites for the enemy ship.
        /// </summary>
        protected BaseSprite[] Sprites;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyShip" /> class.
        /// </summary>
        /// <param name="mainSprite"></param>
        /// <param name="speed"></param>
        /// <param name="canvas"></param>
        protected EnemyShip(BaseSprite mainSprite, int speed, Canvas canvas)
        {
            this.canvas = canvas;
            SetSpeed(speed, SpeedYDirection);
            Sprite = mainSprite;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyShip" /> class.
        /// </summary>
        protected EnemyShip(BaseSprite mainSprite, BaseSprite alternateSprite, int levelMultiplier, Canvas canvas)
        {
            this.canvas = canvas;
            SetSpeed(SpeedXDirection * levelMultiplier, SpeedYDirection);
            Sprite = mainSprite;
            this.Sprites = new[] { mainSprite, alternateSprite };
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds the enemy Sprites to the canvas and sets it initial visibility.
        /// </summary>
        public virtual void AddEnemyToCanvas()
        {
            this.canvas.Children.Add(this.Sprites[0]);
            this.Sprites[0].Visibility = Visibility.Visible;

            this.canvas.Children.Add(this.Sprites[1]);
            this.Sprites[1].Visibility = Visibility.Collapsed;
        }

        /// <summary>
        ///     Special method to add the bonus ship to canvas.
        /// </summary>
        public virtual void AddBonusShipToCanvas()
        {
            //this.canvas.Children.Add(this.Sprites[0]);
            //this.Sprites[0].Visibility = Visibility.Visible;
            this.canvas.Children.Add(Sprite);
        }

        /// <summary>
        ///     Swaps the Sprites.
        /// </summary>
        public virtual void SwapSprites()
        {
            if (this.Sprites[0].Visibility == Visibility.Visible)
            {
                this.Sprites[0].Visibility = Visibility.Collapsed;
                this.Sprites[1].Visibility = Visibility.Visible;
                Sprite = this.Sprites[1];
            }
            else
            {
                this.Sprites[0].Visibility = Visibility.Visible;
                this.Sprites[1].Visibility = Visibility.Collapsed;
                Sprite = this.Sprites[0];
            }
        }

        #endregion
    }
}