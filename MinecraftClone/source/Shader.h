#pragma once

#include <GLEW/glew.h>
#include <GLFW/glfw3.h>

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

#include <string>
#include <fstream>
#include <sstream>
#include <iostream>

class Shader
{
public:
	Shader(const char*, const char*);
	~Shader();

	void ActivateShader();

	void SetBool(const std::string&, bool);
	void SetFloat(const std::string&, float);
	void SetInt(const std::string&, int);

	void SetMatrix4x4(const std::string&, glm::mat4);

private:
	unsigned int ShaderID;
	void checkCompileErrors(unsigned int, std::string);
};

