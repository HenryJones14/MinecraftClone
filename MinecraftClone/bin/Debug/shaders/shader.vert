#version 330 core
layout (location = 0) in vec3 pos;
layout (location = 1) in vec2 uvs;

out vec2 uv;
uniform mat4 transform;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    uv = uvs;
    gl_Position = vec4(pos, 1.0f) * transform * view * projection;
}