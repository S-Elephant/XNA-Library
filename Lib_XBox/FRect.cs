// // // // // // // // // // // // //
// QuadTree and supporting code
// by Kyle Schouviller
// http://www.kyleschouviller.com
//
// December 2006: Original version
// May 06, 2007:  Updated for XNA Framework 1.0
//                and public release.
//
// You may use and modify this code however you
// wish, under the following condition:
// *) I must be credited
// A little line in the credits is all I ask -
// to show your appreciation.
// 
// If you have any questions, please use the
// contact form on my website.
//
// Now get back to making great games!
// // // // // // // // // // // // //
// Updated and changed by Napoleon.


#region Using declarations

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

#endregion

namespace XNALib
{
    /// <summary>
    /// A floating-point rectangle
    /// </summary>
    public struct FRect
    {
        #region Properties

        // Actual variables:
        public Vector2 TopLeft;
        public float Width;
        public float Height;

        public float X
        {
            get { return TopLeft.X; }
            set { TopLeft.X = value; }
        }
        public float Y
        {
            get { return TopLeft.Y; }
            set { TopLeft.Y = value; }
        }

        /// <summary>
        /// Gets the top right of this rectangle
        /// </summary>
        public Vector2 TopRight
        {
            get { return new Vector2(TopLeft.X + Width, TopLeft.Y); }
        }

        /// <summary>
        /// Gets the bottom right of this rectangle
        /// </summary>
        public Vector2 BottomRight
        {
            get { return new Vector2(TopLeft.X + Width, TopLeft.Y + Height); }
        }

        public Vector2 CenterLoc
        {
            get { return (TopLeft + BottomRight) / 2; }
        }

        /// <summary>
        /// Gets the bottom left of this rectangle
        /// </summary>
        public Vector2 BottomLeft
        {
            get { return new Vector2(TopLeft.X, TopLeft.Y + Height); }
        }

        /// <summary>
        /// Gets the top of this rectangle
        /// </summary>
        public float Top
        {
            get { return TopLeft.Y; }
            set { TopLeft.Y = value; }
        }

        /// <summary>
        /// Gets the left of this rectangle
        /// </summary>
        public float Left
        {
            get { return TopLeft.X; }
            set { TopLeft.X = value; }
        }

        /// <summary>
        /// Gets the bottom of this rectangle
        /// </summary>
        public float Bottom
        {
            get { return BottomRight.Y; }
        }

        /// <summary>
        /// Gets the right of this rectangle
        /// </summary>
        public float Right
        {
            get { return BottomRight.X; }
        }

        public bool IsValid
        {
            get { return Width >= 0 && Height >= 0; }
        }

        /// <summary>
        /// Returns a new FRect with all values set to 0.
        /// </summary>
        public static FRect Empty
        {
            get { return new FRect(0, 0, 0, 0); }
        }
        #endregion

        #region Initialization

        /// <summary>
        /// Floating-point rectangle constructor
        /// </summary>
        /// <param name="topleft">The top left point of the rectangle</param>
        /// <param name="bottomright">The bottom right point of the rectangle</param>
        public FRect(Vector2 topleft, Vector2 bottomright)
        {
            TopLeft = topleft;
            Width = bottomright.X - topleft.X;
            Height = bottomright.Y - topleft.Y;
        }

