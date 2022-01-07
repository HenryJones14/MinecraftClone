#pragma once

#include <GLEW/glew.h>
#include <GLFW/glfw3.h>

class MainInput
{
public:
	static void UpdateInput(GLFWwindow*, int, int, int, int);

	static float MoveX, MoveY, MoveZ, LookX, LookY;
};

