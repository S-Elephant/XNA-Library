using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNALib
{
    public static class ColorHelper
    {
        /// <summary>
        /// This is modified code from: http://www.riemers.net/eng/Tutorials/XNA/Csharp/Series2D/Texture_to_Colors.php
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static Color[,] GetColorArray2D(this Texture2D texture)
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
        /// This is modified code from: http://www.riemers.net/eng/Tutorials/XNA/Csharp/Series2D/Texture_to_Colors.php
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static Color[] GetColorArray1D(this Texture2D texture)
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);
            return colors1D;
        }

        /// <summary>
        /// </summary>
        /// <param name="colorArray1D"></param>
        /// <param name="textureWidth"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Color GetPixel(this Color[] colorArray1D, int textureWidth, int x, int y)
        {
            return colorArray1D[x + y * textureWidth];
        }

        public static Color[] SetPixel(this Color[] colorArray1D, int textureWidth, int x, int y, Color color)
        {
            colorArray1D[x + y * textureWidth] = color;
            return colorArray1D;
        }
    }
}
