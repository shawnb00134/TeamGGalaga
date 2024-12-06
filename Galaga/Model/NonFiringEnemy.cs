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
        #region Properties

        /// <summary>
        ///     Gets the score value.
        /// </summary>
        /// <value>
        ///     The score value.
        /// </value>
        public override int ScoreValue { get; set; }

        #endregion

        #region Data members

        private const int SpeedXDirection = 3;
        private const int SpeedYDirection = 0;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="NonFiringEnemy" /> class.
        /// </summary>
        public NonFiringEnemy(BaseSprite mainSprite, BaseSprite alternateSprite, int levelMultiplier, Canvas canvas) :
            base(mainSprite,
                alternateSprite, levelMultiplier, canvas)
        {
            Sprite = mainSprite;
            SetSpeed(SpeedXDirection * levelMultiplier, SpeedYDirection);
            Sprites = new[] { mainSprite, alternateSprite };
        }

        /// <summary>
        ///     Swaps the Sprites.
        /// </summary>
        public override void SwapSprites()
        {
            if (Sprites[0].Visibility == Visibility.Visible)
            {
                Sprites[0].Visibility = Visibility.Collapsed;
                Sprites[1].Visibility = Visibility.Visible;
                Sprite = Sprites[1];
            }
            else
            {
                Sprites[0].Visibility = Visibility.Visible;
                Sprites[1].Visibility = Visibility.Collapsed;
                Sprite = Sprites[0];
            }
        }

        #endregion
    }
}