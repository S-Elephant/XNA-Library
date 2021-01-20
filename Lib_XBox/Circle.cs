using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace XNALib
{
    /// <summary>
    /// Taken from the platformer starter kit from Microsoft. Modified by C.a.r.l.o. v.o.n R.a.n.z.o.w
    /// </summary>
    public struct Circle
    {
        /// <summary> 
        /// Center position of the circle. 
        /// </summary> 
        public Vector2 Center;

        /// <summary> 
        /// Radius of the circle. 
        /// </summary> 
        public float Radius;

        /// <summary> 
        /// Constructs a new circle. 
        /// </summary> 
        public Circle(Vector2 centerLocation, float radius)
        {
            Center = centerLocation;
            Radius = radius;
        }

        public Vector2 TopLeft
        {
            get { return new Vector2(Center.X - Radius, Center.Y - Radius); }
        }

        public float Top
        {
            get { return Center.Y - Radius; }
        }
        public float Bottom
        {
            get { return Center.Y + Radius; }
        }
        public float Left
        {
            get { return Center.X - Radius; }
        }
        public float Right
        {
            get { return Center.X + Radius; }
        }

        public static Circle Empty { get { return new Circle(); } }
        public Rectangle AABB { get { return new Rectangle((int)(Center.X - Radius), (int)(Center.Y - Radius), (int)Radius, (int)Radius); } }
        
            /// <summary> 
        /// Determines if a circle intersects a rectangle. 
        /// </summary> 
        /// <returns>True if the circle and rectangle overlap. False otherwise.
        /// Returns false when the circle is completely inside the rectangle (not tested)</returns> 
        public bool Overlaps(FRect rectangle)
        {
            Vector2 v = new Vector2(MathHelper.Clamp(Center.X, rectangle.Left, rectangle.Right),
                                    MathHelper.Clamp(Center.Y, rectangle.Top, rectangle.Bottom));

            Vector2 direction = Center - v;
            float distanceSquared = direction.LengthSquared();

            return ((distanceSquared > 0) && (distanceSquared < Radius * Radius));
        }

        public bool Collide(Rectangle rectangle)
        {
            return Overlaps(rectangle.ToFRect()) || Collision.PointIsInRect(Center, rectangle);
        }
        public bool Collide(FRect rectangle)
        {
            return Overlaps(rectangle) || rectangle.Contains(Center);
        }
        public bool Collide(Circle circle)
        {
            return Vector2.Distance(circle.Center, Center) <= circle.Radius + Radius;
        }

        /// <summary>
        /// Check for collision between a circle and a rectangle
        /// </summary>
        /// <param name="centerPoint"></param>
        /// <param name="radius"></param>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static bool Collide(Vector2 centerPoint, float radius, Rectangle rectangle)
        {
            if (Collision.PointIsInRect(centerPoint, rectangle))
                return true;


            Vector2 v = new Vector2(MathHelper.Clamp(centerPoint.X, rectangle.Left, rectangle.Right),
                           MathHelper.Clamp(centerPoint.Y, rectangle.Top, rectangle.Bottom));

            Vector2 direction = centerPoint - v;
            float distanceSquared = direction.LengthSquared();

            return ((distanceSquared > 0) && (distanceSquared < radius * radius));
        }

        public void DrawPrimitive(SpriteBatch spriteBatch, Color drawColor)
        {
            for (int i = 0; i < 360; i++)
            {
                float degInRad = MathHelper.ToRadians(i);
                Vector2 drawPoint = Center + new Vector2((float)Math.Cos(degInRad) * Radius, (float)Math.Sin(degInRad) * Radius);
                spriteBatch.Draw(Common.White1px, drawPoint, drawColor);
            }
        }
    }
}
