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
    [Obsolete("Use ScrollMenu instead.")]
    public class BaseMenuBarred : IActiveState
    {
        protected delegate void OnEnterChoiceEventHandler(ChoiceBarred choice);
        protected event OnEnterChoiceEventHandler OnEnterChoice;
        #region Members
        public List<ChoiceBarred> Choices = new List<ChoiceBarred>();

        public ChoiceBarred ActiveChoice
        {
            get
            {
                if (Choices.Count > 0)
                    return Choices[ChoiceIndex];
                else
                    return null;
            }
        }

        private int m_ChoiceIndex = 0;
        protected int ChoiceIndex
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
        public Dictionary<string, string> ChoiceValues = new Dictionary<string, string>();

        public IActiveState Parentmenu;

        protected Color ChoiceDrawColor = Color.Goldenrod;

        private Texture2D m_Texture;
        public Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }

        private Rectangle m_BarDrawRect = Rectangle.Empty;
        private Rectangle BarDrawRect
        {
            get
            {
                if (ActiveChoice == null)
                    return Rectangle.Empty;
                else
                {
                    Vector2 size = ActiveChoice.Font.MeasureString(ActiveChoice.Text);
                    return new Rectangle(ActiveChoice.Location.Xi(), ActiveChoice.Location.Yi(), size.Xi(), size.Yi());
                }
            }
        }

        protected IEngine IEngine;
        public int ChoiceOffsetY = 300;
        #endregion

        public BaseMenuBarred(IEngine engine, IActiveState parent)
        {
            IEngine = engine;
            Parentmenu = parent;
        }

        public ChoiceBarred AddChoice(string name, string text)
        {
            ChoiceBarred newChoice = new ChoiceBarred(new Vector2(0, ChoiceOffsetY), text, name, ChoiceDrawColor);
            newChoice.Location = new Vector2(IEngine.Width / 2 - newChoice.Font.MeasureString(newChoice.Text).X / 2, newChoice.Location.Y);
            Choices.Add(newChoice);
            ChoiceOffsetY = 5 + ChoiceOffsetY + (int)Choices[Choices.Count - 1].Font.MeasureString(Common.MeasureString).Y;
            return newChoice;
        }

        public void AddChoice(string name, string text, params object[] values)
        {
            AddChoice(name, text);
            foreach (object value in values)
                Choices[Choices.Count - 1].Values.Add(value.ToString());
            ChoiceValues.Add(name, values[0].ToString());
        }

        public void AddChoice(string name, string text, int defaultValueIndex, params object[] values)
        {
            AddChoice(name, text);
            foreach (object value in values)
                Choices[Choices.Count - 1].Values.Add(value.ToString());
            Choices.Last().ValueIndex = defaultValueIndex;
            ChoiceValues.Add(name, values[defaultValueIndex].ToString());
        }

        public virtual void Update(GameTime gameTime)
        {
            // Choice index
            if (InputMgr.Instance.IsPressed(null, Keys.Down, Buttons.DPadDown))
                ChoiceIndex++;
            if (InputMgr.Instance.IsPressed(null, Keys.Up, Buttons.DPadUp))
                ChoiceIndex--;
           
            // Choice value
            if (InputMgr.Instance.IsPressed(null, Keys.Right, Buttons.DPadRight))
            {
                ActiveChoice.ValueIndex++;
                if (Choices.Count > 0)
                    ChoiceValues[ActiveChoice.Name] = ActiveChoice.ActiveValue;
            }
            if (InputMgr.Instance.IsPressed(null, Keys.Left, Buttons.DPadLeft))
            {
                ActiveChoice.ValueIndex--;
                if (Choices.Count > 0)
                    ChoiceValues[ActiveChoice.Name] = ActiveChoice.ActiveValue;
            }

            // Confirm/Cancel
            if (Choices.Count > 0 && InputMgr.Instance.IsPressed(null, InputMgr.Instance.DefaultConfirmKey, InputMgr.Instance.DefaultConfirmButton))
                OnEnterChoice(ActiveChoice);
            if (Choices.Count > 0 && InputMgr.Instance.IsPressed(null, InputMgr.Instance.DefaultCancelKey, InputMgr.Instance.DefaultCancelButton))
            {
                if (Parentmenu != null)
                    IEngine.ActiveState = Parentmenu;
            }
        }

        public virtual void Draw()
        {
            if (Texture != null)
                IEngine.SpriteBatch.Draw(Texture, new Rectangle(0, 0, IEngine.Width, IEngine.Height), Color.White);
            if (Choices.Count > 0 && BarDrawRect != Rectangle.Empty)
                IEngine.SpriteBatch.Draw(Common.White1px50Trans, BarDrawRect, Color.White);
            Choices.ForEach(c => c.Draw(IEngine.SpriteBatch));
        }
    }
}