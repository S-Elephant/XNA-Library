using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;
using System.Diagnostics;

namespace TestProject
{
    [TestClass]
    public class CollisionTest
    {
        [TestMethod]
        public void Collision_CenterRects()
        {
            Rectangle A = new Rectangle(0, 0, 5, 5); // center = 3
            Rectangle B = new Rectangle(1000, -500, 10, 10);
            Debug.Assert(Collision.CenterRects(A, B) == new Rectangle(-3, -3, 10, 10), "1"); // -3 and not -2.5 because rectangle only uses ints...
        }
    }
}
