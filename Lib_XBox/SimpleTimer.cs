using System;
using Microsoft.Xna.Framework;

namespace XNALib
{
    public class SimpleTimer
    {
        public int TimeInMS;
        public TimeSpan Timer;
        public bool IsDone = false;
        public float PercentageComplete
        {
            get
            {
                if (IsDone)
                    return 100f;
                else
                    return (((float)Timer.TotalMilliseconds * 100) / (float)TimeInMS);
            }
        }
        public int TimeLeftInSec
        {
            get { return (int)MathHelper.Clamp((TimeInMS - (float)Timer.TotalMilliseconds) / 1000, 0, float.MaxValue); }
        }

        public SimpleTimer(int timeInMS)
        {
            TimeInMS = timeInMS;
            Timer = new TimeSpan();
        }

        public void Reset()
        {
            IsDone = false;
            Timer = new TimeSpan();
        }

        public void Update(GameTime gameTime)
        {
            Timer+=gameTime.ElapsedGameTime;
            if (Timer.TotalMilliseconds >= TimeInMS)
                IsDone = true;
        }
    }
}
