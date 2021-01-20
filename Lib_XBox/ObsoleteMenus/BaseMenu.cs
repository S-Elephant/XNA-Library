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
    [Obsolete("Use ScrollMenu instead.")]
    public class BaseMenu : IActiveState
    {
        protected delegate void OnEnterChoiceEventHandler(Choice choice);
        protected event OnEnterChoiceEventHandler OnEnterChoice;
        protected delegate void OnGoBackEventHandler(IActiveState parentState);
        protected event OnGoBackEventHandler OnGoBack;

        #region Members
        SpriteBatch SpriteBatch;
        Rectangle TextureDrawRect;
        SpriteFont DefaultFont;
        private List<Choice> m_Choices = new List<Choice>();
        protected List<Choice> Choices
        {
            get { return m_Choices; }
            set { m_Choices = value; }
        }

        public Choice ActiveChoice {
            get
            {
                if (Choices.Count > 0)
                    return Choices[ChoiceIndex];
                else
                    return null;
            }
        }

        private int m_ChoiceIndex = 0;
        private int ChoiceIndex
        {
            get { return m_ChoiceIndex; }
            set
            {
                if (Choices.Count > 0)
                    m_ChoiceIndex = (int)MathHelper.Clamp(value, 0, Choices.Count - 1);
                else
                    m_ChoiceIndex = -1;
            }
        }

        public int ActiveChoiceIndex { set { ChoiceIndex = value; } }

        private IActiveState m_Parentmenu;
        public IActiveState Parentmenu
        {
            get { return m_Parentmenu; }
            set { m_Parentmenu = value; }
        }

        public Color SelectedChoiceDrawcolor = Color.White;
        public SpriteFont SelectedChoiceFont;

        private Color m_ChoiceDrawColor;
        protected Color ChoiceDrawColor
        {
            get { return m_ChoiceDrawColor; }
            set { m_ChoiceDrawColor = value; }
        }
        private Texture2D m_Texture;
        public Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }

        private Vector2 m_ChoiceOffset;
        protected Vector2 ChoiceOffset
        {
            get { return m_ChoiceOffset; }
            set { m_ChoiceOffset = value; }
        }

        public bool DrawChoices = true;
        public bool AllowGoBack = true;

        protected int ChoiceSpacingY = 5;
        #endregion
    
        public BaseMenu(SpriteBatch spriteBatch, IActiveState parent, string defaultFont, Size screenSize)
        {
            SpriteBatch = spriteBatch;
            Parentmenu = parent;
            SelectedChoiceFont = DefaultFont = Common.str2Font(defaultFont);
            TextureDrawRect = new Rectangle(0, 0, screenSize.Width, screenSize.Height);
            ChoiceOffset = new Vector2(screenSize.Width / 2 - 128, 192);
        }

        public BaseMenu(SpriteBatch spriteBatch, IActiveState parent, string defaultFont, Size screenSize, Vector2 choiceOffset)
        {
            SpriteBatch = spriteBatch;
            Parentmenu = parent;
            SelectedChoiceFont = DefaultFont = Common.str2Font(defaultFont);
            TextureDrawRect = new Rectangle(0, 0, screenSize.Width, screenSize.Height);
            ChoiceOffset = choiceOffset;
        }

        public void ApplyAllValueChoices()
        {
            foreach (Choice c in Choices)
            {
                if (c.Values.Count > 0)
                    OnEnterChoice(c);
            }
        }

        public Choice AddChoice(string name, string text, SpriteFont font)
        {
            Choice newChoice = new Choice(ChoiceOffset, text, name, ChoiceDrawColor, font) { SelectedFont = SelectedChoiceFont, SelectedColor = SelectedChoiceDrawcolor };
            Choices.Add(newChoice);
            ChoiceOffset = new Vector2(ChoiceOffset.X, ChoiceSpacingY + ChoiceOffset.Y + (int)Choices[Choices.Count - 1].Font.MeasureString(Common.MeasureString).Y);
            return newChoice;
        }

        public Choice AddChoiceWObj(string name, string text, object tag)
        {
            Choice newChoice = AddChoice(name, text);
            newChoice.Tag = tag;
            return newChoice;
        }

        public Choice AddChoice(string name, string text)
        {
            return AddChoice(name, text, DefaultFont);
        }

        public Choice AddChoice(string name)
        {
            return AddChoice(name, null);
        }

        public Choice AddChoice(string name, string text, params string[] values)
        {
            Choice newChoice = AddChoice(name,text);
            newChoice.Values.AddRange(values);
            return newChoice;
        }

        public Choice AddChoice(string name, string text, int valueIndex, params string[] values)
        {
            Choice newChoice = AddChoice(name, text, values);
            newChoice.ValueIndex = valueIndex;
            return newChoice;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (InputMgr.Instance.Keyboard.IsPressed(Keys.Down) || InputMgr.Instance.IsPressed(Buttons.DPadDown))
                ChoiceIndex++;
          
            if(InputMgr.Instance.IsPressed(null, Keys.Up,Buttons.DPadUp))
                ChoiceIndex--;

            if (InputMgr.Instance.IsPressed(null, Keys.Right, Buttons.DPadRight))
            {
                if (ActiveChoice != null)
                    ActiveChoice.ValueIndex++;
            }

            if (InputMgr.Instance.IsPressed(null, Keys.Left, Buttons.DPadLeft))
            {
                if (ActiveChoice != null)
                    ActiveChoice.ValueIndex--;
            }

            if (Choices.Count > 0 && InputMgr.Instance.IsPressed(null,InputMgr.Instance.DefaultConfirmKey,InputMgr.Instance.DefaultConfirmButton))
                OnEnterChoice(ActiveChoice);
     
            if (InputMgr.Instance.IsPressed(null,InputMgr.Instance.DefaultCancelKey,InputMgr.Instance.DefaultCancelButton) && AllowGoBack)
            {
                if (OnGoBack != null && Parentmenu != null)
                    OnGoBack(Parentmenu);
            }
        }

        public virtual void Draw()
        {
            if (Texture != null)
                SpriteBatch.Draw(Texture, TextureDrawRect, Color.White);

            if (DrawChoices)
            {
                foreach (Choice c in Choices)
                {
                    c.Draw(SpriteBatch,ActiveChoice.Equals(c));
                }
            }
        }
    }
}
