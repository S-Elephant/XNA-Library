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
    /// <summary>
    /// Use this class for mp3 playback. Only one at a time.
    /// </summary>
    public class MP3MusicMgr
    {
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

        public MP3MusicMgr()
        {

        }

        public void StopMusic()
        {
            MediaPlayer.Stop();
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
            }
        }
    }
}