#include "NormalMesh.h"

NormalMesh::NormalMesh()
{       
    float tmp_vertices[] =
    {
        1, 0, 0, 1, 0, 0, 1, 0.8, 0.1, 0, 1, 0, 0, 1, 0.8, 0, -0.1, 1, 0, 0, 1, 0.8, -0.1, 0, 1, 0, 0, 1, 0.8, 0, 0.1, 1, 0, 0, 1, 0.82, 0.05, 0, 1, 0, 0, 1, 0.82, 0, -0.05, 1, 0, 0, 1, 0.82, -0.05, 0, 1, 0, 0, 1, 0.82, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0.8, -0.1, 0, 1, 0, 1, 0.1, 0.8, 0, 0, 1, 0, 1, 0, 0.8, 0.1, 0, 1, 0, 1, -0.1, 0.8, 0, 0, 1, 0, 1, 0, 0.82, -0.05, 0, 1, 0, 1, 0.05, 0.82, 0, 0, 1, 0, 1, 0, 0.82, 0.05, 0, 1, 0, 1, -0.05, 0.82, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 1, 1, 0, 0.1, 0.8, 0, 0, 1, 1, 0.1, 0, 0.8, 0, 0, 1, 1, 0, -0.1, 0.8, 0, 0, 1, 1, -0.1, 0, 0.8, 0, 0, 1, 1, 0, 0.05, 0.82, 0, 0, 1, 1, 0.05, 0, 0.82, 0, 0, 1, 1, 0, -0.05, 0.82, 0, 0, 1, 1, -0.05, 0, 0.82, 0, 0, 1, 1, 0.04, 0.04, 0.04, 1, 1, 1, 1, 0.04, 0.04, -0.04, 1, 1, 1, 1, -0.04, 0.04, 0.04, 1, 1, 1, 1, 0.04, -0.04, 0.04, 1, 1, 1, 1, -0.04, -0.04, -0.04, 1, 1, 1, 1,
    };

    unsigned int tmp_indices[] =
    {
        // X tip
        0, 2, 1,
        0, 3, 2,
        0, 4, 3,
        0, 1, 4,

        // X base
         1,  6,  5,
         1,  2,  6,
         2,  7,  6,
         2,  3,  7,
         3,  8,  7,
         3,  4,  8,
         4,  5,  8,
         4,  1,  5,

        // X rod
         5,  6, 28,
        28,  6, 31,
         6,  7, 31,
        31,  7, 30,
         7,  8, 30,
        30,  8, 27,
         8,  5, 27,
        27,  5, 28,



        // Y tip
         9, 11, 10,
         9, 12, 11,
         9, 13, 12,
         9, 10, 13,
        
        // Y base
        10, 15, 14,
        10, 11, 15,
        11, 16, 15,
        11, 12, 16,
        12, 17, 16,
        12, 13, 17,
        13, 14, 17,
        13, 10, 14,

        // Y rod
        14, 15, 28,
        28, 15, 27,
        15, 16, 27,
        27, 16, 29,
        16, 17, 29,
        29, 17, 31,
        17, 14, 31,
        31, 14, 28,



        // Z tip
        18, 20, 19,
        18, 21, 20,
        18, 22, 21,
        18, 19, 22,

        // Z base
        19, 24, 23,
        19, 20, 24,
        20, 25, 24,
        20, 21, 25,
        21, 26, 25,
        21, 22, 26,
        22, 23, 26,
        22, 19, 23,

        // Z rod
        23, 24, 27,
        27, 24, 30,
        24, 25, 30,
        30, 25, 31,
        25, 26, 31,
        31, 26, 29,
        26, 23, 29,
        29, 23, 27,

    };
    IndiceCount = sizeof(tmp_indices) / sizeof(tmp_indices[0]);

    // Starting setup
    glGenVertexArrays(1, &VertexArrayObject);
    glGenBuffers(1, &VertexBufferObject);
    glGenBuffers(1, &ElementBufferObject);

    // Data filling
    glBindVertexArray(VertexArrayObject);

    glBindBuffer(GL_ARRAY_BUFFER, VertexBufferObject);
    glBufferData(GL_ARRAY_BUFFER, sizeof(tmp_vertices), tmp_vertices, GL_STATIC_DRAW);

    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ElementBufferObject);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(tmp_indices), tmp_indices, GL_STATIC_DRAW);

    // Atribute setup
    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 7 * sizeof(float), (void*)0);
    glEnableVertexAttribArray(0);

    glVertexAttribPointer(1, 4, GL_FLOAT, GL_FALSE, 7 * sizeof(float), (void*)(3 * sizeof(float)));
    glEnableVertexAttribArray(1);

    // Cleanup
    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glBindVertexArray(0);
}

void NormalMesh::RenderMesh()
{
    glBindVertexArray(VertexArrayObject);
    glDrawElements(GL_TRIANGLES, IndiceCount, GL_UNSIGNED_INT, 0);
}

NormalMesh::~NormalMesh()
{
    glDeleteVertexArrays(1, &VertexArrayObject);
    glDeleteBuffers(1, &VertexBufferObject);
    glDeleteBuffers(1, &ElementBufferObject);
}