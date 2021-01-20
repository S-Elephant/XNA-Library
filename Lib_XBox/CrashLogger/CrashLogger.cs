#if WINDOWS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace XNALib
{
    /// <summary>
    /// Only compatible with Windows, NOT with the XBOX.
    /// </summary>
    public class CrashLogger
    {
        #region Members
        /// <summary>
        /// Singleton instance
        /// </summary>
        public static CrashLogger Instance = null;

        /// <summary>
        /// Indicates whether or not the game/application crashed.
        /// </summary>
        public bool HasBeenCrashed = false;

        public Exception CrashException = null;

        public CrashScreen CrashScreen;

        #region Path
        private string m_LogDirectoryPath = "Logs/";
        public string LogDirectoryPath
        {
            get { return m_LogDirectoryPath; }
            set
            {
                m_LogDirectoryPath = value;
                if (!Directory.Exists(value))
                    Directory.CreateDirectory(value);
                m_LogPath = string.Format("{0}{1}", value, LogFileName);
            }
        }
        private string m_LogFileName = "Log.txt";
        public string LogFileName
        {
            get { return m_LogFileName; }
            set
            {
                m_LogFileName = value;
                m_LogPath = string.Format("{0}{1}", LogDirectoryPath, value);
            }
        }
        private string m_LogPath = "Logs/Log.txt";
        public string LogPath { get { return m_LogPath; } }
        private string VersionStr;
        #endregion
        #endregion

        public CrashLogger(SpriteBatch spritebatch, SpriteFont crashFont, Rectangle screenArea, Game game, Version appVersion)
        {
            VersionStr = appVersion.ToString();
            CrashScreen = new CrashScreen(crashFont, screenArea, game, spritebatch);
            
            if (!Directory.Exists(LogDirectoryPath))
                Directory.CreateDirectory(LogDirectoryPath);
        }

        private string GetStamps()
        {
            return string.Format("DateTime: {1}{0}Version: {2}{0}Message: ", Environment.NewLine, DateTime.Now, VersionStr);
        }

        public void Log(string message)
        {
            string text = string.Empty;
            
            if(File.Exists(LogPath))
                text = File.ReadAllText(LogPath);
            
            text = string.Format("{0}{1}{2}{2}{3}", GetStamps(), message, Environment.NewLine, text);
            File.WriteAllText(LogPath, text);
        }

        public void Log(Exception ex)
        {
            Log(ex.ToString());
        }

        public void Crash(Exception ex)
        {
            CrashException = ex;
            HasBeenCrashed = true;
            CrashScreen.SetMessage(ex);
            Log(ex);
        }

        public void Crash(string message)
        {
            HasBeenCrashed = true;
            CrashScreen.SetMessage(message);
        }
    }
}
#endif