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
    internal class Credit
    {
        private bool m_IsTitle;

        public bool IsTitle
        {
            get { return m_IsTitle; }
            set { m_IsTitle = value; }
        }
        private string m_Person;

        public string Text
        {
            get { return m_Person; }
            set { m_Person = value; }
        }
        private Vector2 m_Location;

        public Vector2 Location
        {
            get { return m_Location; }
            set { m_Location = value; }
        }

        public Credit(bool isTitle, string text)
        {
            IsTitle = isTitle;
            Text = text;
        }
    }

    public class CreditsMenu : IActiveState
    {
        #region Members
        Texture2D BGTexture = null;

        private SpriteFont m_Font;
        private SpriteFont Font
        {
            get { return m_Font; }
            set { m_Font = value; }
        }
        private SpriteFont m_FontTitle;
        private SpriteFont FontTitle
        {
            get { return m_FontTitle; }
            set { m_FontTitle = value; }
        }

        private Color m_FontColor = Color.White;
        protected Color FontColor
        {
            get { return m_FontColor; }
            set { m_FontColor = value; }
        }

        private List<Credit> m_AllCredits = new List<Credit>();
        private List<Credit> AllCredits
        {
            get { return m_AllCredits; }
            set { m_AllCredits = value; }
        }

        private IEngine Engine;
        private IActiveState Parent;
        #endregion

        public CreditsMenu(IActiveState parent, IEngine engine, string background, string creditFont, string creditTitleFont)
        {
            Parent = parent;
            Engine = engine;
            Font = Common.str2Font(creditFont);
            FontTitle = Common.str2Font(creditTitleFont);

            if (background != null)
                BGTexture = Common.str2Tex(background);
        }

        protected void AddCredit(string text)
        {
            AllCredits.Add(new Credit(false, text));
        }
        protected void AddCreditTitle(string text)
        {
            AllCredits.Add(new Credit(true, text));
        }

        /// <summary>
        /// Call this in the constructor.
        /// </summary>
        protected void SetLocations()
        {
            int y = Engine.Height;
            foreach (Credit credit in AllCredits)
            {
                SpriteFont measureFont;
                if (credit.IsTitle)
                    measureFont = FontTitle;
                else
                    measureFont = Font;
                credit.Location = Common.CenterStringX(measureFont, credit.Text, Engine.Width, y);
                y += (int)measureFont.MeasureString(Common.MeasureString).Y;
            }
        }

        public void Update(GameTime gameTime)
        {
            // Scroll down
            foreach (Credit credit in AllCredits)
                credit.Location = new Vector2(credit.Location.X, credit.Location.Y - 1);

            // Input
            if (InputMgr.Instance.AnythingIsPressed(null))
                Engine.ActiveState = Parent;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (BGTexture != null)
                spriteBatch.Draw(BGTexture, new Rectangle(0, 0, Engine.Width, Engine.Height), Color.White);

            foreach (Credit credit in AllCredits)
            {
                if (credit.IsTitle)
                    spriteBatch.DrawString(FontTitle, credit.Text, credit.Location, FontColor);
                else
                    spriteBatch.DrawString(Font, credit.Text, credit.Location, FontColor);
            }
        }


        public virtual void Draw()
        {
            Draw(Engine.SpriteBatch);
        }
    }
}
