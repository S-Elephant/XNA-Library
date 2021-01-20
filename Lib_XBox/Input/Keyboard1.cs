using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace XNALib
{
    public class Keyboard1
    {
        public KeyboardState OldState;
        public KeyboardState State;

        /// <remarks>
        /// When this function doesn't work then the Keyboard is probably updated twice or more in the same cycle.
        /// </remarks>
        public bool AnyKeyIsPressed { get { return (GetAllReleasedKeys().Count > 0); } }

        private Dictionary<Keys, int> m_KeyDownTimes = new Dictionary<Keys, int>();
        public Dictionary<Keys, int> KeyDownTimes
        {
            get { return m_KeyDownTimes; }
            private set { m_KeyDownTimes = value; }
        }

        private const int KeysCount = 255; // Keys enum has a range from 0-254
        private List<Keys> AllKeys = new List<Keys>();

        public bool UpdateKeyDownTimes = true;

        public Keyboard1()
        {
            OldState = State = Keyboard.GetState();

            // Key down times
            for (int i = 0; i < KeysCount; i++)
            {
                Keys key = (Keys)i;
                AllKeys.Add(key);
                KeyDownTimes.Add(key, 0);
            }
        }

        public void Update(GameTime gameTime)
        {
            OldState = State;
            State = Keyboard.GetState();

            // Key down times
            if (UpdateKeyDownTimes)
            {
                foreach (Keys key in AllKeys)
                {
                    if (State.IsKeyUp(key))
                        KeyDownTimes[key] = 0;
                    else
                        KeyDownTimes[key] += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
        }

        public bool IsPressed(Keys key)
        {
            return OldState.IsKeyDown(key) && State.IsKeyUp(key);
        }

        public bool IsFirstDown(Keys key)
        {
            return OldState.IsKeyUp(key) && State.IsKeyDown(key);
        }

        public string GetCharacterKey()
        {
            List<Keys> releasedKeys = GetAllReleasedKeys();

            foreach (Keys key in releasedKeys)
            {
                if (IsCharacter(key))
                    return key.ToString();
            }
            return null;
        }

        #region Advanced
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true if the key is within the A-Z range</returns>
        public static bool IsCharacter(Keys key)
        {
            int asciiVal = (int)key;
            return (asciiVal >= (int)Keys.A) && asciiVal <= (int)Keys.Z;
        }

        /// <summary>
        /// Returns the first one found. Only works for pressed keys, not downed keys.
        /// </summary>
        /// <returns>-1 if none was released</returns>
        public int GetNumericKey()
        {
            List<Keys> releasedKeys = GetAllReleasedKeys();

            foreach (Keys key in releasedKeys)
            {
                if (key >= Keys.D0 && key <= Keys.D9)
                    return (int)key - 48;
            }
            return -1;
        }
        
        /// <summary>
        /// returns first key found from the given list. only applies if the key is still down in this frame/cycle.
        /// </summary>
        /// <param name="acceptOnlyTheseKeys"></param>
        /// <returns></returns>
        public Keys GetDownedKey(List<Keys> acceptOnlyTheseKeys)
        {
            List<Keys> keysdown = new List<Keys>(State.GetPressedKeys());
            keysdown.Remove(Keys.None);
            
            foreach (Keys key in acceptOnlyTheseKeys)
                if (keysdown.Contains(key))
                    return key;
            
            return Keys.None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acceptOnlyTheseKeys"></param>
        /// <returns>First key found</returns>
        public Keys GetReleasedKey(List<Keys> acceptOnlyTheseKeys)
        {
            List<Keys> releasedKeys = GetAllReleasedKeys();

            foreach (Keys key in acceptOnlyTheseKeys)
                if (releasedKeys.Contains(key))
                    return key;

            return Keys.None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acceptOnlyTheseKeys"></param>
        /// <returns>All keys found</returns>
        public List<Keys> GetAllReleasedKeys(List<Keys> acceptOnlyTheseKeys)
        {
            List<Keys> releasedKeys = GetAllReleasedKeys();
            List<Keys> result = new List<Keys>();

            foreach (Keys key in acceptOnlyTheseKeys)
                if (releasedKeys.Contains(key))
                    result.Add(key);

            return result;
        }

        public List<Keys> GetAllReleasedKeys()
        {
            List<Keys> prevKeysPressed = new List<Keys>(OldState.GetPressedKeys());
            List<Keys> currentKeysPressed = new List<Keys>(State.GetPressedKeys());

            prevKeysPressed.Remove(Keys.None);
            currentKeysPressed.Remove(Keys.None);

            List<Keys> result = new List<Keys>();
            foreach (Keys key in prevKeysPressed)
            {
                if (!currentKeysPressed.Contains(key))
                    result.Add(key);
            }
            return result;
        }
        #endregion
    }
}
