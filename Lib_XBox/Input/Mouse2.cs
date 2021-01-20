using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNALib
{
    /// <summary>
    /// Mouse class for Windows
    /// </summary>
    public class Mouse2
    {
        public Texture2D CursorTexture;
        public MouseState CurrentState = Mouse.GetState();
        public MouseState PreviousState = Mouse.GetState();
        public Vector2 PreviousLocation = Vector2.Zero;
        public Vector2 Location = Vector2.Zero;
        public bool DrawCursorByCenter = false;
        public Color DrawColor = Color.White;
        public bool IsVisible = true;

        #region Auto-Hide
        public bool AutoHide = false;
        private bool m_IsAutoHidden = false;
        public bool IsAutoHidden
        {
            get { return m_IsAutoHidden; }
            private set { m_IsAutoHidden = value; }
        }
        public int IdleHideTimeInMS
        {
            get { return AutoHideTimer.TimeInMS; }
            set { AutoHideTimer.TimeInMS = value; }
        }
        private SimpleTimer AutoHideTimer = new SimpleTimer(3500);
        #endregion

        public bool LeftButtonIsPressed { get; private set; }
        public bool RightButtonIsPressed { get; private set; }
        public bool LeftButtonIsDown { get { return CurrentState.LeftButton == ButtonState.Pressed; } }
        public bool RightButtonIsDown { get { return CurrentState.RightButton == ButtonState.Pressed; } }

        public Mouse2(string cursorTexture)
        {
            CursorTexture = Common.str2Tex(cursorTexture);
        }
        public Mouse2(Texture2D cursorTexture)
        {
            CursorTexture = cursorTexture;
        }

        public void Update(GameTime gameTime, Vector2 locationOffset)
        {
            PreviousState = CurrentState;
            CurrentState = Mouse.GetState();

            LeftButtonIsPressed = PreviousState.LeftButton == ButtonState.Pressed &&
                                  CurrentState.LeftButton == ButtonState.Released;
            RightButtonIsPressed = PreviousState.RightButton == ButtonState.Pressed &&
                                  CurrentState.RightButton == ButtonState.Released;

            PreviousLocation = Location + locationOffset;
            Location = new Vector2(CurrentState.X + locationOffset.X, CurrentState.Y + locationOffset.Y);

            if (AutoHide)
            {
                if ((PreviousLocation == Location) && ((CurrentState.LeftButton != ButtonState.Pressed) && (CurrentState.RightButton != ButtonState.Pressed)))
                {
                    AutoHideTimer.Update(gameTime);
                    if (AutoHideTimer.IsDone)
                        IsAutoHidden = true;
                }
                else
                {
                    AutoHideTimer.Reset();
                    IsAutoHidden = false;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            Update(gameTime, Vector2.Zero);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(IsVisible && !IsAutoHidden)
                Draw(spriteBatch, Vector2.Zero);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (IsVisible && !IsAutoHidden)
            {
                Vector2 drawLoc = Location + offset;
                if (DrawCursorByCenter)
                    drawLoc -= new Vector2(CursorTexture.Width / 2, CursorTexture.Height / 2);
                spriteBatch.Draw(CursorTexture, drawLoc, DrawColor);
            }
        }
    }
}
