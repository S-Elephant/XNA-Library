using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNALib._3D
{
    public class Camera3D
    {
        #region Members
        public bool KeepLookingAtPoint = true;

        private Vector3 m_location;
        public Vector3 Location
        {
            get { return this.m_location; }
            set { this.m_location = value; }
        }

        private Vector3 m_LookAt = Vector3.Zero;
        /// <summary>
        /// The point the camera must keep looking at
        /// </summary>
        public Vector3 LookAt
        {
            get { return m_LookAt; }
            set { m_LookAt = value; }
        }

        private Matrix m_ViewMatrix;
        public Matrix ViewMatrix
        {
            get { return m_ViewMatrix; }
            private set { m_ViewMatrix = value; }
        }

        private Matrix m_ProjectionMatrix;
        public Matrix ProjectionMatrix
        {
            get { return m_ProjectionMatrix; }
            private set { m_ProjectionMatrix = value; }
        }

        private float m_AspectRatio;
        public float AspectRatio
        {
            get { return m_AspectRatio; }
            set
            {
                m_AspectRatio = value;
                CreateProjection();
            }
        }

        private float m_NearPlaneDistance = 1f;
        public float NearPlaneDistance
        {
            get { return m_NearPlaneDistance; }
            set
            {
                m_NearPlaneDistance = value;
                CreateProjection();
            }
        }

        private float m_FarPlaneDistance = 10000f;
        public float FarPlaneDistance
        {
            get { return m_FarPlaneDistance; }
            set
            {
                m_FarPlaneDistance = value;
                CreateProjection();
            }
        }

        private Matrix Rotation = Matrix.Identity;
        [Obsolete("Doesn't work properly!")]
        public bool MouseControlIsEnabled = false;
        private MouseState prevMouseState;
        #endregion

        public Camera3D(Vector3 location, Viewport viewport)
        {
            Location = location;
            this.AspectRatio = ((float)viewport.Width) / ((float)viewport.Height);
            CreateProjection();
        }

        private void CreateProjection()
        {
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                            MathHelper.ToRadians(40.0f),
                            this.AspectRatio,
                            NearPlaneDistance,
                            FarPlaneDistance);
        }

        public void LookAtOnce(Vector3 target)
        {
            ViewMatrix = Matrix.CreateLookAt(Location, target, Vector3.Up) * Rotation;
        }

        public void Update()
        {
            if (MouseControlIsEnabled)
            {
                MouseState currentMouseState = Mouse.GetState();

                float y = prevMouseState.Y - currentMouseState.Y;
                float x = prevMouseState.X - currentMouseState.X;

                Rotation *= Matrix.CreateRotationX(MathHelper.ToRadians(-y)); // Up and down
                Rotation *= Matrix.CreateRotationY(MathHelper.ToRadians(-x));

                prevMouseState = Mouse.GetState();
            }

            if (KeepLookingAtPoint)
            {
                // Update the camera's position
                ViewMatrix = Matrix.CreateLookAt(Location, LookAt, Vector3.Up) * Rotation;
            }
        }
    }
}