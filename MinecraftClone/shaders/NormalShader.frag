#version 330 core
out vec4 FragColor;

in vec3 Normal;
in vec4 Color;
in vec2 UVs;

uniform sampler2D texture0;

void main()
{
    vec4 col = texture(texture0, UVs) * Color;

    if (col.a == 0)
    {
        discard;
    }

    float lig = (dot( Normal , vec3(0, 1, 0)) + 1) * 0.5;
    FragColor = vec4(col.rgb * lig, col.a);
}