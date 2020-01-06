using Microsoft.Xna.Framework.Media;

namespace FerretEngine.Audio
{
    /// <summary>
    /// Unlike <see cref="Sound"/>, there can only be one Music track
    /// playing at the same time.
    /// </summary>
    public class Music
    {
        /// <summary>
        /// The Music track that is currently playing.
        /// </summary>
        public static Music CurrentPlaying { get; private set; }
        
        public bool IsLooped { get; set; }
        
        private readonly Song _song;
        
        
        
        
        internal  Music(Song song)
        {
            _song = song;
        }

        internal void Prepare()
        {
            if (CurrentPlaying != this)
            {
                if (CurrentPlaying != null)
                    CurrentPlaying.Pause();
            }

            CurrentPlaying = this;
            
            MediaPlayer.IsRepeating = IsLooped;
            MediaPlayer.Volume = 1f; // TODO Volume * MasterVolume;
        }

        public void Play()
        {
            
        }


        public void Pause()
        {
            
        }
        
        public void Stop()
        {
            /*
            if (MediaPlayer.State != MediaState.Playing) {
                MediaPlayer.Stop();
            }

            CurrentTime = TimeSpan.FromSeconds(0);
            IsPaused = false;
            IsStopped = true;
            */
        }
    }
}