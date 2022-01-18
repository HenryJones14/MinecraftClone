#pragma once

#include <STB/stb_image.h>
#include <iostream>
#include <vector>

class Texture3D
{
public:
	Texture3D();
	Texture3D(int, int, std::vector<std::string>);
	~Texture3D();

	void ActivateTexture();

private:
	unsigned int TextureID;
	void InitializeTexture(int, int, int);
};

