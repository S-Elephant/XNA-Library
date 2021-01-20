using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNALib.Controls
{
    internal class MyThumbNail
    {
        public Texture2D Texture;
        public Rectangle AABB;
        public int Index;
        public bool IsVisible;

        public MyThumbNail(Texture2D texture, Rectangle aabb, int index)
        {
            Texture = texture;
            AABB = aabb;
            Index = index;
        }

        public void UpdateVisibility(Rectangle controlAABB)
        {
            IsVisible = AABB.Intersects(controlAABB);
        }

        public void UpdateAABB(Rectangle parentControlAABB, int scrollIdx, int spacingX)
        {
            AABB.X = parentControlAABB.X + ((Index - scrollIdx) * (AABB.Width + spacingX));
            UpdateVisibility(parentControlAABB);
        }
    }

    /// <summary>
    /// Warning: TODO: Currently the images can be drawn outside of the ThumbnailScroller AABB. This is a known issue.
    /// </summary>
    public class ThumbnailScroller : BaseControl,  IControl
    {
        #region Events
        public delegate void OnThumbnailClick(Texture2D image);
        /// <summary>
        /// Fired when the user clicks on a thumbnail using the mouse
        /// </summary>
        public event OnThumbnailClick ThumbnailClick;

        /// <summary>
        /// Fired after this control performed a valid ("valid" means that the m_ScrollIdx was actually changed) scroll.
        /// </summary>
        public delegate void OnScroll(ThumbnailScroller thumbnailScroller);
        public event OnScroll Scroll;
        #endregion

        #region Members
        /// <summary>
        /// The list of all thumbnails
        /// </summary>
        private List<MyThumbNail> Thumbnails = new List<MyThumbNail>();

        private int m_ScrollIdx = 0;
        /// <summary>
        /// The scroll index for the thumbnails. This determines the X-location of the thumbnails.
        /// </summary>
        public int ScrollIdx
        {
            get { return m_ScrollIdx; }
            set
            {
                value = (int)MathHelper.Clamp(value, 0, Thumbnails.Count - 1);
                bool scrollwasValid = (m_ScrollIdx != value);
                m_ScrollIdx = value;
                Thumbnails.ForEach(t => t.UpdateAABB(AABB, value, ThumbnailSpacingX));

                if (Scroll != null && scrollwasValid)
                    Scroll(this);
            }
        }
        /// <summary>
        /// The width for each thumbnail
        /// </summary>
        private int ThumbnailWidth;
        
        /// <summary>
        /// The thumbnail spacing X
        /// </summary>
        public int ThumbnailSpacingX = 10;
        
        /// <summary>
        /// The thumbnail spacing Y
        /// </summary>
        public int ThumbnailSpacingY;

        /// <summary>
        /// Indicates if the thumbnails can be scrolled to the left (for the user this is the right direction or right scrollbutton)
        /// </summary>
        public bool CanScrollRight { get { return ScrollIdx < Thumbnails.Count - 1; } }

        /// <summary>
        /// Indicates if the thumbnails can be scrolled to the right (for the user this is the left direction or left scrollbutton)
        /// </summary>
        public bool CanScrollLeft { get { return ScrollIdx > 0 && Thumbnails.Count > 0; } }

        public override Vector2 Location
        {
            get
            {
                return base.Location;
            }
            set
            {
                base.Location = value;
                Thumbnails.ForEach(t => t.UpdateAABB(AABB, ScrollIdx, ThumbnailSpacingX));
            }
        }

        public override Rectangle AABB
        {
            get
            {
                return base.AABB;
            }
            set
            {
                base.AABB = value;
                Thumbnails.ForEach(t => t.UpdateAABB(AABB, ScrollIdx, ThumbnailSpacingX));
            }
        }
        #endregion

        public ThumbnailScroller(Rectangle aabb, int thumbnailWidth, int thumbnailSpacingY, params string[] imagePaths)
            : base()
        {
            AABB = aabb;
            ThumbnailWidth = thumbnailWidth;
            ThumbnailSpacingY = thumbnailSpacingY;

            for (int i = 0; i < imagePaths.Count(); i++)
			{
			                 using (Stream stream = File.Open(imagePaths[i], FileMode.Open))
                {
                    Thumbnails.Add(new MyThumbNail(
                                                    Texture2D.FromStream(ControlMgr.Instance.SpriteBatch.GraphicsDevice, stream), 
                                                    new Rectangle(AABB.X+i * (thumbnailWidth + ThumbnailSpacingX), AABB.Y+thumbnailSpacingY, thumbnailWidth, aabb.Height - 2 * thumbnailSpacingY),
                                                    i
                                                ));
                }
			}

            Thumbnails.ForEach(t => t.UpdateVisibility(aabb));
        }

        public void AddThumbnail(string path)
        {
            int i = Thumbnails.Count;
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                Thumbnails.Add(new MyThumbNail(
                                                Texture2D.FromStream(ControlMgr.Instance.SpriteBatch.GraphicsDevice, stream),
                                                new Rectangle(AABB.X + i * (ThumbnailWidth + ThumbnailSpacingX), AABB.Y + ThumbnailSpacingY, ThumbnailWidth, AABB.Height - 2 * ThumbnailSpacingY),
                                                i
                                            ));
            }

            Thumbnails.ForEach(t => t.UpdateAABB(AABB, ScrollIdx, ThumbnailSpacingX)); // Also updates the visibility
        }
        
        public void ClearThumbnails()
        {
            ScrollIdx = 0;
            Thumbnails.Clear();
        }

        public void Update(GameTime gameTime)
        {
            if (InputMgr.Instance.Mouse != null && IsVisible)
            {
                if (Collision.PointIsInRect(InputMgr.Instance.Mouse.Location, AABB))
                {
                    if (ThumbnailClick != null && InputMgr.Instance.Mouse.LeftButtonIsPressed)
                    {
                        foreach (MyThumbNail tn in Thumbnails)
                        {
                            if (tn.IsVisible && Collision.PointIsInRect(InputMgr.Instance.Mouse.Location, tn.AABB))
                            {
                                ThumbnailClick(tn.Texture);
                                break;
                            }
                        }
                    }
                }
            }

            if (HasFocus)
            {
                if (InputMgr.Instance.Keyboard.IsPressed(Keys.Right))
                    ScrollIdx++;
                else if (InputMgr.Instance.Keyboard.IsPressed(Keys.Left))
                    ScrollIdx--;
            }
        }

        public new void Draw()
        {
            for (int i = 0; i < Thumbnails.Count; i++)
            {
                if (Thumbnails[i].IsVisible)
                    ControlMgr.Instance.SpriteBatch.Draw(Thumbnails[i].Texture, Thumbnails[i].AABB, Color.White);
            }
        }
    }
}
