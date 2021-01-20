using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNALib._3D
{
    /// <summary>
    /// Draws the X,Y,Z axes in a 3D world space.
    /// </summary>
    public class AxisDrawer
    {
        private int m_AxisLength = 100;
        public int AxisLength
        {
            get { return m_AxisLength; }
            set
            {
                m_AxisLength = value;
                SetAxes();
            }
        }

        private VertexPositionColor[] Lines;
        public GraphicsDevice Device;
        public Vector3 Location = Vector3.Zero;

        public AxisDrawer(GraphicsDevice device)
        {
            Device = device;
            SetAxes();
        }

        void SetAxes()
        {
            Lines = new VertexPositionColor[6];
            Lines[0] = new VertexPositionColor(new Vector3(-AxisLength, 0, 0), Color.Red); // X
            Lines[1] = new VertexPositionColor(new Vector3(AxisLength, 0, 0), Color.Red);
            Lines[2] = new VertexPositionColor(new Vector3(0, -AxisLength, 0), Color.Green); // Y
            Lines[3] = new VertexPositionColor(new Vector3(0, AxisLength, 0), Color.Green);
            Lines[4] = new VertexPositionColor(new Vector3(0, 0, -AxisLength), Color.Blue); // Z
            Lines[5] = new VertexPositionColor(new Vector3(0, 0, AxisLength), Color.Blue);
        }

        public void Draw(Camera3D camera)
        {
            BasicEffect effect = new BasicEffect(Device);
            effect.World = Matrix.CreateTranslation(Location);
            effect.View = camera.ViewMatrix;
            effect.Projection = camera.ProjectionMatrix;
            effect.VertexColorEnabled = true;

            for (int i = 0; i < effect.CurrentTechnique.Passes.Count; i++)
            {
                effect.CurrentTechnique.Passes[i].Apply();
                Device.DrawUserPrimitives(PrimitiveType.LineList, Lines, 0, 3);
            }
        }
    }
}