using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNALib
{
    public class MouseXbox
    {
        #region Members
        PlayerIndex PlayerIdx;

        public float MouseSpeed = 1f;
        
        private Vector2 m_Location = Vector2.Zero;
        public Vector2 Location
        {
            get { return m_Location; }
            set { m_Location = value; }
        }

        private Texture2D m_CursorTexture;
        public Texture2D CursorTexture
        {
            get { return m_CursorTexture; }
            set { m_CursorTexture = value; }
        }

        public Color DrawColor = Color.White;

        #region Left/Right buttons
        public bool LeftButtonIsPressed
        {
            get { return InputMgr.Instance.GamePads[(int)PlayerIdx].IsPressed(Btn_LeftClick); }
        }
        public bool LeftButtonIsDown
        {
            get { return InputMgr.Instance.GamePads[(int)PlayerIdx].IsDown(Btn_LeftClick); }
        }
        public bool RightButtonIsPressed
        {
            get { return InputMgr.Instance.GamePads[(int)PlayerIdx].IsPressed(Btn_RightClick); }
        }
        public bool RightButtonIsDown
        {
            get { return InputMgr.Instance.GamePads[(int)PlayerIdx].IsDown(Btn_RightClick); }
        }

        Buttons Btn_MoveUp = Buttons.LeftThumbstickUp;
        Buttons Btn_MoveRight = Buttons.LeftThumbstickRight;
        Buttons Btn_MoveDown = Buttons.LeftThumbstickDown;
        Buttons Btn_MoveLeft = Buttons.LeftThumbstickLeft;
        Buttons Btn_LeftClick = Buttons.A;
        Buttons Btn_RightClick = Buttons.B;
        #endregion

        #endregion

        #region Constructors
        public MouseXbox(PlayerIndex playerIdx, string cursorTexture)
        {
            PlayerIdx = playerIdx;
            CursorTexture = Common.str2Tex(cursorTexture);
        }

        public MouseXbox(PlayerIndex playerIdx, string cursorTexture, Buttons up, Buttons right, Buttons down, Buttons left, Buttons leftClick, Buttons rightClick)
        {
            PlayerIdx = playerIdx;
            CursorTexture = Common.str2Tex(cursorTexture);
            
            Btn_MoveUp = up;
            Btn_MoveRight = right;
            Btn_MoveDown = down;
            Btn_MoveLeft = left;
            Btn_LeftClick = leftClick;
            Btn_RightClick = rightClick;
        }
        #endregion

        public void Update(GameTime gameTime)
        {
            if (InputMgr.Instance.GamePads[(int)PlayerIdx].IsPressed(Btn_MoveUp))
                Location += new Vector2(0, -MouseSpeed);
            if (InputMgr.Instance.GamePads[(int)PlayerIdx].IsPressed(Btn_MoveRight))
                Location += new Vector2(MouseSpeed, 0);
            if (InputMgr.Instance.GamePads[(int)PlayerIdx].IsPressed(Btn_MoveDown))
                Location += new Vector2(0, MouseSpeed);
            if (InputMgr.Instance.GamePads[(int)PlayerIdx].IsPressed(Btn_MoveLeft))
                Location += new Vector2(-MouseSpeed, 0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Vector2.Zero);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.Draw(CursorTexture, Location+offset, DrawColor);
        }
    }
}