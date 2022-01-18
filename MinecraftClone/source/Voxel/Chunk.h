#pragma once

#define CHUNK_TOTAL_SIZE 32768
#define CHUNK_X_SIZE 16
#define CHUNK_Y_SIZE 128
#define CHUNK_Z_SIZE 16

#include "Rendering/Voxel/OpaqueVoxelMesh.h"
#include "BlockList.h"

class Chunk
{
public:
	Chunk(int, int, int);
	Chunk(unsigned int*, int, int);
	~Chunk();

	unsigned int* Blocks;
	int PosX, PosY, PosZ;

	OpaqueVoxelMesh OpaqueMesh;

	void Render();

	static int GetIndex(int, int, int);
	//static unsigned char PackByte(bool*);
	//static bool* UnpackByte(unsigned char);
};