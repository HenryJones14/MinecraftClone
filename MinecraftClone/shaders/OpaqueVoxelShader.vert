#version 330 core
layout (location = 0) in vec3 pos;
layout (location = 1) in vec3 uvs;

uniform mat4 object;
uniform mat4 view;
uniform mat4 projection;

out vec2 UV;
flat out int TEX;

void main() 
{
    gl_Position = projection * view * object * vec4(pos, 1.0);
    UV = uvs.xy;
    TEX = int(uvs.z);
}