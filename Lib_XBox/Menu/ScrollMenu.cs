using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace XNALib
{
    /// <summary>
    /// Author: Napoleon August 24 2011
    /// Note: This one should be a class and not a struct because the SelectedValueIdx variable won't respond to SetSelectedIdx() when it is a struct.
    /// </summary>
    public class ScrollChoice
    {
        public string Name;
        public string DisplayName;
        public List<string> Values;
        public object Tag = null;

        public string DrawString
        {
            get
            {
                if (Values.Count == 0)
                    return DisplayName;
                else
                    return DisplayName + " " + SelectedValue;
            }
        }
       
        private int m_SelectedValueIdx;
        public int SelectedValueIdx
        {
            get { return m_SelectedValueIdx; }
        }

        public string SelectedValue
        {
            get
            {
                if (SelectedValueIdx != -1 && Values.Count > 0)
                    return Values[SelectedValueIdx];
                else
                    return null;
            }
        }

        public ScrollChoice(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
            Values = new List<string>();
            m_SelectedValueIdx = 0;
        }

        public bool SetSelectedIdx(int newValue)
        {
            if (Values.Count > 0 &&
                newValue >= 0 && newValue < Values.Count)
            {
                m_SelectedValueIdx = newValue;
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Author: Napoleon August 24 2011
    /// </summary>
    public class ScrollMenu : IActiveState
    {
        #region Events
        public delegate void OnSelectionChanged(ScrollChoice oldChoice, ScrollChoice newChoice);
        /// <summary>
        /// Occurs when the player selects another choice (not another choice value).
        /// </summary>
        public event OnSelectionChanged SelectionChanged;
        public delegate void OnSelectChoice(ScrollChoice choice);
        public event OnSelectChoice SelectChoice;
        /// <summary>
        /// Occurs when the value of the current active choice changed. Does not trigger when the choice index is for example 0 and the user tries to make it -1 (thus it stays 0 and the value did not change and neither does this event trigger then).
        /// </summary>
        /// <param name="choice"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        public delegate void OnValueChanged(ScrollChoice choice, string oldValue, string newValue);
        public event OnValueChanged ValueChanged;
        #endregion

        #region Members
        public bool DrawSelectionBar = true;
        public bool UseSelectionFont = false;        
        private const int DEFAULT_SCROLL_DELAY_IN_MS = 200;
        private int m_AutoScrollDelayInMS = DEFAULT_SCROLL_DELAY_IN_MS;
        public int AutoScrollDelayInMS
        {
            get { return m_AutoScrollDelayInMS; }
            set
            {
                m_AutoScrollDelayInMS = value;
                AutoScrollDelayTimerDown = new SimpleTimer(value);
                AutoScrollDelayTimerUp = new SimpleTimer(value);
            }
        }

        SimpleTimer AutoScrollDelayTimerDown = new SimpleTimer(DEFAULT_SCROLL_DELAY_IN_MS);
        SimpleTimer AutoScrollDelayTimerUp = new SimpleTimer(DEFAULT_SCROLL_DELAY_IN_MS);

        public Color BarDrawColor = Color.White;
        public Color ChoiceColor = Color.White;
        private int m_SelectedIdx = 0;
        public int SelectedIdx
        {
            get { return m_SelectedIdx; }
            set
            {
                if (value >= 0 && value < Choices.Count)
                {
                    // Remember old value
                    int oldIdx = m_SelectedIdx;

                    // Set new value
                    m_SelectedIdx = value;                   

                    // Scroll (loop it because the index might have changed by more than one)
                    Vector2 drawLoc = GetChoiceDrawLoc(value);
                    while (drawLoc.Y < OffsetTop || drawLoc.Y > (ScreenHeight-OffsetBot))
                    {
                        if (drawLoc.Y < OffsetTop)
                            ScrollValue--;
                        else
                            ScrollValue++;
                        drawLoc = GetChoiceDrawLoc(value);
                    }

                    // Event
                    if (SelectionChanged != null)
                    {
                        if (oldIdx >= 0 && oldIdx < Choices.Count)
                            SelectionChanged(Choices[oldIdx], Choices[value]);
                        else
                            SelectionChanged(null, Choices[value]);
                    }
                }
            }
        }
       
        public Texture2D Texture = null;
        public SpriteBatch SpriteBatch;
        public int ScreenWidth, ScreenHeight;
        public IActiveState Parent = null;

        public bool AllowToSelectByIdx = false;

        /// <summary>
        /// Value is 0 or greater than 0.
        /// </summary>
        int ScrollValue = 0;
        int OffsetTop, OffsetBot, OffsetLeft;
        public bool IsCentered;
        public List<ScrollChoice> Choices = new List<ScrollChoice>();

        private SpriteFont m_Font;
        private SpriteFont Font
        {
            get { return m_Font; }
            set
            {
                m_Font = value;
                FontHeight = value.MeasureString(Common.MeasureString).Y;
            }
        }

        private SpriteFont m_SelectedFont;
        private SpriteFont SelectedFont
        {
            get { return m_SelectedFont; }
            set { m_SelectedFont = value; }
        }
       
        float FontHeight;
        public int ChoiceSpacingY = 6;

        public ScrollChoice ActiveChoice { get { return Choices[SelectedIdx]; } }
        #endregion

        /// <summary>
        /// Centered constructor
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="font"></param>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        /// <param name="offsetTop"></param>
        /// <param name="offsetBot"></param>
        public ScrollMenu(SpriteBatch spriteBatch, string font, int screenWidth, int screenHeight, int offsetTop, int offsetBot)
        {
            SpriteBatch = spriteBatch;
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            OffsetTop = offsetTop;
            OffsetBot = offsetBot;
            Font = Common.str2Font(font);
            IsCentered = true;
        }

        /// <summary>
        /// Left aligned constructor
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="font"></param>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        /// <param name="offsetTop"></param>
        /// <param name="offsetBot"></param>
        /// <param name="offsetLeft"></param>
        public ScrollMenu(SpriteBatch spriteBatch, string font, int screenWidth, int screenHeight, int offsetTop, int offsetBot, int offsetLeft)
        {
            SpriteBatch = spriteBatch;
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            OffsetTop = offsetTop;
            OffsetBot = offsetBot;
            OffsetLeft = offsetLeft;
            Font = Common.str2Font(font);
            IsCentered = false;
        }

        /// <summary>
        /// SelectedFont constructor. Does not use a bar for displaying what choice it selected. Choices are centered.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="font"></param>
        /// <param name="selectedFont"></param>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        /// <param name="offsetTop"></param>
        /// <param name="offsetBot"></param>
        /// <param name="offsetLeft"></param>
        public ScrollMenu(SpriteBatch spriteBatch, string font, string selectedFont, int screenWidth, int screenHeight, int offsetTop, int offsetBot)
        {
            SpriteBatch = spriteBatch;
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            OffsetTop = offsetTop;
            OffsetBot = offsetBot;
            Font = Common.str2Font(font);
            SelectedFont = Common.str2Font(selectedFont);
            DrawSelectionBar = false;
            UseSelectionFont = true;
            IsCentered = true;
        }

        public string GetChoiceValue(string choiceName)
        {
            for (int i = 0; i < Choices.Count; i++)
            {
                if (Choices[i].Name == choiceName)
                    return Choices[i].SelectedValue;
            }
            throw new Exception(string.Format("choice with name: {0} was not found.", choiceName));
        }

        public ScrollChoice GetChoiceByName(string choiceName)
        {
            for (int i = 0; i < Choices.Count; i++)
            {
                if (Choices[i].Name == choiceName)
                    return Choices[i];
            }
            throw new Exception(string.Format("choice with name: {0} was not found.", choiceName));
        }

        /// <summary>
        /// Adds a new choice w/o values to the menu.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public ScrollChoice AddChoice(string name, string displayName)
        {
            ScrollChoice result = new ScrollChoice(name, displayName);
            Choices.Add(result);
            return result;
        }

        /// <summary>
        /// Adds a new choice with values to the menu.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public ScrollChoice AddChoice(string name, string displayName, params object[] values)
        {
            ScrollChoice result = new ScrollChoice(name, displayName);
            
            foreach (object value in values)
                result.Values.Add(value.ToString());
            
            Choices.Add(result);
            return result;
        }

        public virtual void Update(GameTime gameTime)
        {
            #region Scroll down & up
            if (InputMgr.Instance.IsPressed(null, Keys.Down, Buttons.DPadDown) ||
                InputMgr.Instance.IsPressed(Buttons.LeftThumbstickDown) ||
                InputMgr.Instance.IsPressed(Buttons.RightThumbstickDown))
            {
                SelectedIdx++;
            }
            if (InputMgr.Instance.IsPressed(null, Keys.Up, Buttons.DPadUp) ||
                InputMgr.Instance.IsPressed(Buttons.LeftThumbstickUp) ||
                InputMgr.Instance.IsPressed(Buttons.RightThumbstickUp))
            {
                SelectedIdx--;
            }
            #endregion

            #region Auto scroll down & up
            if (InputMgr.Instance.IsDown(null, Keys.Down, Buttons.DPadDown) ||
                InputMgr.Instance.IsDown(Buttons.LeftThumbstickDown) ||
                InputMgr.Instance.IsDown(Buttons.RightThumbstickDown))
            {
                AutoScrollDelayTimerDown.Update(gameTime);
                if (AutoScrollDelayTimerDown.IsDone)
                {
                    AutoScrollDelayTimerDown.Reset();
                    SelectedIdx++;
                }
            }
            else
                AutoScrollDelayTimerDown.Reset();

            if (InputMgr.Instance.IsDown(null, Keys.Up, Buttons.DPadUp) ||
                InputMgr.Instance.IsDown(Buttons.LeftThumbstickUp) ||
                InputMgr.Instance.IsDown(Buttons.RightThumbstickUp))
            {
                AutoScrollDelayTimerUp.Update(gameTime);
                if (AutoScrollDelayTimerUp.IsDone)
                {
                    AutoScrollDelayTimerUp.Reset();
                    SelectedIdx--;
                }
            }
            else
                AutoScrollDelayTimerUp.Reset();
            #endregion

            #region Change choice value if applicable
            if (InputMgr.Instance.IsPressed(null, Keys.Left, Buttons.DPadLeft) ||
                InputMgr.Instance.IsPressed(Buttons.LeftThumbstickLeft) ||
                InputMgr.Instance.IsPressed(Buttons.RightThumbstickLeft))
            {
                if (ActiveChoice.Values.Count > 0)
                {
                    if (ActiveChoice.SetSelectedIdx(ActiveChoice.SelectedValueIdx - 1))
                    {
                        if (ValueChanged != null)
                            ValueChanged(ActiveChoice, ActiveChoice.Values[ActiveChoice.SelectedValueIdx + 1], ActiveChoice.SelectedValue);
                    }
                }
            }
            if (InputMgr.Instance.IsPressed(null, Keys.Right, Buttons.DPadRight) ||
                InputMgr.Instance.IsPressed(Buttons.LeftThumbstickRight) ||
                InputMgr.Instance.IsPressed(Buttons.RightThumbstickRight))
            {
                if (ActiveChoice.Values.Count > 0)
                {
                    if (ActiveChoice.SetSelectedIdx(ActiveChoice.SelectedValueIdx + 1))
                    {
                         if (ValueChanged != null)
                             ValueChanged(ActiveChoice, ActiveChoice.Values[ActiveChoice.SelectedValueIdx - 1], ActiveChoice.SelectedValue);
                    }
                }
            }
            #endregion

            #region Select choice
            bool choiceWasSelected = false;

            // using 'selector'
            if (InputMgr.Instance.IsPressed(null, InputMgr.Instance.DefaultConfirmKey, InputMgr.Instance.DefaultConfirmButton))
            {
                if(SelectChoice != null)
                {
                    SelectChoice(ActiveChoice);
                    choiceWasSelected = true;
                }
            }

            // using index
            if(AllowToSelectByIdx && !choiceWasSelected)
            {
                List<Keys> keysPressed = InputMgr.Instance.Keyboard.GetAllReleasedKeys();
                foreach (Keys k in keysPressed)
                {
                    if (k >= Keys.D0 && k <= Keys.D9)
                    {
                        SelectChoice(Choices[int.Parse(k.ToString().Substring(1, 1)) - 1]);
                        break;
                    }
                }
            }

            #endregion
        }

        /// <summary>
        /// Returns the draw location of the choice at the specified index
        /// </summary>
        /// <param name="atIndex"></param>
        /// <returns></returns>
        private Vector2 GetChoiceDrawLoc(int atIndex)
        {
            if (IsCentered)
                return new Vector2(ScreenWidth / 2 - GetFont(atIndex).MeasureString(Choices[atIndex].DisplayName).X / 2, OffsetTop + (atIndex - ScrollValue) * (FontHeight + ChoiceSpacingY));
            else
                return new Vector2(OffsetLeft, OffsetTop + (atIndex - ScrollValue) * (FontHeight + ChoiceSpacingY));
        }

        private SpriteFont GetFont(int choiceIdx)
        {
            if (UseSelectionFont && SelectedIdx == choiceIdx)
                return SelectedFont;
            else
                return Font;
        }

        public virtual void Draw()
        {
            // Background
            if (Texture != null)
                SpriteBatch.Draw(Texture, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);

            // Choices
            for (int i = 0; i < Choices.Count; i++)
            {
                bool isSelectedChoice = (i == SelectedIdx);
                SpriteFont font = GetFont(i);

                // Draw location
                Vector2 drawLoc = GetChoiceDrawLoc(i);

                // Draw SelectionBar
                if (DrawSelectionBar && isSelectedChoice)
                    SpriteBatch.Draw(Common.White1px50Trans, new Rectangle(drawLoc.Xi(), drawLoc.Yi(), Font.MeasureString(Choices[i].DrawString).Xi(), Font.MeasureString(Choices[i].DrawString).Yi()), BarDrawColor);

                // Draw choice text
                SpriteBatch.DrawString(font, Choices[i].DrawString, drawLoc, ChoiceColor);
            }            
        }
    }
}
