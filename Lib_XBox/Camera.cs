using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace XNALib
{
    public struct Camera
    {
        #region Members
        private Vector2 m_Location;
        public Vector2 Location
        {
            get { return m_Location; }
            set
            {
                if (!RoundLocation)
                    m_Location = value;
                else
                    m_Location = value.Round();
            }
        }

        public Matrix CamMatrix { get { return Matrix.CreateTranslation(-Location.X, -Location.Y, 0); } }

        public bool RoundLocation;

        public int X { get { return (int)Location.X; } }
        public int Y { get { return (int)Location.Y; } }

        private Rectangle m_NoScrollArea;
        public Rectangle NoScrollArea
        {
            get { return m_NoScrollArea; }
            set { m_NoScrollArea = value; }
        }

        public float ScrollSpeed;
        #endregion

        public Camera(Vector2 location, bool roundLocation)
        {
            RoundLocation = roundLocation;
            m_Location = location;
            m_NoScrollArea = Rectangle.Empty;
            ScrollSpeed = 3;
        }

        public Rectangle AddCamera(Rectangle drawRect)
        {
            return new Rectangle(drawRect.X + X, drawRect.Y + Y, drawRect.Width, drawRect.Height);
        }

        public void UpdateScroll(PlayerIndex? playerIdx)
        {
            if (InputMgr.Instance.IsDown(playerIdx, Keys.Up, Buttons.DPadUp))
                Location += new Vector2(0, -ScrollSpeed);

            if (InputMgr.Instance.IsDown(playerIdx, Keys.Right, Buttons.DPadRight))
                Location += new Vector2(ScrollSpeed, 0);

            if (InputMgr.Instance.IsDown(playerIdx, Keys.Down, Buttons.DPadDown))
                Location += new Vector2(0, ScrollSpeed);

            if (InputMgr.Instance.IsDown(playerIdx, Keys.Left, Buttons.DPadLeft))
                Location += new Vector2(-ScrollSpeed, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerRect"></param>
        /// <returns></returns>
        public Vector2 Update(Rectangle playerRect)
        {
            Vector2 correction = Vector2.Zero;

            if (playerRect.Left < NoScrollArea.Left)
                correction.X = NoScrollArea.Left - playerRect.Left;
            if (playerRect.Right > NoScrollArea.Right)
                correction.X = NoScrollArea.Right - playerRect.Right;
            if (playerRect.Top < NoScrollArea.Top)
                correction.Y = NoScrollArea.Top - playerRect.Top;
            if (playerRect.Bottom > NoScrollArea.Bottom)
                correction.Y = NoScrollArea.Bottom - playerRect.Bottom;

            Location += correction;

            return correction;
        }
    }
}