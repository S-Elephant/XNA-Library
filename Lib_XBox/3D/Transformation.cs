using Microsoft.Xna.Framework;

namespace XNALib._3D
{
    public class Transformation
    {
        // Translate
        Vector3 translate;
        // Rotate around the (X, Y, Z) world axes
        Vector3 rotate;
        // Scale the X, Y, Z axes
        Vector3 scale;
        bool needUpdate;
        // Store the combination of the transformations
        Matrix matrix;
        public Vector3 Translate
        {
            get { return translate; }
            set { translate = value; needUpdate = true; }
        }
        public Vector3 Rotate
        {
            get { return rotate; }
            set { rotate = value; needUpdate = true; }
        }
        public Vector3 Scale
        {
            get { return scale; }
            set { scale = value; needUpdate = true; }
        }
        public Matrix Matrix
        {
            get
            {
                if (needUpdate)
                {
                    // Compute the final matrix (Scale * Rotate * Translate)
                    matrix = Matrix.CreateScale(scale) *
                    Matrix.CreateRotationY(MathHelper.ToRadians(rotate.Y)) *
                    Matrix.CreateRotationX(MathHelper.ToRadians(rotate.X)) *
                    Matrix.CreateRotationZ(MathHelper.ToRadians(rotate.Z)) *
                    Matrix.CreateTranslation(translate);
                    needUpdate = false;
                }
                return matrix;
            }
        }
    }
}
