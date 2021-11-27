#version 330 core
out vec4 FragColor;

in vec2 uvpos;
in float light;

uniform sampler2D texture0;

void main()
{
    FragColor = texture(texture0, uvpos);
    vec3 col = FragColor.rgb;
    float lit = (light * 0.5f + 0.5f);

    FragColor = vec4(mix(col, col * lit, FragColor.a), 1);
}