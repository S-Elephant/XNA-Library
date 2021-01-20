using Microsoft.Xna.Framework;
using System.Text;
using System.Collections.Generic;
using System;

namespace XNALib.Controls
{
    public interface IControl
    {
        string Name { get; set; }
        StringBuilder Caption { get; set; }
        Vector2 Location { get; set; }
        /// <summary>
        /// Focus should only be set by the ControlMgr.
        /// </summary>
        bool HasFocus { get; set; }
        Rectangle AABB { get; }

        IControl Parent { get; set; }
        List<IControl> Children { get; }

        /// <summary>
        /// The location without the parent's location
        /// </summary>
        Vector2 BaseLocation { get; }
        void RegisterChildControl(IControl control);
        void UnRegisterChildControl(IControl control);
        void Update(GameTime gameTime);
        void Draw();
        bool IsVisible { get; set; }
        void AddToControls();
        void SetFocus();
        bool MouseIsHovering { get; }

        // Tooltip
        DateTime FocusStart { get; set; }
        ToolTipCtrl ToolTip { get; set; }
        void DrawToolTip();
    }
}
