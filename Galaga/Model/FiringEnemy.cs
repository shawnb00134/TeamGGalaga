using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Galaga.View.Sprites;

namespace Galaga.Model
{
    /// <summary>
    ///     Class for a Level 3 Enemy.
    /// </summary>
    /// <seealso cref="Galaga.Model.EnemyShip" />
    public class FiringEnemy : EnemyShip
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

        /// <summary>
        ///    Initializes a new instance of the <see cref="FiringEnemy" /> class.
        /// </summary>
        /// <param name="mainSprite"></param>
        /// <param name="speed"></param>
        /// <param name="canvas"></param>
        public FiringEnemy(BaseSprite mainSprite, int speed, Canvas canvas) : base(mainSprite, speed, canvas)
        {
            this.canvas = canvas;
            SetSpeed(speed, SpeedYDirection);
            this.sprites = new[] { mainSprite, mainSprite };
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FiringEnemy" /> class.
        /// </summary>
        public FiringEnemy(BaseSprite mainSprite, BaseSprite alternateSprite, int levelMultiplier, Canvas canvas) : base(mainSprite,
            alternateSprite, levelMultiplier, canvas)
        {
            Sprite = mainSprite;
            SetSpeed(SpeedXDirection * levelMultiplier, SpeedYDirection);
            this.sprites = new[] { mainSprite, alternateSprite };
        }

        #endregion

    }
}