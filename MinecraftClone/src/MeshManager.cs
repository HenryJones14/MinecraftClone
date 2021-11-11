using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace MinecraftClone
{
    public class Mesh
    {
        public int vertexcount;
        public float[] vertices;
        public uint[] indices;
        public float[] uvs;

        public Mesh()
        {
            vertexcount = 8;
            vertices = new float[8 * 3]
            {
                                      // front:
                 0.5f,  0.5f,  0.5f,  // top left
                -0.5f, -0.5f,  0.5f,  // bottom right
                 0.5f, -0.5f,  0.5f,  // bottom left
                -0.5f,  0.5f,  0.5f,  // top right

                                      // back:
                -0.5f,  0.5f, -0.5f,  // top left
                 0.5f, -0.5f, -0.5f,  // bottom right
                -0.5f, -0.5f, -0.5f,  // bottom left
                 0.5f,  0.5f, -0.5f,  // top right

            };
            indices = new uint[12 * 3]
            {
                0, 1, 2,    // front first triangle
                0, 3, 1,    // front second triangle

                4, 5, 6,    // back first triangle
                4, 7, 5,    // back second triangle

                3, 6, 1,    // left first triangle
                3, 4, 6,    // left second triangle

                7, 2, 5,    // right first triangle
                7, 0, 2,    // right second triangle

                7, 3, 0,    // up first triangle
                7, 4, 3,    // up second triangle

                2, 6, 5,    // down first triangle
                2, 1, 6,    // down second triangle*/
            };
            uvs = new float[8 * 2]
            {
                0f, 1f,
                1f, 0f,
                0f, 0f,
                1f, 1f,

                0f, 1f,
                1f, 0f,
                0f, 0f,
                1f, 1f,
            };

            InitializeMesh();
        }

        public Mesh(int VertexCount, float[] Vertices, uint[] Indices, float[] UVs)
        {
            vertexcount = VertexCount;
            vertices = Vertices;
            indices = Indices;
            uvs = UVs;

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
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            GL.BindVertexArray(0);
        }

        private int GetMemorySize()
        {
            return vertices.Length * sizeof(float);
        }

        private float[] GetMemoryInfo()
        {
            List<float> info = new List<float>();

            for (int i = 0; i < vertexcount; i++)
            {
                info.Add(vertices[i * 3 + 0]);
                info.Add(vertices[i * 3 + 1]);
                info.Add(vertices[i * 3 + 2]);

                info.Add(uvs[i * 2 + 0]);
                info.Add(uvs[i * 2 + 1]);
            }

            return info.ToArray();
        }

        #endregion
    }
}
