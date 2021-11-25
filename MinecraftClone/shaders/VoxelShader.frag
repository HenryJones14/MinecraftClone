#version 330 core
out vec4 FragColor;

in vec2 uvpos;
in float light;

uniform sampler2D texture0;

void main()
{
    FragColor = texture(texture0, uvpos) * light; //vec4(1.0f, 0.5f, 0.2f, 1.0f);
    
    if (FragColor.a == 0)
    {
        discard;
    }
}

/*
#version 330 core
out vec4 FragColor;

in vec2 uvpos;
in float light;

void main()
{
    FragColor = vec4(light, light, light, 1);//texture(textures, vec3(uvpos, 1)) * light;
    if (FragColor.a == 0)
    {
        discard;
    }
}
*/