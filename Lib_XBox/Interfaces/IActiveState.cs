using Microsoft.Xna.Framework;

namespace XNALib
{
    public interface IActiveState
    {
        void Update(GameTime gameTime);
        void Draw();
    }
}
