using System;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace Galaga.Model
{
    /// <summary>
    ///    Represents a sound manager in the Galaga game.
    /// </summary>
    public class SoundManager
    {
        private MediaPlayer mediaPlayer;

        /// <summary>
        ///     SoundManager constructor.
        /// </summary>
        public SoundManager()
        {
            this.mediaPlayer = new MediaPlayer();
        }

        /// <summary>
        ///     Plays the sound of the player firing.
        /// </summary>
        public void playPlayerFiring()
        {
            this.mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/PlayerShipFiring02.wav"));
            this.mediaPlayer.Play();
        }

        /// <summary>
        ///     Plays the sound of the player being destroyed.
        /// </summary>
        public void playPlayerDestroyed()
        {
            this.mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/PlayerShipDestroyed02.wav"));
            this.mediaPlayer.Play(); ;
        }

        /// <summary>
        ///     Plays the sound of the enemy firing.
        /// </summary>
        public void playEnemyFiring()
        {
            this.mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/EnemyShipFiring02.wav"));
            this.mediaPlayer.Play(); ;
        }

        /// <summary>
        ///     Plays the sound of the enemy being destroyed.
        /// </summary>
        public void playEnemyDestroyed()
        {
            this.mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Sounds/EnemyShipDestroyed02.wav"));
            this.mediaPlayer.Play(); ;
        }
    }
}
