using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
//using System.Linq;

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

                var missile = new PlayerMissile();
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
        public void MoveMissiles(List<GameObject> missiles)
        {
            foreach (var missile in missiles)
            {
                if (missile is PlayerMissile)
                {
                    missile.MoveUp();
                }

                if (missile is EnemyMissile)
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
        public EnemyMissile FireEnemyMissiles(List<EnemyShip> enemyShips, Canvas canvas)
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
                    var missile = selectedShip.FireMissile();
                    canvas.Children.Add(missile.Sprite);
                    missileObject = missile;
                }
            }

            return missileObject as EnemyMissile;
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