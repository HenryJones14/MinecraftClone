#include "MeshLoader.h"

#include <fstream>
#include <sstream>
#include <iostream>

void MeshLoader::LoadMesh(std::vector<float>* VertexList, std::vector<unsigned int>* IndexList)
{
    VertexList->clear();
    VertexList->insert(VertexList->end(),
    {
            1.0f,0.0f,0.0f, // POS
1.0f,0.0f,0.0f, // NOR
1.0f,0.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.8f,0.1f,0.0f, // POS
0.2094473f,0.977421f,0.02792628f, // NOR
1.0f,0.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.8f,0.0f,-0.1f, // POS
0.2094473f,0.02792628f,-0.977421f, // NOR
1.0f,0.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.8f,-0.1f,0.0f, // POS
0.2094473f,-0.977421f,-0.02792628f, // NOR
1.0f,0.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.8f,0.0f,0.1f, // POS
0.2094473f,-0.02792628f,0.977421f, // NOR
1.0f,0.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.82f,0.05f,0.0f, // POS
-0.05708536f,0.998262f,-0.01463726f, // NOR
1.0f,0.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.82f,0.0f,-0.05f, // POS
-0.05420218f,-0.03613477f,-0.9978759f, // NOR
1.0f,0.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.82f,-0.05f,0.0f, // POS
-0.05423571f,-0.9984933f,-0.008343983f, // NOR
1.0f,0.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.82f,0.0f,0.05f, // POS
-0.05708537f,0.01463725f,0.998262f, // NOR
1.0f,0.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.0f,1.0f,0.0f, // POS
0.0f,1.0f,0.0f, // NOR
0.0f,1.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.0f,0.8f,-0.1f, // POS
-0.02792628f,0.2094473f,-0.977421f, // NOR
0.0f,1.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.1f,0.8f,0.0f, // POS
0.977421f,0.2094473f,-0.02792628f, // NOR
0.0f,1.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.0f,0.8f,0.1f, // POS
0.02792628f,0.2094473f,0.977421f, // NOR
0.0f,1.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS

-0.1f,0.8f,0.0f, // POS
-0.977421f,0.2094473f,0.02792628f, // NOR
0.0f,1.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.0f,0.82f,-0.05f, // POS
-0.008343977f,-0.05423571f,-0.9984933f, // NOR
0.0f,1.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.05f,0.82f,0.0f, // POS
0.998262f,-0.05708537f,0.01463725f, // NOR
0.0f,1.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.0f,0.82f,0.05f, // POS
-0.01463725f,-0.05708537f,0.998262f, // NOR
0.0f,1.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS

-0.05f,0.82f,0.0f, // POS
-0.9978759f,-0.05420217f,-0.03613477f, // NOR
0.0f,1.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.0f,0.0f,1.0f, // POS
0.0f,0.0f,1.0f, // NOR
0.0f,0.0f,1.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.0f,0.1f,0.8f, // POS
-0.02792628f,0.977421f,0.2094473f, // NOR
0.0f,0.0f,1.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.1f,0.0f,0.8f, // POS
0.977421f,0.02792628f,0.2094473f, // NOR
0.0f,0.0f,1.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.0f,-0.1f,0.8f, // POS
0.02792628f,-0.977421f,0.2094473f, // NOR
0.0f,0.0f,1.0f,1.0f, // COL
0.0f,0.0f, // UVS

-0.1f,0.0f,0.8f, // POS
-0.977421f,-0.02792628f,0.2094473f, // NOR
0.0f,0.0f,1.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.0f,0.05f,0.82f, // POS
0.01463726f,0.998262f,-0.05708537f, // NOR
0.0f,0.0f,1.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.05f,0.0f,0.82f, // POS
0.998262f,-0.01463726f,-0.05708537f, // NOR
0.0f,0.0f,1.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.0f,-0.05f,0.82f, // POS
-0.03613477f,-0.9978759f,-0.05420218f, // NOR
0.0f,0.0f,1.0f,1.0f, // COL
0.0f,0.0f, // UVS

-0.05f,0.0f,0.82f, // POS
-0.9984933f,-0.008343977f,-0.0542357f, // NOR
0.0f,0.0f,1.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.04f,0.04f,0.04f, // POS
0.5773503f,0.5773503f,0.5773503f, // NOR
0.0f,0.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.04f,0.04f,-0.04f, // POS
0.4016652f,0.4016652f,-0.8230006f, // NOR
0.0f,0.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS

-0.04f,0.04f,0.04f, // POS
-0.8230006f,0.4016652f,0.4016652f, // NOR
0.0f,0.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS

0.04f,-0.04f,0.04f, // POS
0.4016652f,-0.8230006f,0.4016652f, // NOR
0.0f,0.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS

-0.04f,-0.04f,-0.04f, // POS
-0.5773502f,-0.5773502f,-0.5773502f, // NOR
0.0f,0.0f,0.0f,1.0f, // COL
0.0f,0.0f, // UVS
    });

    IndexList->clear();
    IndexList->insert(IndexList->end(),
    {
        // X tip
        0, 2, 1,
        0, 3, 2,
        0, 4, 3,
        0, 1, 4,

        // X base
         1,  6,  5,
         1,  2,  6,
         2,  7,  6,
         2,  3,  7,
         3,  8,  7,
         3,  4,  8,
         4,  5,  8,
         4,  1,  5,

        // X rod
         5,  6, 28,
        28,  6, 31,
         6,  7, 31,
        31,  7, 30,
         7,  8, 30,
        30,  8, 27,
         8,  5, 27,
        27,  5, 28,



        // Y tip
         9, 11, 10,
         9, 12, 11,
         9, 13, 12,
         9, 10, 13,
        
        // Y base
        10, 15, 14,
        10, 11, 15,
        11, 16, 15,
        11, 12, 16,
        12, 17, 16,
        12, 13, 17,
        13, 14, 17,
        13, 10, 14,

        // Y rod
        14, 15, 28,
        28, 15, 27,
        15, 16, 27,
        27, 16, 29,
        16, 17, 29,
        29, 17, 31,
        17, 14, 31,
        31, 14, 28,



        // Z tip
        18, 20, 19,
        18, 21, 20,
        18, 22, 21,
        18, 19, 22,

        // Z base
        19, 24, 23,
        19, 20, 24,
        20, 25, 24,
        20, 21, 25,
        21, 26, 25,
        21, 22, 26,
        22, 23, 26,
        22, 19, 23,

        // Z rod
        23, 24, 27,
        27, 24, 30,
        24, 25, 30,
        30, 25, 31,
        25, 26, 31,
        31, 26, 29,
        26, 23, 29,
        29, 23, 27,

    });
}

