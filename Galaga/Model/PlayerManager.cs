using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace Galaga.Model
{
    /// <summary>
    ///     Managers the Player object
    /// </summary>
    public class PlayerManager
    {
        #region Data members

        private const int PlayerSpeedBoundary = 3;
        private const double PlayerOffsetFromBottom = 30;
        private readonly double canvasHeight;
        private readonly double canvasWidth;

        private readonly Canvas canvas;
        private readonly Player player;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerManager" /> class.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        public PlayerManager(Canvas canvas)
        {
            this.canvas = canvas;
            canvasHeight = canvas.Height;
            canvasWidth = canvas.Width;
            player = new Player();
        }

        #endregion
        
        #region Methods

        /// <summary>
        ///     Creates and place player.
        /// </summary>
        /// <param name="shipList">The ship list.</param>
        /// <returns></returns>
        public IList<GameObject> CreateAndPlacePlayer(IList<GameObject> shipList)
        {
            canvas.Children.Add(player.Sprite);
            shipList.Add(player);

            placePlayerNearBottomOfBackgroundCentered();

            return shipList;
        }

        private void placePlayerNearBottomOfBackgroundCentered()
        {
            player.X = canvasWidth / 2 - player.Width / 2.0;
            player.Y = canvasHeight - player.Height - PlayerOffsetFromBottom;
        }

        /// <summary>
        ///     Moves the player left.
        /// </summary>
        public void MovePlayerLeft()
        {
            if (player.X <= PlayerSpeedBoundary) player.X = PlayerSpeedBoundary;

            player.MoveLeft();
        }

        /// <summary>
        ///     Moves the player right.
        /// </summary>
        public void MovePlayerRight()
        {
            if (player.X >= canvasWidth - player.Width - PlayerSpeedBoundary)
                player.X = canvasWidth - player.Width - PlayerSpeedBoundary;

            player.MoveRight();
        }

        /// <summary>
        ///     Checks the player lives.
        /// </summary>
        /// <param name="playerObject">The player object.</param>
        /// <param name="listOfShips">The list of ships.</param>
        /// <returns>List of GameObjects ships</returns>
        public IList<GameObject> CheckPlayerLives(GameObject playerObject, IList<GameObject> listOfShips)
        {
            player.removePlayerLife();

            canvas.Children.Remove(playerObject.Sprite);
            listOfShips.Remove(playerObject);

            if (player.PlayerLives > 0) return CreateAndPlacePlayer(listOfShips);

            return null;
        }

        /// <summary>
        ///     Gets the player lives count.
        /// </summary>
        /// <returns></returns>
        public int GetPlayerLivesCount()
        {
            return player.PlayerLives;
        }

        /// <summary>
        ///     Returns the player object
        /// </summary>
        /// <returns></returns>
        public GameObject GetPlayer()
        {
            return player;
        }

        /// <summary>
        ///     Adds the player life.
        /// </summary>
        public void AddPlayerLife()
        {
            player.addPlayerLife();
        }

        #endregion
    }
}