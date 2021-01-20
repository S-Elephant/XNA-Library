using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;

namespace XNALib.Controls
{
    /// <summary>
    /// Only Windows compatible
    /// </summary>
    public class TextBox : BaseControl, IControl
    {
        #region Events
        public delegate void OnLostFocus(TextBox textBox);
        public event OnLostFocus LostFocus;
        public delegate void OnTextChanged(TextBox textBox, string oldText, string newText);
        public event OnTextChanged TextChanged;
        #endregion

        /// <summary>
        /// Text filter enumerable
        /// </summary>
        public enum eTextFilter { None = 0, OnlyIntAndNeg, OnlyIntOnlyPos, OnlyNumericAndAlpha, OnlyAlpha }

        public enum eTextLocation { TopLeft, TopCenter, TopRight, CenterLeft, Center }

        #region Members
        const string MINUS = "-";

        public enum eCaptionLocation { Left, Top, Right, Bottom }

        /// <summary>
        /// The location for the caption (if any)
        /// </summary>
        public eCaptionLocation CaptionLocation = eCaptionLocation.Left;

        /// <summary>
        /// Background color
        /// </summary>
        public Color BGColor = Color.White;

        /// <summary>
        /// Indicates whether or not to allow the space-character when alphanumeric characters are allowed
        /// </summary>
        public bool AllowSpace = true;

        /// <summary>
        /// Detemrines where the text within the textbox is aligned .
        /// </summary>
        public eTextLocation TextLocation = eTextLocation.CenterLeft;

        /// <summary>
        /// Text color
        /// </summary>
        public Color ForeColor = Color.Black;
      
        /// <summary>
        /// Caption color
        /// </summary>
        public Color CaptionColor = Color.Black;
        
        private string m_Text = string.Empty;
        public string Text
        {
            get { return m_Text; }
            set
            {
                string oldValue = m_Text;

                if ((TextFilter == eTextFilter.OnlyIntOnlyPos && !Maths.IsInt(value)) ||
                    (TextFilter == eTextFilter.OnlyIntAndNeg && value != MINUS && !Maths.IsInt(value)))
                {
                    if(value != string.Empty) // allow empty textbox
                        return; // cancel it because it's not an allowed integer value
                }

                #region Clamp min and max values
                if (value != string.Empty &&
                    ((TextFilter == eTextFilter.OnlyIntAndNeg && value != MINUS) || TextFilter == eTextFilter.OnlyIntOnlyPos)
                    )
                {
                    int iTemp = int.Parse(value);
                    if (iTemp < MinIntValue)
                        value = MinIntValue.ToString();
                    else if (iTemp > MaxIntValue)
                        value = MaxIntValue.ToString();
                }
                #endregion

                m_Text = value;

                // Raise event if applicable
                if (TextChanged != null && oldValue != m_Text)
                    TextChanged(this, oldValue, m_Text);
            }
        }

        /// <summary>
        /// The maximum allowed length in this textbox for the Text
        /// </summary>
        public int MaxLength = int.MaxValue;

        /// <summary>
        /// Sprite font
        /// </summary>
        SpriteFont Font;

        /// <summary>
        /// Stores whether the textbox had focus one update cycle ago or not.
        /// </summary>
        private bool HadFocus = false;

        private eTextFilter m_TextFilter = eTextFilter.None;
        /// <summary>
        /// Text filter
        /// </summary>
        public eTextFilter TextFilter
        {
            get { return m_TextFilter; }
            set
            {
                m_TextFilter = value;
                if (value == eTextFilter.OnlyIntAndNeg || value == eTextFilter.OnlyIntOnlyPos)
                {
                    if ((Text != string.Empty) && !Maths.IsInt(Text))
                        Text = "0";
                }
            }
        }

