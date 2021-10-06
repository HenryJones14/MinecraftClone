#include <cstdlib>

#include <GLFW/glfw3.h>
#include <OpenSimplexNoise/OpenSimplexNoise.h>

int main(void)
{
    OpenSimplexNoise::Noise noise = OpenSimplexNoise::Noise(100);
    float value = 0;
    float time = 0;
    const float TileSize = 0.0015625f;

    GLFWwindow* window;

    /* Initialize the library */
    if (!glfwInit())
        return -1;

    /* Create a windowed mode window and its OpenGL context */
    window = glfwCreateWindow(1280, 720, "Hello World", NULL, NULL);
    if (!window)
    {
        glfwTerminate();
        return -1;
    }

    /* Make the window's context current */
    glfwMakeContextCurrent(window);

    /* Loop until the user closes the window */
    while (!glfwWindowShouldClose(window))
    {
        time += 0.007f;

        /* Render here */
        glClear(GL_COLOR_BUFFER_BIT);

        /* Create a triangle with legacy OpenGL */
        glBegin(GL_TRIANGLES);
        for (float i = -1; i < 1; i += TileSize)
        {
            value = abs((float)noise.eval(time + i, 0)) * 2 - 1;

            glVertex2f(i, -1.0f);
            glVertex2f(i, value);
            glVertex2f(i + TileSize, -1.0f);

            glVertex2f(i, value);
            glVertex2f(i + TileSize, value);
            glVertex2f(i + TileSize, -1.0f);
        }
        glEnd();


        /* Swap front and back buffers */
        glfwSwapBuffers(window);

        /* Poll for and process events */
        glfwPollEvents();
    }

    glfwTerminate();
    return 0;
}