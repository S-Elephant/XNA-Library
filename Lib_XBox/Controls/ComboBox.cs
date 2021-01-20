using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNALib.Controls
{
    /// <summary>
    /// NOTE: Item's can currently not be scrolled.
    /// </summary>
    public class ComboBox : BaseControl, IControl
    {
        #region Events
        public delegate void OnSelectedIndexChanged(ComboBox cbo);
        public event OnSelectedIndexChanged SelectedIndexChanged;
        #endregion

        #region Members
        public static ComboBox GlobalExpandedComboBox = null;
        public static bool IgnoreExpansionsThisRun = false;

        public Color BGColor = Color.White;
        public Color CaptionColor = Color.Black;
        private Rectangle m_DrawRect;
        public Rectangle DrawRect
        {
            get
            {
                if(Parent == null)
                    return m_DrawRect;
                else
                    return new Rectangle(m_DrawRect.X + Parent.AABB.X, m_DrawRect.Y + Parent.AABB.Y, m_DrawRect.Width, m_DrawRect.Height);
            }
            set
            {
                SetDrawRect(value);
            }
        }

        public override Vector2 BaseLocation
        {
            get
            {
                return m_DrawRect.Location.ToVector2();
            }
        }
        public override Rectangle AABB { get { return DrawRect; } }

        private int m_ExpansionHeight = 256;
        public int ExpansionHeight
        {
            get { return m_ExpansionHeight; }
            set
            {
                m_ExpansionHeight = value;
                ExpandedRectangle = new Rectangle(DrawRect.X, DrawRect.Y, DrawRect.Width, value);

                string text = string.Empty;
                if (TextScroller != null)
                    text = TextScroller.Text;
                TextScroller = new TextScroller(ExpandedRectangle, Font, text) { Drawcolor = FontColor, BGColor = this.BGColor };
            }
        }

        private Rectangle m_DropDownBtnRect;
        public Rectangle DropDownBtnRect
        {
            get
            {
                return m_DropDownBtnRect;
            }
            set { m_DropDownBtnRect = value; }
        }

        private Rectangle m_ExpandedRectangle;
        public Rectangle ExpandedRectangle
        {
            get
            {
                return m_ExpandedRectangle;
            }
            set { m_ExpandedRectangle = value; }
        }

        public override Vector2 Location
        {
            get
            {
                return new Vector2(DrawRect.X, DrawRect.Y);
            }
            set
            {
                DrawRect = new Rectangle(value.Xi(), value.Yi(), DrawRect.Width, DrawRect.Height);
            }
        }
        
        Texture2D DropDownBtnTex;

        private bool m_IsCollapsed = true;
        public bool IsCollapsed
        {
            get { return m_IsCollapsed; }
            private set { m_IsCollapsed = value; }
        }

        private SpriteFont m_Font;
        public SpriteFont Font
        {
            get { return m_Font; }
            set
            {
                m_Font = value;
                FontHeight = value.MeasureString(Common.MeasureString).Y;
            }
        }

        public override IControl Parent
        {
            get { return m_Parent; }
            set
            {
                base.Parent = value;
                SetDrawRect(m_DrawRect);
            }
        }
       

        float FontHeight;

        private List<object> m_Items = new List<object>();
        private List<object> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }
       
        private int m_SelectedIdx = -1;
        public int SelectedIdx
        {
            get { return m_SelectedIdx; }
            set
            {
                if (value < -1 || value > Items.Count)
                    throw new ArgumentOutOfRangeException("SelectedIdx is out of range.");

                m_SelectedIdx = value;
                UpdateText();

                if (SelectedIndexChanged != null)
                    SelectedIndexChanged(this);
            }
        }

        public string SelectedValueStr
        {
            get
            {
                if (SelectedIdx != -1)
                    return Items[SelectedIdx].ToString();
                else
                    return null;
            }
        }
        public object SelectedItem
        {
            get
            {
                if (SelectedIdx != -1)
                    return Items[SelectedIdx];
                else
                    return null;
            }
        }
       
        public Color FontColor = Color.Black;
        TextScroller TextScroller;
        #endregion

        public ComboBox(Rectangle drawRect, Texture2D dropDownBtnTex, SpriteFont font)
        {
            Name = "cbo";
            Font = font;
            DropDownBtnTex = dropDownBtnTex;
            DrawRect = drawRect;
        }

        private void UpdateText()
        {
            TextScroller.Text = string.Empty;
            for (int i = 0; i < Items.Count; i++)
            {
                if (TextScroller.Text == string.Empty)
                    TextScroller.Text += Items[i].ToString();
                else
                    TextScroller.Text += Environment.NewLine + Items[i].ToString();
            }
        }

        private void SetDrawRect(Rectangle value)
        {
            m_DrawRect = value;
            DropDownBtnRect = new Rectangle(DrawRect.Right - DropDownBtnTex.Width, DrawRect.Y, DropDownBtnTex.Width, DrawRect.Height);
            ExpandedRectangle = new Rectangle(DrawRect.X, DrawRect.Y, DrawRect.Width, ExpansionHeight);

            string text = string.Empty;
            if (TextScroller != null)
                text = TextScroller.Text;
            TextScroller = new TextScroller(ExpandedRectangle, Font, text) { Drawcolor = FontColor, BGColor = this.BGColor };
        }

        public void AddItem(object item)
        {
            Items.Add(item);
            UpdateText();
        }

        public void AddItem(params object[] items)
        {
            foreach (object item in items)
                AddItem(item);
        }

        public void SetPreviousItem()
        {
            if (SelectedIdx != -1 && Items.Count > 0)
            {
                if (SelectedIdx != 0)
                    SelectedIdx--;
                else
                    SelectedIdx = Items.Count-1;
            }
        }

        public void SetNextItem()
        {
            if (SelectedIdx != -1 && Items.Count > 0)
            {
                if (SelectedIdx != Items.Count - 1)
                    SelectedIdx++;
                else
                    SelectedIdx = 0;
            }
        }

        public void SetIndexByStrValue(string str)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].ToString() == str)
                {
                    SelectedIdx = i;
                    return;
                }
            }
            throw new Exception("The specified item (" + str + ") was not found.");
        }

        public override void SetFocus()
        {
            HasFocus = !IsCollapsed || 
                       Collision.PointIsInRect(InputMgr.Instance.Mouse.Location, AABB) && (ComboBox.GlobalExpandedComboBox == null);
        }

        public void Update(GameTime gameTime)
        {
            if (IsVisible)
            {
                Vector2 mouseLoc = InputMgr.Instance.Mouse_Location(null);

                if (GlobalExpandedComboBox == null || GlobalExpandedComboBox == this)
                    HasFocus = true;
                else
                {
                    if (IsCollapsed)
                        HasFocus = Collision.PointIsInRect(mouseLoc, DrawRect);
                    else
                        HasFocus = true;
                }

                if (!IgnoreExpansionsThisRun)
                {
                    if (HasFocus && (GlobalExpandedComboBox == null || GlobalExpandedComboBox == this))
                    {
                        if (InputMgr.Instance.Mouse_LeftIsPressed(null) && (ControlMgr.Instance.GetOpenedCombobox() == null || ControlMgr.Instance.GetOpenedCombobox() == this))
                        {
                            // Check for collision for dropdown/expand
                            if (IsCollapsed && Collision.PointIsInRect(mouseLoc, DrawRect))
                            {
                                IsCollapsed = false;
                                GlobalExpandedComboBox = this;
                            }
                            else if (!IsCollapsed && Collision.PointIsInRect(mouseLoc, DropDownBtnRect))
                            {
                                IsCollapsed = true;
                                GlobalExpandedComboBox = null;
                                IgnoreExpansionsThisRun = true;
                            }
                            else if (!IsCollapsed && Collision.PointIsInRect(mouseLoc, ExpandedRectangle)) // check for collision with expanded rectangle if collapsed
                            {
                                // Find out what item was clicked.
                                int selected = -1;
                                string selectedText = TextScroller.GetLineTextAtPoint(mouseLoc, out selected);
                                if (selectedText != null)
                                {
                                    SetIndexByStrValue(selectedText.Replace(Environment.NewLine, "").Trim());
                                    IsCollapsed = true;
                                    GlobalExpandedComboBox = null;
                                    IgnoreExpansionsThisRun = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Draw()
        {
            if (IsVisible)
            {
                // Caption
                if (Caption != null)
                    ControlMgr.Instance.SpriteBatch.DrawString(Font, Caption, new Vector2(DrawRect.X - 6 - Font.MeasureString(Caption).X, DrawRect.Y), CaptionColor);

                // BG and button
                ControlMgr.Instance.SpriteBatch.Draw(Common.White1px, DrawRect, BGColor);
                ControlMgr.Instance.SpriteBatch.Draw(DropDownBtnTex, DropDownBtnRect, Color.White);

                if (IsCollapsed)
                {
                    if (SelectedIdx != -1)
                    {
                        // Draw selected item
                        ControlMgr.Instance.SpriteBatch.DrawString(Font, Items[SelectedIdx].ToString(), Location, FontColor);
                    }
                }
                else
                {
                    // BG
                    //ControlMgr.Instance.SpriteBatch.Draw(Common.White1px, ExpandedRectangle, BGColor);
                    // Text
                    TextScroller.Draw(ControlMgr.Instance.SpriteBatch);
                }

                DrawChildControls();
            }
        }
    }
}
