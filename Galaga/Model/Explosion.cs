using System.Collections;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Galaga.View.Sprites;
using System.Collections.Generic;

namespace Galaga.Model
{
    /// <summary>
    ///     Explosion class for the destruction of ships
    /// </summary>
    public class Explosion
    {
        #region Data members

        private const int ExplosionTimeLimit = 50000000;

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
        public Explosion(GameObject ship, Canvas canvas)
        {
            this.canvas = canvas;
            this.xCoordinate = ship.X;
            this.yCoordinate = ship.Y;
            this.sprites = new BaseSprite[] { new ExplosionSprite1(), new ExplosionSprite2(), new ExplosionSprite3() };
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Plays the explosion.
        /// </summary>
        public async Task playExplosion()
        {
            this.canvas.Children.Add(this.sprites[0]);
            this.sprites[0].RenderAt(this.xCoordinate, this.yCoordinate);
            this.sprites[0].Visibility = Visibility.Visible;

            await Task.Delay(100);

            this.canvas.Children.Add(this.sprites[1]);
            this.sprites[1].RenderAt(this.xCoordinate, this.yCoordinate);
            this.sprites[1].Visibility = Visibility.Visible;

            await Task.Delay(100);

            this.canvas.Children.Add(this.sprites[2]);
            this.sprites[2].RenderAt(this.xCoordinate, this.yCoordinate);
            this.sprites[2].Visibility = Visibility.Visible;

            await Task.Delay(100);

            foreach (var sprite in this.sprites)
            {
                this.canvas.Children.Remove(sprite);
            }
        }

        /// <summary>
        ///     Plays the Sprite for the nuclear explosion.
        /// </summary>
        public void playNuclearExplosion(IList<EnemyShip> enemyShips)
        {
            BaseSprite sprite = new NukeExplosionSprite();

            this.canvas.Children.Add(sprite);
            sprite.RenderAt(this.xCoordinate, (this.canvas.Height / 2) + (sprite.Height / 2));
            sprite.Visibility = Visibility.Visible;
        }

        #endregion
    }
}