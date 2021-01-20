using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNALib.Controls
{
    public class ToolTipProcessorDefault : IToolTipProcessor
    {
        private int m_ToolTipTTLInMS = 1100;
        public int ToolTipTTLInMS
        {
            get { return m_ToolTipTTLInMS; }
            set { m_ToolTipTTLInMS = value; }
        }

        private SpriteFont m_ToolTipFont;
        public SpriteFont ToolTipFont
        {
            get { return m_ToolTipFont; }
            set { m_ToolTipFont = value; }
        }

        private int m_ToolTipShowDelayInMS = 1100;
        public int ToolTipShowDelayInMS
        {
            get { return m_ToolTipShowDelayInMS; }
            set { m_ToolTipShowDelayInMS = value; }
        }

        private int m_ToolTipMaxWidth = 320;
        public int ToolTipMaxWidth
        {
            get { return m_ToolTipMaxWidth; }
            set { m_ToolTipMaxWidth = value; }
        }

        public void Update(GameTime gameTime)
        {
            foreach (IControl control in ControlMgr.Instance.Controls)
            {
                if (control.IsVisible &&
                    !string.IsNullOrEmpty(control.ToolTip.ToolTipText))
                {
                    control.ToolTip.Update(gameTime, control.MouseIsHovering, control.AABB);
                }
            }
        }
    }
}
