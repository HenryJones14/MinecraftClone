#version 330 core
layout (location = 0) in vec3 pos;
layout (location = 1) in vec3 nor;
layout (location = 2) in vec4 col;
layout (location = 3) in vec2 uvs;

uniform mat4 object;
uniform mat4 view;
uniform mat4 projection;

out vec3 Normal;
out vec4 Color;
out vec2 UVs;

void main()
{
    gl_Position = projection * view * object * vec4(pos, 1.0);

    Normal = normalize((object * vec4(nor, 1.0)).xyz - vec3(object[3].x, object[3].y, object[3].z));
    Color = clamp(col, 0, 1);
    UVs = uvs;
}