#include <GLEW/glew.h>
#include <GLFW/glfw3.h>

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

#include <iostream>

#include "Shader.h";
#include "NormalMesh.h";
#include "Camera.h";

void keyCallback(GLFWwindow*, int, int, int, int);

float inx = 0, iny = 0, inz = 0, inya = 0, inpi = 0;

int main(void)
{
    GLFWwindow* window;

    /* Initialize the library */
    if (!glfwInit())
        return -1;

    /* Create a windowed mode window and its OpenGL context */
    window = glfwCreateWindow(1280, 720, "MinecraftClone", NULL, NULL);
    if (!window)
    {
        glfwTerminate();
        return -1;
    }

    /* Make the window's context current */
    glfwMakeContextCurrent(window);

    GLenum err = glewInit();
    if (GLEW_OK != err)
    {
        /* Problem: glewInit failed, something is seriously wrong. */
        std::cout << "Error: " << glewGetErrorString(err) << "\n";
    }

    glfwSetKeyCallback(window, keyCallback);

    glClearColor(0.4921875f, 0.6640625f, 1, 1);
    Shader MainShader = Shader("shaders/shader.vert", "shaders/shader.frag");
    NormalMesh MainMesh = NormalMesh();
    Camera MainCamera = Camera(16.0f / 9.0f, 90);
    MainCamera.GlobalMoveCamera(1, 1, 1);

    glEnable(GL_CULL_FACE);
    glCullFace(GL_FRONT);
    glFrontFace(GL_CCW);

    glEnable(GL_DEPTH_TEST);

    /* Loop until the user closes the window */
    while (!glfwWindowShouldClose(window))
    {
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        MainCamera.LocalMoveCamera(inx * 0.01, iny * 0.01, inz * 0.01);
        MainCamera.LocalRotateCamera(inya, inpi);

        MainShader.ActivateShader();
        MainShader.SetMatrix4x4("object", glm::mat4(1.0f));
        MainShader.SetMatrix4x4("view", MainCamera.GetViewMatrix());
        MainShader.SetMatrix4x4("projection", MainCamera.GetProjectionMatrix());
        MainMesh.RenderMesh();

        /* Swap front and back buffers */
        glfwSwapBuffers(window);

        /* Poll for and process events */
        glfwPollEvents();
    }

    /* Deconstruct everything before exit */
    glfwTerminate();
    return 0;
}

void keyCallback(GLFWwindow* window, int key, int scancode, int action, int mods)
{
    // X
    if (key == GLFW_KEY_D && action == GLFW_PRESS)
    {
        inx += 1;
    }
    else if (key == GLFW_KEY_D && action == GLFW_RELEASE)
    {
        inx -= 1;
    }

    if (key == GLFW_KEY_A && action == GLFW_PRESS)
    {
        inx -= 1;
    }
    else if (key == GLFW_KEY_A && action == GLFW_RELEASE)
    {
        inx += 1;
    }

    // Y
    if (key == GLFW_KEY_SPACE && action == GLFW_PRESS)
    {
        iny += 1;
    }
    else if (key == GLFW_KEY_SPACE && action == GLFW_RELEASE)
    {
        iny -= 1;
    }

    if (key == GLFW_KEY_LEFT_SHIFT && action == GLFW_PRESS)
    {
        iny -= 1;
    }
    else if (key == GLFW_KEY_LEFT_SHIFT && action == GLFW_RELEASE)
    {
        iny += 1;
    }

    // Z
    if (key == GLFW_KEY_W && action == GLFW_PRESS)
    {
        inz += 1;
    }
    else if (key == GLFW_KEY_W && action == GLFW_RELEASE)
    {
        inz -= 1;
    }

    if (key == GLFW_KEY_S && action == GLFW_PRESS)
    {
        inz -= 1;
    }
    else if (key == GLFW_KEY_S && action == GLFW_RELEASE)
    {
        inz += 1;
    }

    // Yaw
    if (key == GLFW_KEY_RIGHT && action == GLFW_PRESS)
    {
        inya += 1;
    }
    else if (key == GLFW_KEY_RIGHT && action == GLFW_RELEASE)
    {
        inya -= 1;
    }

    if (key == GLFW_KEY_LEFT && action == GLFW_PRESS)
    {
        inya -= 1;
    }
    else if (key == GLFW_KEY_LEFT && action == GLFW_RELEASE)
    {
        inya += 1;
    }

    // Pitch
    if (key == GLFW_KEY_UP && action == GLFW_PRESS)
    {
        inpi += 1;
    }
    else if (key == GLFW_KEY_UP && action == GLFW_RELEASE)
    {
        inpi -= 1;
    }

    if (key == GLFW_KEY_DOWN && action == GLFW_PRESS)
    {
        inpi -= 1;
    }
    else if (key == GLFW_KEY_DOWN && action == GLFW_RELEASE)
    {
        inpi += 1;
    }
}