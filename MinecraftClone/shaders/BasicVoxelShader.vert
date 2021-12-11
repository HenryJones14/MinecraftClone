#version 330 core
layout (location = 0) in vec3 pos;
layout (location = 1) in vec3 uvs;
layout (location = 2) in vec3 nor;

out vec3 uvpos;
out float light;

uniform mat4 local;
uniform mat4 world;
uniform mat4 projection;

uniform vec3 sunangle;

const vec3[6] normals = vec3[6]
(
    vec3(1, 0, 0),
    vec3(-1, 0, 0),
    vec3(0, 1, 0),
    vec3(0, -1, 0),
    vec3(0, 0, 1),
    vec3(0, 0, -1)
);

void main()
{
    gl_Position = vec4(pos, 1.0f) * local * world * projection;
    light = ((dot(normals[int(nor)], sunangle) + 1) * 0.25f + 0.5f) * ((1 - clamp(nor.y / 8f, 0, 1)) * 0.8f + 0.2f) * (clamp(min(nor.z, 4) / 4, 0, 1) * 0.7f + 0.3f);
    uvpos = uvs;
}