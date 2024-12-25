using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Galaga.View.Sprites;

namespace Galaga.Model
{
    /// <summary>
    ///     Explosion class for the destruction of ships
    /// </summary>
    public class Explosion : GameObject
    {
        #region Data members

        private readonly Canvas canvas;
        private readonly BaseSprite[] sprites;
        private readonly double xCoordinate;
        private readonly double yCoordinate;

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructor for the Nuke explosion
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="missile"></param>
        /// <param name="canvas"></param>
        public Explosion(BaseSprite sprite, GameObject missile, Canvas canvas)
        {
            this.canvas = canvas;

            Sprite = sprite;
            this.xCoordinate = missile.X - Sprite.Width / 2;
            this.yCoordinate = missile.Y - Sprite.Height / 2;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Explosion" /> class.
        /// </summary>
        /// <param name="objectDestroyed">The ship.</param>
        /// <param name="canvas">The canvas.</param>
        public Explosion(GameObject objectDestroyed, Canvas canvas)
        {
            this.canvas = canvas;
            this.xCoordinate = objectDestroyed.X;
            this.yCoordinate = objectDestroyed.Y;

            this.sprites = new BaseSprite[] { new ExplosionSprite1(), new ExplosionSprite2(), new ExplosionSprite3() };
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Plays the explosion.
        /// </summary>
        public async Task PlayExplosion()
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
        public async void PlayNuclearExplosion()
        {
            this.canvas.Children.Add(Sprite);
            Sprite.Visibility = Visibility.Visible;
            Sprite.RenderAt(this.xCoordinate, this.yCoordinate);

            await Task.Delay(1000);

            this.canvas.Children.Remove(Sprite);
        }

        #endregion
    }
}