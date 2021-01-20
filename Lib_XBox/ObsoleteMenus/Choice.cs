using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;

namespace XNALib.MenuA
{
    public class Choice
    {
        #region Members
        public Color SelectedColor;
        public SpriteFont SelectedFont = null;
        private SpriteFont m_Font;
        public SpriteFont Font
        {
            get { return m_Font; }
            set { m_Font = value; }
        }
        private Texture2D m_Texture;
        public Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }
        public string Text { get; set; }
        public string Name { get; set; }
        public Vector2 Location { get; set; }
        public Color DrawColor { get; set; }

        private List<string> m_Values = new List<string>();
        public List<string> Values
        {
            get { return m_Values; }
            set { m_Values = value; }
        }

        private int m_ValueIndex;
        public int ValueIndex
        {
            get { return m_ValueIndex; }
            set {if (Values.Count > 0) 
                    m_ValueIndex = (int)MathHelper.Clamp(value, 0, Values.Count - 1);
            }
        }

        public string ActiveValue {
            get
            {
                if (Values.Count > 0)
                    return Values[ValueIndex];
                else
                    return null;
            }
        }

        public object Tag = null;
        #endregion

        public Choice(Vector2 location, string text, string name, Color drawColor, SpriteFont font)
        {
            Text = text;
            Location = location;
            SelectedColor = DrawColor = drawColor;
            Name = name;
            Font = font;
        }

        public void Draw(SpriteBatch spriteBatch, bool isSelected)
        {
            // IsSelected Changes.
            SpriteFont drawFont = Font;
            Color finalColor = DrawColor;
            if (isSelected)
            {
                drawFont = SelectedFont;
                finalColor = SelectedColor;
            }

            // Draw
            if (ActiveValue == null)
                spriteBatch.DrawString(drawFont, Text, Location, finalColor);
            else
                spriteBatch.DrawString(drawFont, Text + ": " + ActiveValue, Location, finalColor);
        }
    }
}
