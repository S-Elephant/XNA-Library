using System;
using Microsoft.Xna.Framework;

namespace XNALib
{
    /// <summary>
    /// A simple timer that count's down.
    /// </summary>
    public class SimpleTimer
    {
        #region Members
        public int TimeInMS;
        public TimeSpan Timer;

        /// <summary>
        /// Indicates whether the timer expired or not.
        /// </summary>
        public bool IsDone = false;
        
        /// <summary>
        /// Indicates the timers progress towards expiration
        /// </summary>
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

        /// <summary>
        /// Time left until the timer expires in seconds rounded down.
        /// </summary>
        public int TimeLeftInSec
        {
            get { return (int)MathHelper.Clamp((TimeInMS - (float)Timer.TotalMilliseconds) / 1000, 0, float.MaxValue); }
        }

        /// <summary>
        /// Time left until the timer expires in seconds rounded up.
        /// </summary>
        public int TimeLeftInSecRoundedUp
        {
            get { return (int)Math.Ceiling(MathHelper.Clamp((TimeInMS - (float)Timer.TotalMilliseconds) / 1000, 0, float.MaxValue)); }
        }
        #endregion

        public SimpleTimer(int timeInMS)
        {
            TimeInMS = timeInMS;
            Timer = new TimeSpan();
        }

        /// <summary>
        /// Restarts the timer with the previous TimeInMS.
        /// </summary>
        public void Reset()
        {
            IsDone = false;
            Timer = new TimeSpan();
        }

        /// <summary>
        /// Restarts the timer with the new timespan.
        /// </summary>
        public void Reset(int newTimeInMS)
        {
            TimeInMS = newTimeInMS;
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
