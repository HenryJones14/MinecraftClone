#include "Shader.h"

Shader::Shader(const char* VertexShaderPath, const char* FragmentShaderPath)
{
    // 1. retrieve the vertex/fragment source code from filePath
    std::string vertexCode;
    std::string fragmentCode;
    std::ifstream vShaderFile;
    std::ifstream fShaderFile;

    // ensure ifstream objects can throw exceptions:
    vShaderFile.exceptions(std::ifstream::failbit | std::ifstream::badbit);
    fShaderFile.exceptions(std::ifstream::failbit | std::ifstream::badbit);
    try
    {
        // open files
        vShaderFile.open(VertexShaderPath);
        fShaderFile.open(FragmentShaderPath);
        std::stringstream vShaderStream, fShaderStream;

        // read file's buffer contents into streams
        vShaderStream << vShaderFile.rdbuf();
        fShaderStream << fShaderFile.rdbuf();

        // close file handlers
        vShaderFile.close();
        fShaderFile.close();

        // convert stream into string
        vertexCode = vShaderStream.str();
        fragmentCode = fShaderStream.str();
    }
    catch (std::ifstream::failure e)
    {
        std::cout << "ERROR::SHADER_FILE_NOT_READ::EITHER" << std::endl;
        std::cout << "Vertex shader path: " << VertexShaderPath << std::endl;
        std::cout << "Fragment shader path: " << FragmentShaderPath << std::endl;
    }
    const char* vShaderCode = vertexCode.c_str();
    const char* fShaderCode = fragmentCode.c_str();



    // 2. compile shaders
    unsigned int vertex, fragment;

    // vertex Shader
    vertex = glCreateShader(GL_VERTEX_SHADER);
    glShaderSource(vertex, 1, &vShaderCode, NULL);
    glCompileShader(vertex);

    checkCompileErrors(vertex, "VERTEX");

    // fragment Shader
    fragment = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fragment, 1, &fShaderCode, NULL);
    glCompileShader(fragment);

    checkCompileErrors(fragment, "FRAGMENT");
    
    // shader Program
    ShaderID = glCreateProgram();
    glAttachShader(ShaderID, vertex);
    glAttachShader(ShaderID, fragment);
    glLinkProgram(ShaderID);

    checkCompileErrors(ShaderID, "PROGRAM");

    // delete the shaders as they're linked into our program now and no longer necessary
    glDeleteShader(vertex);
    glDeleteShader(fragment);
}

void Shader::checkCompileErrors(unsigned int shader, std::string type)
{
    int success;
    char infoLog[1024];
    if (type != "PROGRAM")
    {
        glGetShaderiv(shader, GL_COMPILE_STATUS, &success);
        if (!success)
        {
            glGetShaderInfoLog(shader, 1024, NULL, infoLog);
            std::cout << "ERROR::SHADER_COMPILATION_ERROR::" << type << "\n" << infoLog << "\n -- --------------------------------------------------- -- " << std::endl;
        }
    }
    else
    {
        glGetProgramiv(shader, GL_LINK_STATUS, &success);
        if (!success)
        {
            glGetProgramInfoLog(shader, 1024, NULL, infoLog);
            std::cout << "ERROR::PROGRAM_LINKING_ERROR::" << type << "\n" << infoLog << "\n -- --------------------------------------------------- -- " << std::endl;
        }
    }
}

void Shader::ActivateShader()
{
    glUseProgram(ShaderID);
}

void Shader::SetBool(const std::string &VariableName, bool Input)
{
    glUniform1i(glGetUniformLocation(ShaderID, VariableName.c_str()), (int)Input);
}

void Shader::SetFloat(const std::string &VariableName, float Input)
{
    glUniform1f(glGetUniformLocation(ShaderID, VariableName.c_str()), Input);
}

void Shader::SetInt(const std::string &VariableName, int Input)
{
    glUniform1i(glGetUniformLocation(ShaderID, VariableName.c_str()), Input);
}

void Shader::SetMatrix4x4(const std::string &VariableName, glm::mat4 Input)
{
    glUniformMatrix4fv(glGetUniformLocation(ShaderID, VariableName.c_str()), 1, GL_FALSE, glm::value_ptr(Input));
}

Shader::~Shader()
{
    glDeleteProgram(ShaderID);
}