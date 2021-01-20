using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNALib
{
    public class Keyboard1
    {
        private KeyboardState m_PrevKeyboardState;
        public KeyboardState PrevKeyboardState
        {
            get { return m_PrevKeyboardState; }
            set { m_PrevKeyboardState = value; }
        }
        private KeyboardState m_CurrentKeyboardState;
        public KeyboardState CurrentKeyboardState
        {
            get { return m_CurrentKeyboardState; }
            set { m_CurrentKeyboardState = value; }
        }

        /// <summary>
        /// When this function doesn't work then the Keyboard is probably updated twice or more in the same cycle.
        /// </summary>
        public bool AnyKeyIsReleased { get { return (GetAllReleasedKeys().Count > 0); } }

        public Keyboard1()
        {
            PrevKeyboardState = CurrentKeyboardState = Keyboard.GetState();
        }

        public void Update()
        {
            PrevKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
        }

        public static bool IsCharacter(Keys key)
        {
            int asciiVal = (int)key;
            return (asciiVal >= (int)Keys.A) && asciiVal <= (int)Keys.Z;
        }
        
        public Keys GetDownedKey(List<Keys> acceptOnlyTheseKeys) // returns first key found. only applies if the key is still down in this frame/cycle
        {
            List<Keys> keysdown = new List<Keys>(CurrentKeyboardState.GetPressedKeys());
            keysdown.Remove(Keys.None);
            
            foreach (Keys key in acceptOnlyTheseKeys)
                if (keysdown.Contains(key))
                    return key;
            
            return Keys.None;
        }

        public List<Keys> GetAllReleasedKeys() // downed in prev state and now released
        {
            List<Keys> prevKeysPressed = new List<Keys>(PrevKeyboardState.GetPressedKeys());
            List<Keys> currentKeysPressed = new List<Keys>(CurrentKeyboardState.GetPressedKeys());
            
            prevKeysPressed.Remove(Keys.None);
            currentKeysPressed.Remove(Keys.None);

            List<Keys> result = new List<Keys>();
            foreach (Keys key in prevKeysPressed)
            {
                if (!currentKeysPressed.Contains(key)) // dus als de oude list een key heeft die de nieuwe niet heeft dan is die dus released
                    result.Add(key);
            }
            return result;
        }

        public Keys GetReleasedKey(List<Keys> acceptOnlyTheseKeys) // returns first key found
        {
            List<Keys> releasedKeys = GetAllReleasedKeys();
            
            foreach (Keys key in acceptOnlyTheseKeys)
                if (releasedKeys.Contains(key))
                    return key;

            return Keys.None;
        }

        public bool KeyIsReleased(Keys key)
        {
            List<Keys> releasedKeys = GetAllReleasedKeys();
            return releasedKeys.Contains(key);
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

        public List<Keys> GetAllReleasedKeys(List<Keys> acceptOnlyTheseKeys) // returns all keys found
        {
            List<Keys> releasedKeys = GetAllReleasedKeys();
            List<Keys> result = new List<Keys>();
            
            foreach (Keys key in acceptOnlyTheseKeys)
                if (releasedKeys.Contains(key))
                    result.Add(key);
           
            return result;
        }
    }
}
