using System;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;

namespace XNALib
{
    public static class Misc
    {
        /// <summary>
        ///  Swaps characters in a string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="position1"></param>
        /// <param name="position2"></param>
        /// <returns></returns>
        public static StringBuilder SwapCharacters(StringBuilder value, int position1, int position2)
        {
            StringBuilder result = new StringBuilder(value.ToString());
            char c1 = result[position1];
            result[position1] = value[position2];
            result[position2] = c1;
            return result;
        }

        /// <summary>
        /// Creates a new rectangle based upon a center point so that the point will be the center of the new rectangle.
        /// </summary>
        /// <param name="centerLoc"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Rectangle RectFromCenter(Vector2 centerLoc, int width, int height)
        {
            return new Rectangle(centerLoc.Xi() - width / 2, centerLoc.Yi() - height / 2, width, height);
        }

        public static Vector2 RandomV2(int minX, int maxX, int minY, int maxY)
        {
            return new Vector2(Maths.RandomNr(minX, maxX), Maths.RandomNr(minY, maxY));
        }

        public static Color RandomColor()
        {
            return new Color(Maths.RandomNr(0, 255), Maths.RandomNr(0, 255), Maths.RandomNr(0, 255));
        }

        public static Color RandomTransColor()
        {
            return new Color(Maths.RandomNr(0, 255), Maths.RandomNr(0, 255), Maths.RandomNr(0, 255), Maths.RandomNr(0, 255));
        }

        public static List<string> GetSupportedResolutions()
        {
            List<string> result = new List<string>();
            foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                result.Add(string.Format("{0}x{1}", dm.Width, dm.Height));
            }
            result = result.RemoveDuplicates();
            return result;
        }

        /// <summary>
        /// Replacement for Enum.GetValues()
        /// Usage: foreach (DayOfWeek dayOfWeek in Misc.GetValues(new DayOfWeek())) {}
        /// http://ideas.dalezak.ca/2008/11/enumgetvalues-in-compact-framework.html
        /// </summary>
        /// <param name="enumeration"></param>
        /// <returns></returns>
        public static IEnumerable<Enum> GetValues(Enum enumeration)
        {
            List<Enum> enumerations = new List<Enum>();
            foreach (FieldInfo fieldInfo in enumeration.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumerations.Add((Enum)fieldInfo.GetValue(enumeration));
            }
            return enumerations;
        }

        /// <summary>
        /// http://www.xnawiki.com/index.php?title=Basic_Word_Wrapping
        /// Produces garbage!
        /// </summary>
        /// <param name="font"></param>
        /// <param name="text"></param>
        /// <param name="maxLineWidth"></param>
        /// <returns></returns>
        public static StringBuilder WrapText(SpriteFont font, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');
            StringBuilder sb = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = font.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = font.MeasureString(word);
                bool containsNewLineConstant = word.Contains(Environment.NewLine);
                if (lineWidth + size.X < maxLineWidth && !containsNewLineConstant)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    if(!containsNewLineConstant)
                        sb.Append("\n" + word + " ");
                    else
                        sb.Append(word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }

            return sb;
        }

        public static Vector2 Convert3To2(Vector3 convertThis)
        {
            return new Vector2(convertThis.X, convertThis.Y);
        }

        public static float DistanceXY(Vector3 A, Vector3 B)
        {
            Vector2 a = new Vector2(A.X, A.Y);
            Vector2 b = new Vector2(B.X, B.Y);
            return Vector2.Distance(a, b);
        }

        public static Rectangle NewRect(Vector2 v, int width, int height)
        {
            return new Rectangle(v.Xi(), v.Yi(), width, height);
        }


