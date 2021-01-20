using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using XNALib.RectSideExtensions;

namespace XNALib
{
    public static class Collision
    {
        #region (F)Rectangles

        #region Enum
        [Flags]
        public enum RectSide
        {
            None = 0,
            Top = 1,
            Bottom = 2,
            Left = 4,
            Right = 8,
            Inside=16,
            Equal = 1 + 2 + 4 + 8
        }
        public static bool IsSet(this RectSide sides, RectSide flags)
        {
            return (sides & flags) == flags;
        }
        public static bool IsNotSet(this RectSide sides, RectSide flags)
        {
            return (sides & (~flags)) == 0;
        }
        public static RectSide Set(this RectSide sides, RectSide flags)
        {
            return sides | flags;
        }
        public static RectSide Clear(this RectSide sides, RectSide flags)
        {
            return sides & (~flags);
        }
        #endregion

        /// <summary>
        /// Author: Napoleon.
        /// Tested and works.
        /// Only apply this to non-rotated rectangles.
        /// Touching rectangles don't count in this function
        /// </summary>
        /// <param name="movingRect">Usually the player or moving object.</param>
        /// <param name="staticRect">Usually the wall or something. The collided sides are the sides from this rectangle</param>
        /// <returns></returns>
        /// <example>
        ///  if ((collidedSides & Collision.RectSide.Top) == Collision.RectSide.Top)
        ///    // Top side was hit, do something here
        /// </example>
        public static RectSide CollidedSidesNoTouch(this FRect movingRect, FRect staticRect)  
        {
            RectSide result = RectSide.None;

            if (!movingRect.Intersects(staticRect))
                return result; // No intersection at all

            if (movingRect == staticRect)
                return RectSide.Equal;

            // Left 
            if (movingRect.Right > staticRect.Left && movingRect.Left < staticRect.Left)
                result = (result | RectSide.Left);  
 
            // Top  
            if (movingRect.Bottom > staticRect.Top && movingRect.Top < staticRect.Top)
                result = (result | RectSide.Top);  
 
            // Right
            if (movingRect.Left < staticRect.Right && movingRect.Right > staticRect.Right)
                result = (result | RectSide.Right);    
 
            // Bottom
            if (movingRect.Top < staticRect.Bottom && movingRect.Bottom > staticRect.Bottom)
                result = (result | RectSide.Bottom);

            if (result == RectSide.None)
                return RectSide.Inside; // moving rectangle is inside the static rectangle

            return result;  
        }
    
        /// <summary>
        /// Author: Napoleon.
        /// Tested and works.
        /// Only apply this to non-rotated rectangles.
        /// Touching rectangles DO count in this function
        /// </summary>
        /// <param name="movingRect">Usually the player or moving object.</param>
        /// <param name="staticRect">Usually the wall or something. The collided sides are the sides from this rectangle</param>
        /// <returns></returns>
        /// <example>
        ///  if ((collidedSides & Collision.RectSide.Top) == Collision.RectSide.Top)
        ///    // Top side was hit, do something here
        /// </example>
        public static RectSide CollidedSidesWithTouch(FRect movingRect, FRect staticRect)
        {
            RectSide result = RectSide.None;

            if (!movingRect.Intersects(staticRect))
                return result; // No intersection at all

            if (movingRect == staticRect)
                return RectSide.Equal;

            // Left 
            if (movingRect.Right >= staticRect.Left && movingRect.Left <= staticRect.Left)
                result = (result | RectSide.Left);

            // Top  
            if (movingRect.Bottom >= staticRect.Top && movingRect.Top <= staticRect.Top)
                result = (result | RectSide.Top);

            // Right
            if (movingRect.Left <= staticRect.Right && movingRect.Right >= staticRect.Right)
                result = (result | RectSide.Right);

            // Bottom
            if (movingRect.Top <= staticRect.Bottom && movingRect.Bottom >= staticRect.Bottom)
                result = (result | RectSide.Bottom);

            if (result == RectSide.None)
                return RectSide.Inside; // moving rectangle is inside the static rectangle

            return result;
        }

