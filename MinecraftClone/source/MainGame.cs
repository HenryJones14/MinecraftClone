using Noise;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using System;
using System.Collections.Generic;


namespace MinecraftClone
{
    public class MainGame : GameWindow
    {
        Camera MainCamera;

        NormalMesh MainMesh;
        Shader MainShader;
        TextureSingle MainTexture;

        Shader VoxelShader;
        TextureArray VoxelTextures;

        List<Vector3> Positions;
        List<ChunkData> Chunks;

        public MainGame(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
            Console.WriteLine("Window created: ({0}, {1})", width, height);
        }

        #region Rendering

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.4921875f, 0.6640625f, 1, 1);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.FrontFace(FrontFaceDirection.Cw);

            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Point);
            //GL.PointSize(10);

            Positions = new List<Vector3>();
            Chunks = new List<ChunkData>();

            VoxelShader = new Shader("shaders/VoxelShader.vert", "shaders/VoxelShader.frag");
            VoxelShader.Use();

            VoxelTextures = new TextureArray(new string[] {"textures/grass.png", "textures/dirt.png", "textures/stone.png", "textures/ore.png", "textures/bedrock.png", "textures/log.png", "textures/leaves.png", "textures/water.png", "textures/flowers.png" });
            VoxelTextures.Use();

            MainMesh = new NormalMesh();

            MainShader = new Shader("shaders/NormalShader.vert", "shaders/NormalShader.frag");
            MainShader.Use();

            MainTexture = new TextureSingle("textures/engine/box.png");
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
            MainTexture.Use();

            MainShader.SetMatrix4x4("local", Matrix4.CreateTranslation(time, 0, 0));
            //MainShader.SetVector3("color", new Vector3(1, 0, 0));
            MainMesh.RenderMesh();

            MainShader.SetMatrix4x4("local", Matrix4.CreateTranslation(0, time, 0));
            //MainShader.SetVector3("color", new Vector3(0, 1, 0));
            MainMesh.RenderMesh();

            MainShader.SetMatrix4x4("local", Matrix4.CreateTranslation(0, 0, time));
            //MainShader.SetVector3("color", new Vector3(0, 0, 1));
            MainMesh.RenderMesh();

            Vector3 pos = new Vector3((float)Math.Round((MainCamera.Position.X - 32) / 64f), (float)Math.Round((MainCamera.Position.Y - 32) / 64f), (float)Math.Round((MainCamera.Position.Z - 32) / 64f));

            VoxelShader.Use();
            VoxelShader.SetMatrix4x4("projection", MainCamera.GetProjectionMatrix());
            VoxelShader.SetMatrix4x4("world", MainCamera.GetViewMatrix());
            VoxelTextures.Use();

            if (canUpdate)
            {
                for (int x = 0; x < 1; x++)
                {
                    for (int y = 0; y < 1; y++)
                    {
                        for (int z = 0; z < 1; z++)
                        {
                            if (!Positions.Contains(pos + new Vector3(x, y, z)))
                            {
                                Chunks.Add(null);
                                Positions.Add(pos + new Vector3(x, y, z));
                            }
                        }
                    }
                }
            }

            bool newchunk = true;
            for (int i = 0; i < Positions.Count; i++)
            {
                if (Chunks[i] == null && newchunk)
                {
                    newchunk = false;
                    Chunks[i] = new ChunkData(Positions[i]);;
                }
                else if (Chunks[i] != null)
                {
                    VoxelShader.SetMatrix4x4("local", Matrix4.CreateTranslation(Chunks[i].chunkOffset * 64));
                    Chunks[i].Render();
                }
            }

            Console.WriteLine(1 / e.Time);

            Context.SwapBuffers();
            time += (float)e.Time;
            base.OnRenderFrame(e);
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

        bool canUpdate = true;
        bool toggle = true;

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();
            if (input.IsKeyDown(Key.Escape)) { Exit(); }
            base.OnUpdateFrame(e);

            float speed = 20;

            if (input.IsKeyDown(Key.L) && toggle)
            {
                canUpdate = !canUpdate;
                Console.WriteLine("Update chunks: " + canUpdate);
                toggle = false;
            }
            else if (input.IsKeyUp(Key.L))
            {
                toggle = true;
            }

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
                MainCamera.Rotate((float)e.Time * 9 * speed, 0);
            }

            if (input.IsKeyDown(Key.Left))
            {
                MainCamera.Rotate((float)e.Time * -9 * speed, 0);
            }

            if (input.IsKeyDown(Key.Up))
            {
                MainCamera.Rotate(0, (float)e.Time * 9 * speed);
            }

            if (input.IsKeyDown(Key.Down))
            {
                MainCamera.Rotate(0, (float)e.Time * -9 * speed);
            }

            //Console.WriteLine(Math.Round(1 / e.Time));
        }
    }
}