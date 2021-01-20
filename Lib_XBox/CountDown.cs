using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;

namespace XNALib
{
    /// <summary>
    /// Counts down. At every set interval the IsInterval property is set to true for one upadte cycle.
    /// Dependencies: SimpleTimer.
    /// </summary>
    public class CountDown
    {
        private int TotalMS, IntervalMS;
        public bool IsInterval = false, IsDone = false;
        private SimpleTimer Timer;
        public int TimeLeftMS { get { return TotalMS - (int)Timer.Timer.TotalMilliseconds; } }
        public int TimeLeftSec { get { return (TotalMS - (int)Timer.Timer.TotalMilliseconds) / 1000; } }

        public CountDown(int totalMS, int intervalMS)
        {
            TotalMS = totalMS;
            IntervalMS = intervalMS;
            Timer = new SimpleTimer(intervalMS);
        }

        public void Reset()
        {
            IsDone = false;
            Timer = new SimpleTimer(IntervalMS);
        }

        public void Update(GameTime gameTime)
        {
            if (!IsDone)
            {
                IsInterval = false;
                Timer.Update(gameTime);
                if (Timer.IsDone) // Check if interval time elapsed.
                {
                    IsInterval = true;
                    TotalMS -= IntervalMS;
                    
                    // Check if done
                    if (TotalMS <= 0)
                        IsDone = true;
                    else
                        Timer.Reset();
                }
            }
        }
    }
}
