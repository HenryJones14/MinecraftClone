#pragma once

#include <GLEW/glew.h>
#include <GLFW/glfw3.h>

class NormalMesh
{
public:
    NormalMesh();
    ~NormalMesh();

    void RenderMesh();

private:
    unsigned int VertexArrayObject = 0;
    unsigned int VertexBufferObject = 0;
    unsigned int ElementBufferObject = 0;

    unsigned int vertexcount = 8;

    unsigned int indices[6 * 6]
    = {
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

    float vertices[4 * 6 * 3]
    = {
        // right
        0.5f,  0.5f, -0.5f,
        0.5f, -0.5f,  0.5f,
        0.5f, -0.5f, -0.5f,
        0.5f,  0.5f,  0.5f,

        // left
        -0.5f,  0.5f,  0.5f,
        -0.5f, -0.5f, -0.5f,
        -0.5f, -0.5f,  0.5f,
        -0.5f,  0.5f, -0.5f,

        // top
        0.5f,  0.5f, -0.5f,
        -0.5f,  0.5f,  0.5f,
        0.5f,  0.5f,  0.5f,
        -0.5f,  0.5f, -0.5f,

        // bottom
        0.5f, -0.5f,  0.5f,
        -0.5f, -0.5f, -0.5f,
        0.5f, -0.5f, -0.5f,
        -0.5f, -0.5f,  0.5f,

        // front
        0.5f,  0.5f,  0.5f,
        -0.5f, -0.5f,  0.5f,
        0.5f, -0.5f,  0.5f,
        -0.5f,  0.5f,  0.5f,

        // back
        -0.5f,  0.5f, -0.5f,
        0.5f, -0.5f, -0.5f,
        -0.5f, -0.5f, -0.5f,
        0.5f,  0.5f, -0.5f,
    };
    float uvs[4 * 6 * 2]
    = {
        // right
        0.000f, 1.000f,
        0.333f, 0.500f,
        0.000f, 0.500f,
        0.333f, 1.000f,

        // left
        0.333f, 1.000f,
        0.666f, 0.500f,
        0.333f, 0.500f,
        0.666f, 1.000f,

        // top
        0.666f, 1.000f,
        0.999f, 0.500f,
        0.666f, 0.500f,
        0.999f, 1.000f,

        // bottom
        0.000f, 0.500f,
        0.333f, 0.000f,
        0.000f, 0.000f,
        0.333f, 0.500f,

        // front
        0.333f, 0.500f,
        0.666f, 0.000f,
        0.333f, 0.000f,
        0.666f, 0.500f,

        // back
        0.666f, 0.500f,
        0.999f, 0.000f,
        0.666f, 0.000f,
        0.999f, 0.500f,
    };
    float normals[4 * 6 * 3]
    = {
        // right
        0.6f,  0.6f, -0.6f,
        0.6f, -0.6f,  0.6f,
        0.6f, -0.6f, -0.6f,
        0.6f,  0.6f,  0.6f,

        // left
        -0.6f,  0.6f,  0.6f,
        -0.6f, -0.6f, -0.6f,
        -0.6f, -0.6f,  0.6f,
        -0.6f,  0.6f, -0.6f,

        // top
        0.6f,  0.6f, -0.6f,
        -0.6f,  0.6f,  0.6f,
        0.6f,  0.6f,  0.6f,
        -0.6f,  0.6f, -0.6f,

        // bottom
        0.6f, -0.6f,  0.6f,
        -0.6f, -0.6f, -0.6f,
        0.6f, -0.6f, -0.6f,
        -0.6f, -0.6f,  0.6f,

        // front
        0.6f,  0.6f,  0.6f,
        -0.6f, -0.6f,  0.6f,
        0.6f, -0.6f,  0.6f,
        -0.6f,  0.6f,  0.6f,

        // back
        -0.6f,  0.6f, -0.6f,
        0.6f, -0.6f, -0.6f,
        -0.6f, -0.6f, -0.6f,
        0.6f,  0.6f, -0.6f,
    };
};