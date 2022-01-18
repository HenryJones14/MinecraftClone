#version 330 core
out vec4 FragColor;

in vec2 Lighting;
in vec3 UV;

uniform sampler2DArray texture0;

void main()
{
    vec4 col = texture(texture0, UV);

    if (col.a == 0)
    {
        discard;
    }

    FragColor = vec4(col.rgb * Lighting.x * Lighting.y, col.a);
}