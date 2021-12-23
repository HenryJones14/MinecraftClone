#include <GLEW/glew.h>
#include <GLFW/glfw3.h>

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

#include <iostream>

#include "Shader.h";
#include "NormalMesh.h";
#include "Camera.h";

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

    glClearColor(0.4921875f, 0.6640625f, 1, 1);
    Shader MainShader = Shader("shaders/shader.vert", "shaders/shader.frag");
    NormalMesh MainMesh = NormalMesh();
    Camera MainCamera = Camera();
    MainCamera.MoveCamera(-2, 0, 0);

    float time = 0;

    glEnable(GL_CULL_FACE);
    glCullFace(GL_FRONT);
    glFrontFace(GL_CW);

    glEnable(GL_DEPTH_TEST);

    /* Loop until the user closes the window */
    while (!glfwWindowShouldClose(window))
    {
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        time += 0.01f;

        MainCamera.MoveCamera(0, 0, -0.03f);
        MainCamera.RotateCamera(10, 0);

        MainShader.ActivateShader();
        MainShader.SetMatrix4x4("projection", MainCamera.GetProjectionMatrix());
        MainShader.SetMatrix4x4("view", MainCamera.GetViewMatrix());
        MainShader.SetMatrix4x4("object", glm::mat4(1.0f));
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