        /// <summary>
        /// Floating-point rectangle constructor
        /// </summary>
        /// <param name="topleft"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public FRect(Vector2 topleft, float width, float height)
        {
            TopLeft = topleft;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Floating-point rectangle constructor
        /// </summary>
        /// <param name="left">The left of the rectangle</param>
        /// <param name="top">The top of the rectangle</param>
        /// <param name="bottom">The bottom of the rectangle</param>
        /// <param name="right">The right of the rectangle</param>
        public FRect(float x, float y, float width, float height)
        {
            TopLeft = new Vector2(x,y);
            Width = width;
            Height = height;
        }

        #endregion

        #region Intersection testing functions

        /// <summary>
        /// Checks if this rectangle fully encloses a smaller rectangle within it. If one or more eges are touching then it does not count as a full overlap.
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public bool FullyEncloses(FRect rect)
        {
            return rect.Top > Top && rect.Right < Right && rect.Bottom < Bottom && rect.Left > Left;
        }

        /// <summary>
        /// Checks if this rectangle contains a point
        /// </summary>
        /// <param name="Point">The point to test</param>
        /// <returns>Whether or not this rectangle contains the point</returns>
        public bool Contains(Vector2 Point)
        {
            return (TopLeft.X <= Point.X && BottomRight.X >= Point.X &&
                    TopLeft.Y <= Point.Y && BottomRight.Y >= Point.Y);
        }

        /// <summary>
        /// Checks if this rectangle intersects another rectangle
        /// </summary>
        /// <param name="otherRect">The rectangle to check</param>
        /// <returns>Whether or not this rectangle intersects the other</returns>
        public bool Intersects(FRect otherRect)
        {
            return (!( Bottom <= otherRect.Top ||
                       Top >= otherRect.Bottom ||
                       Right <= otherRect.Left ||
                       Left >= otherRect.Right ));
        }

        /// <summary>
        /// Checks if this rectangle intersects another rectangle
        /// </summary>
        /// <param name="otherRect">The rectangle to check</param>
        /// <returns>Whether or not this rectangle intersects the other</returns>
        public bool Intersects(Rectangle otherRect)
        {
            return (!(Bottom <= otherRect.Top ||
                       Top >= otherRect.Bottom ||
                       Right <= otherRect.Left ||
                       Left >= otherRect.Right));
        }

        /// <summary>
        /// Checks if this rectangle intersects another rectangle
        /// </summary>
        /// <param name="otherRect">The rectangle to check</param>
        /// <returns>Whether or not this rectangle intersects or touches the other</returns>
        public bool IntersectsOrTouches(FRect otherRect)
        {
            return (!(Bottom < otherRect.Top ||
                       Top > otherRect.Bottom ||
                       Right < otherRect.Left ||
                       Left > otherRect.Right));
        }

        #endregion

        #region Conversions
        /// <summary>
        /// Rounds the float values down.
        /// </summary>
        /// <returns></returns>
        public Rectangle ToRect()
        {
            return new Rectangle((int)TopLeft.X, (int)TopLeft.Y, (int)Width, (int)Height);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherRect"></param>
        /// <returns>The overlapping area or FRect.Empty when no collision</returns>
        public FRect Overlap(FRect otherRect)
        {
            if (!Intersects(otherRect))
                return FRect.Empty;

            FRect result = FRect.Empty;
            result.Left = Math.Max(Left, otherRect.Left);
            result.Top = Math.Max(Top, otherRect.Top);
            result.Width = Math.Min(Right, otherRect.Right) - result.Left;
            result.Height = Math.Min(Bottom, otherRect.Bottom) - result.Top;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherRect"></param>
        /// <returns>The overlapping area or null when no collision</returns>
        public FRect Overlap(Rectangle otherRect)
        {
            return Overlap(otherRect.ToFRect());
        }

        public void SetNewLocation(Vector2 newTopLeft)
        {
            TopLeft = newTopLeft;
        }

        /// <summary>
        /// Modified code from the XNA platformer starter kit 4.0
        /// </summary>
        /// <param name="rectA"></param>
        /// <param name="rectB"></param>
        /// <returns></returns>
        public Vector2 GetIntersectionDepth(FRect rectB)
        {
            // If we are not intersecting at all, return (0, 0).
            if (!Intersects(rectB))
                return Vector2.Zero;

            // Calculate half sizes.
            float halfWidthA = Width / 2.0f;
            float halfHeightA = Height / 2.0f;
            float halfWidthB = rectB.Width / 2.0f;
            float halfHeightB = rectB.Height / 2.0f;

            // Calculate centers.
            Vector2 centerA = CenterLoc;// new Vector2(Left + halfWidthA, Top + halfHeightA);
            Vector2 centerB = rectB.CenterLoc; //new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            // Calculate and return intersection depths.
            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }

        #region Operator overloading
        public static FRect operator +(FRect A, FRect B)
        {
            A.X += B.X;
            A.Y += B.Y;
            return A;
        }

        public static FRect operator -(FRect A, FRect substractor)
        {
            A.X -= substractor.X;
            A.Y -= substractor.Y;
            return A;
        }

        public static bool operator ==(FRect A, FRect B)
        {
            return A.X == B.X &&
                   A.Y == B.Y &&
                   A.Width == B.Width &&
                   A.Height == B.Height;
        }

        public static bool operator !=(FRect A, FRect B)
        {
            return !(A == B);
        }
        #endregion

        public override string ToString()
        {
            return string.Format("X:{0} Y:{1} W:{2} H:{3}", Left, Top, Width, Height);
        }
    }
}
