using Galaga.View.Sprites;

namespace Galaga.Model
{
    /// <summary>
    ///     Represents a player in the game.
    /// </summary>
    public class Player : GameObject
    {
        #region Data members

        private const int SpeedXDirection = 3;
        private const int SpeedYDirection = 0;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the player lives.
        /// </summary>
        /// <value>
        ///     The player lives.
        /// </value>
        public int PlayerLives { get; private set; } = 3;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Player" /> class.
        /// </summary>
        public Player()
        {
            Sprite = new PlayerSprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Removes a player life.
        /// </summary>
        public void removePlayerLife()
        {
            this.PlayerLives = this.PlayerLives - 1;
        }

        #endregion
    }
}