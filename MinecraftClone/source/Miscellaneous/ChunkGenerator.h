#pragma once
#include <vector>

class ChunkGenerator
{
public:
	void GenerateChunk(std::vector<float>*, std::vector<unsigned int>*);

private:
	void CreateBlock(float, float, float, std::vector<float>*, std::vector<unsigned int>*, bool*);
	unsigned int offset = 0;
};

