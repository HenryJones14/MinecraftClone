#version 330 core
out vec4 FragColor;

in vec3 uvpos;
in float light;

uniform sampler2DArray texture0;

void main()
{
    vec4 tex = texture(texture0, vec3(uvpos));
    if(tex.a == 0){discard;}

    FragColor = vec4(mix(tex.rgb, tex.rgb * light, tex.a), 1);

    float ndc = gl_FragCoord.z * 2.0 - 1.0;
    float near = 0.1f;
    float far = 10f;

    FragColor = mix(FragColor, vec4(0.4921875f, 0.6640625f, 1, 1), pow((2.0 * near * far) / (far + near - ndc * (far - near)) / far, 25));
    FragColor.a = tex.a;
}