using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;
using Microsoft.Xna.Framework.Media;

namespace XNALib
{
    public enum eMusicPlayback { RepeatSong, RepeatAll, PlaySongOnce }
    /// <summary>
    /// Use this class for mp3 playback. Only one at a time.
    /// </summary>
    public class MP3MusicMgr
    {
        #region Members
        public static MP3MusicMgr Instance = new MP3MusicMgr();
        public string MusicFolder = "MP3/";
        Song ActiveMusic = null;

        private bool m_EnableMusic = true;
        public bool EnableMusic
        {
            get { return m_EnableMusic; }
            set
            {
                m_EnableMusic = value;
                if (!value)
                    StopMusic();
            }
        }

        private TimeSpan m_SongElapsed = new TimeSpan();
        public TimeSpan SongElapsed
        {
            get { return m_SongElapsed; }
            private set { m_SongElapsed = value; }
        }
        #endregion

        public MP3MusicMgr()
        {

        }

        public void StopMusic()
        {
            MediaPlayer.Stop();
            SongElapsed = new TimeSpan();
        }

        /// <summary>
        /// Plays the music looped.
        /// </summary>
        /// <param name="name"></param>
        public void PlayMusic(string name)
        {
            if (EnableMusic)
            {
                MediaPlayer.IsRepeating = true;
                ActiveMusic = Global.Content.Load<Song>(MusicFolder + name);
                MediaPlayer.Play(ActiveMusic);
                SongElapsed = new TimeSpan();
            }
        }

        /// <summary>
        /// Plays the music once.
        /// </summary>
        /// <param name="name"></param>
        public void PlayMusicOnce(string name)
        {
            if (EnableMusic)
            {
                MediaPlayer.IsRepeating = false;
                ActiveMusic = Global.Content.Load<Song>(MusicFolder + name);
                MediaPlayer.Play(ActiveMusic);
                SongElapsed = new TimeSpan();
            }
        }

        /// <summary>
        /// Sets the volume of the music.
        /// </summary>
        /// <param name="volume">The volume to set.</param>
        public void SetMusicVolume(float volume)
        {
            if (EnableMusic)
            {
                MediaPlayer.Volume = volume;
            }
        }

        public void Update(GameTime gameTime)
        {
            SongElapsed += gameTime.ElapsedGameTime;
        }
    }
}