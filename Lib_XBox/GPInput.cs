using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNALib
{
//#if XBOX
    public static class GPInput
    {
        #region DownDict
        private static Dictionary<PlayerIndex, Dictionary<Buttons, TimeSpan>> m_BtnDownTime = new Dictionary<PlayerIndex, Dictionary<Buttons, TimeSpan>>()
        {
            {PlayerIndex.One,new Dictionary<Buttons,TimeSpan>()
                {
                    {Buttons.A,new TimeSpan()},
                    {Buttons.B,new TimeSpan()},
                    {Buttons.Back,new TimeSpan()},
                    {Buttons.BigButton,new TimeSpan()},
                    {Buttons.DPadDown,new TimeSpan()},
                    {Buttons.DPadLeft,new TimeSpan()},
                    {Buttons.DPadRight,new TimeSpan()},
                    {Buttons.DPadUp,new TimeSpan()},
                    {Buttons.LeftShoulder,new TimeSpan()},
                    {Buttons.LeftStick,new TimeSpan()},
                    {Buttons.LeftThumbstickDown,new TimeSpan()},
                    {Buttons.LeftThumbstickLeft,new TimeSpan()},
                    {Buttons.LeftThumbstickRight,new TimeSpan()},
                    {Buttons.LeftThumbstickUp,new TimeSpan()},
                    {Buttons.LeftTrigger,new TimeSpan()},
                    {Buttons.RightShoulder,new TimeSpan()},
                    {Buttons.RightStick,new TimeSpan()},
                    {Buttons.RightThumbstickDown,new TimeSpan()},
                    {Buttons.RightThumbstickLeft,new TimeSpan()},
                    {Buttons.RightThumbstickRight,new TimeSpan()},
                    {Buttons.RightThumbstickUp,new TimeSpan()},
                    {Buttons.RightTrigger,new TimeSpan()},
                    {Buttons.Start,new TimeSpan()},
                    {Buttons.X,new TimeSpan()},
                    {Buttons.Y,new TimeSpan()}
                }
            },
            {PlayerIndex.Two,new Dictionary<Buttons,TimeSpan>()
                {
                    {Buttons.A,new TimeSpan()},
                    {Buttons.B,new TimeSpan()},
                    {Buttons.Back,new TimeSpan()},
                    {Buttons.BigButton,new TimeSpan()},
                    {Buttons.DPadDown,new TimeSpan()},
                    {Buttons.DPadLeft,new TimeSpan()},
                    {Buttons.DPadRight,new TimeSpan()},
                    {Buttons.DPadUp,new TimeSpan()},
                    {Buttons.LeftShoulder,new TimeSpan()},
                    {Buttons.LeftStick,new TimeSpan()},
                    {Buttons.LeftThumbstickDown,new TimeSpan()},
                    {Buttons.LeftThumbstickLeft,new TimeSpan()},
                    {Buttons.LeftThumbstickRight,new TimeSpan()},
                    {Buttons.LeftThumbstickUp,new TimeSpan()},
                    {Buttons.LeftTrigger,new TimeSpan()},
                    {Buttons.RightShoulder,new TimeSpan()},
                    {Buttons.RightStick,new TimeSpan()},
                    {Buttons.RightThumbstickDown,new TimeSpan()},
                    {Buttons.RightThumbstickLeft,new TimeSpan()},
                    {Buttons.RightThumbstickRight,new TimeSpan()},
                    {Buttons.RightThumbstickUp,new TimeSpan()},
                    {Buttons.RightTrigger,new TimeSpan()},
                    {Buttons.Start,new TimeSpan()},
                    {Buttons.X,new TimeSpan()},
                    {Buttons.Y,new TimeSpan()}
                }
            },
            {PlayerIndex.Three,new Dictionary<Buttons,TimeSpan>()
                {
                    {Buttons.A,new TimeSpan()},
                    {Buttons.B,new TimeSpan()},
                    {Buttons.Back,new TimeSpan()},
                    {Buttons.BigButton,new TimeSpan()},
                    {Buttons.DPadDown,new TimeSpan()},
                    {Buttons.DPadLeft,new TimeSpan()},
                    {Buttons.DPadRight,new TimeSpan()},
                    {Buttons.DPadUp,new TimeSpan()},
                    {Buttons.LeftShoulder,new TimeSpan()},
                    {Buttons.LeftStick,new TimeSpan()},
                    {Buttons.LeftThumbstickDown,new TimeSpan()},
                    {Buttons.LeftThumbstickLeft,new TimeSpan()},
                    {Buttons.LeftThumbstickRight,new TimeSpan()},
                    {Buttons.LeftThumbstickUp,new TimeSpan()},
                    {Buttons.LeftTrigger,new TimeSpan()},
                    {Buttons.RightShoulder,new TimeSpan()},
                    {Buttons.RightStick,new TimeSpan()},
                    {Buttons.RightThumbstickDown,new TimeSpan()},
                    {Buttons.RightThumbstickLeft,new TimeSpan()},
                    {Buttons.RightThumbstickRight,new TimeSpan()},
                    {Buttons.RightThumbstickUp,new TimeSpan()},
                    {Buttons.RightTrigger,new TimeSpan()},
                    {Buttons.Start,new TimeSpan()},
                    {Buttons.X,new TimeSpan()},
                    {Buttons.Y,new TimeSpan()}
                }
            },
            {PlayerIndex.Four,new Dictionary<Buttons,TimeSpan>()
                {
                    {Buttons.A,new TimeSpan()},
                    {Buttons.B,new TimeSpan()},
                    {Buttons.Back,new TimeSpan()},
                    {Buttons.BigButton,new TimeSpan()},
                    {Buttons.DPadDown,new TimeSpan()},
                    {Buttons.DPadLeft,new TimeSpan()},
                    {Buttons.DPadRight,new TimeSpan()},
                    {Buttons.DPadUp,new TimeSpan()},
                    {Buttons.LeftShoulder,new TimeSpan()},
                    {Buttons.LeftStick,new TimeSpan()},
                    {Buttons.LeftThumbstickDown,new TimeSpan()},
                    {Buttons.LeftThumbstickLeft,new TimeSpan()},
                    {Buttons.LeftThumbstickRight,new TimeSpan()},
                    {Buttons.LeftThumbstickUp,new TimeSpan()},
                    {Buttons.LeftTrigger,new TimeSpan()},
                    {Buttons.RightShoulder,new TimeSpan()},
                    {Buttons.RightStick,new TimeSpan()},
                    {Buttons.RightThumbstickDown,new TimeSpan()},
                    {Buttons.RightThumbstickLeft,new TimeSpan()},
                    {Buttons.RightThumbstickRight,new TimeSpan()},
                    {Buttons.RightThumbstickUp,new TimeSpan()},
                    {Buttons.RightTrigger,new TimeSpan()},
                    {Buttons.Start,new TimeSpan()},
                    {Buttons.X,new TimeSpan()},
                    {Buttons.Y,new TimeSpan()}
                }
            }
        };
        public static Dictionary<PlayerIndex, Dictionary<Buttons,TimeSpan>> BtnDownTime
        {
            get { return m_BtnDownTime; }
            private set { m_BtnDownTime = value; }
        }
        #endregion
        public static bool UpdateBtnDowntimes = false;

        private static Dictionary<PlayerIndex, GamePadState> m_States = new Dictionary<PlayerIndex, GamePadState>()
            {
                {PlayerIndex.One, GamePad.GetState(PlayerIndex.One)},
                {PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two)},
                {PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three)},
                {PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four)}
            };
        public static Dictionary<PlayerIndex, GamePadState> States { get { return m_States; } }

        private static Dictionary<PlayerIndex, GamePadState> m_OldStates = new Dictionary<PlayerIndex, GamePadState>()
                    {
                {PlayerIndex.One, GamePad.GetState(PlayerIndex.One)},
                {PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two)},
                {PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three)},
                {PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four)}
            };
        public static Dictionary<PlayerIndex, GamePadState> OldStates { get { return m_OldStates; } }

        public static void Update(GameTime gameTime)
        {
            SetOldStates();

            m_States[PlayerIndex.One] = GamePad.GetState(PlayerIndex.One);
            m_States[PlayerIndex.Two] = GamePad.GetState(PlayerIndex.Two);
            m_States[PlayerIndex.Three] = GamePad.GetState(PlayerIndex.Three);
            m_States[PlayerIndex.Four] = GamePad.GetState(PlayerIndex.Four);

            // Down times
            if (UpdateBtnDowntimes)
            {
                foreach (PlayerIndex playerIdx in BtnDownTime.Keys)
                {
                    foreach (Buttons btn in Misc.GetValues(new Buttons()))
                    {
                        if (States[playerIdx].IsButtonDown(btn))
                            BtnDownTime[playerIdx][btn] += gameTime.ElapsedGameTime;
                        else
                            BtnDownTime[playerIdx][btn] = new TimeSpan();
                    }
                }
            }
        }

        /// <summary>
        /// Returns largest downtime from all players
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        public static TimeSpan AnyPlayerBtnDownTime(Buttons btn)
        {
            if (!UpdateBtnDowntimes)
                throw new Exception("UpdateBtnDowntimes must be set to true.");

            TimeSpan result = new TimeSpan();
            if (BtnDownTime[PlayerIndex.One][btn] > result)
                result = BtnDownTime[PlayerIndex.One][btn];
            else if (BtnDownTime[PlayerIndex.Two][btn] > result)
                result = BtnDownTime[PlayerIndex.Two][btn];
            else if (BtnDownTime[PlayerIndex.Three][btn] > result)
                result = BtnDownTime[PlayerIndex.Three][btn];
            else if (BtnDownTime[PlayerIndex.Four][btn] > result)
                result = BtnDownTime[PlayerIndex.Four][btn];
            return result;
        }

        private static void SetOldStates()
        {
            m_OldStates[PlayerIndex.One] = m_States[PlayerIndex.One];
            m_OldStates[PlayerIndex.Two] = m_States[PlayerIndex.Two];
            m_OldStates[PlayerIndex.Three] = m_States[PlayerIndex.Three];
            m_OldStates[PlayerIndex.Four] = m_States[PlayerIndex.Four];
        }

        public static bool AnyPlayerReleasesAnyKey(out PlayerIndex pressedPlayerIdx)
        {
            return IsButtonPressed(out pressedPlayerIdx, Buttons.A, Buttons.B, Buttons.X, Buttons.Y, Buttons.BigButton, Buttons.LeftShoulder, Buttons.RightShoulder, Buttons.Start, Buttons.RightTrigger, Buttons.LeftTrigger, Buttons.Back);
        }

        public static bool AnyPlayerReleasesAnyKey()
        {
            return IsButtonPressed(Buttons.A, Buttons.B, Buttons.X, Buttons.Y, Buttons.BigButton, Buttons.LeftShoulder, Buttons.RightShoulder, Buttons.Start, Buttons.RightTrigger, Buttons.LeftTrigger, Buttons.Back);
        }

        [Obsolete("Use IsButtonPressedInstead")]
        public static bool AnyPlayerPressedButton(params Buttons[] buttons)
        {
            foreach (Buttons btn in buttons)
            {
                if (IsButtonPressed(PlayerIndex.One, btn) ||
                IsButtonPressed(PlayerIndex.Two, btn) ||
                IsButtonPressed(PlayerIndex.Three, btn) ||
                IsButtonPressed(PlayerIndex.Four, btn))
                    return true;
            }
            return false;
        }

        public static bool AnyPlayerDownedButton(Buttons button)
        {
            return States[PlayerIndex.One].IsButtonDown(button) ||
                States[PlayerIndex.Two].IsButtonDown(button) ||
                States[PlayerIndex.Three].IsButtonDown(button) ||
                States[PlayerIndex.Four].IsButtonDown(button);
        }

        public static bool MoveUp()
        {
            return MoveUp(PlayerIndex.One) || MoveUp(PlayerIndex.Two) || MoveUp(PlayerIndex.Three) || MoveUp(PlayerIndex.Four);
        }
        public static bool MoveUp(PlayerIndex playerIdx)
        {
            return States[playerIdx].IsButtonDown(Buttons.LeftThumbstickUp) || States[playerIdx].IsButtonDown(Buttons.RightThumbstickUp) || States[playerIdx].IsButtonDown(Buttons.DPadUp);
        }
        public static bool PressedUp()
        {
            return IsButtonPressed(Buttons.LeftThumbstickUp, Buttons.RightThumbstickUp, Buttons.DPadUp);
        }
        public static bool PressedUp(PlayerIndex playerIdx)
        {
            return IsButtonPressed(playerIdx, Buttons.LeftThumbstickUp, Buttons.RightThumbstickUp, Buttons.DPadUp);
        }

        public static bool MoveRight()
        {
            return MoveRight(PlayerIndex.One) || MoveRight(PlayerIndex.Two) || MoveRight(PlayerIndex.Three) || MoveRight(PlayerIndex.Four);
        }
        public static bool MoveRight(PlayerIndex playerIdx)
        {
            return States[playerIdx].IsButtonDown(Buttons.LeftThumbstickRight) || States[playerIdx].IsButtonDown(Buttons.RightThumbstickRight) || States[playerIdx].IsButtonDown(Buttons.DPadRight);
        }
        public static bool PressedRight()
        {
            return IsButtonPressed(Buttons.LeftThumbstickRight, Buttons.RightThumbstickRight, Buttons.DPadRight);
        }
        public static bool PressedRight(PlayerIndex playerIdx)
        {
            return IsButtonPressed(playerIdx, Buttons.LeftThumbstickRight, Buttons.RightThumbstickRight, Buttons.DPadRight);
        }


        public static bool MoveDown()
        {
            return MoveDown(PlayerIndex.One) || MoveDown(PlayerIndex.Two) || MoveDown(PlayerIndex.Three) || MoveDown(PlayerIndex.Four);
        }
        public static bool MoveDown(PlayerIndex playerIdx)
        {
            return States[playerIdx].IsButtonDown(Buttons.LeftThumbstickDown) || States[playerIdx].IsButtonDown(Buttons.RightThumbstickDown) || States[playerIdx].IsButtonDown(Buttons.DPadDown);
        }
        public static bool PressedDown()
        {
            return IsButtonPressed(Buttons.LeftThumbstickDown, Buttons.RightThumbstickDown, Buttons.DPadDown);
        }
        public static bool PressedDown(PlayerIndex playerIdx)
        {
            return IsButtonPressed(playerIdx, Buttons.LeftThumbstickDown, Buttons.RightThumbstickDown, Buttons.DPadDown);
        }

        public static bool MoveLeft()
        {
            return MoveLeft(PlayerIndex.One) || MoveLeft(PlayerIndex.Two) || MoveLeft(PlayerIndex.Three) || MoveLeft(PlayerIndex.Four);
        }
        public static bool MoveLeft(PlayerIndex playerIdx)
        {
            return States[playerIdx].IsButtonDown(Buttons.LeftThumbstickLeft) || States[playerIdx].IsButtonDown(Buttons.RightThumbstickLeft) || States[playerIdx].IsButtonDown(Buttons.DPadLeft);
        }
        public static bool PressedLeft()
        {
            return IsButtonPressed(Buttons.LeftThumbstickLeft, Buttons.RightThumbstickLeft, Buttons.DPadLeft);
        }
        public static bool PressedLeft(PlayerIndex playerIdx)
        {
            return IsButtonPressed(playerIdx, Buttons.LeftThumbstickLeft, Buttons.RightThumbstickLeft, Buttons.DPadLeft);
        }

        public static bool IsButtonPressed(out PlayerIndex pressedPlayerIdx, params Buttons[] buttons)
        {
            for (int playerIdx = 0; playerIdx < 3; playerIdx++)
            {
                if (IsButtonPressed((PlayerIndex)playerIdx, buttons))
                {
                    pressedPlayerIdx = (PlayerIndex)playerIdx;
                    return true;
                }
            }

            pressedPlayerIdx = PlayerIndex.One; // Just return One because there is no null value
            return false;
        }

        public static bool IsButtonPressed(params Buttons[] buttons)
        {
            for (int playerIdx = 0; playerIdx < 3; playerIdx++)
            {
                if (IsButtonPressed((PlayerIndex)playerIdx, buttons))
                    return true;
            }
            return false;
        }
        public static bool IsButtonPressed(Buttons button)
        {
            for (int playerIdx = 0; playerIdx < 3; playerIdx++)
            {
                if (IsButtonPressed((PlayerIndex)playerIdx, button))
                    return true;
            }
            return false;
        }
        public static bool IsButtonPressed(PlayerIndex playerIdx, params Buttons[] buttons)
        {
            foreach (Buttons btn in buttons)
            {
                if (m_OldStates[playerIdx].IsButtonDown(btn) && m_States[playerIdx].IsButtonUp(btn))
                    return true;
            }
            return false;
        }

        public static bool IsButtonDown(PlayerIndex playerIdx, params Buttons[] buttons)
        {
            foreach (Buttons btn in buttons)
            {
                if (m_States[playerIdx].IsButtonDown(btn))
                    return true;
            }
            return false;
        }

        public static bool IsButtonDown(params Buttons[] buttons)
        {
            foreach (Buttons btn in buttons)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (m_States[(PlayerIndex)i].IsButtonDown(btn))
                        return true;
                }
            }
            return false;
        }
    }
//#endif
}
