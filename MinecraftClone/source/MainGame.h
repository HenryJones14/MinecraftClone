#pragma once

#include <GLEW/glew.h>
#include <GLFW/glfw3.h>

#include "Rendering/Shader.h"
#include "Rendering/NormalMesh.h"
#include "Rendering/VoxelMesh.h"
#include "Rendering/Texture2D.h"

#include "Gameplay/Camera.h"
#include "MainInput.h"

class MainGame
{
public:
    void StartGame();
	void UpdateGame();
    void EndGame();

private:
    Camera MainCamera = Camera(16.0f / 9.0f, 90);

    Shader MainShader = Shader();
    NormalMesh MainMesh = NormalMesh();
    Texture2D MainTexture = Texture2D();

    Shader VoxelShader = Shader("shaders/OpaqueVoxelShader.vert", "shaders/OpaqueVoxelShader.frag");
    VoxelMesh VoxelChunkMesh = VoxelMesh();
    Texture2D VoxelTexture = Texture2D("textures/furnace.png");
};