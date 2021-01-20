using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace XNALib
{
    [Obsolete("this class is only present for porting, doesnt work anymore.")]
    public class Mouse1
    {
        #region Events
        /*
        public delegate void LeftBtnReleasedEventHandler();
        public event LeftBtnReleasedEventHandler LeftBtnReleased;
        public delegate void LeftBtnPressedEventHandler();
        public event LeftBtnPressedEventHandler LeftBtnPressed;
        public delegate void RightBtnReleasedEventHandler();
        public event RightBtnReleasedEventHandler RightBtnReleased;
        public delegate void RightBtnPressedEventHandler();
        public event RightBtnPressedEventHandler RightBtnPressed;
         */
        #endregion
        #region Members
        private Color m_MouseColor = Color.White;
        public Color MouseColor
        {
            get { return m_MouseColor; }
            set { m_MouseColor = value; }
        }

        private Texture2D m_Texture = null;

        public Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }
        private Vector2 m_Location = Vector2.Zero;
        public Vector2 Location
        {
            get { return m_Location; }
            set { m_Location = value; }
        }
        public Vector2 CenterLocation { get { return new Vector2(Location.X + Texture.Width / 2, Location.Y + Texture.Height / 2); } }

        #region MouseStateMembers
        private MouseState m_CurrentMouseState;
        public MouseState CurrentMouseState
        {
            get { return m_CurrentMouseState; }
            set { m_CurrentMouseState = value; }
        }
        private MouseState m_PrevMouseState;
        public MouseState PrevMouseState
        {
            get { return m_PrevMouseState; }
            set { m_PrevMouseState = value; }
        }
        #endregion

        private bool m_LeftBtnIsDown = false;
        public bool LeftBtnIsDown
        {
            get { return m_LeftBtnIsDown; }
            set { m_LeftBtnIsDown = value; }
        }
        private bool m_RightBtnIsDown = false;
        public bool RightBtnIsDown
        {
            get { return m_RightBtnIsDown; }
            set { m_RightBtnIsDown = value; }
        }

        //private Stopwatch m_LastDblClickStopWatch = new Stopwatch();
        private int m_DblClicktimeOutInMS = 400;
        public int DblClicktimeOutInMS
        {
            get { return m_DblClicktimeOutInMS; }
            set { m_DblClicktimeOutInMS = value; }
        }
        #endregion

        public Mouse1(string texture)
        {
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
