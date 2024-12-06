using System.Collections;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Galaga.View.Sprites;
using System.Collections.Generic;
using System.Reflection;

namespace Galaga.Model
{
    /// <summary>
    ///     Explosion class for the destruction of ships
    /// </summary>
    public class Explosion : GameObject
    {
        #region Data members

        private const int ExplosionTimeLimit = 50000000;

        private readonly Canvas canvas;
        private readonly BaseSprite[] sprites;
        private double xCoordinate;
        private double yCoordinate;

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructor for the Nuke explosion
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="canvas"></param>
        public Explosion(BaseSprite sprite, GameObject missile, Canvas canvas)
        {
            this.canvas = canvas;

            //TODO: Issue here?
            this.xCoordinate = missile.X + (sprite.Width / 2);
            this.yCoordinate = missile.Y + (sprite.Height / 2);
            Sprite = sprite;

            System.Diagnostics.Debug.WriteLine("Set: " +this.xCoordinate + " : " + this.yCoordinate);
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
        public async void playNuclearExplosion()
        {
            this.canvas.Children.Add(Sprite);
            Sprite.Visibility = Visibility.Visible;

            System.Diagnostics.Debug.WriteLine("Boom: " + this.xCoordinate + " : " + this.yCoordinate);

            await Task.Delay(2000);

            this.canvas.Children.Remove(Sprite);
        }

        #endregion
    }
}