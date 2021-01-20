using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNALib.Controls
{
    /*internal class SortByFocus : IComparer<IControl>
    {
        public int Compare(IControl x, IControl y)
        {
            if (x.HasFocus)
                return 1;
            else if (y.HasFocus)
                return -1;
            else
                return 0;
        }
    }*/

    public class ControlMgr
    {
        public static ControlMgr Instance = null;

        public IToolTipProcessor ToolTipProcessor = new ToolTipProcessorDefault();
        public SpriteBatch SpriteBatch;
        public List<IControl> Controls = new List<IControl>();
        public bool UpdateIsEnabled = true;

        public List<Keys> PressedKeys = new List<Keys>();

        private bool m_AnyControlHasFocus = false;
        public bool AnyControlHasFocus
        {
            get { return m_AnyControlHasFocus; }
            private set { m_AnyControlHasFocus = value; }
        }

        //private static SortByFocus SortByFocus = new SortByFocus();

        public ControlMgr(SpriteBatch spriteBatch)
        {
            SpriteBatch = spriteBatch;
        }

        public ComboBox GetOpenedCombobox()
        {
            return ComboBox.GlobalExpandedComboBox;
            /*
            foreach (IControl ctrl in Controls)
            {
                if (ctrl is ComboBox)
                    if (!((ComboBox)ctrl).IsCollapsed)
                        return (ComboBox)ctrl;
            }
            return null;*/
        }

        public void Update(GameTime gameTime)
        {
            if (UpdateIsEnabled)
            {
                PressedKeys = InputMgr.Instance.Keyboard.GetAllReleasedKeys();
                AnyControlHasFocus = false;
                //Controls.Sort(SortByFocus);
                ComboBox.IgnoreExpansionsThisRun = false;
                for (int i = 0; i < Controls.Count; i++)
                    Controls[i].Update(gameTime);
                bool anyCboIsExpanded = (ComboBox.GlobalExpandedComboBox == null);
                foreach (IControl c in Controls)
                {
                    if (InputMgr.Instance.Mouse != null)
                    {
                        c.SetFocus();
                    }
                    if (c.HasFocus)
                        AnyControlHasFocus = true;
                }

                ToolTipProcessor.Update(gameTime);
            }
        }

        public void Draw()
        {
            foreach (IControl c in Controls)
            {
                if (c.Parent == null & !c.HasFocus)
                    c.Draw();
            }
            foreach (IControl c in Controls)
            {
                if (c.Parent == null & c.HasFocus)
                    c.Draw();
            }
            foreach (IControl c in Controls)
                c.DrawToolTip();
        }
    }
}