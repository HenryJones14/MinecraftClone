#include "Texture2D.h"

#include <GLEW/glew.h>
#include <GLFW/glfw3.h>

Texture2D::Texture2D()
{
	unsigned char data[4] = { (unsigned char)255, (unsigned char)255, (unsigned char)255, (unsigned char)255 };
	InitializeTexture(data, 1, 1, "textures/stone.png");
}

Texture2D::Texture2D(std::string Path)
{
	int width, height, channels;
	stbi_set_flip_vertically_on_load(true);
	unsigned char* data = stbi_load(Path.c_str(), &width, &height, &channels, 0);

	InitializeTexture(data, width, height, Path);

	stbi_image_free(data);
}

void Texture2D::InitializeTexture(unsigned char* Texture, int Width, int Height, std::string Path)
{
	glGenTextures(1, &TextureID);
	glBindTexture(GL_TEXTURE_2D, TextureID);

	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);

	if (Texture)
	{
		glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, Width, Height, 0, GL_RGBA, GL_UNSIGNED_BYTE, Texture);
		glGenerateMipmap(GL_TEXTURE_2D);
	}
	else
	{
		std::cout << "ERROR::TEXTURE_FILE_NOT_READ" << std::endl;
		std::cout << "Texture path: " << Path << std::endl;
	}
}

void Texture2D::ActivateTexture()
{
	glActiveTexture(GL_TEXTURE0);
	glBindTexture(GL_TEXTURE_2D, TextureID);
}

Texture2D::~Texture2D()
{
	glDeleteTextures(1, &TextureID);
}