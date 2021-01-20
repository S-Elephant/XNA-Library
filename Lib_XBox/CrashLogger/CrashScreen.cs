using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNALib
{
    public class CrashScreen : IActiveState
    {
        #region Members
        private StringBuilder Text;
        private SpriteFont Font;
        private Game Game;
        private SpriteBatch Batch;
        private Rectangle ScreenArea;
        public static string Header = "We apologize for the inconvenience, but the application crashed.";
        public static string Footer = "Press any key to terminate.";
        #endregion

        public CrashScreen(SpriteFont font, Rectangle screenArea, Game game, SpriteBatch spritebatch)
        {
            Font = font;
            Game = game;
            Batch = spritebatch;
            ScreenArea = screenArea;
        }

        public void SetMessage(string message)
        {
            Text = Misc.WrapText(Font, string.Format("{0}{2}{2}{1}{2}{2}{3}", Header, message, Environment.NewLine, Footer), ScreenArea.Width);
        }

        public void SetMessage(Exception ex)
        {
            Text = Misc.WrapText(Font, string.Format("{0}{2}{2}{1}{2}{2}{3}", Header, ex, Environment.NewLine, Footer), ScreenArea.Width);
        }

        public void Update(GameTime gameTime)
        {
            if (InputMgr.Instance.AnythingIsPressed(null))
                Game.Exit();
        }

        public void Draw()
        {
            Batch.GraphicsDevice.Clear(Color.Black);
            Batch.DrawString(Font, Text, Vector2.Zero, Color.White);
        }
    }
}