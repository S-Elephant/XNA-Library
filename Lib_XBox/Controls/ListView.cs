using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace XNALib.Controls
{
    public class ListViewItem
    {
        public Vector2 LocationInList;
        public Rectangle DrawRect;

        private string m_StrTexture;
        public string StrTexture
        {
            get { return m_StrTexture; }
            set
            {
                m_StrTexture = value;
                if (!string.IsNullOrEmpty(value))
                    Texture = Common.str2Tex(value);
            }
        }
        public bool Draw = false;
        public Texture2D Texture;
        public object Tag = null;
        public ListViewItem(string texture, Vector2 locationInList)
        {
            LocationInList = locationInList;
            StrTexture = texture;
        }
    }

    /// <summary>
    /// Not fully compatible with XBox
    /// Programmed for BrickWalkerIV
    /// </summary>
    public class ListView : BaseControl, IControl
    {
        #region Members
        public bool AllowScrolling = true;

        // BG
        public Texture2D BGTexture;
        public Color BGColor = Color.White;

        // Items
        List<ListViewItem> Items = new List<ListViewItem>();
        Vector2 ImgSize;
        int RowCnt, ColCnt;
        public Color ItemDrawColor = Color.White;
        const int ItemSpacing = 5;
        public int ItemCnt { get { return Items.Count; } }

        private int m_SelIdx = 0;
        public int SelIdx
        {
            get { return m_SelIdx; }
            set
            {
                if (value > Items.Count-1)
                    m_SelIdx = Items.Count-1;
                else if (value < 0)
                    m_SelIdx = 0;
                else
                    m_SelIdx = value;
            }
        }
        public ListViewItem SelectedItem
        {
            get
            {
                if (Items.Count > 0 && SelIdx != -1)
                    return Items[SelIdx];
                else
                    return null;
            }
        }

        #endregion

        public ListView(Rectangle aabb, Texture2D bg, Vector2 imageSize)
        {
            Name = "imgView";
            AABB = aabb;
            BGTexture = bg;
            ImgSize = imageSize;

            // Caculate the number of images per row and column
            RowCnt = (int)(AABB.Width / (ImgSize.X + ItemSpacing));
            ColCnt = (int)(AABB.Height / (ImgSize.Y + ItemSpacing));
        }

        public void Clear()
        {
            SelIdx = 0;
            Items.Clear();
        }

        private int GetNrOfRows()
        {
            return (int)(Items.Count / RowCnt);
        }

        private Vector2 GetNextItemLocation()
        {
            Vector2 result = Vector2.Zero;

            // Get the total number of current rows
            int nrOfRows = GetNrOfRows();

            // Get the X-index
            int colIndex = Items.Count - (nrOfRows * RowCnt);
            // If the next item will exceed the rowcnt then go to the next row
            if (colIndex > RowCnt)
            {
                nrOfRows++;
                colIndex = 0;
            }

            return new Vector2(colIndex * (ImgSize.Y + ItemSpacing), nrOfRows * (ImgSize.X + ItemSpacing));
        }

        public void AddItems(params string[] textures)
        {
            foreach (string texture in textures)
                Items.Add(new ListViewItem(texture, GetNextItemLocation()));
            Scroll(0); // Wil recalculate the items visibility and their drawrects.
        }

        public void AddItem(string texture, object tag)
        {
            Items.Add(new ListViewItem(texture, GetNextItemLocation()) { Tag = tag });
            Scroll(0); // Wil recalculate the items visibility and their drawrects.
        }

        public void AddItemsWithPrefix(string prefix, params string[] textures)
        {
            foreach (string texture in textures)
                Items.Add(new ListViewItem(prefix + texture, GetNextItemLocation()));
            Scroll(0); // Wil recalculate the items visibility and their drawrects.
        }

        private void UpdateItemRectangles()
        {
            foreach (ListViewItem item in Items)
                item.DrawRect = new Rectangle(AABB.X + item.LocationInList.Xi(), AABB.Y + item.LocationInList.Yi(), ImgSize.Xi(), ImgSize.Yi());
        }

        public void Scroll(float amount)
        {
            foreach (ListViewItem item in Items)
            {
                // Scroll
                item.LocationInList += new Vector2(0, amount);

                // Calculate if we should draw this item
                if (item.LocationInList.Y + ImgSize.Y < 0 ||
                    item.LocationInList.Y > AABB.Height)
                    item.Draw = false;
                else
                    item.Draw = true;
            }
            UpdateItemRectangles();
        }

        public void Update(GameTime gameTime)
        {
            if (IsVisible)
            {
                // Mouse Focus
                if (InputMgr.Instance.Mouse != null)
                {
                    HasFocus = Collision.PointIsInRect(InputMgr.Instance.Mouse.Location, AABB);
                }

                if (HasFocus)
                {
                    // Scrolling
                    if (AllowScrolling)
                    {
                        if (InputMgr.Instance.IsPressed(null, Keys.Down, Buttons.DPadDown, Buttons.LeftThumbstickDown, Buttons.RightThumbstickDown))
                            Scroll(-(ImgSize.Y + ItemSpacing));
                        if (InputMgr.Instance.IsPressed(null, Keys.Up, Buttons.DPadUp, Buttons.LeftThumbstickUp, Buttons.RightThumbstickUp))
                            Scroll(ImgSize.Y + ItemSpacing);
                    }

                    // Selection
                    if (InputMgr.Instance.Mouse != null && InputMgr.Instance.Mouse.LeftButtonIsDown)
                    {
                        Vector2 mouseLoc = InputMgr.Instance.Mouse.Location;
                        for (int i = 0; i < Items.Count; i++)
                        {
                            if (Items[i].Draw && Collision.PointIsInRect(mouseLoc, Items[i].DrawRect))
                            {
                                SelIdx = i;
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
                // BG
                if (BGTexture != null)
                    ControlMgr.Instance.SpriteBatch.Draw(BGTexture, AABB, BGColor);

                // Images
                int i = 0;
                foreach (ListViewItem item in Items)
                {
                    if (item.Draw)
                    {
                        // Draw selection if applicable
                        if (i == SelIdx)
                            ControlMgr.Instance.SpriteBatch.Draw(Common.White1px50Trans, new Rectangle(item.DrawRect.X - ItemSpacing / 2, item.DrawRect.Y - ItemSpacing / 2, item.DrawRect.Width + ItemSpacing, item.DrawRect.Height + ItemSpacing), Color.White);
                        // Draw item
                        ControlMgr.Instance.SpriteBatch.Draw(item.Texture, item.DrawRect, ItemDrawColor);
                    }
                    i++;
                }

                DrawChildControls();
            }
        }
    }
}