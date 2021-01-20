using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace XNALib
{
#if WINDOWS
    public static class KB
    {
        public static Keyboard1 KeyB = new Keyboard1();
        public static List<Keys> ReleasedKeys = new List<Keys>();
        public static void Update()
        {
            KeyB.Update();
            ReleasedKeys = KeyB.GetAllReleasedKeys();
        }
    }
#endif
}
