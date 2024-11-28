﻿using Windows.UI.Xaml;
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
            this.canvas = canvas;
            SetSpeed(SpeedXDirection, SpeedYDirection);
            this.sprites = new[] { mainSprite, alternateSprite };
            this.PrimarySprite = mainSprite;
            this.SecondarySprite = alternateSprite;
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