using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.IO.IsolatedStorage;

namespace XNALib
{
    public struct HighScoreColumn
    {
        public string Name;
        public string Value;

        public HighScoreColumn(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }

    public struct HighScore2
    {
        public string Name;
        public int Score;
        public List<HighScoreColumn> Values;

        public HighScore2(string name, int score, params HighScoreColumn[] values)
        {
            Name = name;
            Score = score;
            Values = new List<HighScoreColumn>();
            Values.AddRange(values);
        }
    }

    /// <summary>
    /// The highest score is placed first in the list.
    /// </summary>
    public class HighScore2Comparer : IComparer<HighScore2>
    {
        public int Compare(HighScore2 x, HighScore2 y)
        {
            return y.Score - x.Score;
        }
    }

    /// <summary>
    /// Compatible with Windows & XBox
    /// Author: Napoleon August 23 2011
    /// Use this class for the saving & loading of highscores.
    /// </summary>
    public class HighScoreMgr2
    {
        public string Path;
        int HighScoreCnt;
        public List<HighScore2> HighScores = null;

#if XBOX
        IsolatedStorageFile FileStorage = IsolatedStorageFile.GetUserStoreForApplication();
#endif

        public HighScoreMgr2(string path, int highScoreCnt)
        {
            Path = path;
            HighScoreCnt = highScoreCnt;
            if (HighScoreCnt <= 0)
                throw new Exception("Number of highscores must be atleast 1.");
#if WINDOWS
            if (File.Exists(Path))
#endif
#if XBOX
            if(FileStorage.FileExists(Path))
#endif
            {
                try
                {
                    Load(Path);
                }
                catch
                {
                    Clear();
                }
            }
            else
                Clear(); // There is no highscore file so create a blank list of highscores
        }

        /// <summary>
        /// Deletes all highscores. Also deletes the scores in the xml file.
        /// </summary>
        public void DeleteAllHighScores()
        {
            Clear();
            Save(Path);
        }

        /// <summary>
        /// Loads current highscores into memory.
        /// </summary>
        public void Load()
        {
            Load(Path);
        }

        /// <summary>
        /// Loads current highscores into memory.
        /// </summary>
        public void Load(string path)
        {
            Clear();

            #if WINDOWS
            if (!File.Exists(Path))
#endif
#if XBOX
            if (!FileStorage.FileExists(Path))
#endif
                return; // Do not load anything when there is no file. Just continue with the blank list in memory.


#if WINDOWS
            XDocument doc = XDocument.Load(Path);
#endif
#if XBOX
            StreamReader stream = new StreamReader(new IsolatedStorageFileStream(Path, FileMode.Open, FileStorage));
            XDocument doc = XDocument.Load(stream);
            stream.Close();
#endif

            XElement scoreMainNode = doc.Root.SelectChildElement("Scores");
            if (scoreMainNode == null)
                throw new NullReferenceException();

            foreach (XElement scoreNode in scoreMainNode.Elements())
            {
                HighScore2 hs = new HighScore2();
                
                // Load Name and score
                hs.Name = scoreNode.Attribute("name").Value;
                hs.Score = int.Parse(scoreNode.Attribute("score").Value);

                // Load Columns
                foreach (XElement colNode in scoreNode.Elements())
                {
                    if (hs.Values == null)
                        hs.Values = new List<HighScoreColumn>();
                    hs.Values.Add(new HighScoreColumn(colNode.Attribute("name").Value, colNode.Attribute("value").Value));
                }

                // Add to HighScores
                HighScores.Add(hs);
            }

            // Sort
            HighScores.Sort(new HighScore2Comparer());
        }

        /// <summary>
        /// Only checks if the score is a highscore but does not save anything.
        /// </summary>
        /// <returns></returns>
        public bool IsHighScore(int score)
        {
            if (HighScores.Count > 0)
                return (score >= HighScores.Last().Score) || (HighScores.Count < HighScoreCnt);
            else
                return true;
        }

        /// <summary>
        /// Adds the score if it is a new highscore
        /// </summary>
        /// <param name="name"></param>
        /// <param name="score"></param>
        /// <param name="dateTime"></param>
        /// <returns>true if it is a highscore or false when the score is not high enough.</returns>
        public bool AttemptScore(string name, int score, params HighScoreColumn[] values)
        {
            if (HighScores.Count < HighScoreCnt) // Always add when the maximum number of highscores has not yet been reached
            {
                HighScores.Add(new HighScore2(name, score, values));
                Save(Path);
                HighScores.Sort(new HighScore2Comparer());
                return true;
            }
            else if (IsHighScore(score)) // Only add if the new score is higher than the lowest highscore
            {
                {
                    HighScores[HighScores.Count - 1] = new HighScore2(name, score, values);
                    Save(Path);
                    HighScores.Sort(new HighScore2Comparer());
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Clears all highscores in memory. Does not clear the highscores in the xml file.
        /// </summary>
        private void Clear()
        {
            HighScores = new List<HighScore2>(HighScoreCnt);
        }

        /// <summary>
        /// Saves the highscores to file.
        /// </summary>
        /// <param name="path"></param>
        private void Save(string path)
        {
            try
            {
                XDocument doc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), new XElement("root"));

                // Main node
                XElement scoreMainNode = new XElement("Scores");
                doc.Root.Add(scoreMainNode);

                foreach (HighScore2 hs in HighScores)
                {
                    // Save Score and Name
                    XElement scoreNode = new XElement("Score",
                        new XAttribute("name", hs.Name),
                        new XAttribute("score", hs.Score)
                        );

                    // Save columns
                    foreach (HighScoreColumn hsc in hs.Values)
                    {
                        XElement scoreValueNode = new XElement("value",
                            new XAttribute("name", hsc.Name),
                            new XAttribute("value", hsc.Value)
                        );
                        scoreNode.Add(scoreValueNode);
                    }

                    // Add node
                    scoreMainNode.Add(scoreNode);
                }


                // Save
#if WINDOWS
                doc.Save(Path, SaveOptions.None);
#endif
#if XBOX
            if (FileStorage.FileExists(Path))
                FileStorage.DeleteFile(Path);
            IsolatedStorageFileStream stream = FileStorage.CreateFile(Path);
            doc.Save(stream);
            stream.Close();
#endif
            }
            catch { }
        }
    }
}
