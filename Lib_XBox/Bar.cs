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
    public class Bar
    {
        public float Percentage = 0; // Update this variable to change the size of the bar

        private Rectangle m_BarDrawRect;
        public Rectangle BarDrawRect
        {
            get { return m_BarDrawRect; }
            set
            {
                m_BarDrawRect = value;
                Location = new Vector2(value.X,value.Y);
            }
        }
        private Vector2 Location;

        public Color BGDrawColor = Color.White;
        public Color DrawColor = Color.White;
        Texture2D BarTexture;
        public enum eDirection { Vertical, Horizontal }
        public eDirection Direction;
        Texture2D BGTexture = null;

        public Bar(Texture2D barTexture, Rectangle barDrawRect, eDirection direction)
        {
            BarTexture = barTexture;
            BarDrawRect = barDrawRect;
            Direction = direction;
        }

        public void SetLocation(int x, int y)
        {
            Location = new Vector2(x,y);
            m_BarDrawRect.X = x;
            m_BarDrawRect.Y = y;
        }

        public Bar(string bgTexture, string barTexture, Rectangle barDrawRect, eDirection direction)
        {
            BGTexture = Common.str2Tex(bgTexture);
            BarTexture = Common.str2Tex(barTexture);
            BarDrawRect = barDrawRect;
            Direction = direction;
        }

        Rectangle GetPercentageBar()
        {
            Rectangle result = new Rectangle(0, 0, BarDrawRect.Width, BarDrawRect.Height);
           
            if (Direction == eDirection.Vertical)
            {
                int difference = (int)((BarDrawRect.Height / ((float)100)) * Percentage);
                result.Height = difference;
                result.Y = BarDrawRect.Y + BarDrawRect.Height - difference;
            }
            else
            {
                int difference = (int)((BarDrawRect.Width / ((float)100)) * Percentage);
                result.Width = difference;
            }
            return result;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (BGTexture != null)
                spriteBatch.Draw(BGTexture, Location, BGDrawColor);
            
            spriteBatch.Draw(BarTexture, Location, GetPercentageBar(), DrawColor);
        }
    }
}
