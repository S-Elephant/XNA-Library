using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNALib.Menu
{
    public class ChoiceBarred
    {
        #region Members
        private SpriteFont m_Font = Common.str2Font("MenuChoice");
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
        #endregion

        public ChoiceBarred(Vector2 location, string text, string name, Color drawColor)
        {
            Text = text;
            Location = location;
            DrawColor = drawColor;
            Name = name;
        }

        public ChoiceBarred(Vector2 location, string text, string name, Color drawColor, int defaultValueIndex, params object[] values)
        {
            Text = text;
            Location = location;
            DrawColor = drawColor;
            Name = name;

            foreach (object value in values)
                Values.Add(value.ToString());
            ValueIndex = defaultValueIndex;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (ActiveValue == null)
                spriteBatch.DrawString(Font, Text, Location, DrawColor);
            else
                spriteBatch.DrawString(Font, Text + ": " + ActiveValue, Location, DrawColor);
        }
    }
}
