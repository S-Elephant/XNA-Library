using Microsoft.Xna.Framework.Graphics;

namespace XNALib._3D
{
    public static class Common3D
    {
        public static void ResetGraphicsDeviceAfter2DDraw(GraphicsDevice gd)
        {
            gd.BlendState = BlendState.Opaque;
            gd.DepthStencilState = DepthStencilState.Default;
            gd.SamplerStates[0] = SamplerState.LinearWrap;
        }
        public static Effect str2Effect(string effect)
        {
            return Global.Content.Load<Effect>(Global.EffectFolder + effect);
        }
    }
}
