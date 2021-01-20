#if WINDOWS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml.Serialization;
using System.Collections;

namespace XNALib
{
    public class HighScoresComparer : IComparer<HighScore>
    {
        public int Compare(HighScore x, HighScore y)
        {
            return y.Score - x.Score;
        }
    }

    public struct HighScore
    {
        public string Name;
        public int Score;

        public HighScore(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }

    public class HighScoreObject
    {
        public List<HighScore> HighScores { get; set; }
        public int MaxHighScores { get; set; }

        public HighScoreObject()
        {
            HighScores = new List<HighScore>();
        }
    }

    [Obsolete("Use HighScoreMgr2 instead.")]
    public static class HighScoreMgr
    {
        public static HighScoreObject Result = null;
        public static int MaxHighScores = 10;
        private const string path = "HighScores.xml";

        public static void Save()
        {
            #if WINDOWS
                IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForDomain();
            #else
                IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
            #endif

            XmlSerializer serializer = new XmlSerializer(Result.GetType());
            
            // This prevents save-data corruption:
            if (fileStorage.FileExists(path))
                fileStorage.DeleteFile(path);
            
            StreamWriter stream = new StreamWriter(new IsolatedStorageFileStream(path, FileMode.Create, fileStorage));
            try
            {
                serializer.Serialize(stream, Result);
            }
            catch (Exception e)
            {
                throw e;
            }
            stream.Close();
        }

        /// <summary>
        /// Resets the highscores but does NOT save it. Only resets the Result variable.
        /// </summary>
        public static void ResetScores()
        {
            Result = new HighScoreObject();
            Result.MaxHighScores = MaxHighScores;            
        }

        public static void DeleteHighScoreFile()
        {
            #if WINDOWS
                IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForDomain();
            #else
                IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
            #endif
            if (fileStorage.FileExists(path))
                fileStorage.DeleteFile(path);
        }

        public static bool IsHighScore(int score)
        {
            if (Result == null)
                Load();

            if (Result.HighScores.Count == 0)
            {
                if (MaxHighScores > 0)
                    return true;
                else
                    return false;
            }
            else
                return score > Result.HighScores.Last().Score;
        }

        public static void AddHighScore(HighScore newHighScore)
        {
            Result.HighScores.Add(newHighScore);
            Result.HighScores.Sort(new HighScoresComparer());
            
            while(Result.HighScores.Count > MaxHighScores)
                Result.HighScores.RemoveAt(Result.HighScores.Count - 1);

            Save();
        }

        public static bool Load()
        {
            ResetScores();

            #if WINDOWS
                IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForDomain();
            #else
                IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
            #endif

            HighScoreObject SaveObj = new HighScoreObject();

            // Check if file is there
            if (fileStorage.FileExists(path))
            {
                XmlSerializer serializer = new XmlSerializer(SaveObj.GetType());
                StreamReader stream = new StreamReader(new IsolatedStorageFileStream(path, FileMode.Open, fileStorage));
                try
                {
                    SaveObj = (HighScoreObject)serializer.Deserialize(stream);
                    stream.Close();
                    Result = SaveObj;
                    Result.HighScores.Sort(new HighScoresComparer());
                    MaxHighScores = Result.MaxHighScores;
                    return true;
                }
                catch (Exception e)
                {
                    stream.Close();
                    PopupMgr.CreatePopup("ERROR: " + e.Message);
                    return false;
                }
            }
            else
                return false;
        }
    }
}
#endif