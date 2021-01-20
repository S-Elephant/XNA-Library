using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace XNALib
{
    /// <summary>
    /// Author: Napoleon August ~20 2011
    /// </summary>
    public class InputMgr
    {
        #region Members
        /// <summary>
        /// Singleton instance
        /// </summary>
        public static InputMgr Instance;

        // Windows
        public Keyboard1 Keyboard;
        /// <summary>
        /// Windows Mouse
        /// </summary>
        public Mouse2 Mouse = null;

        /// <summary>
        /// Whether either shift key is in a down-state this cycle.
        /// </summary>
        public bool ShiftIsDown { get { return m_ShiftIsDown; } }
        private bool m_ShiftIsDown = false;

        // XBox
        private GamePad2[] m_GamePads;
        public GamePad2[] GamePads
        {
            get { return m_GamePads; }
            private set { m_GamePads = value; }
        }
        public bool[] GamePadWasConnected = new bool[] { false, false, false, false };
       
        /// <summary>
        /// XBOX mice
        /// </summary>
        public MouseXbox[] Mouses = null;

        // Default buttons
        public Buttons DefaultConfirmButton = Buttons.A;
        public Buttons DefaultCancelButton = Buttons.B;
        public Keys DefaultConfirmKey = Keys.Enter;
        public Keys DefaultCancelKey = Keys.Escape;

        // Other
        public bool DrawMice = true;
        #endregion

        public InputMgr()
        {
            Keyboard = new Keyboard1();

            GamePads = new GamePad2[4]
           {
               new GamePad2(PlayerIndex.One),
               new GamePad2(PlayerIndex.Two),
               new GamePad2(PlayerIndex.Three),
               new GamePad2(PlayerIndex.Four)
           };
        }

        public static string KeyToString(Keys key)
        {
            if (key >= Keys.A && key <= Keys.Z) // alphanumeric                            
            {
                if (InputMgr.Instance.ShiftIsDown)
                    return key.ToString();
                else
                    return key.ToString().ToLower();
            }
            else if (key >= Keys.D0 && key <= Keys.D9) // numeric
            {
                if (!InputMgr.Instance.ShiftIsDown)
                    return key.ToString().Substring(1, 1);
                else
                {
                    switch (key)
                    {
                        case Keys.D0:
                            return ")";
                        case Keys.D1:
                            return "!";
                        case Keys.D2:
                            return "@";
                        case Keys.D3:
                            return "#";
                        case Keys.D4:
                            return "$";
                        case Keys.D5:
                            return "%";
                        case Keys.D6:
                            return "^";
                        case Keys.D7:
                            return "&";
                        case Keys.D8:
                            return "*";
                        case Keys.D9:
                            return "(";
                        default:
                            throw new Exception("How is this possible?");
                    }
                }
            }
            #region Special keys
            else if (key == Keys.OemCloseBrackets)
            {
                if (InputMgr.Instance.ShiftIsDown)
                    return "}";
                else
                    return "]";
            }
            else if (key == Keys.OemOpenBrackets)
            {
                if (InputMgr.Instance.ShiftIsDown)
                    return "{";
                else
                    return "[";
            }
            else if (key == Keys.OemMinus)
            {
                if (InputMgr.Instance.ShiftIsDown)
                    return "_";
                else
                    return "-";
            }
            else if (key == Keys.Space)
                return " ";
            else if (key == Keys.Divide)
            {
                if (InputMgr.Instance.ShiftIsDown)
                    return "?";
                else
                    return "/";
            }
            else if (key == Keys.OemComma)
            {
                if (InputMgr.Instance.ShiftIsDown)
                    return "<";
                else
                    return ",";
            }
            else if (key == Keys.OemPeriod)
            {
                if (InputMgr.Instance.ShiftIsDown)
                    return ">";
                else
                    return ".";
            }
            else if (key == Keys.OemPipe || key == Keys.OemBackslash)
            {
                if (InputMgr.Instance.ShiftIsDown)
                    return "|";
                else
                    return @"\";
            }
            else if (key == Keys.OemPlus)
            {
                if (InputMgr.Instance.ShiftIsDown)
                    return "+";
                else
                    return "+";
            }
            else if (key == Keys.OemQuestion)
            {
                if (InputMgr.Instance.ShiftIsDown)
                    return "?";
                else
                    return "/";
            }
            else if (key == Keys.OemQuotes)
            {
                if (InputMgr.Instance.ShiftIsDown)
                    return "\"";
                else
                    return "'";
            }
            else if (key == Keys.OemSemicolon)
            {
                if (InputMgr.Instance.ShiftIsDown)
                    return ":";
                else
                    return ";";
            }
            else if (key == Keys.OemTilde)
            {
                if (InputMgr.Instance.ShiftIsDown)
                    return "~";
                else
                    return "`";
            }                
            #endregion

            return string.Empty;
        }

        public void SetUpdateDownTimes(bool enabled)
        {
            Keyboard.UpdateKeyDownTimes = enabled;
            GamePads[0].UpdateButtonDownTimes = GamePads[1].UpdateButtonDownTimes = GamePads[2].UpdateButtonDownTimes = GamePads[3].UpdateButtonDownTimes = enabled;
        }

        public void Update(GameTime gameTime, bool updMouseToo)
        {
#if WINDOWS
            Keyboard.Update(gameTime);
            m_ShiftIsDown = Keyboard.State.IsKeyDown(Keys.LeftShift) || Keyboard.State.IsKeyDown(Keys.RightShift);
            if (Mouse != null && updMouseToo)
                Mouse.Update(gameTime);
#endif
#if XBOX
            for (int i = 0; i < 4; i++)
            {
                GamePads[i].Update(gameTime);

                if (GamePads[i].State.IsConnected)
                    GamePadWasConnected[i] = true;

                 if (Mouses != null)
                     Mouses[i].Update(gameTime);
            }
#endif
        }

        public void Update(GameTime gameTime)
        {
            Update(gameTime, true);
        }

        public bool IsDown(PlayerIndex? player, Keys key, Buttons button)
        {
            if (player.HasValue)
            {
                return Keyboard.State.IsKeyDown(key) || GamePads[(int)player].IsDown(button);
            }
            else
            {
                return Keyboard.State.IsKeyDown(key) ||
                    GamePads[0].IsDown(button) ||
                    GamePads[1].IsDown(button) ||
                    GamePads[2].IsDown(button) ||
                    GamePads[3].IsDown(button);
            }
        }
      
        public bool IsDown(PlayerIndex? player, Keys key, params Buttons[] buttons)
        {
            if (player.HasValue)
            {
                return Keyboard.State.IsKeyDown(key) || IsDown(buttons);
            }
            else
            {
                return Keyboard.State.IsKeyDown(key) ||
                    GamePads[0].IsDown(buttons) ||
                    GamePads[1].IsDown(buttons) ||
                    GamePads[2].IsDown(buttons) ||
                    GamePads[3].IsDown(buttons);
            }
        }

        public bool IsPressed(PlayerIndex ?player, Keys key, Buttons button)
        {
            if (player.HasValue)
            {
                return Keyboard.IsPressed(key) || GamePads[(int)player].IsPressed(button);
            }
            else
            {
                return Keyboard.IsPressed(key) ||
                    GamePads[0].IsPressed(button) ||
                    GamePads[1].IsPressed(button) ||
                    GamePads[2].IsPressed(button) ||
                    GamePads[3].IsPressed(button);
            }
        }

        public bool IsPressed(PlayerIndex? player, Keys key, params Buttons[] buttons)
        {
            if (player.HasValue)
            {
                return Keyboard.IsPressed(key) || IsPressed((PlayerIndex)player, buttons);
            }
            else
            {
                return Keyboard.IsPressed(key) ||
                   IsPressed(PlayerIndex.One, buttons) ||
                   IsPressed(PlayerIndex.Two, buttons) ||
                   IsPressed(PlayerIndex.Three, buttons) ||
                   IsPressed(PlayerIndex.Four, buttons);
            }
        }

        public bool IsFirstDown(PlayerIndex? player, Keys key, Buttons button)
        {
            if (player.HasValue)
            {
                return Keyboard.IsFirstDown(key) || GamePads[(int)player].IsFirstDown(button);
            }
            else
            {
                return Keyboard.IsFirstDown(key) ||
                    GamePads[0].IsFirstDown(button) ||
                    GamePads[1].IsFirstDown(button) ||
                    GamePads[2].IsFirstDown(button) ||
                    GamePads[3].IsFirstDown(button);
            }
        }

        /// <summary>
        /// Returns the largest downtime for the specified key/button.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="key"></param>
        /// <param name="button"></param>
        /// <returns></returns>
        public int GetDownTime(PlayerIndex? player, Keys key, Buttons button)
        {
            if (player.HasValue)
            {
                int controllingPlayer = (int)player;
                return Math.Max(Keyboard.KeyDownTimes[key], GamePads[controllingPlayer].ButtonDownTimes[button]);
            }
            else
            {
                int result = 0;
                result = Math.Max(result, Keyboard.KeyDownTimes[key]);
                result = Math.Max(result, GamePads[0].ButtonDownTimes[button]);
                result = Math.Max(result, GamePads[1].ButtonDownTimes[button]);
                result = Math.Max(result, GamePads[2].ButtonDownTimes[button]);
                result = Math.Max(result, GamePads[3].ButtonDownTimes[button]);
                return result;
            }
        }

        public bool AnythingIsPressed(PlayerIndex? player)
        {
            if (player.HasValue)
            {
                int controllingPlayer = (int)player;
                return Keyboard.AnyKeyIsPressed || GamePads[controllingPlayer].AnyButtonIsPressed;
            }
            else
            {
                return Keyboard.AnyKeyIsPressed ||
                    GamePads[0].AnyButtonIsPressed ||
                    GamePads[1].AnyButtonIsPressed ||
                    GamePads[2].AnyButtonIsPressed ||
                    GamePads[3].AnyButtonIsPressed;
            }
        }

        #region Mouse
        public bool Mouse_LeftIsPressed(PlayerIndex? player)
        {
            if (Mouse != null)
                return Mouse.LeftButtonIsPressed;
            else
            {
                if (player.HasValue)
                {
                    int controllingPlayer = (int)player;
                    return Mouses[controllingPlayer].LeftButtonIsPressed;
                }
                else
                {
                    return Mouses[0].LeftButtonIsPressed ||
                        Mouses[1].LeftButtonIsPressed ||
                        Mouses[2].LeftButtonIsPressed ||
                        Mouses[3].LeftButtonIsPressed;
                }
            }
        }

        public bool Mouse_RightIsPressed(PlayerIndex? player)
        {
            if (Mouse != null)
                return Mouse.RightButtonIsPressed;
            else
            {
                if (player.HasValue)
                {
                    int controllingPlayer = (int)player;
                    return Mouses[controllingPlayer].RightButtonIsPressed;
                }
                else
                {
                    return Mouses[0].RightButtonIsPressed ||
                        Mouses[1].RightButtonIsPressed ||
                        Mouses[2].RightButtonIsPressed ||
                        Mouses[3].RightButtonIsPressed;
                }
            }
        }

        public bool Mouse_LeftIsDown(PlayerIndex? player)
        {
            if (Mouse != null)
                return Mouse.LeftButtonIsDown;
            else
            {
                if (player.HasValue)
                {
                    int controllingPlayer = (int)player;
                    return Mouses[controllingPlayer].LeftButtonIsDown;
                }
                else
                {
                    return Mouses[0].LeftButtonIsDown ||
                        Mouses[1].LeftButtonIsDown ||
                        Mouses[2].LeftButtonIsDown ||
                        Mouses[3].LeftButtonIsDown;
                }
            }
        }

        public bool Mouse_RightIsDown(PlayerIndex? player)
        {
            if (Mouse != null)
                return Mouse.RightButtonIsDown;
            else
            {
                if (player.HasValue)
                {
                    int controllingPlayer = (int)player;
                    return Mouses[controllingPlayer].RightButtonIsDown;
                }
                else
                {
                    return Mouses[0].RightButtonIsDown ||
                        Mouses[1].RightButtonIsDown ||
                        Mouses[2].RightButtonIsDown ||
                        Mouses[3].RightButtonIsDown;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <returns>mouse location of player 1 when its XBox and player-variable equals null.</returns>
        public Vector2 Mouse_Location(PlayerIndex? player)
        {
            if (Mouse != null)
                return Mouse.Location;
            else
            {
                if (player.HasValue)
                {
                    int controllingPlayer = (int)player;
                    return Mouses[controllingPlayer].Location;
                }
                else
                {
                    return Mouses[0].Location;
                }
            }
        }
        #endregion

        #region XBox Specific
        /// <summary>
        /// Returns the number of currently (at the time of calling this procedure) connected gamepads.
        /// </summary>
        /// <returns></returns>
        public int ConnectedGamepadsCnt()
        {
            int result = 0;
            for (int i = 0; i < 4; i++)
            {
                if (GamePads[i].State.IsConnected)
                    result++;
            }
            return result;
        }

        /// <summary>
        /// Returns the X,Y from the thumbsticks of the controller. When both thumbsticks are down at the same time then the LeftThumbSticks value(s) are returned.
        /// </summary>
        /// <param name="playerIdx"></param>
        /// <returns></returns>
        public Vector2 Thumbsticks(int playerIdx)
        {
            float x=0, y=0;
            
            // X
            x = GamePads[playerIdx].State.ThumbSticks.Left.X;
            if(x == 0)
                x = GamePads[playerIdx].State.ThumbSticks.Right.X;

            // Y
            y = GamePads[playerIdx].State.ThumbSticks.Left.Y;
            if (y == 0)
                y = GamePads[playerIdx].State.ThumbSticks.Right.Y;

            return new Vector2(x, y);
        }
        public Vector2 Thumbsticks(PlayerIndex playerIdx)
        {
            return Thumbsticks((int)playerIdx);
        }

        /// <summary>
        /// Checks if any of the supplied buttons for any player is pressed.
        /// </summary>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public bool IsPressed(params Buttons[] buttons)
        {
            foreach (Buttons btn in buttons)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (GamePads[i].IsPressed(btn))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if any of the supplied buttons for the specified player is pressed.
        /// </summary>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public bool IsPressed(PlayerIndex playerIdx, params Buttons[] buttons)
        {
            int iplayerIdx = (int)playerIdx;
            foreach (Buttons btn in buttons)
            {
                if (GamePads[iplayerIdx].IsPressed(btn))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if any of the supplied buttons for any player is down.
        /// </summary>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public bool IsDown(params Buttons[] buttons)
        {
            foreach (Buttons btn in buttons)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (GamePads[i].IsDown(btn))
                        return true;
                }
            }
            return false;
        }
        #endregion

        public void DrawMouse(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (DrawMice)
            {
#if WINDOWS
                // Draws windows mouse
                if (Mouse != null)
                    Mouse.Draw(spriteBatch, offset);
#endif
#if XBOX
                // Draws XBox mouse
                if (Mouses != null)
                {
                    foreach (MouseXbox mouse in Mouses)
                        mouse.Draw(spriteBatch, offset);
                }
#endif
            }
        }

        public void DrawMouse(SpriteBatch spriteBatch)
        {
            DrawMouse(spriteBatch, Vector2.Zero);
        }
    }
}