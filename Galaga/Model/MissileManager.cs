using Galaga.View.Sprites;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace Galaga.Model
{
    /// <summary>
    ///     Missile controller
    /// </summary>
    public class MissileManager
    {
        //TODO: Reduce the creation of a missile to a means to place it on the canvas and assign the appropriate sprite
        #region Data members

        private const int MissileDelayLimit = 10;
        private const int EnemyFireCounter = 30;
        private const int PlayerMissileLimit = 3;
        private const int PlayerMissileSpeed = 10;
        private const int EnemyMissileSpeed = 12;
        private readonly Random random;
        private int delayTicker;

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
            this.random = new Random();
            this.PlayerMissileCount = 0;
            this.delayTicker = 10;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Fires the missile.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="canvas">The canvas.</param>
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

                return missile;
            }

            return null;
        }

        /// <summary>
        ///     Moves the missiles.
        /// </summary>
        /// <param name="missiles">The missiles.</param>
        public void MoveMissiles(IList<GameObject> missiles)
        {
            foreach (var missile in missiles)
            {
                if (missile != null)
                {
                    missile.MoveDown();
                }
            }
        }

        /// <summary>
        ///     Fires the enemy missiles.
        /// </summary>
        /// <param name="enemyShips">The enemy ships.</param>
        /// <param name="canvas">The canvas.</param>
        /// <returns></returns>
        public GameObject FireEnemyMissiles(IList<EnemyShip> enemyShips, Canvas canvas)
        {
            GameObject missileObject = null;

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
                    //var missile = selectedShip.FireMissile();
                    var missile = this.CreateEnemyMissile(selectedShip);
                    canvas.Children.Add(missile.Sprite);
                    missileObject = missile;
                }
            }

            return missileObject;
        }

        /// <summary>
        ///     Sets the position for the enemy missile.
        /// </summary>
        /// <param name="enemyShip"></param>
        /// <returns></returns>
        public GameObject CreateEnemyMissile(GameObject enemyShip)
        {
            var missile = new Missile(EnemyMissileSpeed, new EnemyMissileSprite());
            missile.X = enemyShip.X + enemyShip.Width / 2.0 - missile.Width / 2.0;
            missile.Y = enemyShip.Y + enemyShip.Height;
            return missile;
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

        #endregion
    }
}