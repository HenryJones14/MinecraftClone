#pragma once

#include <GLEW/glew.h>
#include <GLFW/glfw3.h>

class InputManager
{
public:
	static void UpdateKeyboardButtonInput(GLFWwindow*, int, int, int, int);

	static void UpdateMousePositionInput(GLFWwindow*, double, double);
	static void UpdateMouseButtonInput(GLFWwindow*, int, int, int);
	static void UpdateMouseScrollInput(GLFWwindow*, double, double);

	static void Reset();

	// Time
	static double DeltaTime;

	// Keyboard
	static float KeyboardMoveX, KeyboardMoveY, KeyboardMoveZ;
	
	// Mouse
	static float MousePositionX, MousePositionY, MouseScrollX, MouseScrollY;

private:
	static float newposx, newposy, oldposx, oldposy;
	static double time;
};

