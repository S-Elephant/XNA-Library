using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNALib
{
    /// <summary>
    /// Used to enable scrolling in menus when a up or down buttons is pressed for a certain time.
    /// </summary>
    public class MenuScroller
    {
        public delegate void OnScrollUp();
        public event OnScrollUp ScrollUp;
        public delegate void OnScrollDown();
        public event OnScrollDown ScrollDown;

        public int TriggerTimeInMS = 900;
        public List<PlayerIndex> PlayerIndicesAllowed = new List<PlayerIndex>() { PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four };
        public List<Buttons> ScrollableButtonsUp = new List<Buttons>() { Buttons.DPadUp, Buttons.LeftThumbstickUp, Buttons.RightThumbstickUp };
        public List<Buttons> ScrollableButtonsDown = new List<Buttons>() { Buttons.DPadDown, Buttons.LeftThumbstickDown, Buttons.RightThumbstickDown };

        public int DelayTimeInMS = 100;
        private TimeSpan DelayCounter = new TimeSpan(0, 0, 0, 0, 10000);

        public MenuScroller()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns>true if a scroll was made</returns>
        public bool Update(GameTime gameTime)
        {
            DelayCounter += gameTime.ElapsedGameTime;

            foreach (PlayerIndex pIdx in PlayerIndicesAllowed)
            {
                foreach (Buttons btnUp in ScrollableButtonsUp)
                {
                    if (InputMgr.Instance.GetDownTime(pIdx,Keys.Up,Buttons.DPadUp) >= TriggerTimeInMS)
                    {
                        if (TimeExeeded(btnUp))
                            return true;
                    }
                }
                foreach (Buttons btnDown in ScrollableButtonsDown)
                {
                    if (InputMgr.Instance.GetDownTime(pIdx, Keys.Down, Buttons.DPadDown) >= TriggerTimeInMS)
                    {
                        if (TimeExeeded(btnDown))
                            return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="btn"></param>
        /// <returns>true if a scroll was made</returns>
        private bool TimeExeeded(Buttons btn)
        {
            if (DelayCounter.TotalMilliseconds >= DelayTimeInMS)
            {
                if (ScrollableButtonsUp.Contains(btn))
                {
                    if (ScrollUp != null)
                    {
                        DelayCounter = new TimeSpan();
                        ScrollUp();
                        return true;
                    }
                }
                else
                {
                    if (ScrollDown != null)
                    {
                        DelayCounter = new TimeSpan();
                        ScrollDown();
                        return true;
                    }
                }

            }
            return false;
        }
    }
}
