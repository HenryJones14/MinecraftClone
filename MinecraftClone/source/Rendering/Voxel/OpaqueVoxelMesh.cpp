#include "OpaqueVoxelMesh.h"

void OpaqueVoxelMesh::CreateMesh()
{
    if (!created)
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
        glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)0);
        glEnableVertexAttribArray(0);

        glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(3 * sizeof(float)));
        glEnableVertexAttribArray(1);

        glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(6 * sizeof(float)));
        glEnableVertexAttribArray(2);

        // Cleanup
        glBindBuffer(GL_ARRAY_BUFFER, 0);
        glBindVertexArray(0);

        created = true;
    }
}

void OpaqueVoxelMesh::RenderMesh()
{
    if (created)
    {
        glBindVertexArray(VertexArrayObject);
        glDrawElements(GL_TRIANGLES, indices.size(), GL_UNSIGNED_INT, 0);
    }
}

void OpaqueVoxelMesh::DestroyMesh()
{
    if (created)
    {
        glDeleteVertexArrays(1, &VertexArrayObject);
        glDeleteBuffers(1, &VertexBufferObject);
        glDeleteBuffers(1, &ElementBufferObject);

        created = false;
    }
}

OpaqueVoxelMesh::~OpaqueVoxelMesh()
{
    DestroyMesh();
}
