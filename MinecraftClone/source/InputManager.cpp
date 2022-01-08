#include "InputManager.h"



float InputManager::KeyboardMoveX = 0, InputManager::KeyboardMoveY = 0, InputManager::KeyboardMoveZ = 0;

void InputManager::UpdateKeyboardButtonInput(GLFWwindow* window, int button, int scancode, int action, int mods)
{
    // X
    if (button == GLFW_KEY_D && action == GLFW_PRESS)
    {
        KeyboardMoveX += 1;
    }
    else if (button == GLFW_KEY_D && action == GLFW_RELEASE)
    {
        KeyboardMoveX -= 1;
    }

    if (button == GLFW_KEY_A && action == GLFW_PRESS)
    {
        KeyboardMoveX -= 1;
    }
    else if (button == GLFW_KEY_A && action == GLFW_RELEASE)
    {
        KeyboardMoveX += 1;
    }

    // Y
    if (button == GLFW_KEY_SPACE && action == GLFW_PRESS)
    {
        KeyboardMoveY += 1;
    }
    else if (button == GLFW_KEY_SPACE && action == GLFW_RELEASE)
    {
        KeyboardMoveY -= 1;
    }

    if (button == GLFW_KEY_LEFT_SHIFT && action == GLFW_PRESS)
    {
        KeyboardMoveY -= 1;
    }
    else if (button == GLFW_KEY_LEFT_SHIFT && action == GLFW_RELEASE)
    {
        KeyboardMoveY += 1;
    }

    // Z
    if (button == GLFW_KEY_W && action == GLFW_PRESS)
    {
        KeyboardMoveZ += 1;
    }
    else if (button == GLFW_KEY_W && action == GLFW_RELEASE)
    {
        KeyboardMoveZ -= 1;
    }

    if (button == GLFW_KEY_S && action == GLFW_PRESS)
    {
        KeyboardMoveZ -= 1;
    }
    else if (button == GLFW_KEY_S && action == GLFW_RELEASE)
    {
        KeyboardMoveZ += 1;
    }
}



float InputManager::newposx, InputManager::newposy, InputManager::oldposx, InputManager::oldposy;

void InputManager::UpdateMousePositionInput(GLFWwindow* window, double posx, double posy)
{
    newposx = posx;
    newposy = posy;
    
    //glfwSetCursorPos(window, 0, 0);
}



void InputManager::UpdateMouseButtonInput(GLFWwindow* window, int button, int action, int mods)
{

}



float InputManager::MouseScrollX = 0, InputManager::MouseScrollY = 0;

void InputManager::UpdateMouseScrollInput(GLFWwindow* window, double posx, double posy)
{
    MouseScrollX = posx;
    MouseScrollY = posy;
}



float InputManager::MousePositionX = 0, InputManager::MousePositionY = 0;
double InputManager::time = 0, InputManager::DeltaTime = 0;

void InputManager::Reset()
{
    MousePositionX = newposx - oldposx;
    MousePositionY = -(newposy - oldposy);

    oldposx = newposx;
    oldposy = newposy;

    DeltaTime = glfwGetTime() - time;
    time = glfwGetTime();
}