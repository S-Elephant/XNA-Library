using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNALib
{
    public class Thumbnail
    {
        public enum eMode { Stretch, OriginalSize, LimitSize }

        #region Events
        public delegate void OnClick(Thumbnail thumbnail);
        /// <summary>
        /// Fired when the user clicks on the thumbnail using the mouse.
        /// </summary>
        public event OnClick Click;
        #endregion

        #region Members
        private eMode m_Mode;
        /// <summary>
        /// The mode for displaying the image
        /// </summary>
        public eMode Mode
        {
            get { return m_Mode; }
            set
            {
                m_Mode = value;
                UpdateAABB();
            }
        }

        /// <summary>
        /// The location of the thumbnail
        /// </summary>
        public Vector2 Location;

        /// <summary>
        /// The image AABB
        /// </summary>
        private Rectangle AABB;

        /// <summary>
        /// Indicates if this thumbnail is visible
        /// </summary>
        public bool IsVisible = true;

        private Texture2D m_Image = null;
        /// <summary>
        /// The image to display
        /// </summary>
        public Texture2D Image
        {
            get { return m_Image; }
            set
            {
                m_Image = value;
                UpdateAABB();
            }
        }

        /// <summary>
        /// The spritebatch used for drawing
        /// </summary>
        private SpriteBatch Batch;

        /// <summary>
        /// Draw color
        /// </summary>
        public Color DrawColor = Color.White;

        /// <summary>
        /// The (maximum) width for the thumbnail
        /// </summary>
        public int MaxWidth;
        /// <summary>
        /// The (maximum) height for the thumbnail
        /// </summary>
        public int MaxHeight;
        #endregion

        public Thumbnail(Vector2 location, int maxWidth, int maxHeight, SpriteBatch spriteBatch, Texture2D image, eMode mode)
        {
            Image = image;
            Location = location;
            MaxWidth = maxWidth;
            MaxHeight = maxHeight;
            Batch = spriteBatch;
            AABB = new Rectangle(location.Xi(), location.Yi(), maxWidth, maxHeight);
            Mode = mode; // Assign this one AFTER setting the AABB and the Image variables.
        }

        /// <summary>
        /// Updates the image AABB
        /// </summary>
        private void UpdateAABB()
        {
            switch (Mode)
            {
                case eMode.Stretch:
                    AABB = new Rectangle(AABB.X, AABB.Y, MaxWidth, MaxHeight);
                    break;
                case eMode.OriginalSize:
                    AABB = new Rectangle(AABB.X, AABB.Y, Image.Width, Image.Height);
                    break;
                case eMode.LimitSize:
                    AABB = new Rectangle(AABB.X, AABB.Y, Math.Min(MaxWidth, Image.Width), Math.Min(MaxHeight, Image.Height));
                    break;
                default:
                    throw new CaseStatementMissingException();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (InputMgr.Instance.Mouse != null && Click != null && IsVisible && InputMgr.Instance.Mouse.LeftButtonIsPressed)
            {
                if (Collision.PointIsInRect(InputMgr.Instance.Mouse.Location, AABB))
                    Click(this);
            }
        }

        public void Draw()
        {
            if(IsVisible)
                Batch.Draw(Image, AABB, DrawColor);
        }
    }
}
