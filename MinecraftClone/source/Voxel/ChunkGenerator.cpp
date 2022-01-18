#include "ChunkGenerator.h"

#include "Noise/SimplexNoise.h"
static SimplexNoise Noise = SimplexNoise();

void ChunkGenerator::GenerateChunk(Chunk* ChunkReferance)
{
	//xnoise = std::floor((SimplexNoise::noise(time / 11.12374) + 1) * 1.5f);
	//ynoise = std::floor((SimplexNoise::noise(-time / 9.76854) + 1) * 1.5f);

	float xnoise = 0, ynoise = 0;

	for (int x = 0; x < CHUNK_X_SIZE; x++)
	{
		for (int y = 0; y < CHUNK_Y_SIZE; y++)
		{
			for (int z = 0; z < CHUNK_Z_SIZE; z++)
			{
				xnoise = Noise.noise((x + ChunkReferance->PosX * CHUNK_X_SIZE) / 25.12374f, (y + ChunkReferance->PosY * CHUNK_Y_SIZE) / 25.12374f, (z + ChunkReferance->PosZ * CHUNK_Z_SIZE) / 25.12374f) * 10;
				ynoise = Noise.noise(-(x + ChunkReferance->PosX * CHUNK_X_SIZE) / 25.16854f, (y + ChunkReferance->PosY * CHUNK_Y_SIZE) / 25.16854f, (z + ChunkReferance->PosZ * CHUNK_Z_SIZE) / 25.16854f) * 10;

				if (Noise.fractal(5, (xnoise + x + ChunkReferance->PosX * CHUNK_X_SIZE) / 250.0f, (ynoise + z + ChunkReferance->PosZ * CHUNK_Z_SIZE) / 250.0f) * 32 > y + CHUNK_Y_SIZE * ChunkReferance->PosY)
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
					else if (!ChunkReferance->Blocks[Chunk::GetIndex(x, y + 3, z)] > 0 && SimplexNoise::noise(x, z) >= 0)
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
				cap = Noise.noise((x + ChunkReferance->PosX * CHUNK_X_SIZE) / 25.0f, (y + ChunkReferance->PosY * CHUNK_Y_SIZE) / 25.0f, (z + ChunkReferance->PosZ * CHUNK_Z_SIZE) / 25.0f);

				if (ChunkReferance->Blocks[Chunk::GetIndex(x, y, z)] > 0)
				{
					if (!(std::max((cap + 1) * 0.5f, std::abs(Noise.fractal(3, (x + ChunkReferance->PosX * CHUNK_X_SIZE) / 50.0f, (y + ChunkReferance->PosY * CHUNK_Y_SIZE) / 50.0f, (z + ChunkReferance->PosZ * CHUNK_Z_SIZE) / 50.0f))) > 0.3))
					{
						ChunkReferance->Blocks[Chunk::GetIndex(x, y, z)] = 0;
					}
				}
			}
		}
	}
}