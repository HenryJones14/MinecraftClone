#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec4 col;

out vec4 color;

uniform mat4 projection;
uniform mat4 view;
uniform mat4 object;

void main()
{
	gl_Position = projection * view * object * vec4(aPos, 1.0);
	color = col;
}