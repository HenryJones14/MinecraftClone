#version 330 core
layout (location = 0) in vec3 pos;
layout (location = 1) in vec2 uvs;
layout (location = 2) in vec3 nor;

out vec2 uvpos;
out vec3 normal;

uniform mat4 local;
uniform mat4 world;
uniform mat4 projection;

void main()
{
    gl_Position = vec4(pos, 1.0f) * local * world * projection;
    normal = nor;
    uvpos = uvs;
}