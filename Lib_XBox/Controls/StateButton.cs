using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace XNALib.Controls
{
    internal class ButtonState
    {
        public Texture2D UpTex, HoverTex, DownTex;

        public ButtonState(string upTexture, string hoverTexture, string downTexture)
        {
            UpTex = Common.str2Tex(upTexture);
            HoverTex = Common.str2Tex(hoverTexture);
            DownTex = Common.str2Tex(downTexture);
        }
    }

    /// <summary>
    /// A button with near unlimited states (contains textures)
    /// </summary>
    public class StateButton : Button
    {
        private List<ButtonState> States = new List<ButtonState>();
        private int m_ActiveStateIdx = 0;
        public int ActiveStateIdx
        {
            get { return m_ActiveStateIdx; }
            set
            {
                m_ActiveStateIdx = value;
                if (value >= States.Count)
                    m_ActiveStateIdx = 0;
                else if (value < 0)
                    m_ActiveStateIdx = States.Count - 1;
                else
                    m_ActiveStateIdx = value;

                Texture = ActiveState.UpTex;
                HoverTexture = ActiveState.HoverTex;
                DownTexture = ActiveState.DownTex;
            }
        }
        private ButtonState ActiveState { get { return States[ActiveStateIdx]; } }

        public StateButton(Vector2 location, string upTexture, string hoverTexture, string downTexture)
            : base(location, upTexture, hoverTexture, downTexture)
        {
            Click += new OnClick(StateButton_Click);
            States.Add(new ButtonState(upTexture, hoverTexture, downTexture));
        }

        public StateButton(Vector2 location, string upTexture, string hoverTexture, string downTexture, string upTexture2, string hoverTexture2, string downTexture2)
            : base(location, upTexture, hoverTexture, downTexture)
        {
            Click += new OnClick(StateButton_Click);
            States.Add(new ButtonState(upTexture, hoverTexture, downTexture));
            States.Add(new ButtonState(upTexture2, hoverTexture2, downTexture2));
        }

        ~StateButton()
        {
            Click -= new OnClick(StateButton_Click);
        }

        public void AddState(string upTexture, string hoverTexture, string downTexture)
        {
            States.Add(new ButtonState(upTexture, hoverTexture, downTexture));
        }

        void StateButton_Click(Button button)
        {
            ActiveStateIdx++;
        }
    }
}
