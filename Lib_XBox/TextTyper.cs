using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNALib
{
    internal class TTItem
    {
        public string Text = string.Empty;
        public Vector2 Location = Common.InvalidVector2;
        public TTItem()
        {
        }
        public TTItem(Vector2 location, string text)
        {
            Location = location;
            Text = text;
        }
    }

    /// <summary>
    /// TODO: break on words
    /// </summary>
    public class TextTyper
    {
        #region Members
        private SpriteFont m_Font;
        private SpriteFont Font
        {
            get { return m_Font; }
            set { m_Font = value; }
        }
        private Texture2D m_Texture = Common.Black1px50Trans;
        public Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }

        Rectangle DrawRect;
        public string Text;
        int SpeedInMS;
        TimeSpan TypeCounter = new TimeSpan();
        string TypeSound;
        AudioMgr AudioMgr;

        public Color DrawColor = Color.White;
        List<TTItem> Items = new List<TTItem>();
        TTItem currentItem = new TTItem();
        int Cursor = 0;
        bool WaitForNewPage = false;
        eOverflowType OverflowType;
        public bool Done = false;
        public bool AllowLeadingSpaces = false;

        public enum eOverflowType { Wait, Scroll }
        #endregion

        /// <summary>
        /// </summary>
        /// <param name="drawRect"></param>
        /// <param name="font">Text font</param>
        /// <param name="speedInMS">Type-speed</param>
        /// <param name="kb"></param>
        /// <param name="audioMgr">May be null for no sound</param>
        /// <param name="typeSound">May be null for no sound</param>
        /// <param name="overflowType"></param>
        /// <param name="text">The text to type</param>
        public TextTyper(Rectangle drawRect, string font, int speedInMS, AudioMgr audioMgr, string typeSound, eOverflowType overflowType, string text)
        {
            Font = Common.str2Font(font);
            DrawRect = drawRect;
            SpeedInMS = speedInMS;
            TypeSound = typeSound;
            AudioMgr = audioMgr;
            OverflowType = overflowType;
            Text = text;

            ResetItems();
        }

        private void ResetItems()
        {
            Items.Clear();
            currentItem.Location = new Vector2(DrawRect.X, DrawRect.Y);
            Items.Add(currentItem);
        }

        /// <summary>
        /// Keyboard should be updated outside of the console
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (!Done)
            {
                TypeCounter += gameTime.ElapsedGameTime;
                if (TypeCounter.TotalMilliseconds > SpeedInMS && !WaitForNewPage)
                {
                    if (TypeSound != null && AudioMgr != null)
                        AudioMgr.PlaySound(TypeSound);

                    do
                    {
                        char newChar = Text[Cursor];
                        if (newChar != '\n')
                            currentItem.Text += newChar;
                        if (Font.MeasureString(currentItem.Text).X > DrawRect.Width || newChar == '\n') // if true then we need to wrap and jump to a new row
                        {
                            if (DrawRect.Y + (Items.Count + 1) * Font.MeasureString(Common.MeasureString).Y > DrawRect.Bottom) // if true then the drawrectangle is filled with text and has no more space for new rows
                            {
                                if (OverflowType == eOverflowType.Wait)
                                    WaitForNewPage = true;
                                else if (OverflowType == eOverflowType.Scroll)
                                {
                                    Items.RemoveAt(0);
                                    foreach (TTItem item in Items)
                                    {
                                        item.Location.Y -= Font.MeasureString(Common.MeasureString).Y;
                                    }
                                }
                            }
                            // Jump to new row
                            currentItem = new TTItem(new Vector2(DrawRect.X, DrawRect.Y + Items.Count * Font.MeasureString(Common.MeasureString).Y), string.Empty);
                            Items.Add(currentItem);
                        }
                        if (!AllowLeadingSpaces)
                            currentItem.Text = currentItem.Text.TrimStart(' ');
                        Cursor++;
                        Done = Cursor >= Text.Length;
                    } while (!Done && Text[Cursor] == ' ' && !WaitForNewPage);

                    TypeCounter = new TimeSpan();
                }

                if (InputMgr.Instance.AnythingIsPressed(null) && WaitForNewPage)
                {
                    WaitForNewPage = false;
                    ResetItems();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Items.ForEach(i => spriteBatch.DrawString(Font, i.Text, i.Location, DrawColor));
        }
    }
}
