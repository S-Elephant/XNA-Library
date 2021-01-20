using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace XNALib
{
    public static class GraphicsLib
    {
        /// <summary>
        /// Fills a texture with a color. It resets the render target and requires the passed spritebatch not to be in Begin() mode.
        /// Don't forget to dispose the render target after use.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="texture">Note that this texture is not altered.</param>
        /// <param name="fillColor"></param>
        /// <returns>The filled rendertarget</returns>
        public static RenderTarget2D FillTexture(GraphicsDevice device, SpriteBatch spriteBatch, Texture2D texture, Color fillColor)
        {
            RenderTarget2D rTarget = new RenderTarget2D(device, texture.Width, texture.Height);
            device.SetRenderTarget(rTarget);
            spriteBatch.Begin();
            spriteBatch.Draw(Common.White1px, new Rectangle(0, 0, texture.Width, texture.Height), fillColor);
            spriteBatch.End();
            device.SetRenderTarget(null);
            return rTarget;
        }

        public static Texture2D Str2TexFromStream(GraphicsDevice device, string path)
        {
            Texture2D result;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                result = Texture2D.FromStream(device, fs);
                fs.Close();
            }
            return result;
        }
    }
}
