#version 330 core
out vec4 FragColor;

in vec2 uvpos;
in vec3 normal;

uniform vec3 color;
uniform sampler2D texture0;

void main()
{
    FragColor = texture(texture0, uvpos) * vec4(color.xyz, 1.0f) * ((dot(vec3(0, 1, 0), normal) + 1) * 0.25f + 0.5f); //vec4(1.0f, 0.5f, 0.2f, 1.0f);
}