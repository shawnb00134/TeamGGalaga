using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Galaga.View.Sprites;

namespace Galaga.Model
{
    /// <summary>
    ///     Explosion class for the destruction of ships
    /// </summary>
    public class Explosion
    {
        #region Data members

        private readonly Canvas canvas;
        private readonly BaseSprite[] sprites;
        private readonly double xCoordinate;
        private readonly double yCoordinate;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Explosion" /> class.
        /// </summary>
        /// <param name="ship">The ship.</param>
        /// <param name="canvas">The canvas.</param>
        public Explosion(EnemyShip ship, Canvas canvas)
        {
            this.canvas = canvas;
            this.xCoordinate = ship.X;
            this.yCoordinate = ship.Y;
            this.sprites = new BaseSprite[] { new ExplosionSprite1(), new ExplosionSprite2(), new ExplosionSprite3() };

            this.hideSprites();
        }

        #endregion

        #region Methods

        private void hideSprites()
        {
            foreach (var sprite in this.sprites)
            {
                sprite.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        ///     Plays the explosion.
        /// </summary>
        public void playExplosion()
        {
            foreach (var sprite in this.sprites)
            {
                this.canvas.Children.Add(sprite);
                sprite.RenderAt(this.xCoordinate, this.yCoordinate);
                sprite.Visibility = Visibility.Visible;
            }
        }

        #endregion
    }
}