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
        // public values
        public Vector3 Position { get { return position; } set { position = value; UpdateVectors(); } }
        public float Yaw { get { return MathHelper.RadiansToDegrees(yaw); } set { yaw = MathHelper.DegreesToRadians(value); UpdateVectors(); } }
        public float Pitch { get { return MathHelper.RadiansToDegrees(pitch); } set { pitch = MathHelper.DegreesToRadians(MathHelper.Clamp(value, -89, 89)); UpdateVectors(); } }

        public Vector3 Forward { get { return forward; } }
        public Vector3 Upward { get { return upward; } }
        public Vector3 Right { get { return right; } }



        // private values
        private Vector3 position;
        private float yaw;
        private float pitch;

        private Vector3 forward;
        private Vector3 upward;
        private Vector3 right;

        private int fov;
        private float ratio;



        public Camera(float Ratio, int FOV)
        {
            position = Vector3.Zero;
            yaw = 0;
            pitch = 0;

            ChangeFOV(Ratio, FOV);
            UpdateVectors();
        }

        public Camera(float Ratio, int FOV, Vector3 SpawnPosition, float SpawnYaw, float SpawnPitch)
        {
            position = Vector3.Zero;
            yaw = MathHelper.DegreesToRadians(SpawnYaw);
            pitch = MathHelper.DegreesToRadians(SpawnPitch);

            UpdateVectors();

            ratio = Ratio;
            fov = FOV;
        }

        public void ChangeFOV(float Ratio, int FOV)
        {
            ratio = Ratio;
            fov = FOV;
        }

        public void Move(float MoveX, float MoveY, float MoveZ)
        {
            position += right * MoveX;
            position += upward * MoveY;
            position += forward * MoveZ;

            UpdateVectors();
        }

        public void Rotate(float RotateYaw, float RotatePitch)
        {
            Yaw -= RotateYaw;
            Pitch += RotatePitch;

            UpdateVectors();
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(position, position - forward, upward);
        }

        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreateScale(1, 1, -1) * Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fov), ratio, 0.1f, 1000.0f);
        }

        private void UpdateVectors()
        {
            // ctrl cv from https://github.com/opentk/LearnOpenTK/blob/3.x/Common/Camera.cs

            // First the front matrix is calculated using some basic trigonometry
            forward.X = (float)Math.Cos(pitch) * (float)Math.Cos(yaw);
            forward.Y = (float)Math.Sin(pitch);
            forward.Z = (float)Math.Cos(pitch) * (float)Math.Sin(yaw);

            // We need to make sure the vectors are all normalized, as otherwise we would get some funky results
            forward = Vector3.Normalize(forward);

            // Calculate both the right and the up vector using cross product
            // Note that we are calculating the right from the global up, this behaviour might
            // not be what you need for all cameras so keep this in mind if you do not want a FPS camera
            right = Vector3.Normalize(Vector3.Cross(forward, Vector3.UnitY));
            upward = Vector3.Normalize(Vector3.Cross(right, forward));

            right *= -1;
        }
    }
}
