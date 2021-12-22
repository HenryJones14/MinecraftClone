#include <GLEW/glew.h>
#include <GLFW/glfw3.h>

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

#include <iostream>

#include "Shader.h";
#include "NormalMesh.h";

int main(void)
{
    GLFWwindow* window;

    /* Initialize the library */
    if (!glfwInit())
        return -1;

    /* Create a windowed mode window and its OpenGL context */
    window = glfwCreateWindow(720, 720, "MinecraftClone", NULL, NULL);
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

    float time = 0;

    /* Loop until the user closes the window */
    while (!glfwWindowShouldClose(window))
    {
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        time += 0.01f;

        MainShader.ActivateShader();
        MainShader.SetMatrix4x4("transform", glm::rotate(glm::mat4(1.0f), time, glm::vec3(0, 1, 1)));
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