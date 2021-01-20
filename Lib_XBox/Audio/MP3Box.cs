using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;

namespace XNALib
{
    /// <summary>
    /// Contains information about the song
    /// </summary>
    public class BGSong
    {
        public string Filename;
        public StringBuilder DisplayName;

        public BGSong(string filename, string displayName)
        {
            Filename = filename;
            DisplayName = new StringBuilder(displayName);
        }
    }

    /// <summary>
    /// This singleton handles the ingame MP3 music player.
    /// </summary>
    public class MP3Box
    {
        #region Members
        /// <summary>
        /// Singleton instance
        /// </summary>
        public static MP3Box Instance = new MP3Box();

        /// <summary>
        /// The playback type
        /// </summary>
        private eMusicPlayback PlaybackType = eMusicPlayback.RepeatSong;

        /// <summary>
        /// The X-offsets before and after the song's name for the arrows
        /// </summary>
        const int DisplayNameOffsetsX = 4;

        // Textures
        private static Texture2D LeftArrow;
        private static Texture2D RightArrow;

        // Font
        SpriteFont Font;

        /// <summary>
        /// The list of available songs
        /// </summary>
        public List<BGSong> Songs = new List<BGSong>();

        private int m_SongIdx = 0;
        /// <summary>
        /// The currently active song by index
        /// </summary>
        private int SongIdx
        {
            get { return m_SongIdx; }
            set
            {
                if (value < 0)
                    m_SongIdx = Songs.Count - 1;
                else if (value == Songs.Count)
                    m_SongIdx = 0;
                else
                    m_SongIdx = value;
            }
        }

        /// <summary>
        /// The MP3 'box' is visible while this timer is running
        /// </summary>
        SimpleTimer DrawBoxTimer = new SimpleTimer(3000);

        /// <summary>
        /// The active song
        /// </summary>
        private BGSong ActiveSong { get { return Songs[SongIdx]; } }

        #endregion

        public MP3Box()
        {
            DrawBoxTimer.IsDone = true;
        }

        public void Initialize(string font, string leftArrowText, string rightArrowTex, params BGSong[] songs)
        {
            Font = Common.str2Font(font);
            LeftArrow = Common.str2Tex(leftArrowText);
            RightArrow = Common.str2Tex(rightArrowTex);

            Songs = new List<BGSong>(songs);
        }

        /// <summary>
        /// Stops the music
        /// </summary>
        public void StopMusic()
        {
            MP3MusicMgr.Instance.StopMusic();
        }

        /// <summary>
        /// Plays song by name. Also sets the active song to this one.
        /// </summary>
        /// <param name="name">Filename of the song.</param>
        public void PlaySong(string name)
        {
            SongIdx = Songs.FindIndex(s => s.Filename == name);
            Play();
        }

        /// <summary>
        /// Plays the next song in the list
        /// </summary>
        public void NextSong()
        {
            SongIdx++;
            Play();
        }

        /// <summary>
        /// Plays the previous song in the list
        /// </summary>
        public void PreviousSong()
        {
            SongIdx--;
            Play();
        }

        void Play()
        {
            if (MP3MusicMgr.Instance.EnableMusic)
            {
                if (PlaybackType == eMusicPlayback.RepeatSong)
                    MP3MusicMgr.Instance.PlayMusic(ActiveSong.Filename);
                else
                    MP3MusicMgr.Instance.PlayMusicOnce(ActiveSong.Filename);

                DrawBoxTimer.Reset();
            }
        }

        public void Update(GameTime gameTime)
        {
            DrawBoxTimer.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            if (!DrawBoxTimer.IsDone)
            {
                float halfTextWidth = Font.MeasureString(ActiveSong.DisplayName).X / 2;
                spriteBatch.Draw(LeftArrow, new Vector2(screenWidth / 2 - halfTextWidth - 24 - 4, screenHeight - 28), Color.White);
                spriteBatch.DrawString(Font, ActiveSong.DisplayName, new Vector2(screenWidth / 2 - halfTextWidth, screenHeight - 32), Color.White);
                spriteBatch.Draw(RightArrow, new Vector2(screenWidth / 2 + halfTextWidth + 4, screenHeight - 28), Color.White);
            }
        }
    }
}