void MeshLoader::LoadMesh(std::vector<float>* VertexList, std::vector<unsigned int>* IndexList,std::string Path)
{
    VertexList->clear();
    IndexList->clear();

    // open file
    std::ifstream file;
    file.open(Path);

    // read file
    std::stringstream vShaderStream;
    vShaderStream << file.rdbuf();

    // save file
    std::string output;
    output = vShaderStream.str();

    // close file
    file.close();

    int start = output.find("Vertices");

    int front = 0;
    int back = 0;

    int sep1 = 0;
    int sep2 = 0;
    int sep3 = 0;

    while (true)
    {
        // Position
        start = output.find("\"Position\"", start);

        if (start < 0)
        {
            break;
        }
        else
        {
            start += 10;
        }

        front = output.find("\"", start) + 1;
        back = output.find("\"", front);

        sep1 = output.find(":", front) + 1;
        sep2 = output.find(":", sep1) + 1;

        VertexList->push_back((float)atof(output.substr(front, sep1 - front - 1).c_str()));
        VertexList->push_back((float)atof(output.substr(sep1, sep2 - sep1 - 1).c_str()));
        VertexList->push_back((float)atof(output.substr(sep2, back - sep2).c_str()));

        // Normal
        start = output.find("\"Normal\"", start);

        if (start < 0)
        {
            break;
        }
        else
        {
            start += 8;
        }

        front = output.find("\"", start) + 1;
        back = output.find("\"", front);

        sep1 = output.find(":", front) + 1;
        sep2 = output.find(":", sep1) + 1;

        VertexList->push_back((float)atof(output.substr(front, sep1 - front - 1).c_str()));
        VertexList->push_back((float)atof(output.substr(sep1, sep2 - sep1 - 1).c_str()));
        VertexList->push_back((float)atof(output.substr(sep2, back - sep2).c_str()));

        // Color
        start = output.find("\"Color\"", start);

        if (start < 0)
        {
            break;
        }
        else
        {
            start += 7;
        }

        front = output.find("\"", start) + 1;
        back = output.find("\"", front);

        sep1 = output.find(":", front) + 1;
        sep2 = output.find(":", sep1) + 1;
        sep3 = output.find(":", sep2) + 1;

        VertexList->push_back((float)atof(output.substr(front, sep1 - front - 1).c_str()));
        VertexList->push_back((float)atof(output.substr(sep1, sep2 - sep1 - 1).c_str()));
        VertexList->push_back((float)atof(output.substr(sep2, sep3 - sep2 - 1).c_str()));
        VertexList->push_back((float)atof(output.substr(sep3, back - sep3).c_str()));

        // UV
        start = output.find("\"UV\"", start);

        if (start < 0)
        {
            break;
        }
        else
        {
            start += 4;
        }

        front = output.find("\"", start) + 1;
        back = output.find("\"", front);

        sep1 = output.find(":", front) + 1;

        VertexList->push_back((float)atof(output.substr(front, sep1 - front - 1).c_str()));
        VertexList->push_back((float)atof(output.substr(sep1, back - sep1).c_str()));
    }

    start = output.find("Triangles");

    front = 0;
    back = 0;

    while (true)
    {
        // Indices
        start = output.find("\"Indices\"", start);

        if (start < 0)
        {
            break;
        }
        else
        {
            start += 10;
        }

        front = output.find("\"", start) + 1;
        back = output.find("\"", front);

        sep1 = output.find(":", front) + 1;
        sep2 = output.find(":", sep1) + 1;

        IndexList->push_back(atoi(output.substr(front, sep1 - front - 1).c_str()));
        IndexList->push_back(atoi(output.substr(sep1, sep2 - sep1 - 1).c_str()));
        IndexList->push_back(atoi(output.substr(sep2, back - sep2).c_str()));
    }
}