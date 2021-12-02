#version 330 core
out vec4 FragColor;

in vec3 uvpos;
in vec3 light;

uniform sampler2DArray texture0;

void main()
{
    FragColor = texture(texture0, vec3(uvpos));
    vec3 col = FragColor.rgb;
    float lit = (light.x * 0.5f + 0.5f) * clamp((light.y + 256) / 256, 0, 1) * (light.z * 0.5f + 0.5f);

    FragColor = vec4(mix(col, col * lit, FragColor.a), 1);
}