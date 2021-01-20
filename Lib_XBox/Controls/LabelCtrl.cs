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
    public class LabelCtrl : BaseControl, IControl
    {
        #region Members
        public bool AutoSize = true;

        private StringBuilder m_Text;
        public StringBuilder Text
        {
            get { return m_Text; }
            set
            {
                m_Text = value;
                if (AutoSize)
                {
                    Point measure = Font.MeasureString(Text).ToPoint();
                    AABB = new Rectangle(AABB.X, AABB.Y, measure.X, measure.Y);
                }
            }
        }

        public static SpriteFont DefaultLabelFont = null;
        public SpriteFont Font;
        public Color BGColor = new Color(0, 0, 0, 0);
        public Color ForeColor = Color.Black;
        public override bool HasFocus
        {
            get
            {
                return false;
            }
            set
            {
                throw new Exception("Labels can not have their focus set");
            }
        }
        #endregion

        public LabelCtrl(Vector2 location, StringBuilder text)
        {
            if (DefaultLabelFont == null)
                throw new Exception("LabelCtrl.DefaultLabelFont is null.");
            Font = DefaultLabelFont;
            Location = location;
            Text = text;
        }

        public LabelCtrl(Vector2 location)
        {
            if (DefaultLabelFont == null)
                throw new Exception("LabelCtrl.DefaultLabelFont is null.");
            Font = DefaultLabelFont;
            Location = location;
            Text = new StringBuilder();
        }

        public LabelCtrl(Vector2 location, StringBuilder text, string font)
        {
            Font = Common.str2Font(font);
            Location = location;
            Text = text;
        }

        public void Update(GameTime gameTime)
        {
            // Do nothing
        }

        public void Draw()
        {
            if (IsVisible)
            {
                ControlMgr.Instance.SpriteBatch.Draw(Common.White1px, AABB, BGColor);
                ControlMgr.Instance.SpriteBatch.DrawString(Font, Text, Location, ForeColor);

                DrawChildControls();
            }
        }

        public override void SetFocus()
        {
            // Do nothing
        }
    }
}