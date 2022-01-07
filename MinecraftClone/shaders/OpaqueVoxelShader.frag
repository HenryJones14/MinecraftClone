#version 330 core
out vec4 FragColor;

in vec2 UV;
flat in int TEX;

uniform sampler2D texture0;

vec2 offset[4] = vec2[4]( vec2(0, 1), vec2(1, 0), vec2(0, 0), vec2(1, 1) );

void main()
{
    vec4 col = texture(texture0, (UV + offset[TEX]) * 0.5);

    if (col.a == 0)
    {
        discard;
    }

    FragColor = vec4(col.rgb, col.a);
}