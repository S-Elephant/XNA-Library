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
    public class Panel : BaseControl, IControl
    {
        #region Members
        public Color DrawColor = new Color(0, 0, 0, 0);
       
        #endregion

        public Panel(Vector2 location, int width, int height)
        {
            Location = location;
            AABB = new Rectangle(location.Xi(), location.Yi(), width, height);
        }

        public void Update(GameTime gameTime)
        {
            // Do nothing
        }

        public void Draw()
        {
            if (IsVisible)
            {
                ControlMgr.Instance.SpriteBatch.Draw(Common.White1px, AABB, DrawColor);
                DrawChildControls();
            }
        }
    }
}