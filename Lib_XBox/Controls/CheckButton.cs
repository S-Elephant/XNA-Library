using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNALib;
using Microsoft.Xna.Framework.Input;

namespace XNALib.Controls
{
    public class CheckButton : Button
    {
        #region Members
        private bool m_IsChecked = false;
        public bool IsChecked
        {
            get { return m_IsChecked; }
            set
            {
                m_IsChecked = value;
            }
        }

        public Texture2D CheckedTexture;
        public Texture2D UnCheckedTexture;
        public Texture2D DownCheckedTexture;
        public Texture2D DownUnCheckedTexture;
        #endregion

        public CheckButton(Vector2 location, string uncheckedTexture, string checkedTexture, string downUnCheckedTexture, string downCheckedTexture) :
            base(location, null, null, null)
        {
            UnCheckedTexture = Common.str2Tex(uncheckedTexture);
            CheckedTexture = Common.str2Tex(checkedTexture);
            DownUnCheckedTexture = Common.str2Tex(downUnCheckedTexture);
            DownCheckedTexture = Common.str2Tex(downCheckedTexture);
        }

        public CheckButton(Vector2 location, string uncheckedTexture, string checkedTexture) :
            base(location, uncheckedTexture)
        {
            UnCheckedTexture = Common.str2Tex(uncheckedTexture);
            CheckedTexture = Common.str2Tex(checkedTexture);
        }

        public void SwitchCheck()
        {
            IsChecked = !IsChecked;
        }

        protected override void DrawByState(Color drawColor)
        {
            if (State != eState.Down)
            {
                if (IsChecked)
                    ControlMgr.Instance.SpriteBatch.Draw(CheckedTexture, Location, drawColor);
                else
                    ControlMgr.Instance.SpriteBatch.Draw(UnCheckedTexture, Location, drawColor);
            }
            else
            {
                if (IsChecked)
                {
                    if (DownCheckedTexture != null)
                        ControlMgr.Instance.SpriteBatch.Draw(DownCheckedTexture, Location, drawColor);
                    else
                        ControlMgr.Instance.SpriteBatch.Draw(CheckedTexture, Location, drawColor);
                }
                else
                {
                    if (DownUnCheckedTexture != null)
                        ControlMgr.Instance.SpriteBatch.Draw(DownUnCheckedTexture, Location, drawColor);
                    else
                        ControlMgr.Instance.SpriteBatch.Draw(UnCheckedTexture, Location, drawColor);
                }
            }
        }
    }
}
