using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNALib._3D
{
    public class BasicModel
    {
        public Model Model;
        private Matrix BaseWorld;
        public Matrix Rotation = Matrix.Identity;
        
        public Matrix World
        {
            get { return BaseWorld * Rotation; }
        }


        public BasicModel(Vector3 location, string model, Matrix scaleMatrix)
        {
            BaseWorld = scaleMatrix * Matrix.CreateTranslation(location);
            Model = Common.str2Model(model);
        }

        public BasicModel(Vector3 location, Model model, Matrix scaleMatrix)
        {
            BaseWorld = scaleMatrix * Matrix.CreateTranslation(location);
            Model = model;
        }

        public bool CollidesWith(Model otherModel, Matrix otherWorld)
        {
            // Loop through each ModelMesh in both objects and compare
            // all bounding spheres for collisions
            foreach (ModelMesh myModelMeshes in Model.Meshes)
            {
                foreach (ModelMesh hisModelMeshes in otherModel.Meshes)
                {
                    if (myModelMeshes.BoundingSphere.Transform(World).Intersects(hisModelMeshes.BoundingSphere.Transform(otherWorld)))
                        return true;
                }
            }
            return false;
        }

        public virtual void Update() { }

        public void Draw(Camera3D camera)
        {
            Draw(camera.ProjectionMatrix, camera.ViewMatrix);
        }

        public void Draw(Matrix projection, Matrix view)
        {
            Matrix[] transforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.EnableDefaultLighting();
                    be.PreferPerPixelLighting = true;
                    
                    be.Projection = projection;
                    be.View = view;
                    be.World = World * mesh.ParentBone.Transform;
                }
                mesh.Draw();
            }            
        }
    }
}
