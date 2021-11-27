using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace MinecraftClone
{
    public class NormalMesh
    {
        public int vertexcount;
        public Vector3[] vertices;
        public uint[] indices;
        public Vector2[] uvs;
        public Vector3[] normals;

        public NormalMesh()
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
            normals = new Vector3[4 * 6]
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

            InitializeMesh();
        }

        public NormalMesh(int VertexCount, Vector3[] Vertices, uint[] Indices, Vector2[] UVs, Vector3[] Normals)
        {
            vertexcount = VertexCount;
            vertices = Vertices;
            indices = Indices;
            uvs = UVs;
            normals = Normals;

            InitializeMesh();
        }

        public NormalMesh(string meshPath)
        {
            string[] lines = File.ReadAllLines(meshPath);
            List<Vector3> newvertices = new List<Vector3>();
            List<uint> newindices = new List<uint>();
            List<Vector2> newuvs = new List<Vector2>();
            List<Vector3> newnormals = new List<Vector3>();

            List<Vector2> alluvs = new List<Vector2>();
            List<Vector3> allnormals = new List<Vector3>();

            foreach (string line in lines)
            {
                //Console.WriteLine(line);

                if (line.Substring(0, 2) == "v ")
                {
                    newvertices.Add(new Vector3(float.Parse(line.Split(" "[0])[1]), float.Parse(line.Split(" "[0])[2]), float.Parse(line.Split(" "[0])[3])));
                }

                if (line.Substring(0, 3) == "vt ")
                {
                    alluvs.Add(new Vector2(float.Parse(line.Split(" "[0])[1]), float.Parse(line.Split(" "[0])[2])));
                }

                if (line.Substring(0, 3) == "vn ")
                {
                    allnormals.Add(new Vector3(float.Parse(line.Split(" "[0])[1]), float.Parse(line.Split(" "[0])[2]), float.Parse(line.Split(" "[0])[3])));
                }

                if (line.Substring(0, 2) == "f ")
                {
                    newindices.Add(uint.Parse(line.Split(" "[0])[1].Split("/"[0])[0]) - 1);
                    newindices.Add(uint.Parse(line.Split(" "[0])[2].Split("/"[0])[0]) - 1);
                    newindices.Add(uint.Parse(line.Split(" "[0])[3].Split("/"[0])[0]) - 1);
                    
                    newuvs.Add(alluvs[
                        int.Parse(line.Split(" "[0])[1].Split("/"[0])[1]) - 1
                    ]);
                    newuvs.Add(alluvs[
                        int.Parse(line.Split(" "[0])[2].Split("/"[0])[1]) - 1
                    ]);
                    newuvs.Add(alluvs[
                        int.Parse(line.Split(" "[0])[3].Split("/"[0])[1]) - 1
                    ]);

                    newnormals.Add(allnormals[
                        int.Parse(line.Split(" "[0])[1].Split("/"[0])[2]) - 1
                    ]);
                    newnormals.Add(allnormals[
                        int.Parse(line.Split(" "[0])[2].Split("/"[0])[2]) - 1
                    ]);
                    newnormals.Add(allnormals[
                        int.Parse(line.Split(" "[0])[3].Split("/"[0])[2]) - 1
                    ]);
                }
            }

            vertexcount = newvertices.Count;
            vertices = newvertices.ToArray();
            indices = newindices.ToArray();
            uvs = newuvs.ToArray();
            normals = newnormals.ToArray();

            Console.WriteLine("newvertices " + newvertices.Count);
            Console.WriteLine("newindices " + newindices.Count);
            Console.WriteLine("newuvs " + newuvs.Count);
            Console.WriteLine("alluvs " + alluvs.Count);
            Console.WriteLine("allnormals " + allnormals.Count);

            for (int i = 0; i < vertexcount; i++)
            {
                Console.WriteLine("Index: {0}, Position: {1}, UV: {2}", i, vertices[i], uvs[i]);
            }

            for (int i = 0; i < indices.Length / 3; i++)
            {
                Console.WriteLine("face: ({0}, {1}, {2})", indices[i * 3 + 0], indices[i * 3 + 1], indices[i * 3 + 2]);
            }

            InitializeMesh();
        }

        public NormalMesh(int SizeX, int SizeY, float noiseScale)
        {
            List<Vector3> newvertices = new List<Vector3>();
            List<uint> newindices = new List<uint>();
            List<Vector2> newuvs = new List<Vector2>();

            int index = 0;
            int num = 0;

            Noise.OpenSimplexNoise noise = new Noise.OpenSimplexNoise();

            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    newvertices.Add(new Vector3(x, (float)noise.Evaluate(x / noiseScale, y / noiseScale), y));
                    newuvs.Add(new Vector2(x, y) * (1 / ((SizeX + SizeY) / 2f)));

                    if (x < SizeX - 1 && y < SizeY - 1)
                    {
                        newindices.Add((uint)(index));
                        newindices.Add((uint)(index + 1));
                        newindices.Add((uint)(index + SizeX));

                        newindices.Add((uint)(index + 1));
                        newindices.Add((uint)(index + SizeX + 1));
                        newindices.Add((uint)(index + SizeX));
                    }
                    index++;
                    num++;
                }
            }

            vertexcount = SizeX * SizeY;
            vertices = newvertices.ToArray();
            uvs = newuvs.ToArray();
            normals = newvertices.ToArray();
            indices = newindices.ToArray();

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
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));

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

                info.Add(normals[i].X);
                info.Add(normals[i].Y);
                info.Add(normals[i].Z);
            }

            return info.ToArray();
        }

        #endregion
    }
}
