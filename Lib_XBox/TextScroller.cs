using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNALib
{
    internal struct Line
    {
        /// <summary>
        /// The text itself
        /// </summary>
        public string Text;
        
        /// <summary>
        /// Determines whether or not this line will be drawn
        /// </summary>
        public bool Draw;

        /// <summary>
        /// The location of the text-line
        /// </summary>
        public Vector2 Location;
        
        public Line(string text, Vector2 location, bool draw)
        {
            Text = text;
            Location = location;
            Draw = draw;
        }
    }

    public class TextScroller
    {
        private SpriteFont m_Font;
        /// <summary>
        /// The text font
        /// </summary>
        public SpriteFont Font
        {
            get { return m_Font; }
            set
            {
                m_Font = value;
                TextHeight = value.MeasureString(Common.MeasureString).Y;
            }
        }

        private string m_Text;
        /// <summary>
        /// All of the text in this textscroller
        /// </summary>
        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        /// <summary>
        /// The text foreground color
        /// </summary>
        public Color Drawcolor = Color.White;

        /// <summary>
        /// The textscroller's area color
        /// </summary>
        public Color BGColor = Color.Transparent;

        public const float ScrollCorrection = 1.115f;


        public float ScrollOffsetY = 0;
        Rectangle ScissorRect;
        float TextHeight;

        public bool LimitScrollByText = true;

        public TextScroller(Rectangle DrawRect, SpriteFont font, string text)
        {
            ScissorRect = DrawRect;
            Font = font;
            Text = text;
        }

        public void ScrollToBottom()
        {
            Scroll(-int.MaxValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scroll">value 0 or smaller. The smaller the value the more the text will be scrolled down.</param>
        public void Scroll(int scroll)
        {
            ScrollOffsetY += scroll;
            if (LimitScrollByText)
            {
                float strSize = -Font.MeasureString(Misc.WrapText(Font, Text, ScissorRect.Width)).Y * ScrollCorrection;
                ScrollOffsetY = MathHelper.Clamp(ScrollOffsetY, strSize + ScissorRect.Height - TextHeight * 2, 0f);
            }
        }

#warning this function requires more testing. especially the part with "+(int)ScrollOffsetY" because it has not been tested.
        public string GetLineTextAtPoint(Vector2 point, out int selectedLineIdx)
        {
            selectedLineIdx = -1;

            if (!Collision.PointIsInRect(point, ScissorRect))
                return null;

            List<Line> visibleLines = WrapTextByHeight(Font, Misc.WrapText(Font, Text, ScissorRect.Width).ToString(), new Vector2(ScissorRect.X, ScissorRect.Y + ScrollOffsetY), ScissorRect);
            visibleLines.RemoveAll(l => !l.Draw);

            for (int i = 0; i < visibleLines.Count; i++)
            {
                Rectangle lineRect = new Rectangle(visibleLines[i].Location.Xi(), visibleLines[i].Location.Yi()+(int)ScrollOffsetY, ScissorRect.Width, (int)TextHeight);
                if (Collision.PointIsInRect(point, lineRect))
                {
                    selectedLineIdx = i;
                    return visibleLines[i].Text;
                }
            }

            return null;
        }

        private List<Line> WrapTextByHeight(SpriteFont font, string text, Vector2 textLocation, Rectangle drawRect)
        {
            string[] splittedText = text.Split('\n');
            List<Line> result = new List<Line>();

            for (int i = 0; i < splittedText.Count(); i++)
            {
                if (textLocation.Y + TextHeight * i < drawRect.Top) // Above rectangle check
                    result.Add(new Line(splittedText[i], Common.InvalidVector2, false));
                else
                {
                    if (textLocation.Y + TextHeight * (i + 1) > drawRect.Bottom) // Below rectangle check
                        return result;
                    else
                        result.Add(new Line(splittedText[i], new Vector2(textLocation.X, textLocation.Y + TextHeight * i), true));
                }
            }

            return result;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Common.White1px, ScissorRect, BGColor);
            List<Line> visibleLines = WrapTextByHeight(Font, Misc.WrapText(Font, Text, ScissorRect.Width).ToString(), new Vector2(ScissorRect.X, ScissorRect.Y + ScrollOffsetY), ScissorRect);

            foreach (Line visibleLine in visibleLines)
            {
                if (visibleLine.Draw)
                    spriteBatch.DrawString(Font, visibleLine.Text, visibleLine.Location, Drawcolor);
            }
        }
    }
}
