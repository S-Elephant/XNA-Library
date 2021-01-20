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
    public class FRectTest
    {
        [TestMethod]
        public void FRect_Creation()
        {
            FRect test = new FRect(10, 11, 12, 13);

            Debug.Assert(test.X == 10 && test.Left == 10);
            Debug.Assert(test.Y == 11 && test.Top == 11);
            Debug.Assert(test.Width == 12);
            Debug.Assert(test.Height == 13);
            Debug.Assert(test.Right == 10 + 12);
            Debug.Assert(test.Bottom == 11 + 13);

            Debug.Assert(test.TopLeft == new Vector2(test.X, test.Y));
            Debug.Assert(test.TopRight == new Vector2(test.X + test.Width, test.Y));
            Debug.Assert(test.BottomRight == new Vector2(test.X + test.Width, test.Y + test.Height));
            Debug.Assert(test.BottomLeft == new Vector2(test.X, test.Y + test.Height));
        }

        [TestMethod]
        public void FRect_OperatorOverloads()
        {
            FRect A = new FRect(32, 32, 64, 64);
            FRect B = new FRect(32, 32, 64, 64);
            FRect C = new FRect(32, 32, 65, 64);
            FRect D = new FRect(32, -32, 64, 64);
            FRect E = new FRect(32, 32, 16, 16);
            FRect T;

            Debug.Assert(A == B);
            Debug.Assert(A != C);
            Debug.Assert(A != D);

            T = A+E;
            Debug.Assert(T.X == 64 && T.Y == 64 && T.Width == 64 && T.Height == 64);
            T = A - E;
            Debug.Assert(T.X == 0 && T.Y == 0 && T.Width == 64 && T.Height == 64);
        }

        [TestMethod]
        public void FRect_Touch()
        {
            FRect A = new FRect(0, 0, 32, 32);
            FRect B = new FRect(4, 4, 24, 24); // inside A
            FRect C = new FRect(-4, -4, 36, 36); // fully overlaps A
            FRect D = new FRect(32, 0, 32, 32);
            FRect E = new FRect(0, -32, 32, 32);
            FRect F = new FRect(0, -33, 32, 32);
            FRect G = new FRect(0, 500, 32, 32);

            Debug.Assert(!A.AreOnlyTouching(B, 0));
            Debug.Assert(!A.AreOnlyTouching(C, 0));
            Debug.Assert(A.AreOnlyTouching(D, 0));
            Debug.Assert(A.AreOnlyTouching(E, 0));
            Debug.Assert(A.AreOnlyTouching(F, 1));
            Debug.Assert(!A.AreOnlyTouching(F, 0));
            Debug.Assert(!A.AreOnlyTouching(G, 0));
        }

        [TestMethod]
        public void FRect_FullyEncloses()
        {
            FRect A = new FRect(0, 0, 10, 10);
            FRect B = new FRect(3, 3, 3, 3);
            FRect C = new FRect(1, 1, 9, 9); // touching bottom and right
            FRect D = new FRect(3, 3, 11, 11);
            FRect E = new FRect(-1, -1, 10, 10);

            Debug.Assert(!A.FullyEncloses(A));
            Debug.Assert(A.FullyEncloses(B));
            Debug.Assert(!A.FullyEncloses(C));
            Debug.Assert(!A.FullyEncloses(D));
            Debug.Assert(!A.FullyEncloses(E));
        }

        [TestMethod]
        public void FRect_Contains()
        {
            FRect A = new FRect(10, 10, 10, 10);
            Vector2 B = new Vector2(10, 10);
            Vector2 C = new Vector2(-10, -10);
            Vector2 D = new Vector2(12, 13);
            Vector2 E = new Vector2(20, 20);
            Vector2 F = new Vector2(15, 25);

            Debug.Assert(A.Contains(B));
            Debug.Assert(!A.Contains(C));
            Debug.Assert(A.Contains(D));
            Debug.Assert(A.Contains(E));
            Debug.Assert(!A.Contains(F));
        }

        [TestMethod]
        public void FRect_Intersects()
        {
            FRect A = new FRect(0,0,5,5);
            FRect B = new FRect(3,3,5,5);
            FRect C = new FRect(1,1,1,1);
            FRect D = new FRect(5,5,5,5);
            FRect E = new FRect(6,5,5,5);
            FRect F = new FRect(5,6,5,5);
            FRect G = new FRect(-1,-1,7,7);

            Debug.Assert(A.Intersects(A),"1");
            Debug.Assert(A.Intersects(B), "2");
            Debug.Assert(A.Intersects(C), "3");
            Debug.Assert(!A.Intersects(D), "4");
            Debug.Assert(!A.Intersects(E), "5");
            Debug.Assert(!A.Intersects(F), "6");
            Debug.Assert(A.Intersects(G), "7");

            Debug.Assert(A.IntersectsOrTouches(A), "a");
            Debug.Assert(A.IntersectsOrTouches(B), "b");
            Debug.Assert(A.IntersectsOrTouches(C), "c");
            Debug.Assert(A.IntersectsOrTouches(D), "d");
            Debug.Assert(!A.IntersectsOrTouches(E), "e");
            Debug.Assert(!A.IntersectsOrTouches(F), "f");
            Debug.Assert(A.IntersectsOrTouches(G), "g");
        }


        [TestMethod]
        public void FRect_CollidedSidesNoTouch()
        {
            FRect A = new FRect(0, 0, 5, 5);
            FRect B = new FRect(-4, 0, 5, 5);
            FRect C = new FRect(-2, -2, 6, 8);
            FRect D = new FRect(-20, -20, 1, 1);
            FRect E = new FRect(1, 1, 3, 3);

            Collision.RectSide side = Collision.CollidedSidesNoTouch(A,A);
            Debug.Assert(side.IsSet(Collision.RectSide.Equal), "1");

            side = Collision.CollidedSidesNoTouch(B, A);
            Debug.Assert(side.IsSet(Collision.RectSide.Left), "2");

            side = Collision.CollidedSidesNoTouch(C, A);
            Debug.Assert(side.IsSet(Collision.RectSide.Left) &&
                         side.IsSet(Collision.RectSide.Top) &&
                         side.IsSet(Collision.RectSide.Bottom),
                         "3");

            side = Collision.CollidedSidesNoTouch(D, A);
            Debug.Assert(side.IsSet(Collision.RectSide.None), "4");

            side = Collision.CollidedSidesNoTouch(E, A);
            Debug.Assert(side.IsSet(Collision.RectSide.Inside), "5");
        }

        [TestMethod]
        public void FRect_CollidedSidesWithTouch()
        {
            FRect A = new FRect(0, 0, 5, 5);
            FRect B = new FRect(-4, 0, 5, 5);
            FRect C = new FRect(-2, -2, 6, 8);
            FRect D = new FRect(-20, -20, 1, 1);
            FRect E = new FRect(1, 1, 3, 3);
            FRect F = new FRect(-1, 0, 5, 5);

            Collision.RectSide side = Collision.CollidedSidesWithTouch(A, A);
            Debug.Assert(side.IsSet(Collision.RectSide.Equal), "1");

            side = Collision.CollidedSidesWithTouch(B, A);
            Debug.Assert(side.IsSet(Collision.RectSide.Left) &&
                         side.IsSet(Collision.RectSide.Top) &&
                         side.IsSet(Collision.RectSide.Bottom),
                         "2");

            side = Collision.CollidedSidesWithTouch(C, A);
            Debug.Assert(side.IsSet(Collision.RectSide.Left) &&
                         side.IsSet(Collision.RectSide.Top) &&
                         side.IsSet(Collision.RectSide.Bottom),
                         "3");

            side = Collision.CollidedSidesWithTouch(D, A);
            Debug.Assert(side.IsSet(Collision.RectSide.None), "4");

            side = Collision.CollidedSidesWithTouch(F, A);
            Debug.Assert(side.IsSet(Collision.RectSide.Left) &&
                         side.IsSet(Collision.RectSide.Top) &&
                         side.IsSet(Collision.RectSide.Bottom),
                         "5");
        }

        [TestMethod]
        public void FRect_Misc()
        {
            FRect A = new FRect(1, 1, -1, 1);
            FRect B = new FRect(1, 1, 1, -1);
            FRect C = new FRect(1, 1, -1, -1);

            Debug.Assert(!A.IsValid);
            Debug.Assert(!B.IsValid);
            Debug.Assert(!C.IsValid);
        }
    }
}
