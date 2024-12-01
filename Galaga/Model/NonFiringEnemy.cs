﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Galaga.View.Sprites;

namespace Galaga.Model
{
    /// <summary>
    ///     Enemy level 1 class.
    /// </summary>
    /// <seealso cref="Galaga.Model.EnemyShip" />
    public class NonFiringEnemy : EnemyShip
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
        public override int ScoreValue { get; set; }

        #endregion

        #region Constructors

        public NonFiringEnemy(BaseSprite mainSprite, int speed, Canvas canvas) : base(mainSprite, speed, canvas)
        {
            this.canvas = canvas;
            SetSpeed(speed, SpeedYDirection);
            this.sprites = new[] { mainSprite, mainSprite };
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NonFiringEnemy" /> class.
        /// </summary>
        public NonFiringEnemy(BaseSprite mainSprite, BaseSprite alternateSprite, int levelMultiplier, Canvas canvas) : base(mainSprite,
            alternateSprite, levelMultiplier, canvas)
        {
            Sprite = mainSprite;
            SetSpeed(SpeedXDirection * levelMultiplier, SpeedYDirection);
            this.sprites = new[] { mainSprite, alternateSprite };
        }

        #endregion
    }
}