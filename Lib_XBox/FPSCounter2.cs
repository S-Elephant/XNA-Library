using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Text;

namespace XNALib
{
    /// <summary>
    /// All credits for this one go to Shawn Hargreaves
    /// http://blogs.msdn.com/b/shawnhar/archive/2007/06/08/displaying-the-framerate.aspx
    /// </summary>
    public class FPSCounter2 : DrawableGameComponent
    {
        ContentManager content;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;
        StringBuilder outputSB = new StringBuilder(8);
        const string PREFIX = "fps: ";

        string FontString;

        public FPSCounter2(Game game, string font)
            : base(game)
        {
            content = new ContentManager(game.Services);
            FontString = font;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Common.str2Font(FontString);
            base.LoadContent();
        }


        protected override void UnloadContent()
        {
            content.Unload();
            base.UnloadContent();
        }


        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }


        public override void Draw(GameTime gameTime)
        {
            frameCounter++;

            outputSB.Remove(0, outputSB.Length);
            outputSB.Append(PREFIX);
            outputSB.Append(frameRate);

            spriteBatch.Begin();

            spriteBatch.DrawString(spriteFont, outputSB, new Vector2(33, 33), Color.Black);
            spriteBatch.DrawString(spriteFont, outputSB, new Vector2(32, 32), Color.White);

            spriteBatch.End();
        }
    }
}
