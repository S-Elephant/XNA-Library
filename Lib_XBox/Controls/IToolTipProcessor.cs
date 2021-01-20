using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNALib.Controls
{
    public interface IToolTipProcessor
    {
        int ToolTipTTLInMS { get; set; }
        int ToolTipShowDelayInMS { get; set; }
        int ToolTipMaxWidth { get; set; }
        SpriteFont ToolTipFont { get; set; }
        void Update(GameTime gameTime);
    }
}
