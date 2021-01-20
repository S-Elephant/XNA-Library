using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace XNALib
{
    public static class IO2
    {
        public static string ReadAllText(string path)
        {
            string line;
            string result = string.Empty;

            StreamReader file = new StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                if (result == string.Empty)
                    result = line;
                else
                    result += Environment.NewLine + line;
            }
            file.Close();
            return result; ;
        }

#if WINDOWS
        public static string MakeSafeDirPath(this string directoryPath)
        {
            string invalidChars = Regex.Escape(new string(Path.GetInvalidPathChars()));
            string invalidReStr = string.Format(@"[{0}]+", invalidChars);
            return Regex.Replace(directoryPath, invalidReStr, "_");
        }

        public static string MakeSafeFilename(this string filename)
        {
            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidReStr = string.Format(@"[{0}]+", invalidChars);
            return Regex.Replace(filename, invalidReStr, "_");
        }
#endif
    }
}
