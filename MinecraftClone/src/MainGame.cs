using Noise;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using System;

namespace MinecraftClone
{
    public class MainGame : GameWindow
    {
        NormalMesh MainMesh;
        Shader MainShader;
        Texture MainTexture;
        Camera MainCamera;

        NormalMesh[,,] GeneratedMesh;
        public static Terrain NoiseTerrain;

        public MainGame(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
            Console.WriteLine("Window created: ({0}, {1})", width, height);
        }

        #region Rendering

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.FrontFace(FrontFaceDirection.Cw);

            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Point);
            //GL.PointSize(10);

            NoiseTerrain = new Terrain(0);

            GeneratedMesh = new NormalMesh[5, 3, 5];
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int z = 0; z < 5; z++)
                    {
                        GeneratedMesh[x, y, z] = new NormalMesh(new ChunkData(new Vector3(x, y, z)));
                    }
                }
            }


            MainMesh = new NormalMesh();

            MainShader = new Shader("shaders/NormalShader.vert", "shaders/NormalShader.frag");
            MainShader.Use();

            MainTexture = new Texture("textures/box.png");
            MainTexture.Use();

            MainCamera = new Camera((float)Width / (float)Height, 90);

            base.OnLoad(e);
        }

        float time = 0;

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            MainShader.SetMatrix4x4("projection", MainCamera.GetProjectionMatrix());
            MainShader.SetMatrix4x4("world", MainCamera.GetViewMatrix());
            MainTexture = new Texture("textures/box.png");
            MainTexture.Use();

            MainShader.SetMatrix4x4("local", Matrix4.CreateTranslation(time, 0, 0) * Matrix4.CreateScale(0.7f, 0.7f, 0.7f));
            //MainShader.SetVector3("color", new Vector3(1, 0, 0));
            MainMesh.RenderMesh();

            MainShader.SetMatrix4x4("local", Matrix4.CreateTranslation(0, time, 0) * Matrix4.CreateScale(0.7f, 0.7f, 0.7f));
            //MainShader.SetVector3("color", new Vector3(0, 1, 0));
            MainMesh.RenderMesh();

            MainShader.SetMatrix4x4("local", Matrix4.CreateTranslation(0, 0, time) * Matrix4.CreateScale(0.7f, 0.7f, 0.7f));
            //MainShader.SetVector3("color", new Vector3(0, 0, 1));
            MainMesh.RenderMesh();

            /*MainShader.SetMatrix4x4("local", Matrix4.CreateRotationX(-time) * Matrix4.CreateScale(0.3f, 0.3f, 0.3f) * Matrix4.CreateTranslation(1, 0, 0));
            MainMesh.RenderMesh();

            MainShader.SetMatrix4x4("local", Matrix4.CreateRotationX(time) * Matrix4.CreateScale(0.3f, 0.3f, 0.3f) * Matrix4.CreateTranslation(-1, 0, 0));
            MainMesh.RenderMesh();

            MainShader.SetMatrix4x4("local", Matrix4.CreateRotationY(-time) * Matrix4.CreateScale(0.3f, 0.3f, 0.3f) * Matrix4.CreateTranslation(0, 1, 0));
            MainMesh.RenderMesh();

            MainShader.SetMatrix4x4("local", Matrix4.CreateRotationY(time) * Matrix4.CreateScale(0.3f, 0.3f, 0.3f) * Matrix4.CreateTranslation(0, -1, 0));
            MainMesh.RenderMesh();

            MainShader.SetMatrix4x4("local", Matrix4.CreateRotationZ(-time) * Matrix4.CreateScale(0.3f, 0.3f, 0.3f) * Matrix4.CreateTranslation(0, 0, 1));
            MainMesh.RenderMesh();

            MainShader.SetMatrix4x4("local", Matrix4.CreateRotationZ(time) * Matrix4.CreateScale(0.3f, 0.3f, 0.3f) * Matrix4.CreateTranslation(0, 0, -1));
            MainMesh.RenderMesh();*/

            MainTexture = new Texture("textures/Minecraft/grass.png");
            MainShader.SetVector3("color", new Vector3(1, 1, 1));
            MainTexture.Use();

            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int z = 0; z < 5; z++)
                    {
                        MainShader.SetMatrix4x4("local", Matrix4.CreateTranslation(x * 32, y * 32, z * 32));
                        GeneratedMesh[x, y, z].RenderMesh();
                    }
                }
            }

            Context.SwapBuffers();
            base.OnRenderFrame(e);
            time += (float)e.Time;
        }

        protected override void OnUnload(EventArgs e)
        {
            /*
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.BindVertexArray(0);
                GL.UseProgram(0);
                
                GL.DeleteBuffer(VertexBufferObject);
                GL.DeleteVertexArray(VertexArrayObject);
            */

            MainShader.Dispose();
            base.OnUnload(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            MainCamera.ChangeFOV((float)Width / (float)Height, 90);
            base.OnResize(e);
        }

        #endregion

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();
            if (input.IsKeyDown(Key.Escape)) { Exit(); }
            base.OnUpdateFrame(e);

            float speed = 5;

            if (input.IsKeyDown(Key.W))
            {
                MainCamera.position -= MainCamera.forward * (float)e.Time * speed;
            }

            if (input.IsKeyDown(Key.S))
            {
                MainCamera.position -= -MainCamera.forward * (float)e.Time * speed;
            }

            if (input.IsKeyDown(Key.D))
            {
                MainCamera.position += MainCamera.right * (float)e.Time * speed;
            }

            if (input.IsKeyDown(Key.A))
            {
                MainCamera.position += -MainCamera.right * (float)e.Time * speed;
            }

            if (input.IsKeyDown(Key.Space))
            {
                MainCamera.position += MainCamera.upward * (float)e.Time * speed;
            }

            if (input.IsKeyDown(Key.LShift))
            {
                MainCamera.position += -MainCamera.upward * (float)e.Time * speed;
            }

            /*if (input.IsKeyDown(Key.Right))
            {
                MainCamera.Rotation *= Quaternion.FromEulerAngles(0, 1 * (float)e.Time, 0);
            }

            if (input.IsKeyDown(Key.Left))
            {
                MainCamera.Rotation *= Quaternion.FromEulerAngles(0, -1 * (float)e.Time, 0);
            }

            if (input.IsKeyDown(Key.Up))
            {
                MainCamera.Rotation *= Quaternion.FromEulerAngles(1 * (float)e.Time, 0, 0);
            }

            if (input.IsKeyDown(Key.Down))
            {
                MainCamera.Rotation *= Quaternion.FromEulerAngles(-1 * (float)e.Time, 0, 0);
            }*/

            var mouse = Mouse.GetState();

            // Calculate the offset of the mouse position
            var deltaX = mouse.X - _lastPos.X;
            var deltaY = mouse.Y - _lastPos.Y;
            _lastPos = new Vector2(mouse.X, mouse.Y);

            // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
            MainCamera._yaw -= deltaX * (float)e.Time * 0.1f;
            MainCamera._pitch += deltaY * (float)e.Time * 0.1f; // reversed since y-coordinates range from bottom to top
        }

        private Vector2 _lastPos;

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (Focused) // check to see if the window is focused
            {
                Mouse.SetPosition(X + Width / 2f, Y + Height / 2f);
            }

            base.OnMouseMove(e);
        }
    }
}