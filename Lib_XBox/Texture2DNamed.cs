using Microsoft.Xna.Framework.Graphics;

namespace XNALib
{
    public class Texture2DNamed
    {
        public Texture2D Texture;
        public string Name;
        public Texture2DNamed(string texture)
        {
            Name = texture;
            Texture = Common.str2Tex(texture);
        }
    }
}
