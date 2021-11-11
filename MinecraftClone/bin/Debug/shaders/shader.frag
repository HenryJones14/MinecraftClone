#version 330 core
out vec4 FragColor;

in vec2 uv;

void main()
{
    FragColor = vec4(uv, 0, 1.0); //vec4(1.0f, 0.5f, 0.2f, 1.0f);
}