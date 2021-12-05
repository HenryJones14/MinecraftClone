#version 330 core
out vec4 FragColor;

in vec3 uvpos;
in vec3 light;

uniform sampler2DArray texture0;

void main()
{
    FragColor = texture(texture0, vec3(uvpos));
    vec3 col = FragColor.rgb;
    float lit = (light.x * 0.5f + 0.5f) * clamp((light.y + 256) / 256, 0, 1) * ((min(light.z, 3) / 3) * 0.6f + 0.4f);

    FragColor = vec4(mix(col, col * lit, FragColor.a), 1);

    float ndc = gl_FragCoord.z * 2.0 - 1.0;
    float near = 0.1f;
    float far = 10f;

    FragColor = mix(FragColor, vec4(0.4921875f, 0.6640625f, 1, 1), pow((2.0 * near * far) / (far + near - ndc * (far - near)) / far, 25));
}