﻿using System.Collections.Generic;
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
            this.canvasHeight = canvas.Height;
            this.canvasWidth = canvas.Width;
            this.player = new Player();
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
            this.canvas.Children.Add(this.player.Sprite);
            shipList.Add(this.player);

            this.placePlayerNearBottomOfBackgroundCentered();

            return shipList;
        }

        private void placePlayerNearBottomOfBackgroundCentered()
        {
            this.player.X = this.canvasWidth / 2 - this.player.Width / 2.0;
            this.player.Y = this.canvasHeight - this.player.Height - PlayerOffsetFromBottom;
        }

        /// <summary>
        ///     Moves the player left.
        /// </summary>
        public void MovePlayerLeft()
        {
            if (this.player.X <= PlayerSpeedBoundary)
            {
                this.player.X = PlayerSpeedBoundary;
            }

            this.player.MoveLeft();
        }

        /// <summary>
        ///     Moves the player right.
        /// </summary>
        public void MovePlayerRight()
        {
            if (this.player.X >= this.canvasWidth - this.player.Width - PlayerSpeedBoundary)
            {
                this.player.X = this.canvasWidth - this.player.Width - PlayerSpeedBoundary;
            }

            this.player.MoveRight();
        }

        /// <summary>
        ///     Checks the player lives.
        /// </summary>
        /// <param name="playerObject">The player object.</param>
        /// <param name="listOfShips">The list of ships.</param>
        /// <returns>List of GameObjects ships</returns>
        public IList<GameObject> CheckPlayerLives(GameObject playerObject, IList<GameObject> listOfShips)
        {
            this.player.RemovePlayerLife();

            this.canvas.Children.Remove(playerObject.Sprite);
            listOfShips.Remove(playerObject);

            if (this.player.PlayerLives > 0)
            {
                return this.CreateAndPlacePlayer(listOfShips);
            }

            return null;
        }

        /// <summary>
        ///     Gets the player lives count.
        /// </summary>
        /// <returns></returns>
        public int GetPlayerLivesCount()
        {
            return this.player.PlayerLives;
        }

        /// <summary>
        ///     Returns the player object
        /// </summary>
        /// <returns></returns>
        public GameObject GetPlayer()
        {
            return this.player;
        }

        /// <summary>
        ///     Adds the player life.
        /// </summary>
        public void AddPlayerLife()
        {
            this.player.AddPlayerLife();
        }

        #endregion
    }
}