        /// <summary>
        /// Check for collision between a circle and a rectangle
        /// </summary>
        /// <param name="centerPoint"></param>
        /// <param name="radius"></param>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static bool Intersects(Vector2 circleCenterLoc, float radius, Rectangle rectangle)
        {
            if (Collision.PointIsInRect(circleCenterLoc, rectangle))
                return true;


            Vector2 v = new Vector2(MathHelper.Clamp(circleCenterLoc.X, rectangle.Left, rectangle.Right),
                           MathHelper.Clamp(circleCenterLoc.Y, rectangle.Top, rectangle.Bottom));

            Vector2 direction = circleCenterLoc - v;
            float distanceSquared = direction.LengthSquared();

            return ((distanceSquared > 0) && (distanceSquared < radius * radius));
        }

        /// <summary>
        /// Check for collision between a circle and a FRectangle
        /// </summary>
        /// <param name="centerPoint"></param>
        /// <param name="radius"></param>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static bool Intersects(Vector2 circleCenterLoc, float radius, FRect rectangle)
        {
            if (Collision.PointIsInRect(circleCenterLoc, rectangle))
                return true;


            Vector2 v = new Vector2(MathHelper.Clamp(circleCenterLoc.X, rectangle.Left, rectangle.Right),
                           MathHelper.Clamp(circleCenterLoc.Y, rectangle.Top, rectangle.Bottom));

            Vector2 direction = circleCenterLoc - v;
            float distanceSquared = direction.LengthSquared();

            return ((distanceSquared > 0) && (distanceSquared < radius * radius));
        }

        /// <summary>
        /// Make sure that Rect1 & Rect2 intersect
        /// Source ZiggyWare (now defunct)
        /// </summary>
        /// <param name="Rect1"></param>
        /// <param name="Rect2"></param>
        /// <returns></returns>
        /// <example>
        ///  if (Player.PlayerCollisionRect.Intersects(VolcanoBorderRect))  
        ///    {  
        ///        //Since our rectangles are colliding, we need to seperate them.  This provides our seperation amount once they have collided.   
        ///        Vector2 displacement = GameplayScreen.CalcualteMinimumTranslationDistance(Player.PlayerCollisionRect, VolcanoBorderRect);  
        ///        Player.PlayerPos += displacement;  //Move player with seperation amount  
        ///    } 
        /// </example>
        public static Vector2 CalculateMinimumTranslationDistance(Rectangle Rect1, Rectangle Rect2)
        {
            return CalculateMinimumTranslationDistance(Rect1.ToFRect(), Rect2.ToFRect());
        }

        /// <summary>
        /// Make sure that Rect1 & Rect2 intersect
        /// Source ZiggyWare (now defunct)
        /// </summary>
        /// <param name="Rect1"></param>
        /// <param name="Rect2"></param>
        /// <returns></returns>
        /// <example>
        ///  if (Player.PlayerCollisionRect.Intersects(VolcanoBorderRect))  
        ///    {  
        ///        //Since our rectangles are colliding, we need to seperate them.  This provides our seperation amount once they have collided.   
        ///        Vector2 displacement = GameplayScreen.CalcualteMinimumTranslationDistance(Player.PlayerCollisionRect, VolcanoBorderRect);  
        ///        Player.PlayerPos += displacement;  //Move player with seperation amount  
        ///    } 
        /// </example>
        public static Vector2 CalculateMinimumTranslationDistance(FRect Rect1, FRect Rect2)
        {
            Vector2 result = Vector2.Zero;
            float difference = 0.0f;
            float minimumTranslationDistance = 0.0f;  //The absolute minimum distance we'll need to separate our colliding object.  
            int axis = 0; // Axis stores the value of X or Y.  X = 0, Y = 1.  
            int side = 0; // Side stores the value of left (-1) or right (+1).  

            // Left  
            difference = Rect1.Right - Rect2.Left;
            minimumTranslationDistance = difference;
            axis = 0;
            side = -1;

            // Right  
            difference = Rect2.Right - Rect1.Left;
            if (difference < minimumTranslationDistance)
            {
                minimumTranslationDistance = difference;
                axis = 0;
                side = 1;
            }

            // Down  
            difference = Rect1.Bottom - Rect2.Top;
            if (difference < minimumTranslationDistance)
            {
                minimumTranslationDistance = difference;
                axis = 1;
                side = -1;
            }

            // Up  
            difference = Rect2.Bottom - Rect1.Top;
            if (difference < minimumTranslationDistance)
            {
                minimumTranslationDistance = difference;
                axis = 1;
                side = 1;
            }

            if (axis == 1) //Y Axis  
            { result.Y = (float)side * minimumTranslationDistance; }
            else
            {   //X Axis  
                result.X = (float)side * minimumTranslationDistance;
            }
            //result = new Vector2((float)side * minimumTranslationDistance, (float)side * minimumTranslationDistance);
            return result;
        }

