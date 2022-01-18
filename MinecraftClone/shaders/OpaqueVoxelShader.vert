#version 330 core
layout (location = 0) in vec3 pos;
layout (location = 1) in vec3 uvs;
layout (location = 2) in vec2 lig;

uniform mat4 object;
uniform mat4 view;
uniform mat4 projection;

out vec2 Lighting;
out vec3 UV;

void main() 
{
    gl_Position = projection * view * object * vec4(pos, 1.0);
    Lighting = vec2((lig.x + 1.5) * 0.4, lig.y);
    UV = uvs;
}