using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNALib
{
    public static class PopupMgr
    {
        public static List<Popup> Popups = new List<Popup>();
        public static SpriteFont DefaultTextFont = null;
        public static string DefaultBG = "popupBG";
        public static GraphicsDeviceManager GDM;

        public static bool HasPopups { get { return Popups.Count > 0; } }

        public static Popup CreatePopup(string text)
        {
            Popup newPopup = new Popup(GDM, DefaultBG, text, DefaultTextFont);
            Popups.Add(newPopup);
            return newPopup;
        }

        public static void Update()
        {
            Popups.ForEach(p => p.Update());
            Popups.RemoveAll(p => p.IsDisposed);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            Popups.ForEach(p => p.Draw(spriteBatch));
        }
    }
}
