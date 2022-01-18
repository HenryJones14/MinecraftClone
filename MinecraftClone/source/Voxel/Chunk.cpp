#include "Chunk.h"
#include "Voxel/ChunkGenerator.h"
#include "Voxel/MeshGenerator.h"

#include <thread>

/*static unsigned char PackByte(bool* Information)
{
    char Byte = 0;
    for (int i = 0; i < 8; i++)
    {
        if (Information[i])
        {
            Byte |= 1 << i;
        }
    }
    return Byte;
}

static bool* UnpackByte(unsigned char Information)
{

}*/

Chunk::Chunk(int x, int y, int z)
{
	Blocks = new unsigned int[CHUNK_TOTAL_SIZE]{};
    PosX = x; PosY = y; PosZ = z;
    GenerationFinished = false;
    ReadyToRender = false;

    std::thread GenerationThread = std::thread(Chunk::Generate, this);
    GenerationThread.detach();
}

void Chunk::Generate(Chunk* Referance)
{
    // generate chunk
    ChunkGenerator::GenerateChunk(Referance);

    // generate mesh
    MeshGenerator::GenerateMesh(Referance);

    // mark finish
    Referance->GenerationFinished = true;
}

void Chunk::Render()
{
    if (GenerationFinished && ReadyToRender)
    {
        OpaqueMesh.RenderMesh();
    }
    else if (GenerationFinished)
    {
        OpaqueMesh.CreateMesh();
        ReadyToRender = true;
    }
}

Chunk::~Chunk()
{
	delete Blocks;
    if (GenerationFinished && ReadyToRender)
    {
        OpaqueMesh.DestroyMesh();
    }
}

int Chunk::GetIndex(int x, int y, int z)
{
    if (x < 0 || y < 0 || z < 0 || x >= CHUNK_X_SIZE || y >= CHUNK_Y_SIZE || z >= CHUNK_Z_SIZE)
    {
        return -1;
    }
	return (z * CHUNK_X_SIZE * CHUNK_Y_SIZE) + (y * CHUNK_X_SIZE) + x;
}