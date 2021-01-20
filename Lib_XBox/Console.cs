using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNALib
{
    public class XNAConsole
    {
        #region Events
        public delegate void OnEscape();
        public event OnEscape Escape;
        public delegate void OnCommandExecuted(string cmd);
        public event OnCommandExecuted CommandExecuted;
        #endregion

        #region Members
        /// <summary>
        /// Background texture's draw color
        /// </summary>
        public Color DrawColor = Color.Black;

        private SpriteFont m_Font;
        /// <summary>
        /// Console font
        /// </summary>
        private SpriteFont Font
        {
            get { return m_Font; }
            set { m_Font = value; }
        }
        private Texture2D m_Texture = Common.White1px50Trans;
        /// <summary>
        /// Console background texture
        /// </summary>
        public Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }

        /// <summary>
        /// The full console area
        /// </summary>
        Rectangle DrawRect;
        
        Rectangle TextRect;

        private string m_Text = string.Empty;
        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        public string AllText
        {
            get { return TextScroller.Text; }
            set { TextScroller.Text = value; }
        }
       
        public bool IsActive = false;              
       
        public string SystemMsgPrefix = "System> ";
        TextScroller TextScroller;
        #endregion

        public XNAConsole(Rectangle drawRect, string font)
        {
            Font = Common.str2Font(font);
            DrawRect = drawRect;
            TextRect = new Rectangle(DrawRect.X, DrawRect.Bottom - Font.MeasureString(Common.MeasureString).Yi(), DrawRect.Width, Font.MeasureString(Common.MeasureString).Yi());
            TextScroller = new TextScroller(new Rectangle(DrawRect.X, DrawRect.Y,DrawRect.Width, DrawRect.Height - TextRect.Height), Font, string.Empty);
        }

        public void AddMsg(string msg)
        {
            TextScroller.Text = string.Format("{0}{1}{2}", TextScroller.Text, Environment.NewLine, msg);
        }

        public void Clear()
        {
            TextScroller.Text = string.Empty;
        }

        public void AddSystemMsg(string msg)
        {
            TextScroller.Text = string.Format("{0}{1}{2}{3}", TextScroller.Text, Environment.NewLine, SystemMsgPrefix, msg);
        }

        public void AddMsg(string[] msg)
        {
            foreach (string str in msg)
                TextScroller.Text = string.Format("{0}{1}{2}", TextScroller.Text, Environment.NewLine, msg);
        }

        /// <summary>
        /// Keyboard should be updated outside of the console
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                List<Keys> keys = InputMgr.Instance.Keyboard.GetAllReleasedKeys();
                bool ShiftIsDown = InputMgr.Instance.Keyboard.State.IsKeyDown(Keys.LeftShift) || InputMgr.Instance.Keyboard.State.IsKeyDown(Keys.RightShift);
                if (keys.Count > 0)
                {
                    foreach (Keys k in keys)
                    {
                        #region Special commands
                        if (InputMgr.Instance.Keyboard.IsPressed(InputMgr.Instance.DefaultCancelKey))
                        {
                            IsActive = false;
                            if (Escape != null)
                                Escape();
                        }
                        else if (k == Keys.Back || k == Keys.Delete)
                        {
                            if (Text.Length > 0)
                                Text = Text.Substring(0, Text.Length - 1);
                        }
                        else if (InputMgr.Instance.Keyboard.IsPressed(InputMgr.Instance.DefaultConfirmKey))
                        {
                            if (Text != string.Empty)
                            {
                                AddMsg(Text);
                                if (CommandExecuted != null)
                                    CommandExecuted(Text);
                                Text = string.Empty;
                            }
                        }
                        #endregion
                        else if (k == Keys.OemTilde && !ShiftIsDown) // Filter tilde out
                            Text += "`";
                        else
                            Text += InputMgr.KeyToString(k);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                spriteBatch.Draw(Texture, DrawRect, DrawColor);
                spriteBatch.Draw(Texture, TextRect, Color.Black);

                TextScroller.ScrollToBottom();
                TextScroller.Draw(spriteBatch);

                spriteBatch.DrawString(Font, Text, new Vector2(TextRect.X, TextRect.Y), Color.White);
            }
        }
    }
}
