using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNALib._3D
{
    public class RotatingModel : BasicModel
    {
        public float RotationSpeed = MathHelper.Pi / 180;

        public RotatingModel(Vector3 location, Model m)
            : base(location, m, Matrix.Identity)
        {
        }
        public RotatingModel(Vector3 location, string m)
            : base(location, m, Matrix.Identity)
        {
        }
        public override void Update()
        {
            Rotation *= Matrix.CreateRotationY(RotationSpeed);
        }
    }
}