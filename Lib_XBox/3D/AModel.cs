using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace XNALib._3D
{
    public class AModel
    {

        private Vector3 m_Location;
        public Vector3 Location
        {
            get { return m_Location; }
            set
            {
                m_Location = value;
                BoundingBox = GetBoundingBox(GetWorldTransform());
            }
        }

        public Model Model;

        public float AspectRatio;
        // Set the position of the model in world space, and set the rotation.
        public Vector3 Rotation = Vector3.Zero;
        public float Speed = 0f;
        public Matrix Scale = Matrix.Identity;

        private BoundingBox m_BoundingBox;
        public BoundingBox BoundingBox
        {
            get { return m_BoundingBox; }
            private set { m_BoundingBox = value; }
        }

        public AModel(Vector3 location, Model model)
        {
            m_Location = location;
            Model = model;
            BoundingBox = GetBoundingBox(GetWorldTransform());
        }

        public AModel(Vector3 location, string model)
        {
            m_Location = location;
            Model = Common.str2Model(model);
            BoundingBox = GetBoundingBox(GetWorldTransform());
        }

        public Matrix GetWorldTransform()
        {
            return Matrix.CreateRotationX(Rotation.X)
                        * Matrix.CreateRotationY(Rotation.Y)
                        * Matrix.CreateRotationZ(Rotation.Z)
                        * Matrix.CreateTranslation(Location)
                        * Scale;
        }

        public BoundingBox GetBoundingBox(Matrix worldTransform)
        {
            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // For each mesh of the model
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), worldTransform);

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }

            // Create and return bounding box
            return new BoundingBox(min, max);
        }

        public void DrawTerrain(Camera3D camera3D)
        {
            Matrix[] boneTransforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = boneTransforms[mesh.ParentBone.Index] * Matrix.Identity;
                    effect.View = camera3D.ViewMatrix;
                    effect.Projection = camera3D.ProjectionMatrix;

                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    // Set the fog to match the black background color
                    effect.FogEnabled = true;
                    effect.FogColor = Vector3.Zero;
                    effect.FogStart = 1000;
                    effect.FogEnd = 3200;
                }

                mesh.Draw();
            }
        }

        public void Update()
        {
            BoundingBox = GetBoundingBox(GetWorldTransform());
        }

        public void Draw(Camera3D camera3D)
        {
            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in Model.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = GetWorldTransform();
                    effect.View = camera3D.ViewMatrix;
                    effect.Projection = camera3D.ProjectionMatrix;// Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), AspectRatio, 1.0f, 10000.0f);
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }

        /*
        Effect spot = Common3D.str2Effect("SpotLight");
        public void DrawWithDiffuse(Camera3D camera3D)
        {

            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(transforms);

            spot.Parameters["activeLights"].SetValue(activeSpotLights);
            spot.Parameters["lightDecayExponent"].SetValue(LightDecayExponent.ToArray());
            spot.Parameters["lightAngleCosine"].SetValue(LightAngleCosine.ToArray());
            spot.Parameters["lightDirection"].SetValue(LightDirection.ToArray());
            spot.Parameters["LightPosition"].SetValue(LightPosition.ToArray());
            spot.Parameters["LightColor"].SetValue(LightColor.ToArray());
            spot.Parameters["lightRadius"].SetValue(LightRadius.ToArray());
            spot.Parameters["SpecularColor"].SetValue(SpecularColor.ToVector4());
            spot.Parameters["SpecularPower"].SetValue(SpecularPower);
            spot.Parameters["SpecularIntensity"].SetValue(SpecularIntensity);
            spot.Parameters["AmbientColor"].SetValue(AmbientColor.ToVector4());
            spot.Parameters["AmbientIntensity"].SetValue(AmbientIntensity);
            spot.Parameters["DiffuseIntensity"].SetValue(DiffuseIntensity);   

            //effect2.EnableDefaultLighting();
            /*
            if (effect2.Parameters["world"] != null)
                effect2.Parameters["world"].SetValue(Matrix.CreateTranslation(Location));
            if (effect2.Parameters["wvp"] != null)
                effect2.Parameters["wvp"].SetValue(Matrix.CreateTranslation(Location) * camera3D.ViewMatrix * camera3D.ProjectionMatrix);
            if (effect2.Parameters["itw"] != null)
                effect2.Parameters["itw"].SetValue(Matrix.Invert(Matrix.Transpose(Matrix.CreateTranslation(Location))));

            if (effect2.Parameters["LightDirection"] != null)
                // Calculate the light direction in relation to me.
                effect2.Parameters["LightDirection"].SetValue(LightPosition- Location);

            if (effect2.Parameters["AmbientColor"] != null)
                effect2.Parameters["AmbientColor"].SetValue(AmbientColor.ToVector4());
            if (effect2.Parameters["AmbientIntensity"] != null)
                effect2.Parameters["AmbientIntensity"].SetValue(AmbientIntensity);
            */

        /*
            for (int pass = 0; pass < effect2.CurrentTechnique.Passes.Count; pass++)
            {
                for (int msh = 0; msh < Model.Meshes.Count; msh++)
                {
                    ModelMesh mesh = Model.Meshes[msh];
                    for (int prt = 0; prt < mesh.MeshParts.Count; prt++)
                        mesh.MeshParts[prt].Effect = effect2;
                    mesh.Draw();
                }
            }
        }*/
    }
}