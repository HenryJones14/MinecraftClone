#pragma once

#include <vector>
#include <string>

#include <GLEW/glew.h>
#include <GLFW/glfw3.h>

class NormalMesh
{
public:
    NormalMesh();
    NormalMesh(std::string);
    ~NormalMesh();

    void RenderMesh();

private:
    unsigned int VertexArrayObject = 0;
    unsigned int VertexBufferObject = 0;
    unsigned int ElementBufferObject = 0;

    std::vector<float> vertices;
    std::vector<unsigned int> indices;

    void InitializeMesh();
};