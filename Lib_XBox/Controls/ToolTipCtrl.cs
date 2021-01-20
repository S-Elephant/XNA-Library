using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNALib.Controls
{
    public class ToolTipCtrl
    {
        private SimpleTimer TTL;
        private SimpleTimer ShowDelay;
        public bool IsShowingToolTip;
        private string m_ToolTipText = string.Empty;
        public string ToolTipText
        {
            get { return m_ToolTipText; }
            set { m_ToolTipText = value; }
        }
        private StringBuilder ToolTipTextWrapped = new StringBuilder();
        private Vector2 ToolTipLocation;

        public ToolTipCtrl()
        {
            TTL = new SimpleTimer(ControlMgr.Instance.ToolTipProcessor.ToolTipTTLInMS);
            ShowDelay = new SimpleTimer(ControlMgr.Instance.ToolTipProcessor.ToolTipShowDelayInMS);
            IsShowingToolTip = false;
        }

        public void Show(Rectangle controlAABB)
        {
            if (!string.IsNullOrEmpty(ToolTipText))
            {
                IsShowingToolTip = true;
                TTL.Reset();
                ToolTipTextWrapped.ClearCompact();
                ToolTipTextWrapped.Append(Misc.WrapText(ControlMgr.Instance.ToolTipProcessor.ToolTipFont, ToolTipText, ControlMgr.Instance.ToolTipProcessor.ToolTipMaxWidth));

                #region tooltip location
                Vector2 controlCenter = controlAABB.CenterVector();
                Vector2 textMeasure = ControlMgr.Instance.ToolTipProcessor.ToolTipFont.MeasureString(ToolTipTextWrapped);
                if (controlCenter.X > textMeasure.X)
                    ToolTipLocation.X = controlAABB.X - textMeasure.X;
                else
                    ToolTipLocation.X = controlAABB.Right;

                if (controlCenter.Y > textMeasure.Y)
                    ToolTipLocation.Y = controlAABB.Y - textMeasure.Y;
                else
                    ToolTipLocation.Y = controlAABB.Bottom;
                #endregion
            }
        }

        public void Update(GameTime gameTime, bool ToolTipConditionsMet, Rectangle controlAABB)
        {
            if (IsShowingToolTip)
            {
                if (!ToolTipConditionsMet) // Only update the TTL when the conditions for the tooltip are not met.
                {
                    TTL.Update(gameTime);
                    if (TTL.IsDone)
                    {
                        IsShowingToolTip = false;
                        TTL.Reset();
                    }
                }
            }
            else
            {
                if (ToolTipConditionsMet)
                {
                    ShowDelay.Update(gameTime);
                    if (ShowDelay.IsDone)
                    {
                        Show(controlAABB);
                        ShowDelay.Reset();
                    }
                }
            }
        }

        public void Draw()
        {
            if (IsShowingToolTip)
            {
                Vector2 measure = ControlMgr.Instance.ToolTipProcessor.ToolTipFont.MeasureString( ToolTipTextWrapped);
                ControlMgr.Instance.SpriteBatch.Draw(Common.White1px50Trans, new Rectangle(ToolTipLocation.Xi(), ToolTipLocation.Yi(), (int)measure.X, (int)measure.Y), Color.Black);

                ControlMgr.Instance.SpriteBatch.DrawString(ControlMgr.Instance.ToolTipProcessor.ToolTipFont, ToolTipTextWrapped, ToolTipLocation, Color.White);
            }
        }
    }
}
