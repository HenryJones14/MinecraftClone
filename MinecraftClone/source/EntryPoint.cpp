#include <GLEW/glew.h>
#include <GLFW/glfw3.h>

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

#include <iostream>
#include <vector>
#include <string>

#include "InputManager.h"

#include "Rendering/Shader.h"
#include "Rendering/NormalMesh.h"
#include "Rendering/Texture2D.h"
#include "Rendering/Texture3D.h"

#include "Gameplay/Camera.h"
#include "Voxel/Chunk.h"

#include "Noise/SimplexNoise.h"
#include "Voxel/BlockList.h"

#define SizeX 5
#define SizeY 2
#define SizeZ 5

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

    for (size_t i = 0; i < 256; i++)
    {
        std::cout << i << ": <" << char(i) << ">" << std::endl;
    }

    Camera MainCamera = Camera(16.0f / 9.0f, 90);

    Shader MainShader = Shader();
    NormalMesh MainMesh = NormalMesh();
    Texture2D MainTexture = Texture2D();

    Shader VoxelShader = Shader("shaders/OpaqueVoxelShader.vert", "shaders/OpaqueVoxelShader.frag");
    std::vector<Chunk*> VoxelChunks;
    Texture3D VoxelTexture = Texture3D(32, 32, BlockList::Textures);

    int x = -SizeX, y = -SizeY, z = -SizeZ;
    bool newchunk;

    /* Loop until the user closes the window */
    while (!glfwWindowShouldClose(window))
    {
        newchunk = false;

        if (x < SizeX - 1)
        {
            x++;
            newchunk = true;
        }
        else if (y < SizeY - 1)
        {
            y++;
            x = -SizeX;
            newchunk = true;
        }
        else if (z < SizeZ - 1)
        {
            z++;
            x = -SizeX;
            y = -SizeY;
            newchunk = true;
        }

        if (newchunk)
        {
            VoxelChunks.push_back(new Chunk(x, y, z));
        }

        // Update game
        MainCamera.MoveCamera(InputManager::KeyboardMoveX * InputManager::DeltaTime * 10, InputManager::KeyboardMoveY * InputManager::DeltaTime * 10, InputManager::KeyboardMoveZ * InputManager::DeltaTime * 10);
        MainCamera.RotateCamera(InputManager::MousePositionX * 0.04, InputManager::MousePositionY * 0.04);



        MainShader.ActivateShader();
        MainTexture.ActivateTexture();

        MainShader.SetMatrix4x4("view", MainCamera.GetViewMatrix());
        MainShader.SetMatrix4x4("projection", MainCamera.GetProjectionMatrix());

        MainShader.SetMatrix4x4("object", glm::mat4(1));
        MainMesh.RenderMesh();



        VoxelShader.ActivateShader();
        VoxelTexture.ActivateTexture();

        VoxelShader.SetMatrix4x4("view", MainCamera.GetViewMatrix());
        VoxelShader.SetMatrix4x4("projection", MainCamera.GetProjectionMatrix());

        for (int i = 0; i < VoxelChunks.size(); i++)
        {
            VoxelShader.SetMatrix4x4("object", glm::translate(glm::mat4(1), glm::vec3(VoxelChunks[i]->PosX * CHUNK_X_SIZE, VoxelChunks[i]->PosY * CHUNK_Y_SIZE, VoxelChunks[i]->PosZ * CHUNK_Z_SIZE)));
            VoxelChunks[i]->Render();
        }

        //std::cout << (int)(1 / InputManager::DeltaTime) << std::endl;

        /* Swap front and back buffers */
        glfwSwapBuffers(window);

        /* Clear color and depth buffers */
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        /* Poll for and process events */
        glfwPollEvents();
        InputManager::Reset();

        /* Close window if "Escape" is pressed*/
        if (glfwGetKey(window, GLFW_KEY_ESCAPE))
        {
            glfwSetWindowShouldClose(window, GLFW_TRUE);
        }
    }

    // End game
    for (int i = 0; i < VoxelChunks.size(); i++)
    {
        delete VoxelChunks[i];
    }

    /* Deconstruct everything and exit */
    glfwTerminate();
    return 0;
}