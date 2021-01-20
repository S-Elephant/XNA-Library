using System;
using Microsoft.Xna.Framework;

namespace XNALib
{

    public static class Maths
    {

        #region Clamping & value limiting
        /// <summary>
        /// When the value is greater than the max value the min value is returned and vice versa.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static int ClampEndless(this int value, int minValue, int maxValue)
        {
            if (value < minValue)
                return maxValue;
            else if (value > maxValue)
                return minValue;
            return value;
        }

        public static int Clamp(this int value, int minValue, int maxValue)
        {
            if (value < minValue)
                return minValue;
            else if (value > maxValue)
                return maxValue;
            return value;
        }

        public static int Min(this int value, int minValue)
        {
            if (value < minValue)
                return minValue;
            return value;
        }

        #endregion

        #region Miscellaneous

        public static bool IsInt(string value)
        {
            int temp;
            return int.TryParse(value, out temp);
        }

        public static bool IsBetween(int value, int min, int max)
        {
            return (value >= min) && (value <= max);
        }

        public static bool IsEven(this int value)
        {
            return value % 2 == 0;
        }

        #endregion

        #region Grid
        public static Vector2 SnapToGrid(this Vector2 coordinate, Vector2 gridSize)
        {
            float hSnap = gridSize.X, vSnap = gridSize.Y, originalX = coordinate.X, originalY = coordinate.Y;
            return new Vector2((float)(Math.Round(coordinate.X / hSnap) * hSnap), (float)(Math.Round(coordinate.Y / vSnap) * vSnap));
        }
        
        public static Vector2 SnapToTopLeft(this Vector2 coordinate, Vector2 gridSize, Vector2 gridOffset)
        {
            float hSnap = gridSize.X, vSnap = gridSize.Y, originalX = coordinate.X, originalY = coordinate.Y;
            return new Vector2((float)(Math.Floor(coordinate.X / hSnap) * hSnap), (float)(Math.Floor(coordinate.Y / vSnap) * vSnap)) + new Vector2(gridOffset.X % gridSize.X, gridOffset.Y % gridSize.Y);
        }

        public static Vector2 SnapToTopLeft(this Vector2 coordinate, Vector2 gridSize)
        {
            float hSnap = gridSize.X, vSnap = gridSize.Y, originalX = coordinate.X, originalY = coordinate.Y;
            return new Vector2((float)(Math.Floor(coordinate.X / hSnap) * hSnap), (float)(Math.Floor(coordinate.Y / vSnap) * vSnap));
        }

        public static Vector2 SnapToBotRight(this Vector2 coordinate, Vector2 gridSize)
        {
            float hSnap = gridSize.X, vSnap = gridSize.Y, originalX = coordinate.X, originalY = coordinate.Y;
            return new Vector2((float)(Math.Ceiling(coordinate.X / hSnap) * hSnap), (float)(Math.Ceiling(coordinate.Y / vSnap) * vSnap));
        }
        #endregion

        #region Direction & angles

        /// <summary>
        /// The vector parameter is the facing point. the vector (0,0 is the start point. It calculates the angle between those 2.
        /// So an input parameter of (1,1) returns 45 degrees in radians.
        /// DOESNT WORK!
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>Angle in RADIANS</returns>
        public static float Vector2ToAngleRad(Vector2 vector)
        {
            return (float)Math.Atan2(vector.X, vector.Y);
        }

        /// <summary>
        /// Tested and Works.
        /// </summary>
        /// <param name="angleInRadians">The angle of the (for example) cannon in order to calculate the movement direction of the projectile.</param>
        /// <returns>The move-vector</returns>
        public static Vector2 AngleToVector2(float angleInRadians)
        {
            if (angleInRadians < 0f || angleInRadians > 6.28318530717958647692528676656f)
                throw new Exception("Invalid angle. Must be between 0f-6.29~f (radian)");

            Vector2 zeroAngle = new Vector2(0, -1);
            Matrix rotationMatrix = Matrix.CreateRotationZ(angleInRadians);
            return Vector2.Transform(zeroAngle, rotationMatrix);
        }

        [Obsolete("Use MathHelper.ToDegrees()")]
        public static float RadianToDegrees(float radian)
        {
            return radian * (180f / (float)Math.PI);
        }
        [Obsolete("Use MathHelper.ToRadians()")]
        public static float DegreesToRadian(float degrees)
        {
            return degrees * ((float)Math.PI / 180);
        }

        /// <summary>
        /// Returns the angle in radians between 2 points.
        /// DOESNT WORK!
        /// </summary>
        /// <param name="location"></param>
        /// <param name="FaceThisLocation"></param>
        /// <returns></returns>
        public static float AimXOnYAngleRad(Vector2 location, Vector2 FaceThisLocation)
        {
            /*
            float x = FaceThisLocation.X - location.X;
            float y = FaceThisLocation.Y - location.Y;
            return (float)Math.Atan2(y, x);*/

            float result = Vector2ToAngleRad(FaceThisLocation - location);
            if (result > 0)
                return Math.Abs(result - MathHelper.ToRadians(180));
            else
                return (result * -1) + MathHelper.ToRadians(180);
            //return Vector2ToAngleRad(FaceThisLocation - location);
                //return (float)Math.Atan2((FaceThisLocation.X - location.X), (FaceThisLocation.Y - location.Y));
        }

        /// <summary>
        /// Also normalizes
        /// </summary>
        /// <param name="start"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static Vector2 GetMoveDir(Vector2 start, Vector2 destination)
        {
            if (destination - start == Vector2.Zero) // If-statement is needed because normalizing a zero value results in a NaN value
                return Vector2.Zero;
            else
                return Vector2.Normalize(destination - start);
        }

        #endregion

        #region Random
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chance">value between 1-100. A value of 0 and lower means 0% chance.</param>
        /// <returns>True if it happens and false if it does not happen.</returns>
        public static bool Chance(int chance)
        {
            return Maths.RandomNr(1, 100) <= chance;
        }

        public static bool RandomBool()
        {
            return Maths.RandomNr(0,1) == 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chance">value between 0f and 1f. So 0.5f means 50% chance.</param>
        /// <returns>True if it happens and false if it does not happen.</returns>
        public static bool Chance(float chance)
        {
            return Maths.RandomNr(1, 100) <= chance * 100;
        }

        public static float DistanceBetween(Point A, Point B)
        {
            double x = A.X - B.X;
            double y = A.Y - B.Y;
            return (float)Math.Sqrt(x * x + y * y);
        }

        private static Random rand = new Random();
        public static int RandomNr(int min, int max)
        {
            /*
                * If Random.Next() is called with no arguments, the result is a non-negative random number greater than or equal to zero, and less than 2,147,483,647.
                * If one argument is passed in, the result is a non-negative random number greater than or equal to 0, and less than the value in the argument.
                * If two arguments are passed in, the result is a random number greater than or equal to the first number, and less than the second.
            */
            return rand.Next(min, max + 1);
        }

        public static Vector2 RandomVector2(float minXY, float maxXY)
        {
            return new Vector2(RandomFloat(minXY, maxXY), RandomFloat(minXY, maxXY));
        }

        /// <summary>
        /// Accuracy is 1 decimaal achter de komma.
        /// This function isn't fully tested but seems to work so far.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float RandomFloat(float min, float max)
        {
            return (float)rand.Next((int)(min * 100), (int)(max * 100) + 1) / 100;
        }

        /// <summary>
        /// Returns a Double between 0.0 and 1.0
        /// </summary>
        /// <returns></returns>
        public static Double RandomDouble()
        {
            return rand.NextDouble();
        }
        #endregion
    }
}
