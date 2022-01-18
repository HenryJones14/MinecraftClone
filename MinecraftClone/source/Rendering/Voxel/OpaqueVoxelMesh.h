#pragma once

#include <vector>
#include <string>

#include <GLEW/glew.h>
#include <GLFW/glfw3.h>

class OpaqueVoxelMesh
{
public:
    ~OpaqueVoxelMesh();

    std::vector<float> vertices;
    std::vector<unsigned int> indices;

    void CreateMesh();
    void RenderMesh();
    void DestroyMesh();

private:
    bool created = false;

    unsigned int VertexArrayObject = 0;
    unsigned int VertexBufferObject = 0;
    unsigned int ElementBufferObject = 0;
};

