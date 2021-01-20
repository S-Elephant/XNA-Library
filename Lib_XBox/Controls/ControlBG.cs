using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNALib.Controls
{
    public class ControlBG
    {
        #region Members
        private Texture2D m_Texture = null;
        public Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }

        private Color m_BGColor = Color.White;
        public Color BGColor
        {
            get { return m_BGColor; }
            set { m_BGColor = value; }
        }

        private Rectangle m_DrawRect = Rectangle.Empty;
        public Rectangle DrawRect
        {
            get { return m_DrawRect; }
            set { m_DrawRect = value; }
        }
        #endregion
        #region Constructors
        public ControlBG(Texture2D texture, Rectangle drawRectangle)
        {
            Texture = texture;
            DrawRect = drawRectangle;
        }

        public ControlBG(Texture2D texture, Vector2 location)
        {
            Texture = texture;
            DrawRect = new Rectangle((int)location.X, (int)location.Y, Texture.Width, Texture.Height);
        }

        public ControlBG(Color bgColor, Rectangle drawRectangle)
        {
            BGColor = bgColor;
            DrawRect = drawRectangle;
        }
        #endregion

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Texture == null)
                spriteBatch.Draw(Common.White1px, DrawRect, BGColor);
            else
                spriteBatch.Draw(Texture, DrawRect, BGColor);
        }
    }
}
