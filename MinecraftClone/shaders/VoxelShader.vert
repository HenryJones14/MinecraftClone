#version 330 core
layout (location = 0) in vec3 pos;
layout (location = 1) in vec3 uvs;
layout (location = 2) in vec3 nor;

out vec3 uvpos;
out float light;

uniform mat4 local;
uniform mat4 world;
uniform mat4 projection;

void main()
{
    gl_Position = vec4(pos, 1.0f) * local * world * projection;
    light = (nor.x * 0.3f + 0.7f) * clamp((nor.y + 256) / 256, 0, 1) * ((min(nor.z, 4) / 4) * 0.6f + 0.4f);
    uvpos = uvs;
}