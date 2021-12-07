#version 330 core
out vec4 FragColor;

in vec3 uvpos;
in float light;

uniform sampler2DArray texture0;

void main()
{
    FragColor = texture(texture0, vec3(uvpos));
    if(FragColor.a < 0.5){discard;}
    vec3 col = FragColor.rgb;

    FragColor = vec4(mix(col, col * light, FragColor.a), 1);

    float ndc = gl_FragCoord.z * 2.0 - 1.0;
    float near = 0.1f;
    float far = 10f;

    FragColor = mix(FragColor, vec4(0.4921875f, 0.6640625f, 1, 1), pow((2.0 * near * far) / (far + near - ndc * (far - near)) / far, 25));
}