        private int m_MaxIntValue = int.MaxValue;
        /// <summary>
        /// Only applies if the Textfilter is set to a numeric one
        /// </summary>
        public int MaxIntValue
        {
            get { return m_MaxIntValue; }
            set
            {
                m_MaxIntValue = value;
                if (Maths.IsInt(Text) && int.Parse(Text) > MaxIntValue)
                    Text = MaxIntValue.ToString();
            }
        }
        
        private int m_MinIntValue = int.MinValue;
        /// <summary>
        /// Only applies if the Textfilter is set to a numeric one
        /// </summary>
        public int MinIntValue
        {
            get { return m_MinIntValue; }
            set
            {
                m_MinIntValue = value;
                if (Maths.IsInt(Text) && int.Parse(Text) < MinIntValue)
                    Text = MinIntValue.ToString();
            }
        }
       
        #endregion

        public TextBox(Rectangle aabb, string font)
        {
            AABB = aabb;
            Font = Common.str2Font(font);
        }

        public TextBox(Rectangle aabb, SpriteFont font)
        {
            AABB = aabb;
            Font = font;
        }

        public void Update(GameTime gameTime)
        {
            if (IsVisible)
            {
                if (HasFocus)
                {
                    if (ControlMgr.Instance.PressedKeys.Count > 0)
                    {
                        if (ControlMgr.Instance.PressedKeys[0] == Keys.Back)
                        {
                            if (Text.Length > 0)
                                Text = Text.Remove(Text.Length - 1, 1);
                        }
                        else if (ControlMgr.Instance.PressedKeys[0] == Keys.Delete)
                            Text = string.Empty;
                        else if (Text.Length < MaxLength)
                        {
                            switch (TextFilter)
                            {
                                case eTextFilter.None:
                                    foreach (Keys k in InputMgr.Instance.Keyboard.GetAllReleasedKeys())
                                    {
                                        Text += InputMgr.KeyToString(k);
                                    }
                                    break;
                                case eTextFilter.OnlyIntAndNeg:
                                    // Allow minus character
                                    if (ControlMgr.Instance.PressedKeys[0] == Keys.OemMinus && TextFilter != eTextFilter.OnlyIntOnlyPos)
                                        Text += MINUS;

                                    if (ControlMgr.Instance.PressedKeys[0] >= Keys.D0 && ControlMgr.Instance.PressedKeys[0] <= Keys.D9)
                                        Text += ControlMgr.Instance.PressedKeys[0].ToString().Substring(1, 1);
                                    break;
                                case eTextFilter.OnlyIntOnlyPos:
                                    if (ControlMgr.Instance.PressedKeys[0] >= Keys.D0 && ControlMgr.Instance.PressedKeys[0] <= Keys.D9)
                                        Text += ControlMgr.Instance.PressedKeys[0].ToString().Substring(1, 1);
                                    break;
                                case eTextFilter.OnlyNumericAndAlpha:
                                    if (ControlMgr.Instance.PressedKeys[0] == Keys.OemMinus && TextFilter != eTextFilter.OnlyIntOnlyPos)
                                    {
                                        if (InputMgr.Instance.ShiftIsDown)
                                            Text += ControlMgr.Instance.PressedKeys[0].ToString();
                                        else
                                            Text += ControlMgr.Instance.PressedKeys[0].ToString().ToLower();
                                    }

                                    if (ControlMgr.Instance.PressedKeys[0] >= Keys.D0 && ControlMgr.Instance.PressedKeys[0] <= Keys.D9)
                                        Text += ControlMgr.Instance.PressedKeys[0].ToString().Substring(1, 1);

                                    if (ControlMgr.Instance.PressedKeys[0] >= Keys.A && ControlMgr.Instance.PressedKeys[0] <= Keys.Z)
                                    {
                                        if (InputMgr.Instance.ShiftIsDown)
                                            Text += ControlMgr.Instance.PressedKeys[0].ToString();
                                        else
                                            Text += ControlMgr.Instance.PressedKeys[0].ToString().ToLower();
                                    }

                                    if (AllowSpace && ControlMgr.Instance.PressedKeys[0] == Keys.Space)
                                        Text += " ";
                                    break;
                                case eTextFilter.OnlyAlpha:
                                    if (ControlMgr.Instance.PressedKeys[0] >= Keys.A && ControlMgr.Instance.PressedKeys[0] <= Keys.Z)
                                    {
                                        if (InputMgr.Instance.ShiftIsDown)
                                            Text += ControlMgr.Instance.PressedKeys[0].ToString();
                                        else
                                            Text += ControlMgr.Instance.PressedKeys[0].ToString().ToLower();
                                    }

                                    if (AllowSpace && ControlMgr.Instance.PressedKeys[0] == Keys.Space)
                                        Text += " ";
                                    break;
                                default:
                                    throw new CaseStatementMissingException();
                            }
                        }
                    }
                }
                else
                {
                    if (LostFocus != null && HadFocus)
                        LostFocus(this);
                }

                HadFocus = HasFocus;
            }
        }

