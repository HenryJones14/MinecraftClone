#version 330 core
layout (location = 0) in vec3 pos;
layout (location = 1) in vec2 uvs;
layout (location = 2) in float nor;

out vec2 uvpos;
out float light;

uniform mat4 local;
uniform mat4 world;
uniform mat4 projection;

void main()
{
    gl_Position = vec4(pos, 1.0f) * local * world * projection;
    light = clamp(nor, 0f, 1f);
    uvpos = uvs;
}

/*
#version 330 core
layout (location = 0) in vec3 pos;
layout (location = 1) in vec2 uvs;
layout (location = 2) in float nor;

out vec2 uvpos;
out float light;

uniform vec3 offset;
uniform mat4 projection;

void main()
{
    gl_Position = vec4(pos + (offset * 32), 1.0f) * projection;
    uvpos = uvs;
    light = nor;
}
*/