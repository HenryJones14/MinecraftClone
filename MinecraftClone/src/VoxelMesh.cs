using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace MinecraftClone
{
    public class VoxelMesh
    {
        public uint[] indices;
        public uint[] vertices;

        /*public VoxelMesh()
        {
            vertexcount = 4 * 6;

            vertices = new Vector3[4 * 6]
            {
                // right
                new Vector3( 0.5f,  0.5f, -0.5f),
                new Vector3( 0.5f, -0.5f,  0.5f),
                new Vector3( 0.5f, -0.5f, -0.5f),
                new Vector3( 0.5f,  0.5f,  0.5f),

                // left
                new Vector3(-0.5f,  0.5f,  0.5f),
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f,  0.5f),
                new Vector3(-0.5f,  0.5f, -0.5f),

                // top
                new Vector3( 0.5f,  0.5f, -0.5f),
                new Vector3(-0.5f,  0.5f,  0.5f),
                new Vector3( 0.5f,  0.5f,  0.5f),
                new Vector3(-0.5f,  0.5f, -0.5f),

                // bottom
                new Vector3( 0.5f, -0.5f,  0.5f),
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3( 0.5f, -0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f,  0.5f),

                // front
                new Vector3( 0.5f,  0.5f,  0.5f),
                new Vector3(-0.5f, -0.5f,  0.5f),
                new Vector3( 0.5f, -0.5f,  0.5f),
                new Vector3(-0.5f,  0.5f,  0.5f),

                // back
                new Vector3(-0.5f,  0.5f, -0.5f),
                new Vector3( 0.5f, -0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3( 0.5f,  0.5f, -0.5f),
            };
            indices = new uint[6 * 6]
            {
                // right
                 0,  1,  2,
                 0,  3,  1,
                 
                // left
                 4,  5,  6,
                 4,  7,  5,
                 
                // top
                 8,  9, 10,
                 8, 11,  9,
                 
                // bottom
                12, 13, 14,
                12, 15, 13,
                
                // front
                16, 17, 18,
                16, 19, 17,
                
                // bask
                20, 21, 22,
                20, 23, 21,
            };
            uvs = new Vector2[4 * 6]
            {
                // right
                new Vector2(0.000f, 1.000f),
                new Vector2(0.333f, 0.500f),
                new Vector2(0.000f, 0.500f),
                new Vector2(0.333f, 1.000f),
                
                // left
                new Vector2(0.333f, 1.000f),
                new Vector2(0.666f, 0.500f),
                new Vector2(0.333f, 0.500f),
                new Vector2(0.666f, 1.000f),
                
                // top
                new Vector2(0.666f, 1.000f),
                new Vector2(0.999f, 0.500f),
                new Vector2(0.666f, 0.500f),
                new Vector2(0.999f, 1.000f),
                
                // bottom
                new Vector2(0.000f, 0.500f),
                new Vector2(0.333f, 0.000f),
                new Vector2(0.000f, 0.000f),
                new Vector2(0.333f, 0.500f),
                
                // front
                new Vector2(0.333f, 0.500f),
                new Vector2(0.666f, 0.000f),
                new Vector2(0.333f, 0.000f),
                new Vector2(0.666f, 0.500f),
                
                // back
                new Vector2(0.666f, 0.500f),
                new Vector2(0.999f, 0.000f),
                new Vector2(0.666f, 0.000f),
                new Vector2(0.999f, 0.500f),
            };
            lights = new float[4 * 6]
            {
                // right
                0.50f, 0.50f, 0.50f, 0.50f, 

                // left
                0.50f, 0.50f, 0.50f, 0.50f, 

                // top
                1.00f, 1.00f, 1.00f, 1.00f, 

                // bottom
                0.00f, 0.00f, 0.00f, 0.00f, 

                // front
                0.75f, 0.75f, 0.75f, 0.75f, 

                // back
                0.25f, 0.25f, 0.25f, 0.25f,
            };

            InitializeMesh();
        }*/

        public VoxelMesh(uint[] Triangles, uint[] VertexInfo)
        {
            indices = Triangles;
            vertices = VertexInfo;

            Console.WriteLine("Raw pos " + Convert.ToString(VertexInfo[0], 2));
            Console.WriteLine("Vec3 pos " + UnpackPosition(VertexInfo[0]));
            Console.WriteLine("UVs " + Convert.ToString(VertexInfo[1], 2));
            Console.WriteLine("Light " + Convert.ToString(VertexInfo[2], 2));

            Console.WriteLine("Raw pos " + Convert.ToString(VertexInfo[3], 2));
            Console.WriteLine("Vec3 pos " + UnpackPosition(VertexInfo[3]));
            Console.WriteLine("UVs " + Convert.ToString(VertexInfo[4], 2));
            Console.WriteLine("Light " + Convert.ToString(VertexInfo[5], 2));

            Console.WriteLine("Raw pos " + Convert.ToString(VertexInfo[6], 2));
            Console.WriteLine("Vec3 pos " + UnpackPosition(VertexInfo[6]));
            Console.WriteLine("UVs " + Convert.ToString(VertexInfo[7], 2));
            Console.WriteLine("Light " + Convert.ToString(VertexInfo[8], 2));

            Console.WriteLine("Raw pos " + Convert.ToString(VertexInfo[9], 2));
            Console.WriteLine("Vec3 pos " + UnpackPosition(VertexInfo[9]));
            Console.WriteLine("UVs " + Convert.ToString(VertexInfo[10], 2));
            Console.WriteLine("Light " + Convert.ToString(VertexInfo[11], 2));

            InitializeMesh();
        }

        #region Rendering

        public int VertexArrayObject;
        private int VertexBufferObject;
        private int ElementBufferObject;

        public void RenderMesh()
        {
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }

        private void InitializeMesh()
        {
            VertexArrayObject = GL.GenVertexArray();
            VertexBufferObject = GL.GenBuffer();
            ElementBufferObject = GL.GenBuffer();

            GL.BindVertexArray(VertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(uint), vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.UnsignedInt, false, 3 * sizeof(uint), 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.UnsignedInt, false, 3 * sizeof(uint), 1 * sizeof(uint));

            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.UnsignedInt, false, 3 * sizeof(uint), 2 * sizeof(uint));

            GL.BindVertexArray(0);
        }

        #endregion

        // resources:
        // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators
        // https://stackoverflow.com/questions/6556961/use-of-the-bitwise-operators-to-pack-multiple-values-in-one-int

        public static uint PackPosition(Vector3 Input)
        {
            return ((uint)Input.X << 12) | ((uint)Input.Y << 6) | ((uint)Input.Z << 0);
        }
        public static uint PackPosition(Vector2 Input)
        {
            return ((uint)Input.X << 12) | ((uint)Input.Y << 6) | (0 << 0);
        }

        public static Vector3 UnpackPosition(uint Input)
        {
            return new Vector3((Input >> 12) & 0b111111, (Input >> 6) & 0b111111, (Input >> 0) & 0b111111);
        }
    }
}