        public void Draw()
        {
            if (IsVisible)
            {
                ControlMgr.Instance.SpriteBatch.Draw(Common.White1px, AABB, BGColor);

                Vector2 stringMeasure;
                switch (TextLocation)
                {
                    case eTextLocation.TopLeft:
                        ControlMgr.Instance.SpriteBatch.DrawString(Font, Text, Location, ForeColor);
                        break;
                    case eTextLocation.TopCenter:
                        stringMeasure = Font.MeasureString(Text);
                        ControlMgr.Instance.SpriteBatch.DrawString(Font, Text, new Vector2(Location.X + AABB.Width / 2 - stringMeasure.X / 2, Location.Y), ForeColor);
                        break;
                    case eTextLocation.TopRight:
                        stringMeasure = Font.MeasureString(Text);
                        ControlMgr.Instance.SpriteBatch.DrawString(Font, Text, new Vector2(Location.X + AABB.Width - stringMeasure.X, Location.Y), ForeColor);
                        break;
                    case eTextLocation.CenterLeft:
                        stringMeasure = Font.MeasureString(Text);
                        ControlMgr.Instance.SpriteBatch.DrawString(Font, Text, new Vector2(Location.X, Location.Y + AABB.Height / 2 - stringMeasure.Y / 2), ForeColor);
                        break;
                    case eTextLocation.Center:
                        stringMeasure = Font.MeasureString(Text);
                        ControlMgr.Instance.SpriteBatch.DrawString(Font, Text, new Vector2(Location.X + AABB.Width / 2 - stringMeasure.X / 2, Location.Y + AABB.Height / 2 - stringMeasure.Y / 2), ForeColor);
                        break;
                    default:
                        throw new CaseStatementMissingException();
                }

                if (Caption != null)
                {
                    Vector2 captionMeasure = Font.MeasureString(Caption);
                    Vector2 captionLoc;
                    switch (CaptionLocation)
                    {
                        case eCaptionLocation.Left:
                            captionLoc = new Vector2(Location.X - captionMeasure.X - 5, Location.Y + AABB.Height / 2 - captionMeasure.Y / 2);
                            break;
                        case eCaptionLocation.Top:
                            captionLoc = new Vector2(Location.X, Location.Y - captionMeasure.Y - 5);
                            break;
                        case eCaptionLocation.Right:
                            captionLoc = new Vector2(AABB.Right + 5, Location.Y + AABB.Height / 2 - captionMeasure.Y / 2);
                            break;
                        case eCaptionLocation.Bottom:
                            captionLoc = new Vector2(Location.X, AABB.Bottom + captionMeasure.Y + 5);
                            break;
                        default:
                            throw new CaseStatementMissingException();
                    }

                    ControlMgr.Instance.SpriteBatch.DrawString(Font, Caption, captionLoc, CaptionColor);
                }

                DrawChildControls();
            }
        }
    }
}