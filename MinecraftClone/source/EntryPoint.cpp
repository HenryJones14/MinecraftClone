#include <GLEW/glew.h>
#include <GLFW/glfw3.h>

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

#include <iostream>

#include "InputManager.h"

#include "Rendering/Shader.h"
#include "Rendering/NormalMesh.h"
#include "Rendering/VoxelMesh.h"
#include "Rendering/Texture2D.h"

#include "Gameplay/Camera.h"

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

    /* Set input callback functions */
    glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_DISABLED);
    if (glfwRawMouseMotionSupported())
    {
        glfwSetInputMode(window, GLFW_RAW_MOUSE_MOTION, GLFW_TRUE);
    }

    glfwSetInputMode(window, GLFW_STICKY_KEYS, 1);
    glfwSetInputMode(window, GLFW_STICKY_MOUSE_BUTTONS, 1);

    glfwSetKeyCallback(window, InputManager::UpdateKeyboardButtonInput);
    glfwSetCursorPosCallback(window, InputManager::UpdateMousePositionInput);
    glfwSetMouseButtonCallback(window, InputManager::UpdateMouseButtonInput);
    glfwSetScrollCallback(window, InputManager::UpdateMouseScrollInput);

    // Start game
    glClearColor(0.4921875f, 0.6640625f, 1, 1);

    glFrontFace(GL_CW);

    #ifdef NDEBUG
        // release code
        glEnable(GL_CULL_FACE);
        glCullFace(GL_BACK);
    #else
        // debug code
        glPolygonMode(GL_BACK, GL_LINE);
        glLineWidth(2);
    #endif

    glEnable(GL_DEPTH_TEST);

    Camera MainCamera = Camera(16.0f / 9.0f, 90);

    Shader MainShader = Shader();
    NormalMesh MainMesh = NormalMesh();
    Texture2D MainTexture = Texture2D();

    Shader VoxelShader = Shader("shaders/OpaqueVoxelShader.vert", "shaders/OpaqueVoxelShader.frag");
    VoxelMesh VoxelChunkMesh = VoxelMesh();
    Texture2D VoxelTexture = Texture2D("textures/furnace.png");


    /* Loop until the user closes the window */
    while (!glfwWindowShouldClose(window))
    {
        // Update game
        MainCamera.MoveCamera(InputManager::KeyboardMoveX * InputManager::DeltaTime, InputManager::KeyboardMoveY * InputManager::DeltaTime, InputManager::KeyboardMoveZ * InputManager::DeltaTime);
        MainCamera.RotateCamera(InputManager::MousePositionX * 0.03, InputManager::MousePositionY * 0.03);

        MainShader.ActivateShader();
        MainTexture.ActivateTexture();

        MainShader.SetMatrix4x4("object", glm::mat4(1));
        MainShader.SetMatrix4x4("view", MainCamera.GetViewMatrix());
        MainShader.SetMatrix4x4("projection", MainCamera.GetProjectionMatrix());

        MainMesh.RenderMesh();


        VoxelShader.ActivateShader();
        VoxelTexture.ActivateTexture();

        VoxelShader.SetMatrix4x4("object", glm::translate(glm::mat4(1), glm::vec3(1, 1, 1)));
        VoxelShader.SetMatrix4x4("view", MainCamera.GetViewMatrix());
        VoxelShader.SetMatrix4x4("projection", MainCamera.GetProjectionMatrix());

        VoxelChunkMesh.RenderMesh();

        /* Swap front and back buffers */
        glfwSwapBuffers(window);

        /* Clear color and depth buffers */
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        /* Poll for and process events */
        glfwPollEvents();
        InputManager::Reset();
        //std::cout << (int)(1 / InputManager::DeltaTime) << std::endl;
    }

    // End game

    /* Deconstruct everything and exit */
    glfwTerminate();
    return 0;
}