#version 330 core
layout (location = 0) in uint pos;

uniform mat4 offset;
uniform mat4 projection;

void main()
{
    gl_Position = vec4
    (
        float(pos & 0x3Fu),
        float((pos & 0xFC0u) >> 6u),
        float((pos & 0x3F000u) >> 12u),
        1.0f
    )
    * offset * projection;
}