#include "ChunkGenerator.h"

#include "Noise/RandomNoise.h"
static RandomNoise RandomNoiseInstance = RandomNoise();

#include "Noise/SimplexNoise.h"
static SimplexNoise SimplexNoiseInstance = SimplexNoise();

#include "Noise/CellularNoise.h"
static CellularNoise CellularNoiseInstance = CellularNoise();

void ChunkGenerator::GenerateChunk(Chunk* ChunkReferance)
{
	// std::round(SimplexNoise::noise((x + ChunkReferance->PosX * CHUNK_X_SIZE) / 111.2374f, (z + ChunkReferance->PosZ * CHUNK_Z_SIZE) / 111.2374f)) + 1;
	// std::round(SimplexNoise::noise(-(x + ChunkReferance->PosX * CHUNK_X_SIZE) / 97.6854f, -(z + ChunkReferance->PosZ * CHUNK_Z_SIZE) / 97.6854f)) + 1;
	
	float noise = 0, xnoise = 0, ynoise = 0, rivers = 0;

	for (int x = 0; x < CHUNK_X_SIZE; x++)
	{
		for (int y = 0; y < CHUNK_Y_SIZE; y++)
		{
			for (int z = 0; z < CHUNK_Z_SIZE; z++)
			{
				xnoise = SimplexNoiseInstance.noise((x + ChunkReferance->PosX * CHUNK_X_SIZE) / 25.12374f, (y + ChunkReferance->PosY * CHUNK_Y_SIZE) / 25.12374f, (z + ChunkReferance->PosZ * CHUNK_Z_SIZE) / 25.12374f) * 10;
				ynoise = SimplexNoiseInstance.noise(-(x + ChunkReferance->PosX * CHUNK_X_SIZE) / 25.16854f, (y + ChunkReferance->PosY * CHUNK_Y_SIZE) / 25.16854f, -(z + ChunkReferance->PosZ * CHUNK_Z_SIZE) / 25.16854f) * 10;

				noise = (SimplexNoiseInstance.fractal(4, (xnoise + x + ChunkReferance->PosX * CHUNK_X_SIZE) / 250.0f, (ynoise + z + ChunkReferance->PosZ * CHUNK_Z_SIZE) / 250.0f));

				if (noise * 32 > y + CHUNK_Y_SIZE * ChunkReferance->PosY)
				{
					ChunkReferance->Blocks[Chunk::GetIndex(x, y, z)] = 1;
				}
			}
		}
	}
	
	for (int x = 0; x < CHUNK_X_SIZE; x++)
	{
		for (int y = 0; y < CHUNK_Y_SIZE; y++)
		{
			for (int z = 0; z < CHUNK_Z_SIZE; z++)
			{
				if (ChunkReferance->Blocks[Chunk::GetIndex(x, y, z)] > 0)
				{
					if (!ChunkReferance->Blocks[Chunk::GetIndex(x, y + 1, z)] > 0)
					{
						ChunkReferance->Blocks[Chunk::GetIndex(x, y, z)] = 2;
					}
					else if (!ChunkReferance->Blocks[Chunk::GetIndex(x, y + 2, z)] > 0)
					{
						ChunkReferance->Blocks[Chunk::GetIndex(x, y, z)] = 3;
					}
					else if (!ChunkReferance->Blocks[Chunk::GetIndex(x, y + 3, z)] > 0 && RandomNoise::noise(x, z) >= 0)
					{
						ChunkReferance->Blocks[Chunk::GetIndex(x, y, z)] = 3;
					}
					else
					{
						ChunkReferance->Blocks[Chunk::GetIndex(x, y, z)] = 4;
					}
				}
			}
		}
	}

	float cap = 0;

	for (int x = 0; x < CHUNK_X_SIZE; x++)
	{
		for (int y = 0; y < CHUNK_Y_SIZE; y++)
		{
			for (int z = 0; z < CHUNK_Z_SIZE; z++)
			{
				cap = SimplexNoiseInstance.noise((x + ChunkReferance->PosX * CHUNK_X_SIZE) / 25.0f, (y + ChunkReferance->PosY * CHUNK_Y_SIZE) / 25.0f, (z + ChunkReferance->PosZ * CHUNK_Z_SIZE) / 25.0f);

				if (ChunkReferance->Blocks[Chunk::GetIndex(x, y, z)] > 0)
				{
					if (!(std::max((cap + 1) * 0.5f, std::abs(SimplexNoiseInstance.fractal(3, (x + ChunkReferance->PosX * CHUNK_X_SIZE) / 50.0f, (y + ChunkReferance->PosY * CHUNK_Y_SIZE) / 50.0f, (z + ChunkReferance->PosZ * CHUNK_Z_SIZE) / 50.0f))) > 0.3))
					{
						ChunkReferance->Blocks[Chunk::GetIndex(x, y, z)] = 0;
					}
				}
			}
		}
	}
}