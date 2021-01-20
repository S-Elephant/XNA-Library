using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNALib
{
    public enum eDiagResult { None, Ok, Cancel }
    public class Popup
    {
        public static Texture2D DefaultTexture = null;

        private Texture2D m_Texture = null;
        public Texture2D Texture
        {
            get { return m_Texture; }
            set
            {
                if (value == null)
                    m_Texture = DefaultTexture;
                else
                    m_Texture = value;
            }
        }

        private Rectangle m_DrawRect;
        public Rectangle DrawRect
        {
            get { return m_DrawRect; }
            set { m_DrawRect = value; TextRect = new Rectangle(value.X + WidthOffset, value.Y + HeightOffset, value.Width - 2 * WidthOffset, value.Height - HeightOffset); }
        }
        Rectangle TextRect;
        eDiagResult DialogResult = eDiagResult.None;

        string Text;
        const int WidthOffset = 15, HeightOffset = 10;

        private SpriteFont m_TextFont;
        public SpriteFont TextFont
        {
            get { return m_TextFont; }
            set { m_TextFont = value; }
        }

        public GraphicsDeviceManager GDM;

        const int DefaultWidth = 450, DefaultHeight = 250;
        public Color Drawcolor = Color.White;

        private bool m_IsDisposed = false;
        public bool IsDisposed
        {
            get { return m_IsDisposed; }
            private set { m_IsDisposed = value; }
        }
        bool isFirstUpdateCycle = true; // Used to ignore the first button press

        public Popup(GraphicsDeviceManager gdm, string texture, string text, SpriteFont textFont)
        {
            GDM = gdm;
            Texture = Common.str2Tex(texture);
            Text = text;
            TextFont = textFont;

            DrawRect = new Rectangle(GDM.PreferredBackBufferWidth / 2 - DefaultWidth / 2, GDM.PreferredBackBufferHeight / 2 - DefaultHeight / 2, DefaultWidth, DefaultHeight);
        }

        public void Dispose()
        {
            IsDisposed = true;
        }

        public void Update()
        {
            if (isFirstUpdateCycle)
                isFirstUpdateCycle = false;
            else
            {
                if (InputMgr.Instance.IsPressed(null, InputMgr.Instance.DefaultConfirmKey, InputMgr.Instance.DefaultConfirmButton))
                {
                    DialogResult = eDiagResult.Ok;
                    Dispose();
                }
            }


            if (InputMgr.Instance.IsPressed(null, InputMgr.Instance.DefaultCancelKey, InputMgr.Instance.DefaultCancelButton))
            {
                DialogResult = eDiagResult.Cancel;
                Dispose();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (DialogResult == eDiagResult.None)
            {
                spriteBatch.Draw(Texture, DrawRect, Color.White);
                spriteBatch.DrawString(TextFont, Misc.WrapText(TextFont, Text, TextRect.Width), TextRect.Location.ToVector2(), Drawcolor);
            }
        }
    }
}