        public enum eCollisionType { None, Touch, Collision }
        public static bool PointIsInRect(Vector2 point, Rectangle rectangle)
        {
            return (point.X >= rectangle.Left && point.X <= rectangle.Right && point.Y >= rectangle.Top && point.Y <= rectangle.Bottom);
        }

        public static bool PointIsInRect(Vector2 point, FRect rectangle)
        {
            return (point.X >= rectangle.Left && point.X <= rectangle.Right && point.Y >= rectangle.Top && point.Y <= rectangle.Bottom);
        }

        /// <summary>
        /// Returns the center coordinate of the specified rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static Vector2 GetRectOrigin(this Rectangle rectangle)
        {
            return new Vector2(rectangle.Left + rectangle.Width / 2, rectangle.Top + rectangle.Height / 2);
        }

        /// <summary>
        /// positions rectangle RectToCenter' center in the center of rectangle destRect
        /// *********
        /// *A ***  *
        /// *  *B*  *
        /// *  ***  *
        /// *********
        /// </summary>
        /// <param name="destRect"></param>
        /// <param name="RectToCenter"></param>
        /// <returns></returns>
        public static Rectangle CenterRects(this Rectangle destRect, Rectangle RectToCenter)
        {
            Vector2 moveVector = GetRectOrigin(destRect) - GetRectOrigin(RectToCenter);
            return new Rectangle(RectToCenter.X + (int)moveVector.X, RectToCenter.Y + (int)moveVector.Y, RectToCenter.Width, RectToCenter.Height);
        }

        /// <summary>
        /// Puts rectangle RectToBottom on the bottom of destRect and centers it by the X-axis
        ///   ******
        ///   *dest*
        ///   *    *
        ///************
        ///*  * B  *  *
        ///************
        /// </summary>
        /// <param name="destRect"></param>
        /// <param name="RectToBottom"></param>
        /// <returns></returns>
        public static Rectangle CenterBottom(Rectangle destRect, Rectangle RectToBottom)
        {
            Rectangle result = CenterRects(destRect, RectToBottom);
            result.Y = destRect.Bottom - RectToBottom.Height;
            return result;
        }

        public static Rectangle CenterTop(Rectangle destRect, Rectangle RectToBottom)
        {
            Rectangle result = CenterRects(destRect, RectToBottom);
            result.Y = destRect.Top;
            return result;
        }

        /// <summary>
        /// Rectangle with all of its values set to int.MinValue
        /// </summary>
        public static Rectangle RectangleInvalid = new Rectangle(int.MinValue, int.MinValue, int.MinValue, int.MinValue);




