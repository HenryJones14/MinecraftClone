#include "Chunk.h"
#include "Voxel/ChunkGenerator.h"
#include "Voxel/MeshGenerator.h"

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

    // generate chunk
    ChunkGenerator::GenerateChunk(this);

    // generate mesh
    MeshGenerator::GenerateMesh(this);

    OpaqueMesh.CreateMesh();
}

void Chunk::Render()
{
    OpaqueMesh.RenderMesh();
}

Chunk::~Chunk()
{
	delete Blocks;
    OpaqueMesh.DestroyMesh();
}

int Chunk::GetIndex(int x, int y, int z)
{
    if (x < 0 || y < 0 || z < 0 || x >= CHUNK_X_SIZE || y >= CHUNK_Y_SIZE || z >= CHUNK_Z_SIZE)
    {
        return -1;
    }
	return (z * CHUNK_X_SIZE * CHUNK_Y_SIZE) + (y * CHUNK_X_SIZE) + x;
}