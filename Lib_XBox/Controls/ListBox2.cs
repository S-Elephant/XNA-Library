using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace XNALib.Controls
{
    public class ShopItem
    {
        public object Value;
        public int Price;
        public int PriceOffsetRight;
        public ShopItem(object value, int price, int priceOffsetRight)
        {
            Value = value;
            Price = price;
            PriceOffsetRight = priceOffsetRight;
        }
        public override string ToString()
        {
            return Value.ToString();//.PadRight(ColWidth, ' ') + Price.ToString();
        }
    }

    public class ListBoxItem
    {
        #region Members
        public object Value;
        public Texture2D Icon;
        public object Tag;
        public object Tag2;
        #endregion
        public ListBoxItem(object value)
        {
            Value = value;
        }
        public ListBoxItem(object value, Texture2D icon)
        {
            Value = value;
            Icon = icon;
        }
    }

    public class ListBox2 : BaseControl, IControl
    {
        #region Events
        public delegate void OnItemSelectedEventHandler(ListBoxItem selectedItem);
        public event OnItemSelectedEventHandler ItemSelected;
        public delegate void OnSelectionChangedEventHandler(ListBoxItem newSelectedItem);
        public event OnSelectionChangedEventHandler SelectionChanged;
        public delegate void OnItemInfoEventHandler(ListBoxItem selectedItem);
        public event OnItemInfoEventHandler ItemInfo;
        #endregion

        #region Members
        public Color SelectorColor = Color.White;

        public enum eAction { Up, Down, Accept, Cancel}
        public Dictionary<eAction, Keys> KeyConfig = new Dictionary<eAction, Keys>() { { eAction.Up, Keys.Up }, { eAction.Down, Keys.Down }, { eAction.Accept, Keys.Enter }, { eAction.Cancel, Keys.Escape } };

        private SpriteFont m_Font;
        public SpriteFont Font
        {
            get { return m_Font; }
            set { m_Font = value; TextHeight = (int)value.MeasureString(Common.MeasureString).Y; }
        }

        public int ShopColWidth = 15;

        private Texture2D m_Texture = null;
        public Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }

        private Color m_BGColor = Color.Pink;
        public Color BGColor
        {
            get { return m_BGColor; }
            set { m_BGColor = value; }
        }

        private List<ListBoxItem> m_Items = new List<ListBoxItem>();
        public List<ListBoxItem> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        private Color m_ForeColor = Color.Black;
        public Color ForeColor
        {
            get { return m_ForeColor; }
            set { m_ForeColor = value; }
        }

        private int m_TextHeight;
        private int TextHeight
        {
            get { return m_TextHeight; }
            set { m_TextHeight = value; }
        }
        private int LineHeight { get { return Math.Max(TextHeight, IconSize); } }

        private Color m_ItemHoverColor = Color.LightGray;
        public Color ItemHoverColor
        {
            get { return m_ItemHoverColor; }
            set { m_ItemHoverColor = value; }
        }

        private Color m_SelectedItemColor = Color.Wheat;
        public Color SelectedItemColor
        {
            get { return m_SelectedItemColor; }
            set { m_SelectedItemColor = value; }
        }

        public ListBoxItem SelectedItem
        {
            get
            {
                if (SelectedIndex == -1)
                    return null;
                else
                    return Items[SelectedIndex];
            }
        }

        private int m_SelectedIndex = -1; // Always use the public member! the private member is not always clamped or set to -1.
        public int SelectedIndex
        {
            get
            {
                if (Items.Count > 0)
                    return (int)MathHelper.Clamp(m_SelectedIndex, 0, Items.Count - 1); // the MathHelper.Clamp is needed because if the last item was selected and was removed then the selected index would be out of range.
                else
                    return -1;
            }
            set
            {
                m_SelectedIndex = value; // Clamp is done in the get{}
            }
        }

        private int MaxItemsToDisplay
        {
            get { return AABB.Height / (LineHeight + spacingY); }
        }
        private int TextWidth
        {
            get { return AABB.Width - 2 * indentX; }
        }

        public bool HasHiddenItemsUp { get { return ScrollValue > 0; } }
        public bool HasHiddenItemsDown { get { return MaxItemsToDisplay + ScrollValue < Items.Count; } }
        public int LowestTextBottom = int.MinValue;
        public int IndentY = 16;

        public int ScrollValue = 0;
        const int IconSize = 32;
        #endregion

        const Buttons ConfirmBtn = Buttons.A;
        const Buttons HelpBtn = Buttons.Y;

        const int spacingY = 3;
        const int indentX = 3;
        const int IconSpacing = 2;
        public PlayerIndex ?ControllingPlayer = null;
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mouse1">Pass null to disable mouse input</param>
        /// <param name="ctrlBG"></param>
        public ListBox2(string name, SpriteFont font, Rectangle drawRectangle)
        {
            Name = name;

            if (font == null)
                Font = Common.DefaultFont;
            else
                Font = font;

            TextHeight = (int)Font.MeasureString(Common.MeasureString).Y;
            AABB = drawRectangle;
        }

        public ListBoxItem AddItem(object value)
        {
            Items.Add(new ListBoxItem(value));
            return Items[Items.Count - 1];
        }
        public ListBoxItem AddItem(object value, Texture2D icon)
        {
            Items.Add(new ListBoxItem(value, icon));
            return Items[Items.Count - 1];
        }
        public ListBoxItem AddShopItem(object value, Texture2D icon, int price, object tag, object tag2)
        {
            ListBoxItem newItem = new ListBoxItem(new ShopItem(value, price, ShopColWidth), icon);
            newItem.Tag = tag;
            newItem.Tag2 = tag2;
            Items.Add(newItem);
            return newItem;
        }
        public ListBoxItem PopSelectedItem()
        {
            ListBoxItem removedItem = Items[SelectedIndex];
            Items.RemoveAt(SelectedIndex);
            return removedItem;
        }

        private void RefreshSelection()
        {
            if (SelectedItem != null)
            {
                if (Items.Contains(SelectedItem))
                {
                    for (int index = 0; index < Items.Count; index++)
                        if (Items[index].Equals(SelectedItem))
                            SelectedIndex = index;
                }
                else
                {
                    SelectedIndex = -1;
                }
            }
        }

        private void HandleScroll()
        {
            if (SelectedIndex >= MaxItemsToDisplay + ScrollValue)
            {
                ScrollValue++;
                SelectedIndex = MaxItemsToDisplay + ScrollValue - 1;
            }
            else if (SelectedIndex < ScrollValue)
            {
                ScrollValue--;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (IsVisible)
            {
                RefreshSelection(); // Must refresh because an item might have been deleted and that could mean a different index and/or a missing selecteditem

                if (HasFocus)
                {
                    if (InputMgr.Instance.IsPressed(ControllingPlayer, Keys.Down, Buttons.DPadDown))
                    {
                        SelectedIndex++;
                        HandleScroll();
                        if (SelectionChanged != null && SelectedItem != null)
                            SelectionChanged(SelectedItem);
                    }
                    if (InputMgr.Instance.IsPressed(ControllingPlayer, Keys.Up, Buttons.DPadUp))
                    {
                        SelectedIndex--;
                        HandleScroll();
                        if (SelectionChanged != null && SelectedItem != null)
                            SelectionChanged(SelectedItem);
                    }
                    if (InputMgr.Instance.IsPressed(ControllingPlayer, InputMgr.Instance.DefaultConfirmKey, InputMgr.Instance.DefaultConfirmButton))
                    {
                        if (ItemSelected != null && SelectedItem != null)
                            ItemSelected(SelectedItem);
                    }
                    if (InputMgr.Instance.IsPressed(ControllingPlayer, Keys.H, Buttons.LeftShoulder))
                    {
                        if (ItemInfo != null)
                            ItemInfo(SelectedItem);
                    }
                }
            }
        }

        private Rectangle GetTextDrawRect(int index)
        {
            return new Rectangle(AABB.Left + indentX, AABB.Top + IndentY + index * (spacingY + LineHeight), TextWidth, LineHeight);
        }

        public void Draw()
        {
            if (IsVisible)
            {
                // Draw background
                if (BGColor != Color.Pink)
                    ControlMgr.Instance.SpriteBatch.Draw(Common.White1px, AABB, BGColor);
                if (Texture != null)
                    ControlMgr.Instance.SpriteBatch.Draw(Texture, AABB, Color.White);

                // Draw Hovering item
                //if (HoveringIndex != -1)
                //    ControlMgr.Instance.SpriteBatch.Draw(Common.White1px, GetTextDrawRect(HoveringIndex), ItemHoverColor);

                // Draw items
                if (Items.Count > 0)
                {
                    int offsetY = IndentY;
                    for (int index = ScrollValue; index < MaxItemsToDisplay + ScrollValue; index++)
                    {
                        if (index < Items.Count && index >= 0)
                        {
                            if (index == SelectedIndex)
                            {
                                // Selector
                                ControlMgr.Instance.SpriteBatch.Draw(Common.White1px50Trans, GetTextDrawRect(SelectedIndex - ScrollValue), SelectorColor);
                                // Draw text
                                DrawItemText(ControlMgr.Instance.SpriteBatch, index, offsetY, SelectedItemColor);
                            }
                            else
                            {
                                // Draw text
                                DrawItemText(ControlMgr.Instance.SpriteBatch, index, offsetY, ForeColor);
                            }
                            // Draw icon
                            if (Items[index].Icon != null)
                                ControlMgr.Instance.SpriteBatch.Draw(Items[index].Icon, new Rectangle(AABB.Left + indentX, AABB.Top + offsetY, IconSize, IconSize), Color.White);
                            // Adjust offset
                            offsetY += spacingY + LineHeight;

                            LowestTextBottom = AABB.Top + offsetY - spacingY;
                        }
                    }
                }

                DrawChildControls();
            }
        }

        private void DrawItemText(SpriteBatch spriteBatch, int index, int offsetY, Color color)
        {
            // Draw Text
            spriteBatch.DrawString(Font, Items[index].Value.ToString(), new Vector2(AABB.Left + indentX + IconSize + IconSpacing, AABB.Top + offsetY + ((LineHeight - TextHeight) / 2)), color);
            // Draw price if applicable
            if (Items[index].Value is ShopItem)
                spriteBatch.DrawString(Font, string.Format("{0:#,###,###.##}", ((ShopItem)Items[index].Value).Price), new Vector2(AABB.Right - ((ShopItem)Items[index].Value).PriceOffsetRight, AABB.Top + offsetY + ((LineHeight - TextHeight) / 2)), color);
        }
    }
}
