#pragma once

#include <vector>
#include <string>

class MeshLoader
{
public:
	static void LoadMesh(std::vector<float>*, std::vector<unsigned int>*);
	static void LoadMesh(std::vector<float>*, std::vector<unsigned int>*, std::string);
};

