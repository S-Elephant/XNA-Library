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
    public class IconListItem
    {
        public Texture2D Icon;
        public Point Index = new Point(-2,-2);
        public object Tag;
        public Color DrawColor = Color.White;

        public IconListItem(string iconTexture, object tag)
        {
            Icon = Common.str2Tex(iconTexture);
            Tag = tag;
        }
    }

    public class IconList : BaseControl, IControl
    {
        public delegate void OnSelectedIndexChanged();
        public event OnSelectedIndexChanged SelectedIndexChanged;
        #region Members

        private Size m_IconSize;
        public Size IconSize
        {
            get { return m_IconSize; }
            set
            {
                m_IconSize = value;
            }
        }
       
        Size Size;
        public Color SelectedColor = Color.Red;

        private Point m_IconSpacing = new Point(8,8);
        public Point IconSpacing
        {
            get { return m_IconSpacing; }
            set
            {
                m_IconSpacing = value;
            }
        }
       
        readonly Point EmptySelectionPoint = new Point(-1, -1);
        
        private Point m_SelectedIdx = new Point(-1,-1);
        public Point SelectedIdx
        {
            get { return m_SelectedIdx; }
            set
            {
                if (Items.Count == 0)
                    m_SelectedIdx = EmptySelectionPoint;
                else
                {
                    Point newIdx = new Point((int)MathHelper.Clamp(value.X, 0, int.MaxValue), (int)MathHelper.Clamp(value.Y, 0, UpdateIconsPerRow()));

                    if (Items.Any(i => i.Index == newIdx)) // Only assign if it exists
                    {
                        m_SelectedIdx = newIdx;
                        if (SelectedIndexChanged != null)
                            SelectedIndexChanged();

                        // Scrolling
                        if (m_SelectedIdx.Y + ScrollStep < 0)
                            ScrollStep++;
                        else if (m_SelectedIdx.Y + ScrollStep + 1 > VisibleRowCount())
                            ScrollStep--;
                    }
                }
            }
        }
        public override Rectangle AABB { get { return new Rectangle(Location.Xi(), Location.Yi(), Size.Width, Size.Height); } }

        private int m_ScrollStep = 0;
        public int ScrollStep
        {
            get { return m_ScrollStep; }
            private set { m_ScrollStep = value; }
        }

        public List<IconListItem> Items = new List<IconListItem>();

        public IconListItem SelectedItem
        {
            get { return Items.Find(i=>i.Index == SelectedIdx); }
        }

        public IconListItem LastItem { get { return Items[Items.Count - 1]; } }
        public Texture2D BackGround = null;

        #endregion

        public IconList(Vector2 location, Size size, Size iconSize)
        {
            Location = location;
            Size = size;
            IconSize = iconSize;
        }

        public void AddItem(string texture, object tag)
        {
            Items.Add(new IconListItem(texture, tag));
            UpdateIconIndices();

            if (SelectedIdx == EmptySelectionPoint)
                SelectedIdx = new Point(0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Teh number of maximum visible rows (whetther the iconlist has that many rows or not)</returns>
        int VisibleRowCount()
        {
            return (int)((double)Size.Height / (IconSpacing.Y + IconSize.Height));
        }

        public void UpdateIconIndices()
        {
            int idx = 0;
            int rowLength = UpdateIconsPerRow();
            int rowCnt = RowCount();

            for (int x = 0; x < rowLength; x++)
            {
                for (int y = 0; y < rowCnt; y++)
                {
                    if (idx < Items.Count)
                    {
                        Items[idx].Index = new Point(x, y);
                        idx++;
                    }
                }
            }
        }

        Rectangle Index2DrawRect(Point index)
        {
            return new Rectangle(Location.Xi() + index.X * (IconSize.Width + IconSpacing.X),
                                 Location.Yi() + index.Y * (IconSize.Height + IconSpacing.Y) + ScrollStep * (IconSize.Height + IconSpacing.Y),
                                 IconSize.Width, IconSize.Height);
        }

        int UpdateIconsPerRow()
        {
            return Size.Width / (IconSize.Width + IconSpacing.X);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns the TOTAL amount of rows including any invisible rows</returns>
        int RowCount()
        {
            return (int)Math.Ceiling((double)Items.Count / (double)UpdateIconsPerRow());
        }

        public void Update(GameTime gameTime)
        {
            if (IsVisible)
            {
                UpdateIconIndices();

                if (InputMgr.Instance.IsPressed(null, Keys.Up, Buttons.DPadUp))
                {
                    SelectedIdx = new Point(SelectedIdx.X, SelectedIdx.Y - 1);
                }
                else if (InputMgr.Instance.IsPressed(null, Keys.Right, Buttons.DPadRight))
                {
                    SelectedIdx = new Point(SelectedIdx.X + 1, SelectedIdx.Y);
                }
                else if (InputMgr.Instance.IsPressed(null, Keys.Down, Buttons.DPadDown))
                {
                    SelectedIdx = new Point(SelectedIdx.X, SelectedIdx.Y + 1);
                }
                else if (InputMgr.Instance.IsPressed(null, Keys.Left, Buttons.DPadLeft))
                {
                    SelectedIdx = new Point(SelectedIdx.X - 1, SelectedIdx.Y);
                }
            }
        }

        public void Draw()
        {
            if (IsVisible)
            {
                if (BackGround != null)
                    ControlMgr.Instance.SpriteBatch.Draw(BackGround, new Rectangle(Location.Xi(), Location.Yi(), Size.Width, Size.Height), Color.White);

                foreach (IconListItem item in Items)
                {
                    if (item.Index == SelectedIdx)
                        ControlMgr.Instance.SpriteBatch.Draw(item.Icon, Index2DrawRect(item.Index), SelectedColor);
                    else
                        ControlMgr.Instance.SpriteBatch.Draw(item.Icon, Index2DrawRect(item.Index), item.DrawColor);
                }

                DrawChildControls();
            }
        }
    }
}
