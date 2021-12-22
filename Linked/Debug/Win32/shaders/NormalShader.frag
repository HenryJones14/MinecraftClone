#version 330 core
out vec4 FragColor;

in vec2 uvpos;
in vec3 normal;

//uniform sampler2D texture0;

void main()
{
    vec4 col = vec4(1, 1, 1, 1); //texture(texture0, uvpos);
    float lig = (dot(vec3(0, 1, 0), normal) + 1) * 0.5;
    FragColor = vec4(col.xyz * lig, col.a);

    if (FragColor.a == 0)
    {
        discard;
    }
}