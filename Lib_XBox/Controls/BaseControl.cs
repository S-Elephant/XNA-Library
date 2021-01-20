using System;
using System.Collections;
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
    public class BaseControl
    {
        public delegate void OnParentHasChanged(IControl oldParent, IControl newParent);
        public event OnParentHasChanged ParentHasChanged;

        public virtual Vector2 BaseLocation
        {
            get { return m_Location; }
        }

        #region Tooltip
        private DateTime m_FocusStart = DateTime.MinValue;
        public DateTime FocusStart
        {
            get { return m_FocusStart; }
            set { m_FocusStart = value; }
        }

        private ToolTipCtrl m_ToolTip = new ToolTipCtrl();
        public ToolTipCtrl ToolTip
        {
            get { return m_ToolTip; }
            set { m_ToolTip = value; }
        }
        #endregion

        private bool m_IsVisible = true;
        public bool IsVisible
        {
            get
            {
                if (Parent == null)
                    return m_IsVisible;
                else
                    return m_IsVisible && Parent.IsVisible;
            }
            set { m_IsVisible = value; }
        }

        private string m_Name = null;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
       
        protected StringBuilder m_Caption = null;
        public virtual StringBuilder Caption
        {
            get { return m_Caption; }
            set { m_Caption = value; }
        }
        
        protected bool m_HasFocus = false;
        public virtual bool HasFocus
        {
            get { return m_HasFocus; }
            set { m_HasFocus = value && IsVisible; }
        }

        protected Vector2 m_Location;
        public virtual Vector2 Location
        {
            get
            {
                if (Parent == null)
                    return m_Location;
                else
                    return m_Location + Parent.Location;
            }
            set
            {
                m_Location = value;
                m_AABB.Location = value.ToPoint();
            }
        }

        protected Rectangle m_AABB;
        public virtual Rectangle AABB
        {
            get
            {
                if (Parent == null)
                    return m_AABB;
                else
                    return new Rectangle(m_AABB.X + Parent.AABB.X, m_AABB.Y + Parent.AABB.Y, m_AABB.Width, m_AABB.Height);
            }
            set
            {
                m_AABB = value;
                m_Location = value.Location.ToVector2();
            }
        }

        protected IControl m_Parent = null;
        public virtual IControl Parent
        {
            get { return m_Parent; }
            set
            {
                IControl oldParent = Parent;
                if (oldParent != null)
                    oldParent.UnRegisterChildControl((IControl)this);
                m_Parent = value;
                if (value != null)
                    value.RegisterChildControl((IControl)this);

                if (ParentHasChanged != null)
                    ParentHasChanged(oldParent, Parent);
            }
        }

        protected List<IControl> m_Children = new List<IControl>();
        public List<IControl> Children
        {
            get { return m_Children; }
            protected set { m_Children = value; }
        }

        public BaseControl()
        {

        }

        public void RegisterChildControl(IControl control)
        {
            Children.Add(control);
        }
        public void UnRegisterChildControl(IControl control)
        {
            Children.Remove(control);
        }
        public void AddToControls()
        {
            ControlMgr.Instance.Controls.Add((IControl)this);
        }

        public virtual void SetFocus()
        {
            HasFocus = Collision.PointIsInRect(InputMgr.Instance.Mouse.Location, AABB) && (ComboBox.GlobalExpandedComboBox == null);
        }

        private bool m_MouseIsHovering = false;
        public bool MouseIsHovering
        {
            get { return HasFocus && !InputMgr.Instance.Mouse.LeftButtonIsDown && !InputMgr.Instance.Mouse.RightButtonIsDown; }
        }

        public void DrawToolTip()
        {
            ToolTip.Draw();
        }

        public virtual void DrawChildControls()
        {
            foreach (IControl c in Children)
            {
                if(!c.HasFocus)
                    c.Draw();
            }
            foreach (IControl c in Children)
            {
                if (c.HasFocus)
                    c.Draw();
            }
        }
    }
}