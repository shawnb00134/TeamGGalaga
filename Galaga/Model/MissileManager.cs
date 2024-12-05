using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Galaga.View.Sprites;

namespace Galaga.Model
{
    /// <summary>
    ///     Missile controller
    /// </summary>
    public class MissileManager
    {
        #region Data members

        private const int MissileDelayLimit = 10;
        private const int EnemyFireCounter = 30;
        private const int PlayerMissileLimit = 3;
        private const int PlayerMissileSpeed = -10;
        private const int NukeMissileSpeed = -5;
        private const int EnemyMissileSpeed = 12;

        private readonly SoundManager soundManager;

        private readonly Random random;
        private int delayTicker;

        /// <summary>
        ///     Public access to the NukeEnabled property
        /// </summary>
        public bool NukeEnabled { get; private set; }

        #endregion

        #region Properties

        private int PlayerMissileCount { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MissileManager" /> class.
        /// </summary>
        public MissileManager()
        {
            this.soundManager = new SoundManager();

            this.random = new Random();
            this.PlayerMissileCount = 0;
            this.delayTicker = 10;
            this.NukeEnabled = false;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Fires the missile.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns></returns>
        public GameObject FireMissile(GameObject player, Canvas canvas)
        {
            if (this.PlayerMissileCount < PlayerMissileLimit && this.delayTicker > MissileDelayLimit)
            {
                this.PlayerMissileCount++;
                this.delayTicker = 0;

                //var missile = new PlayerMissile();
                var missile = new Missile(PlayerMissileSpeed, new PlayerMissileSprite());
                missile.X = player.X + player.Width / 2.0 - missile.Width / 2.0;
                missile.Y = player.Y - missile.Height;
                canvas.Children.Add(missile.Sprite);

                this.soundManager.playPlayerFiring();

                return missile;
            }

            return null;
        }

        /// <summary>
        ///     Moves the missiles.
        /// </summary>
        /// <param name="missiles">The missiles.</param>
        public void MoveMissiles(IList<GameObject> missiles, Canvas canvas)
        {
            foreach (var missile in missiles)
            {
                if (missile != null)
                {
                    //TODO: Fix this, it casuses missiles to go invisible
                    //const double tolerance = 1.0;

                    //if (missile.Sprite is NukeBombSprite &&
                    //    Math.Abs(missile.Y - (canvas.Height / 2 + missile.Height / 2)) < tolerance)
                    //{
                    //    canvas.Children.Remove(missile.Sprite);
                    //}
                    //{
                    //    canvas.Children.Remove(missile.Sprite);
                    //}
                    missile.MoveDown();
                    missile.MoveRight();
                }
            }
        }

        /// <summary>
        ///     Fires the enemy missiles.
        /// </summary>
        /// <param name="enemyShips">The enemy ships.</param>
        /// <param name="playerShip"></param>
        /// <returns></returns>
        public GameObject FireEnemyMissiles(IList<EnemyShip> enemyShips, GameObject playerShip, Canvas canvas)
        {
            if (this.random.Next(EnemyFireCounter) == 0)
            {
                FiringEnemy selectedShip = null;
                var eligibleCount = 0;

                foreach (var ship in enemyShips)
                {
                    if (ship is FiringEnemy firingEnemy)
                    {
                        eligibleCount++;

                        if (this.random.Next(eligibleCount) == 0)
                        {
                            selectedShip = firingEnemy;
                        }
                    }
                }

                if (selectedShip != null)
                {
                    var missile = this.CreateEnemyMissile(selectedShip, playerShip);
                    canvas.Children.Add(missile.Sprite);

                    this.soundManager.playEnemyFiring();

                    return missile;
                }
            }

            return null;
        }

        /// <summary>
        ///     Sets the position for the enemy missile.
        /// </summary>
        /// <param name="enemyShip"></param>
        /// <param name="playerShip"></param>
        /// <returns></returns>
        public GameObject CreateEnemyMissile(GameObject enemyShip, GameObject playerShip)
        {
            Missile missile;
            if (enemyShip.Sprite is EnemyLevel4Sprite || enemyShip.Sprite is EnemyLevel4SpriteAlternate)
            {
                double[] speed = this.calculateVerticalHorizontalSpeed((EnemyShip) enemyShip, playerShip);
                missile = new Missile(speed[0] * EnemyMissileSpeed, speed[1] * EnemyMissileSpeed, new EnemyMissileSprite());
            }
            else
            {
                missile = new Missile(EnemyMissileSpeed, new EnemyMissileSprite());
            }
            
            missile.X = enemyShip.X + enemyShip.Width / 2.0 - missile.Width / 2.0;
            missile.Y = enemyShip.Y + enemyShip.Height;
            return missile;
        }

        /// <summary>
        ///     Check to see if missile gameobject is a player missile for decrementing player missile count.
        /// </summary>
        /// <param name="missile"></param>
        public void checkForPlayerMissile(GameObject missile)
        {
            if (missile.Sprite.GetType() == typeof(PlayerMissileSprite))
            {
                this.DecrementPlayerMissileCount();
            }
        }

        /// <summary>
        ///     Decrements the player missile count.
        /// </summary>
        public void DecrementPlayerMissileCount()
        {
            this.PlayerMissileCount--;
        }

        /// <summary>
        ///     Updates the delay tick.
        /// </summary>
        public void UpdateDelayTick()
        {
            this.delayTicker++;
        }

        private double[] calculateVerticalHorizontalSpeed(EnemyShip enemy, GameObject player)
        {
            double deltaX = player.X - enemy.X;
            double deltaY = player.Y - enemy.Y;

            double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            deltaX /= distance;
            deltaY /= distance;

            return new[] {deltaX, deltaY};
        }

        /// <summary>
        ///     Enables the nuke to be fired
        /// </summary>
        public void EnableNuke()
        {
            this.NukeEnabled = true;
        }

        /// <summary>
        ///     Launches the nuke.
        /// </summary>
        public GameObject FireNuke()
        {
            Missile missile = new Missile(NukeMissileSpeed, new NukeBombSprite());
            return missile;
        }

        #endregion
    }
}