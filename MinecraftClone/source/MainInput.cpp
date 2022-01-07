#include "MainInput.h"

float MainInput::MoveX = 0, MainInput::MoveY = 0, MainInput::MoveZ = 0, MainInput::LookX = 0, MainInput::LookY = 0;

void MainInput::UpdateInput(GLFWwindow* window, int key, int scancode, int action, int mods)
{
    // X
    if (key == GLFW_KEY_D && action == GLFW_PRESS)
    {
        MoveX += 1;
    }
    else if (key == GLFW_KEY_D && action == GLFW_RELEASE)
    {
        MoveX -= 1;
    }

    if (key == GLFW_KEY_A && action == GLFW_PRESS)
    {
        MoveX -= 1;
    }
    else if (key == GLFW_KEY_A && action == GLFW_RELEASE)
    {
        MoveX += 1;
    }

    // Y
    if (key == GLFW_KEY_SPACE && action == GLFW_PRESS)
    {
        MoveY += 1;
    }
    else if (key == GLFW_KEY_SPACE && action == GLFW_RELEASE)
    {
        MoveY -= 1;
    }

    if (key == GLFW_KEY_LEFT_SHIFT && action == GLFW_PRESS)
    {
        MoveY -= 1;
    }
    else if (key == GLFW_KEY_LEFT_SHIFT && action == GLFW_RELEASE)
    {
        MoveY += 1;
    }

    // Z
    if (key == GLFW_KEY_W && action == GLFW_PRESS)
    {
        MoveZ += 1;
    }
    else if (key == GLFW_KEY_W && action == GLFW_RELEASE)
    {
        MoveZ -= 1;
    }

    if (key == GLFW_KEY_S && action == GLFW_PRESS)
    {
        MoveZ -= 1;
    }
    else if (key == GLFW_KEY_S && action == GLFW_RELEASE)
    {
        MoveZ += 1;
    }

    // Yaw
    if (key == GLFW_KEY_RIGHT && action == GLFW_PRESS)
    {
        LookX += 1;
    }
    else if (key == GLFW_KEY_RIGHT && action == GLFW_RELEASE)
    {
        LookX -= 1;
    }

    if (key == GLFW_KEY_LEFT && action == GLFW_PRESS)
    {
        LookX -= 1;
    }
    else if (key == GLFW_KEY_LEFT && action == GLFW_RELEASE)
    {
        LookX += 1;
    }

    // Pitch
    if (key == GLFW_KEY_UP && action == GLFW_PRESS)
    {
        LookY += 1;
    }
    else if (key == GLFW_KEY_UP && action == GLFW_RELEASE)
    {
        LookY -= 1;
    }

    if (key == GLFW_KEY_DOWN && action == GLFW_PRESS)
    {
        LookY -= 1;
    }
    else if (key == GLFW_KEY_DOWN && action == GLFW_RELEASE)
    {
        LookY += 1;
    }
}