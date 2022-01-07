#include "VoxelMesh.h"
#include "NormalMesh.h"

#include "Miscellaneous/ChunkGenerator.h"

VoxelMesh::VoxelMesh()
{
    // RightTopFront, LeftTopFront, RightBottomFront, LeftBottomFront,
    // RightTopBack, LeftTopBack, RightBottomBack, LeftBottomBack,
    ChunkGenerator Generator = ChunkGenerator();
    Generator.GenerateChunk(&vertices, &indices);

    InitializeMesh();
}

void VoxelMesh::InitializeMesh()
{
    // Starting setup
    glGenVertexArrays(1, &VertexArrayObject);
    glGenBuffers(1, &VertexBufferObject);
    glGenBuffers(1, &ElementBufferObject);

    // Data filling
    glBindVertexArray(VertexArrayObject);

    glBindBuffer(GL_ARRAY_BUFFER, VertexBufferObject);
    glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.size(), &vertices[0], GL_STATIC_DRAW);

    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ElementBufferObject);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(unsigned int) * indices.size(), &indices[0], GL_STATIC_DRAW);

    // Atribute setup
    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), (void*)0);
    glEnableVertexAttribArray(0);

    glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), (void*)(3 * sizeof(float)));
    glEnableVertexAttribArray(1);

    // Cleanup
    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glBindVertexArray(0);
}

void VoxelMesh::RenderMesh()
{
    glBindVertexArray(VertexArrayObject);
    glDrawElements(GL_TRIANGLES, indices.size(), GL_UNSIGNED_INT, 0);
}

VoxelMesh::~VoxelMesh()
{
    glDeleteVertexArrays(1, &VertexArrayObject);
    glDeleteBuffers(1, &VertexBufferObject);
    glDeleteBuffers(1, &ElementBufferObject);
}
