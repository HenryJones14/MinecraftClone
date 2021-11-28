using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace MinecraftClone
{
    public class VoxelMesh
    {
        public int vertexcount;
        public Vector3[] vertices;
        public uint[] indices;
        public Vector3[] uvs;
        public float[] lights;

        public VoxelMesh()
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
            uvs = new Vector3[4 * 6]
            {
                // right
                new Vector3(0.000f, 1.000f, 0),
                new Vector3(0.333f, 0.500f, 0),
                new Vector3(0.000f, 0.500f, 0),
                new Vector3(0.333f, 1.000f, 0),
                
                // left
                new Vector3(0.333f, 1.000f, 0),
                new Vector3(0.666f, 0.500f, 0),
                new Vector3(0.333f, 0.500f, 0),
                new Vector3(0.666f, 1.000f, 0),
                
                // top
                new Vector3(0.666f, 1.000f, 0),
                new Vector3(0.999f, 0.500f, 0),
                new Vector3(0.666f, 0.500f, 0),
                new Vector3(0.999f, 1.000f, 0),
                
                // bottom
                new Vector3(0.000f, 0.500f, 0),
                new Vector3(0.333f, 0.000f, 0),
                new Vector3(0.000f, 0.000f, 0),
                new Vector3(0.333f, 0.500f, 0),
                
                // front
                new Vector3(0.333f, 0.500f, 0),
                new Vector3(0.666f, 0.000f, 0),
                new Vector3(0.333f, 0.000f, 0),
                new Vector3(0.666f, 0.500f, 0),
                
                // back
                new Vector3(0.666f, 0.500f, 0),
                new Vector3(0.999f, 0.000f, 0),
                new Vector3(0.666f, 0.000f, 0),
                new Vector3(0.999f, 0.500f, 0),
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
        }

        public VoxelMesh(int VertexCount, Vector3[] Vertices, uint[] Indices, Vector3[] UVs, float[] Lights)
        {
            vertexcount = VertexCount;
            vertices = Vertices;
            indices = Indices;
            uvs = UVs;
            lights = Lights;

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
            GL.BufferData(BufferTarget.ArrayBuffer, GetMemorySize(), GetMemoryInfo(), BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 7 * sizeof(float), 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 7 * sizeof(float), 3 * sizeof(float));

            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 1, VertexAttribPointerType.Float, false, 7 * sizeof(float), 6 * sizeof(float));

            GL.BindVertexArray(0);
        }



        private int GetMemorySize()
        {
            return GetMemoryInfo().Length * sizeof(float);
        }

        private float[] GetMemoryInfo()
        {
            List<float> info = new List<float>();

            for (int i = 0; i < vertexcount; i++)
            {
                info.Add(vertices[i].X);
                info.Add(vertices[i].Y);
                info.Add(vertices[i].Z);

                info.Add(uvs[i].X);
                info.Add(uvs[i].Y);
                info.Add(uvs[i].Z);

                info.Add(lights[i]);
            }

            return info.ToArray();
        }

        #endregion
    }
}
