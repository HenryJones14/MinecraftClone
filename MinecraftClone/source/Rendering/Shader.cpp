#include "Shader.h"

Shader::Shader()
{
    std::string vertexCode;
    std::string fragmentCode;

    vertexCode = "#version 330 core \n layout(location = 0) in vec3 pos; \n layout(location = 1) in vec3 nor; \n layout(location = 2) in vec4 col; \n layout(location = 3) in vec2 uvs; \n uniform mat4 object; \n uniform mat4 view; \n uniform mat4 projection; \n out vec3 Normal; \n out vec4 Color; \n out vec2 UVs; \n void main() \n { \n gl_Position = projection * view * object * vec4(pos, 1.0); \n Normal = normalize((object * vec4(nor, 1.0)).xyz - vec3(object[3].x, object[3].y, object[3].z)); \n Color = clamp(col, 0, 1); \n UVs = uvs; \n }";
    fragmentCode = "#version 330 core \n out vec4 FragColor; \n in vec3 Normal; \n in vec4 Color; \n in vec2 UVs; \n uniform sampler2D texture0; \n void main() \n { \n vec4 col = texture(texture0, UVs) * Color; \n if (col.a == 0) \n { \n discard; \n } \n float lig = (dot( Normal , vec3(0, 1, 0)) + 1) * 0.5; \n FragColor = vec4(col.rgb * lig, col.a); \n }";

    InitializeShader(vertexCode.c_str(), fragmentCode.c_str());
}

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

    InitializeShader(vertexCode.c_str(), fragmentCode.c_str());
}

void Shader::InitializeShader(const char* vShaderCode, const char* fShaderCode)
{
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