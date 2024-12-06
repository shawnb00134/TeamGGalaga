using System;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace Galaga.Model
{
    /// <summary>
    ///     Represents a sound manager in the Galaga game.
    /// </summary>
    public class SoundManager
    {
        #region Data members

        private readonly MediaPlayer mediaPlayer;

        #endregion

        #region Constructors

        /// <summary>
        ///     SoundManager constructor.
        /// </summary>
        public SoundManager()
        {
            mediaPlayer = new MediaPlayer();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Plays the sound of the player firing.
        /// </summary>
        public void playPlayerFiring()
        {
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/PlayerShipFiring02.wav"));
            mediaPlayer.Play();
        }

        /// <summary>
        ///     Plays the sound of the player being destroyed.
        /// </summary>
        public void playPlayerDestroyed()
        {
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/PlayerShipDestroyed02.wav"));
            mediaPlayer.Play();
        }

        /// <summary>
        ///     Plays the sound of the enemy firing.
        /// </summary>
        public void playEnemyFiring()
        {
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/EnemyShipFiring02.wav"));
            mediaPlayer.Play();
        }

        /// <summary>
        ///     Plays the sound of the enemy being destroyed.
        /// </summary>
        public void playEnemyDestroyed()
        {
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/EnemyShipDestroyed02.wav"));
            mediaPlayer.Play();
        }

        /// <summary>
        ///     Plays the bonus ship creation.
        /// </summary>
        public void playBonusShipCreation()
        {
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/bonusShip.wav"));
            mediaPlayer.Play();
        }

        /// <summary>
        ///     Plays the power up.
        /// </summary>
        public void playPowerUp()
        {
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/powerUp.wav"));
            mediaPlayer.Play();
        }

        /// <summary>
        ///     Plays the nuke explosion.
        /// </summary>
        public void playNukeExplosion()
        {
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/nukeExplosion.wav"));
            mediaPlayer.Play();
        }

        #endregion
    }
}