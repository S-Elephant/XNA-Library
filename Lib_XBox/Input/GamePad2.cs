using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace XNALib
{
    public class GamePad2
    {
        public GamePadState State;
        public GamePadState OldState;

        private PlayerIndex m_PlayerIdx;
        public PlayerIndex PlayerIdx
        {
            get { return m_PlayerIdx; }
            private set { m_PlayerIdx = value; }
        }

        private Dictionary<Buttons,int> m_ButtonDownTimes = new Dictionary<Buttons,int>();
        public Dictionary<Buttons,int> ButtonDownTimes
        {
            get { return m_ButtonDownTimes; }
            private set { m_ButtonDownTimes = value; }
        }
       
        const int BUTTON_CNT = 25;
        public bool UpdateButtonDownTimes = true;

        private static readonly List<Buttons> AllButtons = new List<Buttons>()
        {
            Buttons.A,
            Buttons.B,
            Buttons.Back,
            Buttons.BigButton,
            Buttons.DPadDown,
            Buttons.DPadLeft,
            Buttons.DPadRight,
            Buttons.DPadUp,
            Buttons.LeftShoulder,
            Buttons.LeftStick,
            Buttons.LeftThumbstickDown,
            Buttons.LeftThumbstickLeft,
            Buttons.LeftThumbstickRight,
            Buttons.LeftThumbstickUp,
            Buttons.LeftTrigger,
            Buttons.RightShoulder,
            Buttons.RightStick,
            Buttons.RightThumbstickDown,
            Buttons.RightThumbstickLeft,
            Buttons.RightThumbstickRight,
            Buttons.RightThumbstickUp,
            Buttons.RightTrigger,
            Buttons.Start,
            Buttons.X,
            Buttons.Y
        };


        public bool AnyButtonIsPressed
        {
            get
            {
                foreach (Buttons btn in AllButtons)
                {
                    if (IsPressed(btn))
                        return true;
                }
                return false;
            }
        }

        public GamePad2(PlayerIndex playerIdx)
        {
            PlayerIdx = playerIdx;

            // Button down times
            foreach (Buttons btn in AllButtons)
                ButtonDownTimes.Add(btn, 0);
        }

        public void Update(GameTime gameTime)
        {
            OldState = State;
            State = GamePad.GetState(PlayerIdx);

            // Button down times
            if (UpdateButtonDownTimes)
            {
                foreach (Buttons btn in AllButtons)
                {
                    if (State.IsButtonUp(btn))
                        ButtonDownTimes[btn] = 0;
                    else
                        ButtonDownTimes[btn] += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
        }

        public bool IsPressed(Buttons button)
        {
            return OldState.IsButtonUp(button) && State.IsButtonDown(button);
        }

        public bool IsDown(Buttons button)
        {
            return State.IsButtonDown(button);
        }

        public bool IsDown(params Buttons[] buttons)
        {
            foreach (Buttons btn in buttons)
            {
                if(IsDown(btn))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Occurs when a button is released after pressing it in the previous state
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool IsReleased(Buttons button)
        {
            return OldState.IsButtonDown(button) && State.IsButtonUp(button);
        }

        public bool IsFirstDown(Buttons button)
        {
            return OldState.IsButtonUp(button) && State.IsButtonDown(button);
        }

        #region SimpleMove
        public bool MoveUp()
        {
            return State.IsButtonDown(Buttons.DPadUp) || State.IsButtonDown(Buttons.RightThumbstickUp) || State.IsButtonDown(Buttons.LeftThumbstickUp);
        }
        public bool MoveRight()
        {
            return State.IsButtonDown(Buttons.DPadRight) || State.IsButtonDown(Buttons.RightThumbstickRight) || State.IsButtonDown(Buttons.LeftThumbstickRight);
        }
        public bool MoveDown()
        {
            return State.IsButtonDown(Buttons.DPadDown) || State.IsButtonDown(Buttons.RightThumbstickDown) || State.IsButtonDown(Buttons.LeftThumbstickDown);
        }
        public bool MoveLeft()
        {
            return State.IsButtonDown(Buttons.DPadLeft) || State.IsButtonDown(Buttons.RightThumbstickLeft) || State.IsButtonDown(Buttons.LeftThumbstickLeft);
        }
        #endregion
    }
}