        /// <summary>
        /// Calculates the signed depth of intersection between two rectangles.
        /// Note: Taken from the Platformer Start Kit from Microsoft (MS-Pl license)
        /// </summary>
        /// <param name="rectA">the mover like the player's boundary</param>
        /// <param name="rectB">the static object like the wall's boundary</param>
        /// <returns>
        /// The amount of overlap between two intersecting rectangles. These
        /// depth values can be negative depending on which wides the rectangles
        /// intersect. This allows callers to determine the correct direction
        /// to push objects in order to resolve collisions.
        /// If the rectangles are not intersecting, Vector2.Zero is returned.
        /// </returns>
        public static Vector2 GetIntersectionDepth(this Rectangle rectA, Rectangle rectB)
        {
            // Calculate half sizes.
            float halfWidthA = rectA.Width / 2.0f;
            float halfHeightA = rectA.Height / 2.0f;
            float halfWidthB = rectB.Width / 2.0f;
            float halfHeightB = rectB.Height / 2.0f;

            // Calculate centers.
            Vector2 centerA = new Vector2(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
            Vector2 centerB = new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector2.Zero;

            // Calculate and return intersection depths.
            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }

        public static Vector2 GetIntersectionDepth(this FRect rectA, FRect rectB)
        {
            // Calculate half sizes.
            float halfWidthA = rectA.Width / 2.0f;
            float halfHeightA = rectA.Height / 2.0f;
            float halfWidthB = rectB.Width / 2.0f;
            float halfHeightB = rectB.Height / 2.0f;

            // Calculate centers.
            Vector2 centerA = new Vector2(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
            Vector2 centerB = new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector2.Zero;

            // Calculate and return intersection depths.
            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }

        public static Vector2 GetIntersectionDepth(this Rectangle rectA, FRect rectB)
        {
            // Calculate half sizes.
            float halfWidthA = rectA.Width / 2.0f;
            float halfHeightA = rectA.Height / 2.0f;
            float halfWidthB = rectB.Width / 2.0f;
            float halfHeightB = rectB.Height / 2.0f;

            // Calculate centers.
            Vector2 centerA = new Vector2(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
            Vector2 centerB = new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector2.Zero;

            // Calculate and return intersection depths.
            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }

        // perhaps see: http://stackoverflow.com/questions/8307834/fastest-algorithm-to-edge-detect-between-two-simple-rectangles for an extensive touching example.

        /// <summary>
        /// ONLY tests for touching rectangles. So if rect A contains rectB they do NOT touch!
        /// </summary>
        /// <param name="rectA"></param>
        /// <param name="rectB"></param>
        /// <param name="allowedDifference"></param>
        /// <returns></returns>
        public static bool AreOnlyTouching(this FRect rectA, FRect rectB, float allowedDifference)
        {
            // They are at the same point
            if (rectA.X == rectB.X && rectA.Y == rectB.Y)
                return true;

            // A's right side touches B's left side
            if (
                    (Math.Abs(rectA.Right - rectB.Left) <= allowedDifference) && // X-axis
                    ((rectA.Top - allowedDifference) <= rectB.Bottom && // Y-axis
                    rectA.Bottom >= (rectB.Top - allowedDifference)) // Y-axis
                )
                return true;

            // A's left side touches B's right side
            if (
                    (Math.Abs(rectA.Left - rectB.Right) <= allowedDifference) && // X-axis
                    ((rectA.Top - allowedDifference) <= rectB.Bottom && // Y-axis
                    rectA.Bottom >= (rectB.Top - allowedDifference)) // Y-axis
                )
                return true;


            // A's bottom touches B's top
            if (
                    (Math.Abs(rectA.Bottom - rectB.Top) <= allowedDifference) && // Y-axis
                    ((rectA.Left + allowedDifference) <= rectB.Right && // X-axis
                    rectA.Right >= (rectB.Left - allowedDifference)) // X-axis
                )
                return true;

            // A's top touches B's bottom
            // A's left side touches B's right side
            if (
                    (Math.Abs(rectA.Top - rectB.Bottom) <= allowedDifference) && // Y-axis
                    ((rectA.Left + allowedDifference) <= rectB.Right && // X-axis
                    rectA.Right >= (rectB.Left - allowedDifference)) // X-axis
                )
                return true;

            return false;
        }

        public static Vector2 IntersectionDepth(Rectangle staticRect, Rectangle movingRect)
        {
            // Calculate half sizes.
            float halfWidthA = staticRect.Width / 2f;
            float halfHeightA = staticRect.Height / 2f;
            float halfWidthB = movingRect.Width / 2f;
            float halfHeightB = movingRect.Height / 2f;

            // Calculate centers.
            Vector2 centerA = staticRect.CenterVector();
            Vector2 centerB = movingRect.CenterVector();

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            // If we are not intersecting at all:
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Common.InvalidVector2;

            // Calculate and return intersection depths.
            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }

        /// <summary>
        /// Only returns true when they touch. Even returns false when they intersect but don't touch.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="allowedTouchDistance">The amount of overlap allowed so it would still return as touch</param>
        /// <returns></returns>
        public static bool AreTouching(this Rectangle A, Rectangle B, float allowedTouchDistance)
        {
            return Math.Abs(A.Left - B.Right) <= allowedTouchDistance ||
                Math.Abs(A.Top - B.Bottom) <= allowedTouchDistance ||
                Math.Abs(A.Right - B.Left) <= allowedTouchDistance ||
                Math.Abs(A.Bottom - B.Top) <= allowedTouchDistance;
        }

        /// <summary>
        /// Returns the intersection or an empty rectangle if they don't intersect.
        /// When they touch each other it is also seen as an intersection.
        /// </summary>
        /// <param name="rectangleA"></param>
        /// <param name="rectangleB"></param>
        /// <returns></returns>
        public static Rectangle Intersection(Rectangle rectangleA, Rectangle rectangleB)
        {
            int x1 = Math.Max(rectangleA.Left, rectangleB.Left);
            int y1 = Math.Max(rectangleA.Top, rectangleB.Top);
            int x2 = Math.Min(rectangleA.Right, rectangleB.Right);
            int y2 = Math.Min(rectangleA.Bottom, rectangleB.Bottom);

            if ((x2 >= x1) && (y2 >= y1))
            {
                return new Rectangle(x1, y1, x2 - x1, y2 - y1);
            }
            return Rectangle.Empty;
        }

        #endregion

        #region Outcommented testing block
        // Below for testing
        /*
            Rectangle A = new Rectangle(100, 100, 100, 100);
            Rectangle B = new Rectangle(500, 500, 50, 50);
            B = Collision.AlignYBottomCenterX(A, B);
            spriteBatch.Draw(Common.White1px, A, Color.Blue);
            spriteBatch.Draw(Common.White1px, B, Color.Red);
         */
        #endregion

        #region Colors & Pixels

        private static Color[,] TextureTo2DColorArray(Texture2D texture)
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);
            
            Color[,] colors2D = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                    colors2D[x, y] = colors1D[x + y * texture.Width];

            return colors2D;
        }



        /// <summary>
        /// Doesn't work with scaled, rotated and sprites that use source rectangles
        /// </summary>
        /// <param name="rectangleA"></param>
        /// <param name="dataA"></param>
        /// <param name="rectangleB"></param>
        /// <param name="dataB"></param>
        /// <returns></returns>
        public static bool PixelPerfect(Rectangle rectangleA, Texture2D textureA, Rectangle rectangleB, Texture2D textureB)
        {
            Color[] colorA = new Color[textureA.Width * textureA.Height];
            textureA.GetData<Color>(colorA);
            
            Color[] colorB = new Color[textureB.Width * textureB.Height];
            textureB.GetData<Color>(colorB);

            return IntersectPixels(rectangleA, colorA, rectangleB, colorB);
        }

        #region Take 3
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*public static bool Take3A(Texture2D textureA, Matrix transformA, Texture2D textureB, Matrix transformB)
        {
            // A
            Color[] colorsA = new Color[textureA.Width * textureA.Height];
            textureA.GetData(colorsA);

            // B            
            Color[] colorsB = new Color[textureB.Width * textureB.Height];
            textureB.GetData(colorsB);

            return IntersectPixelsHeavy(transformA, textureA.Width, textureA.Height, colorsA, transformB, textureB.Width, textureB.Height, colorsB);
        }*/

        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels between two
        /// sprites.
        /// </summary>
        /// <param name="transformA">World transform of the first sprite.</param>
        /// <param name="widthA">Width of the first sprite's texture.</param>
        /// <param name="heightA">Height of the first sprite's texture.</param>
        /// <param name="dataA">Pixel color data of the first sprite.</param>
        /// <param name="transformB">World transform of the second sprite.</param>
        /// <param name="widthB">Width of the second sprite's texture.</param>
        /// <param name="heightB">Height of the second sprite's texture.</param>
        /// <param name="dataB">Pixel color data of the second sprite.</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        public static bool IntersectPixelsHeavy(
                            Matrix transformA, int widthA, int heightA, Color[] dataA,
                            Matrix transformB, int widthB, int heightB, Color[] dataB)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);


            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                int overlappingWidthA = yA * widthA;

                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    // Round to the nearest pixel
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB &&
                        0 <= yB && yB < heightB)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = dataA[xA + overlappingWidthA];
                        Color colorB = dataB[xB + yB * widthB];

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found
                            return true;
                        }
                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return false;
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region PP col detect with matrix
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static Color[,] TextureTo2DArray(Texture2D texture)
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);

            Color[,] colors2D = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                    colors2D[x, y] = colors1D[x + y * texture.Width];

            return colors2D;
        }
        public static Vector2 PPSimplifier(Texture2D textureA, Matrix transformMatrixA, Texture2D textureB, Matrix transformMatrixB)
        {
            Color[,] colorA = TextureTo2DArray(textureA);
            Color[,] colorB = TextureTo2DArray(textureB);

            return PPColDetection(colorA, transformMatrixA, colorB, transformMatrixB);
        }
        /// <summary>
        /// http://www.riemers.net/eng/Tutorials/XNA/Csharp/Series2D/Coll_Detection_Overview.php
        /// </summary>
        /// <param name="tex1"></param>
        /// <param name="mat1"></param>
        /// <param name="tex2"></param>
        /// <param name="mat2"></param>
        /// <returns>Vector2(-1,-1) when NO collision is found</returns>
        private static Vector2 PPColDetection(Color[,] tex1, Matrix mat1, Color[,] tex2, Matrix mat2)
        {
            Matrix mat1to2 = mat1 * Matrix.Invert(mat2);
            int width1 = tex1.GetLength(0);
            int height1 = tex1.GetLength(1);
            int width2 = tex2.GetLength(0);
            int height2 = tex2.GetLength(1);

            for (int x1 = 0; x1 < width1; x1++)
            {
                for (int y1 = 0; y1 < height1; y1++)
                {
                    Vector2 pos1 = new Vector2(x1, y1);
                    Vector2 pos2 = Vector2.Transform(pos1, mat1to2);

                    int x2 = (int)pos2.X;
                    int y2 = (int)pos2.Y;
                    if ((x2 >= 0) && (x2 < width2))
                    {
                        if ((y2 >= 0) && (y2 < height2))
                        {
                            if (tex1[x1, y1].A > 0) // 0 is the alpha color
                            {
                                if (tex2[x2, y2].A > 0) // 0 is the alpha color
                                {
                                    return Vector2.Transform(pos1, mat1);
                                }
                            }
                        }
                    }
                }
            }

            return new Vector2(-1, -1); // No collision found
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        /// <summary>
        /// Does NOT work with scaled, rotated and spritesheets
        /// </summary>
        /// <param name="rectangleA"></param>
        /// <param name="dataA"></param>
        /// <param name="rectangleB"></param>
        /// <param name="dataB"></param>
        /// <returns></returns>
        public static bool IntersectPixels(Rectangle rectangleA, Color[] dataA, Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection 
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds 
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point 
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent, 
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found 
                        return true;
                    }
                }
            }
            // No intersection found 
            return false;
        }

        #endregion
    }
}
