#include <GLEW/glew.h>
#include <GLFW/glfw3.h>

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

#include <iostream>

#include "MainGame.h"
#include "MainInput.h"

int main(void)
{
    /* Create a window object */
    GLFWwindow* window;

    /* Initialize the GLFW library */
    if (!glfwInit())
        return -1;

    /* Change window hints to have anti-aliasing */
    glfwWindowHint(GLFW_SAMPLES, 8);

    /* Create a windowed mode window and its OpenGL context */
    window = glfwCreateWindow(1280, 720, "MinecraftClone", NULL, NULL);
    if (!window)
    {
        glfwTerminate();
        return -1;
    }

    /* Make the window's context current */
    glfwMakeContextCurrent(window);

    /* Initialize the GLEW library */
    GLenum err = glewInit();
    if (GLEW_OK != err)
    {

        return -1;
    }

    /* Enable anti-aliasing */
    glEnable(GL_MULTISAMPLE);

    /* Set input callback function */
    glfwSetKeyCallback(window, MainInput::UpdateInput);

    // Start game
    MainGame MainGameLoop = MainGame();
    MainGameLoop.StartGame();


    /* Loop until the user closes the window */
    while (!glfwWindowShouldClose(window))
    {
        // Update game
        MainGameLoop.UpdateGame();

        /* Swap front and back buffers */
        glfwSwapBuffers(window);

        /* Clear color and depth buffers */
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        /* Poll for and process events */
        glfwPollEvents();
    }

    // End game
    MainGameLoop.EndGame();

    /* Deconstruct everything and exit */
    glfwTerminate();
    return 0;
}