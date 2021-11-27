#version 330 core
out vec4 FragColor;

in vec2 uvpos;
in vec3 normal;

uniform sampler2D texture0;

void main()
{
    vec4 col = texture(texture0, uvpos); //vec4(1.0f, 0.5f, 0.2f, 1.0f);
    vec3 lig = col.xyz * ((dot(vec3(0, 1, 0), normal) + 1) * 0.4f + 0.2);
    FragColor = vec4((col.xyz * (1 - col.a)) + (lig.xyz * col.a), 1);

    if (FragColor.a == 0)
    {
        discard;
    }
}