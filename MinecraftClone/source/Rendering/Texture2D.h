#pragma once

#include <STB/stb_image.h>
#include <iostream>

class Texture2D
{
public:
	Texture2D();
	Texture2D(std::string);
	~Texture2D();

	void ActivateTexture();

private:
	unsigned int TextureID;
	void InitializeTexture(unsigned char*, int, int, std::string);
};

