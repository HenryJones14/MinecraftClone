using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using System;

namespace MinecraftClone
{
    public class MainGame : GameWindow
    {
        Shader MainShader;
        Mesh MainMesh;

        public MainGame(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
            Console.WriteLine("Window created: ({0}, {1})", width, height);
        }

        #region Rendering

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            MainMesh = new Mesh();
            MainShader = new Shader("shaders/shader.vert", "shaders/shader.frag");

            base.OnLoad(e);
        }

        float time = 0;

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            MainShader.Use();
            MainShader.SetMatrix4x4("transform", Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(time)));
            MainShader.SetMatrix4x4("view", Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f));
            MainShader.SetMatrix4x4("projection", Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), Width / Height, 0.1f, 100.0f));
            MainMesh.RenderMesh();

            Context.SwapBuffers();
            base.OnRenderFrame(e);
            time++;
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
            base.OnResize(e);
        }

        #endregion

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();
            if (input.IsKeyDown(Key.Escape)) { Exit(); }
            base.OnUpdateFrame(e);
        }
    }
}