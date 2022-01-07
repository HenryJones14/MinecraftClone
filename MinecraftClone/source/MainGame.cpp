#include "MainGame.h"

// Game start
void MainGame::StartGame() 
{
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
}

// Game update
void MainGame::UpdateGame()
{
    MainCamera.LocalMoveCamera(MainInput::MoveX * 0.01f, MainInput::MoveY * 0.01f, MainInput::MoveZ * 0.01f);
    MainCamera.LocalRotateCamera(MainInput::LookX, MainInput::LookY);

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
}

// Game end
void MainGame::EndGame()
{

}