using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNALib
{
    public struct Size : IEquatable<Size>
    {
        public int Width, Height;
        public static Size Empty { get { return new Size(0, 0); } }
        public static Size Invalid { get { return new Size(int.MinValue, int.MinValue); } }
        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public static Size operator +(Size s1, Size s2)
        {
            return new Size(s1.Width + s2.Width, s1.Height + s2.Height);
        }

        public static Size operator -(Size s1, Size s2)
        {
            return new Size(s1.Width - s2.Width, s1.Height - s2.Height);
        }

        public static bool operator ==(Size s1, Size s2)
        {
            return (s1.Width == s2.Width) && (s1.Height == s2.Height);
        }

        public static bool operator !=(Size s1, Size s2)
        {
            return !(s1 == s2);
        }

        public bool Equals(Size other)
        {
            return (Width == other.Width) && (Height == other.Height) && (GetHashCode() == other.GetHashCode());
        }

        //http://msdn.microsoft.com/en-us/library/336aedhh%28v=vs.71%29.aspx
        public override bool Equals(object o)
        {
            return o is Size && this == (Size)o;
        }

        //http://msdn.microsoft.com/en-us/library/336aedhh%28v=vs.71%29.aspx
        public override int GetHashCode()
        {
            return Width.GetHashCode() ^ Height.GetHashCode();
        }

        public static Size Str2Size(string str)
        {
            string[] temp = str.Split(';');
            return new Size(int.Parse(temp[0]), int.Parse(temp[1]));
        }

        public string ToStringForSaving()
        {
            return string.Format("{0};{1}", Width, Height);
        }
    }
}
