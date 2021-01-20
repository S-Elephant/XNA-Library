using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace XNALib
{
    public class VideoSettings
    {
        public Video Video;
        public bool Loop;
        public VideoPlayer VideoPlayer;
        public Rectangle DrawRectangle;
        public bool IsFullScreen;

        private TimeSpan m_PlayPosition;
        public TimeSpan PlayPosition
        {
            get { return m_PlayPosition; }
            internal set { m_PlayPosition = value; } 
        }

        public VideoSettings(Video video, bool loop, Rectangle drawRectangle, bool isFullScreen)
        {
            Video = video;
            Loop = loop;
            VideoPlayer = new VideoPlayer();
            DrawRectangle = drawRectangle;
            IsFullScreen = isFullScreen;
            m_PlayPosition = new TimeSpan();
        }

        public void Restart()
        {
            VideoPlayer.Stop();
            VideoPlayer.Play(Video);
            PlayPosition = new TimeSpan();
        }

        public void Update(GameTime gameTime)
        {
            if (VideoPlayer.State == MediaState.Playing)
                PlayPosition += gameTime.ElapsedGameTime;
        }
    }

    public static class VideoMgr
    {
        public delegate void OnVideoFinishPlaying(VideoSettings videoSettings);
        public static event OnVideoFinishPlaying VideoFinishPlaying;

        public static string VideoDir = "Videos/";
        public static Dictionary<string, VideoSettings> Videos = new Dictionary<string, VideoSettings>();

        /// <summary>
        /// Fullscreen constructor
        /// </summary>
        /// <param name="videoAssetName"></param>
        /// <param name="loop"></param>
        public static void Play(string videoKey, string videoAssetName, bool loop)
        {
            AddVideo(videoKey, videoAssetName, loop, Rectangle.Empty, true);
            Videos[videoKey].VideoPlayer.Play(Videos[videoKey].Video);
        }
        /// <summary>
        /// Non-fullscreen constructor
        /// </summary>
        /// <param name="videoAssetName"></param>
        /// <param name="loop"></param>
        /// <param name="drawRectangle"></param>
        public static void Play(string videoKey, string videoAssetName, bool loop, Rectangle drawRectangle)
        {
            AddVideo(videoKey, videoAssetName, loop, drawRectangle, false);
            Videos[videoKey].VideoPlayer.Play(Videos[videoKey].Video);
        }

        public static void AddVideo(string key, string videoAssetName, bool loop, Rectangle drawRectangle, bool isFullScreen)
        {
            Videos.Add(key, new VideoSettings(Global.Content.Load<Video>(VideoDir + videoAssetName), loop, drawRectangle, isFullScreen));
        }

        public static void Stop(string videoKey)
        {
            Videos[videoKey].VideoPlayer.Stop();
        }

        public static void Update(GameTime gameTime)
        {
            Videos["intro"].Update(gameTime);
            foreach (VideoSettings v in Videos.Values)
                v.Update(gameTime);
        }

        [Obsolete("videoSetting.VideoPlayer.PlayPosition and its state throw crossthreading exceptions?")]
        public static void Draw(SpriteBatch spriteBatch, Size fullScreenSize)
        {
            foreach (VideoSettings videoSetting in Videos.Values)
            {
                if (videoSetting.VideoPlayer.State == MediaState.Playing)
                {
                    if (!videoSetting.IsFullScreen)
                        spriteBatch.Draw(videoSetting.VideoPlayer.GetTexture(), videoSetting.DrawRectangle, Color.White);
                    else
                        spriteBatch.Draw(videoSetting.VideoPlayer.GetTexture(), new Rectangle(0, 0, fullScreenSize.Width, fullScreenSize.Height), Color.White);
                }
                if (videoSetting.PlayPosition.TotalMilliseconds >= videoSetting.Video.Duration.TotalMilliseconds)
                {
                    if (videoSetting.Loop)
                    {
                        videoSetting.Restart();
                    }
                    else
                    {
                        videoSetting.VideoPlayer.Stop();
                        if (VideoFinishPlaying != null)
                            VideoFinishPlaying(videoSetting);
                    }
                }
            }
        }
    }
}
