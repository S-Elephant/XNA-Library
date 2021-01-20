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
    public class CreditsMenu2 : IActiveState
    {
        #region Members
        private SpriteBatch SpriteBatch;

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

        private Rectangle ScreenArea;

        protected IActiveState Parent;
        #endregion

        #region Constructors
        public CreditsMenu2(IActiveState parent, SpriteBatch spriteBatch, Rectangle screenArea, string background, string creditFont, string creditTitleFont)
        {
            Parent = parent;
            Font = Common.str2Font(creditFont);
            FontTitle = Common.str2Font(creditTitleFont);
            ScreenArea = screenArea;
            SpriteBatch = spriteBatch;

            if (background != null)
                BGTexture = Common.str2Tex(background);
        }
        #endregion

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
            int y = ScreenArea.Height;

            foreach (Credit credit in AllCredits)
            {
                SpriteFont measureFont;
                if (credit.IsTitle)
                    measureFont = FontTitle;
                else
                    measureFont = Font;
                credit.Location = Common.CenterStringX(measureFont, credit.Text, ScreenArea.Width, y);
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
                OnExit();
        }

        public virtual void OnExit() { throw new Exception("Override me. Do not call me."); }

        public virtual void Draw()
        {
            if (BGTexture != null)
                SpriteBatch.Draw(BGTexture, new Rectangle(0, 0, ScreenArea.Width, ScreenArea.Height), Color.White);

            foreach (Credit credit in AllCredits)
            {
                if (credit.IsTitle)
                    SpriteBatch.DrawString(FontTitle, credit.Text, credit.Location, FontColor);
                else
                    SpriteBatch.DrawString(Font, credit.Text, credit.Location, FontColor);
            }
        }
    }
}
