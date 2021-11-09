#version 330 core
layout (location = 0) in vec3 aPosition;

out vec4 vertexColor;

void main()
{
    vertexColor = vec4(aPosition.x + 0.5f, aPosition.y + 0.5f, aPosition.z + 0.5f, 1.0);
    gl_Position = vec4(aPosition, 1.0);
}