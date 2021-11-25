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

        VoxelMesh VoxelMesh;
        Shader VoxelShader;

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



            MainMesh = new NormalMesh();
            VoxelMesh = new VoxelMesh();

            MainShader = new Shader("shaders/NormalShader.vert", "shaders/NormalShader.frag");
            VoxelShader = new Shader("shaders/VoxelShader.vert", "shaders/VoxelShader.frag");

            MainTexture = new Texture("textures/box.png");
            MainTexture.Use();

            MainCamera = new Camera((float)Width / (float)Height, 90);

            base.OnLoad(e);
        }

        float time = 0;

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            MainShader.Use();
            MainShader.SetMatrix4x4("projection", MainCamera.GetProjectionMatrix());
            MainShader.SetMatrix4x4("world", MainCamera.GetViewMatrix());

            MainShader.SetMatrix4x4("local", Matrix4.CreateTranslation(time, 0, 0) * Matrix4.CreateScale(0.7f, 0.7f, 0.7f));
            MainMesh.RenderMesh();

            MainShader.SetMatrix4x4("local", Matrix4.CreateTranslation(0, time, 0) * Matrix4.CreateScale(0.7f, 0.7f, 0.7f));
            MainMesh.RenderMesh();

            MainShader.SetMatrix4x4("local", Matrix4.CreateTranslation(0, 0, time) * Matrix4.CreateScale(0.7f, 0.7f, 0.7f));
            MainMesh.RenderMesh();

            VoxelShader.Use();
            VoxelShader.SetMatrix4x4("projection", MainCamera.GetProjectionMatrix());
            VoxelShader.SetMatrix4x4("world", MainCamera.GetViewMatrix());

            VoxelShader.SetMatrix4x4("local", Matrix4.CreateScale(1, 1, 1));
            VoxelMesh.RenderMesh();

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
            VoxelShader.Dispose();
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
                MainCamera.Move(0, 0, (float)e.Time * speed);
            }

            if (input.IsKeyDown(Key.S))
            {
                MainCamera.Move(0, 0, (float)e.Time * -speed);
            }

            if (input.IsKeyDown(Key.D))
            {
                MainCamera.Move((float)e.Time * speed, 0, 0);
            }

            if (input.IsKeyDown(Key.A))
            {
                MainCamera.Move((float)e.Time * -speed, 0, 0);
            }

            if (input.IsKeyDown(Key.Space))
            {
                MainCamera.Move(0, (float)e.Time * speed, 0);
            }

            if (input.IsKeyDown(Key.LShift))
            {
                MainCamera.Move(0, (float)e.Time * -speed, 0);
            }

            if (input.IsKeyDown(Key.Right))
            {
                MainCamera.Rotate((float)e.Time * 90, 0);
            }

            if (input.IsKeyDown(Key.Left))
            {
                MainCamera.Rotate((float)e.Time * -90, 0);
            }

            if (input.IsKeyDown(Key.Up))
            {
                MainCamera.Rotate(0, (float)e.Time * 90);
            }

            if (input.IsKeyDown(Key.Down))
            {
                MainCamera.Rotate(0, (float)e.Time * -90);
            }

            Console.WriteLine(Math.Round(1 / e.Time));
        }
    }
}