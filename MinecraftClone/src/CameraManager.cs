using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftClone
{
    class Camera
    {
        public Vector3 position;

        public Vector3 forward;
        public Vector3 upward;
        public Vector3 right;

        private int fov;
        private float ratio;

        public float _pitch = 0;
        public float _yaw = 0;

        public Camera(float Ratio, int FOV)
        {
            position = Vector3.Zero;

            forward = -Vector3.UnitZ;
            upward = Vector3.UnitY;
            right = Vector3.UnitX;

            ratio = Ratio;
            fov = FOV;
        }

        public void ChangeFOV(float Ratio, int FOV)
        {
            ratio = Ratio;
            fov = FOV;
        }

        public Matrix4 GetViewMatrix()
        {
            UpdateVectors();
            return Matrix4.LookAt(position, position + forward, upward);
        }

        public Matrix4 GetProjectionMatrix()
        {
            UpdateVectors();
            return Matrix4.CreateScale(1, 1, -1) * Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fov), ratio, 0.1f, 100.0f);
        }

        private void UpdateVectors()
        {
            _pitch = Math.Max(-1.3f, Math.Min(_pitch, 1.3f));

            // First, the front matrix is calculated using some basic trigonometry.
            forward.X = (float)(Math.Cos(_pitch) * Math.Cos(_yaw));
            forward.Y = (float)(Math.Sin(_pitch));
            forward.Z = (float)(Math.Cos(_pitch) * Math.Sin(_yaw));

            // We need to make sure the vectors are all normalized, as otherwise we would get some funky results.
            forward = Vector3.Normalize(forward);

            // Calculate both the right and the up vector using cross product.
            // Note that we are calculating the right from the global up; this behaviour might
            // not be what you need for all cameras so keep this in mind if you do not want a FPS camera.
            right = Vector3.Normalize(Vector3.Cross(forward, Vector3.UnitY));
            upward = Vector3.Normalize(Vector3.Cross(right, forward));
        }
    }
}
