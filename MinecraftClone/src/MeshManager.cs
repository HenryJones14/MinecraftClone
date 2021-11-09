using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftClone
{
    public class Mesh
    {
        public float[] vertices;
        public uint[] indices;

        public Mesh()
        {
            vertices = new float[8*3]
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

            indices = new uint[12*3]
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
        }

        public Mesh(float[] Vertices, uint[] Indices)
        {
            vertices = Vertices;
            indices = Indices;
        }
    }
}