        #region Romans
        private static int[] values = new int[] { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
        private static string[] numerals = new string[] { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
        /// <summary>
        /// Example: Converts 7 to XII
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        /// <remarks>http://www.blackwasp.co.uk/NumberToRoman.aspx</remarks>
        public static string NumberToRoman(int nr)
        {
            // Validate
            if (nr < 0 || nr > 3999)
            {
                throw new ArgumentException("Value must be between 0 - 3999");
            }

            if (nr == 0)
                return "N";

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < 13; i++)
            {
                // If the number being converted is less than the test value, append
                // the corresponding numeral or numeral pair to the resultant string
                while (nr >= values[i])
                {
                    nr -= values[i];
                    result.Append(numerals[i]);
                }
            }
            return result.ToString();
        }

        private enum RomanDigit
        {
            I = 1,
            V = 5,
            X = 10,
            L = 50,
            C = 100,
            D = 500,
            M = 1000
        }
        /// <summary>
        /// Example: Converts VII to 7
        /// </summary>
        /// <param name="roman"></param>
        /// <returns></returns>
        /// <remarks>http://www.blackwasp.co.uk/RomanToNumber.aspx</remarks>
        public static int RomanToNumber(string roman)
        {
            roman = roman.ToUpper().Trim();
            if (roman == "N")
                return 0;
            if (roman.Split('V').Length > 2 || roman.Split('L').Length > 2 || roman.Split('D').Length > 2)
                throw new ArgumentException("The numerals that represent numbers beginning with a '5' (V, L and D) may only appear once in each Roman numeral. This rule permits XVI but not VIV.");

            int count = 1;
            char last = 'Z';
            foreach (char numeral in roman)
            {
                // Valid character?
                if ("IVXLCDM".IndexOf(numeral) == -1)
                {
                    throw new ArgumentException("Invalid numeral");
                }

                // Duplicate?
                if (numeral == last)
                {
                    count++;
                    if (count == 4)
                    {
                        throw new ArgumentException("A single letter may be repeated up to three times consecutively with each occurrence of the value being additive. This means that I is one, II means two and III is three. However, IIII is incorrect for four.");
                    }
                }
                else
                {
                    count = 1;
                    last = numeral;
                }
            }

            // Create an ArrayList containing the values
            int ptr = 0;
            List<int> values = new List<int>();
            int maxDigit = 1000;
            while (ptr < roman.Length)
            {
                // Base value of digit
                char numeral = roman[ptr];
                int digit = (int)Enum.Parse(typeof(RomanDigit), numeral.ToString(), true);

                // Rule 3
                if (digit > maxDigit)
                {
                    throw new ArgumentException("Rule 3");
                }

                // Next digit
                int nextDigit = 0;
                if (ptr < roman.Length - 1)
                {
                    char nextNumeral = roman[ptr + 1];
                    nextDigit = (int)Enum.Parse(typeof(RomanDigit), nextNumeral.ToString(), true);

                    if (nextDigit > digit)
                    {
                        if ("IXC".IndexOf(numeral) == -1 ||
                            nextDigit > (digit * 10) ||
                            roman.Split(numeral).Length > 3)
                        {
                            throw new ArgumentException("Rule 3");
                        }

                        maxDigit = digit - 1;
                        digit = nextDigit - digit;
                        ptr++;
                    }
                }

                values.Add(digit);

                // Next digit
                ptr++;
            }

            // Rule 5
            for (int i = 0; i < values.Count - 1; i++)
            {
                if ((int)values[i] < (int)values[i + 1])
                {
                    throw new ArgumentException("Rule 5");
                }
            }

            // Rule 2
            int total = 0;
            foreach (int digit in values)
            {
                total += digit;
            }
            return total;
        }
        #endregion

        public static string Direction8ToString(Vector2 direction)
        {
            //-1-1	0-1 	1-1
            //-10	00	10
            //-11	01	11
            int x = (int)Math.Round(direction.X);
            int y = (int)Math.Round(direction.Y);

            if (x == -1 && y == -1)
                return "NW";
            if (x == 0 && y == -1)
                return "N";
            if (x == 1 && y == -1)
                return "NE";
            if (x == 1 && y == 0)
                return "E";
            if (x == 1 && y == 1)
                return "SE";
            if (x == 0 && y == 1)
                return "S";
            if (x == -1 && y == 1)
                return "SW";
            if (x == -1 && y == 0)
                return "W";
            if (direction == Vector2.Zero)
                return null;

            throw new Exception("Invalid movedirection");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="moveDir"></param>
        /// <returns>0 = N, 1 = NE, 2 = E etc.</returns>
        public static int GetAnimationDirection(Vector2 moveDir, float tolerance)
        {
            if (moveDir.X >= -tolerance && moveDir.X <= tolerance && moveDir.Y < tolerance)
                return 0; // N

            if (moveDir.X > tolerance && moveDir.Y < -tolerance)
                return 1; // NE

            if (moveDir.X >= tolerance && moveDir.Y > -tolerance && moveDir.Y < tolerance)
                return 2; // E

            if (moveDir.X > tolerance && moveDir.Y > tolerance)
                return 3; // SE

            if (moveDir.X >= -tolerance && moveDir.X <= tolerance && moveDir.Y > tolerance)
                return 4; // S

            if (moveDir.X < tolerance && moveDir.Y > tolerance)
                return 5; // SW

            if (moveDir.X < -tolerance && moveDir.Y > -tolerance && moveDir.Y < tolerance)
                return 6; // W

            return 7; // NW
        }
    }
}