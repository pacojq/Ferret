using System;
using Microsoft.Xna.Framework.Audio;

namespace FerretEngine.Audio
{
    public class Sound
    {
        public TimeSpan Duration { get; }

        public float Volume
        {
            get => _sfx.Volume;
            set => _sfx.Volume = value;
        }

        public float Pan
        {
            get => _sfx.Pan;
            set => _sfx.Pan = value;
        }
        
        public float Pitch
        {
            get => _sfx.Pitch;
            set => _sfx.Pitch = value;
        }
        
        public bool IsLooped
        {
            get => _sfx.IsLooped;
            set => _sfx.IsLooped = value;
        }



        private readonly SoundEffectInstance _sfx;

        internal Sound(SoundEffectInstance sfx, TimeSpan duration)
        {
            _sfx = sfx;
            Duration = duration;
        }


        public void Play()
        {
            _sfx.Stop();
            _sfx.Play();
        }
        
        public void Resume()
        {
            _sfx.Resume();
        }

        public void Pause()
        {
            _sfx.Pause();
        }

        public void Stop()
        {
            _sfx.Stop();
        }

        public void Stop(bool immediate)
        {
            _sfx.Stop(immediate);
        }
        
    }
}