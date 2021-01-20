using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace XNALib
{
    public class CounterMgr
    {
        public Dictionary<string, Counter> Counters = new Dictionary<string, Counter>();

        public void Update(GameTime gameTime)
        {
            foreach (Counter c in Counters.Values)
                c.Update(gameTime);
        }
    }

    public class Counter
    {
        public bool IsActive;
        public TimeSpan TimeCounter = new TimeSpan();
        public int MaxTimeInMS;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="maxTimeInMS"></param>
        public Counter(int maxTimeInMS)
        {
            MaxTimeInMS = maxTimeInMS;
            IsActive = true;
        }

        /// <summary>
        /// Inactive counter constructor
        /// </summary>
        public Counter()
        {
            IsActive = false;
        }

        public void Update(GameTime gameTime)
        {
            TimeCounter += gameTime.ElapsedGameTime;
            if (IsActive)
            {
                if (TimeCounter.TotalMilliseconds >= MaxTimeInMS)
                {
                    IsActive = false;
                }
            }
        }
    }
}
