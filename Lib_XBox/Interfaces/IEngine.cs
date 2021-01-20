using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNALib
{
    public interface IEngine
    {
        IActiveState ActiveState { get; set; }
        int Width { get; }
        int Height { get; }
        Game Game { get; set; }
        GraphicsDeviceManager Graphics { get; set; }
        SpriteBatch SpriteBatch { get; set; }
        Rectangle SafeArea { get; }
    }
}
