using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace XNALib
{
    public static class TimerContainer
    {
        private static Dictionary<string, Timer> m_Timers = new Dictionary<string, Timer>();
        public static Dictionary<string, Timer> Timers
        {
            get { return m_Timers; }
            set { m_Timers = value; }
        }
    }

    public class Timer
    {
        public delegate void OnTickEventHandler(Timer timer);
            public event OnTickEventHandler OnTick;

        public enum eCountDirection { CountUp = 1, CountDown = -1}
        private TimeSpan m_counter;
        public TimeSpan Counter
        {
            get { return m_counter; }
            set { m_counter = value; }
        }

        private bool m_Enabled = false;
        public bool Enabled
        {
            get { return m_Enabled; }
            set { m_Enabled = value; }
        }

        private eCountDirection CountDirection { get; set; }
        public int TickValue { get; set; }
        public TimeSpan InitialValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialValue"> in MS</param>
        /// <param name="tickValue">in MS</param>
        /// <param name="countDirection"></param>
        public Timer(int initialValue, int tickValue, eCountDirection countDirection)
        {
            InitialValue = new TimeSpan(0, 0, 0, 0, initialValue);
            Counter = InitialValue;
            CountDirection = countDirection;
            TickValue = tickValue;
        }

        public void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                if (CountDirection == eCountDirection.CountUp)
                {
                    Counter += gameTime.ElapsedGameTime;

                    if (Counter.TotalMilliseconds >= TickValue)
                    {
                        if (OnTick != null)
                            OnTick(this);
                        Counter = new TimeSpan();
                    }
                }
                else
                {
                    Counter -= gameTime.ElapsedGameTime;

                    if (Counter.TotalMilliseconds <= TickValue)
                    {
                        if (OnTick != null)
                            OnTick(this);
                        Counter = InitialValue;
                    }
                }
            }
        }
    }
}
