#include <GLEW/glew.h>
#include <GLFW/glfw3.h>

#include <cstdlib>
#include <OpenSimplexNoise/OpenSimplexNoise.h>

float vertices[] =
{
    -0.7f, -0.7f, 0.0f,
     0.0f,  0.7f, 0.0f,
     0.7f, -0.7f, 0.0f,
};

const char* vertexShaderSource = "#version 330 core\n"
"layout (location = 0) in vec3 aPos;\n"
"void main()\n"
"{\n"
"   gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);\n"
"}\0";

const char* fragmentShaderSource = "#version 330 core\n"
"out vec4 FragColor;\n"
"void main()\n"
"{\n"
    "FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);\n"
"}\0";

int success;
unsigned int VBO;
unsigned int VAO;
unsigned int shaderProgram;

void CustomOpenGL()
{

#pragma region ShaderSetup

    // Create ID for and generate Vertex Shader (it converts vertices to Fragments for use in Fragment Shader)
    unsigned int vertexShader;
    vertexShader = glCreateShader(GL_VERTEX_SHADER);

    // Load and compile Vertex Shader
    glShaderSource(vertexShader, 1, &vertexShaderSource, NULL);
    glCompileShader(vertexShader);

    // Create ID for and generate Fragment Shader (it uses Fragments it got from Vertex Shader to draw on the screen)
    unsigned int fragmentShader;
    fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);

    // Load and compile Fragment Shader
    glShaderSource(fragmentShader, 1, &fragmentShaderSource, NULL);
    glCompileShader(fragmentShader);

    // Check for Shader errors
    glGetShaderiv(vertexShader, GL_COMPILE_STATUS, &success);
    if (!success)
    {
        success = -4;
    }

    // Create Program witch is a collection of Shaders (it will be used in Render Calls to render something)
    shaderProgram = glCreateProgram();

    // Send compiled Shaders into the Program and initialize them
    glAttachShader(shaderProgram, vertexShader);
    glAttachShader(shaderProgram, fragmentShader);
    glLinkProgram(shaderProgram);

    // Check for Program errors
    glGetProgramiv(shaderProgram, GL_LINK_STATUS, &success);
    if (!success)
    {
        success = -5;
    }

    // Delete shader Source Code after Program to save on Vram
    glDeleteShader(vertexShader);
    glDeleteShader(fragmentShader);

#pragma endregion

#pragma region RenderingSetup

    // Create ID for and generate "VertexBufferObject" (VBO) (it will be used to send Vertex data to the GPU memory)
    glGenBuffers(1, &VBO);

    // Create ID for and generate "VertexArrayObject" (VAO) (this is a renderable object with the same shaders)
    glGenVertexArrays(1, &VAO);

    // Initialize VAO for use as renderable object
    glBindVertexArray(VAO);

    // Set type of VBO to accept verticies and send that vertex data to GPU with binded FBO
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);

    // Show OpenGL how to place the vertex data into the shader
    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void*)0);
    glEnableVertexAttribArray(0);

#pragma endregion

}

int main(void)
{
    //OpenSimplexNoise::Noise noise = OpenSimplexNoise::Noise(100);
    //float newValue = (float)noise.eval(0, 0);

    GLFWwindow* window;

    /* Initialize the GLFW library */
    if (!glfwInit())
        return -1;


    /* Create a windowed mode window and its OpenGL context */
    window = glfwCreateWindow(1280, 720, "MinecraftClone", NULL, NULL);
    if (!window)
    {
        glfwTerminate();
        return -2;
    }

    /* Make the window's context current */
    glfwMakeContextCurrent(window);

    /* Initialize the GLEW library */
    if (glewInit() != GLEW_OK)
    {
        return -3;
    }

    // All of the OpenGL setup and Game close on error;
    CustomOpenGL();
    if (success < 0)
    {
        return success;
    }

    /* Loop until the user closes the window */
    while (!glfwWindowShouldClose(window))
    {
        /* Render here */
        glClear(GL_COLOR_BUFFER_BIT);

        // render Triangle
        glUseProgram(shaderProgram);
        glBindVertexArray(VAO);
        glDrawArrays(GL_TRIANGLES, 0, 3);


        /* Swap front and back buffers */
        glfwSwapBuffers(window);

        /* Poll for and process events */
        glfwPollEvents();
    }

    glfwTerminate();
    return 0;
}