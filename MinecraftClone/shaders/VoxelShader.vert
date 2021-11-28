#version 330 core
layout (location = 0) in uint pos;
layout (location = 1) in uint uvs;
layout (location = 2) in uint nor;

out vec2 uvpos;
out float light;

uniform mat4 local;
uniform mat4 world;
uniform mat4 projection;

void main()
{
    float x = (pos & 0x3F000u) >> 12u;
    float y = (pos & 0xFC0u) >> 6u;
    float z = (pos & 0x3Fu);
    
    float u = float((uvs & 0x3F000u) >> 12u);
    float v = float((uvs & 0xFC0u) >> 6u);
    float b = float((uvs & 0x3Fu) >> 0u);

    gl_Position = vec4(x, y, z, 1.0f) * local * world * projection;
    light = nor / 100f;
    uvpos = vec2(u, v);
}