#if WINDOWS
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
    public class Mouse2
    {
        public Texture2D CursorTexture;
        public MouseState CurrentState = Mouse.GetState();
        public MouseState PreviousState = Mouse.GetState();
        public Vector2 Location = Vector2.Zero;
        public bool DrawCursorByCenter = false;
        public Color DrawColor = Color.White;

        public bool LeftButtonPressed
        {
            get
            {
                return PreviousState.LeftButton == ButtonState.Pressed &&
                    CurrentState.LeftButton == ButtonState.Released;
            }
        }
        public bool RightButtonPressed
        {
            get
            {
                return PreviousState.RightButton == ButtonState.Pressed &&
                    CurrentState.RightButton == ButtonState.Released;
            }
        }

        public Mouse2(string cursorTexture)
        {
            CursorTexture = Common.str2Tex(cursorTexture);
        }
        public Mouse2(Texture2D cursorTexture)
        {
            CursorTexture = cursorTexture;
        }

        public void Update(GameTime gameTime)
        {
            PreviousState = CurrentState;
            CurrentState = Mouse.GetState();
            Location = new Vector2(CurrentState.X, CurrentState.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 drawLoc = Location;
            if (DrawCursorByCenter)
                Location -= new Vector2(CursorTexture.Width / 2, CursorTexture.Height / 2);
            spriteBatch.Draw(CursorTexture, Location, DrawColor);
        }
    }
}
#endif
#if XBOX
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
    public class Mouse2
    {
        // TODO:
        public Texture2D CursorTexture;
        public Vector2 Location = Vector2.Zero;
        public bool DrawCursorByCenter = false;
        public Color DrawColor = Color.White;
        public Buttons LeftButton = Buttons.X;
        public Buttons RightButton = Buttons.Y;

        bool LeftButtonWasDown = false;
        bool RightButtonWasDown = false;

        public bool LeftButtonPressed
        {
            get
            {
                return LeftButtonWasDown &&
                    GPInput.AnyPlayerDownedButton(LeftButton);
            }
        }
        public bool RightButtonPressed
        {
            get
            {
                return RightButtonWasDown &&
                      GPInput.AnyPlayerDownedButton(RightButton);
            }
        }

        public Mouse2(string cursorTexture)
        {
            CursorTexture = Common.str2Tex(cursorTexture);
        }
        public Mouse2(Texture2D cursorTexture)
        {
            CursorTexture = cursorTexture;
        }

        public void Update(GameTime gameTime)
        {
            LeftButtonWasDown = GPInput.AnyPlayerDownedButton(LeftButton);
            RightButtonWasDown = GPInput.AnyPlayerDownedButton(RightButton);

            throw new NotImplementedException();
            //Location = new Vector2(CurrentState.X, CurrentState.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 drawLoc = Location;
            if (DrawCursorByCenter)
                Location -= new Vector2(CursorTexture.Width / 2, CursorTexture.Height / 2);
            spriteBatch.Draw(CursorTexture, Location, DrawColor);
        }
    }
}
#endif