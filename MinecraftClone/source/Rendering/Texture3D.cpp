#include "Texture3D.h"

#include <GLEW/glew.h>
#include <GLFW/glfw3.h>

Texture3D::Texture3D()
{
	InitializeTexture(1, 1, 1);
	unsigned char data[4] = { (unsigned char)255, (unsigned char)255, (unsigned char)255, (unsigned char)255 };
	glTexSubImage3D(GL_TEXTURE_2D_ARRAY, 0, 0, 0, 0, 1, 1, 1, GL_RGBA, GL_UNSIGNED_BYTE, data);
	glGenerateMipmap(GL_TEXTURE_2D_ARRAY);
}

Texture3D::Texture3D(int Width, int Height, std::vector<std::string> Paths)
{
	InitializeTexture(Paths.size(), Width, Height);
	for (int i = 0; i < Paths.size(); i++)
	{
		int width, height, channels;
		stbi_set_flip_vertically_on_load(true);
		unsigned char* data = stbi_load(Paths[i].c_str(), &width, &height, &channels, 0);

		glTexSubImage3D(GL_TEXTURE_2D_ARRAY, 0, 0, 0, i, Width, Height, 1, GL_RGBA, GL_UNSIGNED_BYTE, data);
		glGenerateMipmap(GL_TEXTURE_2D_ARRAY);

		stbi_image_free(data);
	}
}

void Texture3D::InitializeTexture(int TextureCount, int Width, int Height)
{
	glGenTextures(1, &TextureID);
	glBindTexture(GL_TEXTURE_2D_ARRAY, TextureID);

	glTexParameteri(GL_TEXTURE_2D_ARRAY, GL_TEXTURE_WRAP_S, GL_CLAMP);
	glTexParameteri(GL_TEXTURE_2D_ARRAY, GL_TEXTURE_WRAP_T, GL_CLAMP);
	glTexParameteri(GL_TEXTURE_2D_ARRAY, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
	glTexParameteri(GL_TEXTURE_2D_ARRAY, GL_TEXTURE_MAG_FILTER, GL_NEAREST);

	glTexImage3D(GL_TEXTURE_2D_ARRAY, 0, GL_RGBA, Width, Height, TextureCount, 0, GL_RGBA, GL_UNSIGNED_BYTE, nullptr);
}

void Texture3D::ActivateTexture()
{
	glActiveTexture(GL_TEXTURE0);
	glBindTexture(GL_TEXTURE_2D_ARRAY, TextureID);
}

Texture3D::~Texture3D()
{
	glDeleteTextures(1, &TextureID);
}