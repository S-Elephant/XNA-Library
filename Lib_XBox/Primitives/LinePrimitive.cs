using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;

namespace XNALib
{
    /// <summary>
    /// Draws a primitive lines by vectors. Also supports a primitive circle.
    /// </summary>
    public class LinePrimitive
    {
        Texture2D pixel;
        List<Vector2> Vectors;

        /// <summary>
        /// Line Color
        /// </summary>
        public Color DrawColor;

        /// <summary>
        /// Draw offset
        /// </summary>
        public Vector2 DrawOffset;

        /// <summary>
        /// Gets/sets the render depth of the primitive line object (0 = front, 1 = back)
        /// </summary>
        public float DrawDepth;

        /// <summary>
        /// Number of vectors
        /// </summary>
        public int VectorsCnt
        {
            get
            {
                return Vectors.Count;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public LinePrimitive(GraphicsDevice graphicsDevice, Color drawColor)
        {
            // create pixels
            pixel = Common.White1px;

            DrawColor = drawColor;
            DrawOffset = new Vector2(0, 0);
            DrawDepth = 0;

            Vectors = new List<Vector2>();
        }

        /// <summary>
        /// Adds a new vector to the line.
        /// </summary>
        /// <param name="vector">The vector to add.</param>
        public void AddVector(Vector2 vector)
        {
            Vectors.Add(vector);
        }

        /// <summary>
        /// Inserts a vector into the line.
        /// </summary>
        /// <param name="index">The index to insert it at.</param>
        /// <param name="vector">The vector to insert.</param>
        public void InsertVector(int index, Vector2 vector)
        {
            Vectors.Insert(index, vector);
        }

        /// <summary>
        /// Removes a vector from the line.
        /// </summary>
        /// <param name="vector">The vector to remove.</param>
        public void RemoveVector(Vector2 vector)
        {
            Vectors.Remove(vector);
        }

        /// <summary>
        /// Removes a vector from the line at the specified index.
        /// </summary>
        /// <param name="index">The index of the vector to remove.</param>
        public void RemoveVector(int index)
        {
            Vectors.RemoveAt(index);
        }

        /// <summary>
        /// Clears all vectors from the line.
        /// </summary>
        public void ClearVectors()
        {
            Vectors.Clear();
        }

        /// <summary>
        /// Renders the line.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use to render the primitive line object.</param>
        public void Render(SpriteBatch spriteBatch)
        {
            if (Vectors.Count < 2)
                return;

            for (int i = 1; i < Vectors.Count; i++)
            {
                Vector2 vector1 = Vectors[i - 1];
                Vector2 vector2 = Vectors[i];

                // calculate the distance between the two vectors
                float distance = Vector2.Distance(vector1, vector2);

                // calculate the angle between the two vectors
                float angle = (float)Math.Atan2((double)(vector2.Y - vector1.Y),
                    (double)(vector2.X - vector1.X));

                // stretch the pixel between the two vectors
                spriteBatch.Draw(pixel,
                    DrawOffset + vector1,
                    null,
                    DrawColor,
                    angle,
                    Vector2.Zero,
                    new Vector2(distance, 1),
                    SpriteEffects.None,
                    DrawDepth);
            }
        }

        /// <summary>
        /// Creates a circle starting from 0, 0.
        /// </summary>
        /// <param name="radius">The radius (half the width) of the circle.</param>
        /// <param name="sides">The number of sides on the circle (the more the detailed).</param>
        public void CreateCircle(float radius, int sides)
        {
            Vectors.Clear();

            float max = 2 * (float)Math.PI;
            float step = max / (float)sides;

            for (float theta = 0; theta < max; theta += step)
            {
                Vectors.Add(new Vector2(radius * (float)Math.Cos((double)theta),
                    radius * (float)Math.Sin((double)theta)));
            }

            // then add the first vector again so it's a complete loop
            Vectors.Add(new Vector2(radius * (float)Math.Cos(0),
                    radius * (float)Math.Sin(0)));
        }
    }
}