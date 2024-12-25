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
            this.mediaPlayer = new MediaPlayer();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Plays the sound of the player firing.
        /// </summary>
        public void PlayPlayerFiring()
        {
            this.mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/PlayerShipFiring02.wav"));
            this.mediaPlayer.Play();
        }

        /// <summary>
        ///     Plays the sound of the player being destroyed.
        /// </summary>
        public void PlayPlayerDestroyed()
        {
            this.mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/PlayerShipDestroyed02.wav"));
            this.mediaPlayer.Play();
        }

        /// <summary>
        ///     Plays the sound of the enemy firing.
        /// </summary>
        public void PlayEnemyFiring()
        {
            this.mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/EnemyShipFiring02.wav"));
            this.mediaPlayer.Play();
        }

        /// <summary>
        ///     Plays the sound of the enemy being destroyed.
        /// </summary>
        public void PlayEnemyDestroyed()
        {
            this.mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/EnemyShipDestroyed02.wav"));
            this.mediaPlayer.Play();
        }

        /// <summary>
        ///     Plays the bonus ship creation.
        /// </summary>
        public void PlayBonusShipCreation()
        {
            this.mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/bonusShip.wav"));
            this.mediaPlayer.Play();
        }

        /// <summary>
        ///     Plays the power up.
        /// </summary>
        public void PlayPowerUp()
        {
            this.mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/powerUp.wav"));
            this.mediaPlayer.Play();
        }

        /// <summary>
        ///     Plays the nuke explosion.
        /// </summary>
        public void PlayNukeExplosion()
        {
            this.mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/nukeExplosion.wav"));
            this.mediaPlayer.Play();
        }

        #endregion
    }
}