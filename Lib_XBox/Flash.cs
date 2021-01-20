using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNALib
{
    // Use this class to make items or pickups and such flash
    public class Flash
    {
        SimpleTimer Timer;

        private bool m_DrawThisCycle = true;
        public bool DrawThisCycle
        {
            get { return m_DrawThisCycle || !IsFlashing; }
            private set { m_DrawThisCycle = value; }
        }

        public bool IsFlashing = false; // Set to false to disable
        public Flash(int flashSpeedInMS)
        {
            Timer = new SimpleTimer(flashSpeedInMS);
        }

        public Flash()
        {
            Timer = new SimpleTimer(100);
        }

        public void Update(GameTime gameTime)
        {
            if (IsFlashing)
            {
                Timer.Update(gameTime);
                if (Timer.IsDone)
                {
                    DrawThisCycle = !DrawThisCycle;
                    Timer.Reset();
                }
            }
        }
    }
}