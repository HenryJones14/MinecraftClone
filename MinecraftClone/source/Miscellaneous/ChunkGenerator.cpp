#include "ChunkGenerator.h"

#include <iostream>

char CreateByte(bool* Information)
{
    char Byte = 0;
    for (int i = 0; i < 8; i++)
    {
        if (Information[i])
        {
            Byte |= 1 << i;
        }
    }
    return Byte;
}

bool UnpackByte(char& Byte, int Location)
{
    return (Byte & (1 << Location)) != 0;
}

void ChunkGenerator::GenerateChunk(std::vector<float>* VertexList, std::vector<unsigned int>* IndexList)
{
    VertexList->clear();
    IndexList->clear();

    for (size_t i = 0; i < 256; i++)
    {
        std::cout << i << ": <" << char(i) << ">" << std::endl;
    }

    offset = 0;
    for (int x = 0; x < 1; x++)
    {
        for (int y = 0; y < 1; y++)
        {
            for (int z = 0; z < 1; z++)
            {
                bool sides[8] = { true, true, true, true,   true, true, true, true };
                CreateBlock((float)x, (float)y, (float)z, VertexList, IndexList, sides);
            }
        }
    }
}

void ChunkGenerator::CreateBlock(float X, float Y, float Z, std::vector<float>* VertexList, std::vector<unsigned int>* IndexList, bool* BlockBits)
{
    bool solidblock = true;

    // RightTopFront, LeftTopFront, RightBottomFront, LeftBottomFront,
    // RightTopBack, LeftTopBack, RightBottomBack, LeftBottomBack,

    // Right
    if (BlockBits[0] && BlockBits[2] && BlockBits[4] && BlockBits[6])
    {
        VertexList->insert(VertexList->end(), { 1 + X, 1 + Y, 0 + Z, 0.5, 0.5, 1,   1 + X, 0 + Y, 1 + Z, 1, 0, 1,   1 + X, 0 + Y, 0 + Z, 0.5, 0, 1,   1 + X, 1 + Y, 1 + Z, 1, 0.5, 1});
        IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
        offset += 4;
    }
    else
    {
        solidblock = false;

        if (BlockBits[0])
        {
            // Top Front
            VertexList->insert(VertexList->end(), { 1, 1, 0.5, 0.75, 0.5,   1, 0.5, 1, 1, 0.25,   1, 0.5, 0.5, 0.75, 0.25,   1, 1, 1, 1, 0.5 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (BlockBits[2])
        {
            // Bottom Front
            VertexList->insert(VertexList->end(), { 1, 0.5, 0.5, 0.75, 0.25,   1, 0, 1, 1, 0,   1, 0, 0.5, 0.75, 0,   1, 0.5, 1, 1, 0.25 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (BlockBits[4])
        {
            // Top Back
            VertexList->insert(VertexList->end(), { 1, 1, 0, 0.5, 0.5,   1, 0.5, 0.5, 0.75, 0.25,   1, 0.5, 0, 0.5, 0.25,   1, 1, 0.5, 0.75, 0.5 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (BlockBits[6])
        {
            // Bottom Back
            VertexList->insert(VertexList->end(), { 1, 0.5, 0, 0.5, 0.25,   1, 0, 0.5, 0.75, 0,   1, 0, 0, 0.5, 0,   1, 0.5, 0.5, 0.75, 0.25 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
    }

    // Left
    if (BlockBits[1] && BlockBits[3] && BlockBits[5] && BlockBits[7])
    {
        VertexList->insert(VertexList->end(), { 0 + X, 1 + Y, 1 + Z, 0.5, 0.5, 1,   0 + X, 0 + Y, 0 + Z, 1, 0, 1,   0 + X, 0 + Y, 1 + Z, 0.5, 0, 1,   0 + X, 1 + Y, 0 + Z, 1, 0.5, 1 });
        IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
        offset += 4;
    }
    else
    {
        solidblock = false;

        if (BlockBits[1])
        {
            // Top Front
            VertexList->insert(VertexList->end(), { 0, 1, 1, 0.5, 0.5,   0, 0.5, 0.5, 0.75, 0.25,   0, 0.5, 1, 0.5, 0.25,   0, 1, 0.5, 0.75, 0.5 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (BlockBits[3])
        {
            // Bottom Front
            VertexList->insert(VertexList->end(), { 0, 0.5, 1, 0.5, 0.25,   0, 0, 0.5, 0.75, 0,   0, 0, 1, 0.5, 0,   0, 0.5, 0.5, 0.75, 0.25 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (BlockBits[5])
        {
            // Top Back
            VertexList->insert(VertexList->end(), { 0, 1, 0.5, 0.75, 0.5,   0, 0.5, 0, 1, 0.25,   0, 0.5, 0.5, 0.75, 0.25,   0, 1, 0, 1, 0.5 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (BlockBits[7])
        {
            // Bottom Back
            VertexList->insert(VertexList->end(), { 0, 0.5, 0.5, 0.75, 0.25,   0, 0, 0, 1, 0,   0, 0, 0.5, 0.75, 0,   0, 0.5, 0, 1, 0.25 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
    }

    // Top
    if (BlockBits[0] && BlockBits[1] && BlockBits[4] && BlockBits[5])
    {
        VertexList->insert(VertexList->end(), { 0 + X, 1 + Y, 1 + Z, 0, 1, 0,   1 + X, 1 + Y, 0 + Z, 0.5, 0.5, 0,   0 + X, 1 + Y, 0 + Z, 0, 0.5, 0,   1 + X, 1 + Y, 1 + Z, 0.5, 1, 0 });
        IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
        offset += 4;
    }
    else
    {
        solidblock = false;

        if (BlockBits[0])
        {
            // Right Front
            VertexList->insert(VertexList->end(), { 0.5, 1, 1, 0.25, 1,   1, 1, 0.5, 0.5, 0.75,   0.5, 1, 0.5, 0.25, 0.75,   1, 1, 1, 0.5, 1 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (BlockBits[1])
        {
            // Left Front
            VertexList->insert(VertexList->end(), { 0, 1, 1, 0, 1,   0.5, 1, 0.5, 0.25, 0.75,   0, 1, 0.5, 0, 0.75,   0.5, 1, 1, 0.25, 1 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (BlockBits[4])
        {
            // Right Back
            VertexList->insert(VertexList->end(), { 0.5, 1, 0.5, 0.25, 0.75,   1, 1, 0, 0.5, 0.5,   0.5, 1, 0, 0.25, 0.5,   1, 1, 0.5, 0.5, 0.75 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (BlockBits[5])
        {
            // Left Back
            VertexList->insert(VertexList->end(), { 0, 1, 0.5, 0, 0.75,   0.5, 1, 0, 0.25, 0.5,   0, 1, 0, 0, 0.5,   0.5, 1, 0.5, 0.25, 0.75 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
    }

    // Bottom
    if (BlockBits[2] && BlockBits[3] && BlockBits[6] && BlockBits[7])
    {
        VertexList->insert(VertexList->end(), { 1 + X, 0 + Y, 1 + Z, 0, 0.5, 2,   0 + X, 0 + Y, 0 + Z, 0.5, 0, 2,   1 + X, 0 + Y, 0 + Z, 0, 0, 2,   0 + X, 0 + Y, 1 + Z, 0.5, 0.5, 2 });
        IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
        offset += 4;
    }
    else
    {
        solidblock = false;

        if (BlockBits[2])
        {
            // Right Front
            VertexList->insert(VertexList->end(), { 1, 0, 1, 0, 0.5,   0.5, 0, 0.5, 0.25, 0.25,   1, 0, 0.5, 0, 0.25,   0.5, 0, 1, 0.25, 0.5 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (BlockBits[3])
        {
            // Left Front
            VertexList->insert(VertexList->end(), { 0.5, 0, 1, 0.25, 0.5,   0, 0, 0.5, 0.5, 0.25,   0.5, 0, 0.5, 0.25, 0.25,   0, 0, 1, 0.5, 0.5 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (BlockBits[6])
        {
            // Right Back
            VertexList->insert(VertexList->end(), { 1, 0, 0.5, 0, 0.25,   0.5, 0, 0, 0.25, 0,   1, 0, 0, 0, 0,   0.5, 0, 0.5, 0.25, 0.25 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (BlockBits[7])
        {
            // Left Back
            VertexList->insert(VertexList->end(), { 0.5, 0, 0.5, 0.25, 0.25,   0, 0, 0, 0.5, 0,   0.5, 0, 0, 0.25, 0,   0, 0, 0.5, 0.5, 0.25 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
    }

    // Front
    if (BlockBits[0] && BlockBits[1] && BlockBits[2] && BlockBits[3])
    {
        VertexList->insert(VertexList->end(), { 1 + X, 1 + Y, 1 + Z, 0.5, 1, 3,   0 + X, 0 + Y, 1 + Z, 1, 0.5, 3,   1 + X, 0 + Y, 1 + Z, 0.5, 0.5, 3,   0 + X, 1 + Y, 1 + Z, 1, 1, 3 });
        IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
        offset += 4;
    }
    else
    {
        solidblock = false;

        if (BlockBits[0])
        {
            // Right Top
            VertexList->insert(VertexList->end(), { 1, 1, 1, 0.5, 1,   0.5, 0.5, 1, 0.75, 0.75,   1, 0.5, 1, 0.5, 0.75,   0.5, 1, 1, 0.75, 1 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (BlockBits[1])
        {
            // Left Top
            VertexList->insert(VertexList->end(), { 0.5, 1, 1, 0.75, 1,   0, 0.5, 1, 1, 0.75,   0.5, 0.5, 1, 0.75, 0.75,   0, 1, 1, 1, 1 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (BlockBits[2])
        {
            // Right Bottom
            VertexList->insert(VertexList->end(), { 1, 0.5, 1, 0.5, 0.75,   0.5, 0, 1, 0.75, 0.5,   1, 0, 1, 0.5, 0.5,   0.5, 0.5, 1, 0.75, 0.75 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (BlockBits[3])
        {
            // Left Bottom
            VertexList->insert(VertexList->end(), { 0.5, 0.5, 1, 0.75, 0.75,   0, 0, 1, 1, 0.5,   0.5, 0, 1, 0.75, 0.5,   0, 0.5, 1, 1, 0.75 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
    }

    // Back
    if (BlockBits[4] && BlockBits[5] && BlockBits[6] && BlockBits[7])
    {
        VertexList->insert(VertexList->end(), { 0 + X, 1 + Y, 0 + Z, 0.5, 0.5, 1,   1 + X, 0 + Y, 0 + Z, 1, 0, 1,   0 + X, 0 + Y, 0 + Z, 0.5, 0, 1,   1 + X, 1 + Y, 0 + Z, 1, 0.5, 1 });
        IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
        offset += 4;
    }
    else
    {
        solidblock = false;

        if (BlockBits[4])
        {
            // Right Top
            VertexList->insert(VertexList->end(), { 0.5, 1, 0, 0.75, 0.5,   1, 0.5, 0, 1, 0.25,   0.5, 0.5, 0, 0.75, 0.25,   1, 1, 0, 1, 0.5 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (BlockBits[5])
        {
            // Left Top
            VertexList->insert(VertexList->end(), { 0, 1, 0, 0.5, 0.5,   0.5, 0.5, 0, 0.75, 0.25,   0, 0.5, 0, 0.5, 0.25,   0.5, 1, 0, 0.75, 0.5 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (BlockBits[6])
        {
            // Right Bottom
            VertexList->insert(VertexList->end(), { 0.5, 0.5, 0, 0.75, 0.25,   1, 0, 0, 1, 0,   0.5, 0, 0, 0.75, 0,   1, 0.5, 0, 1, 0.25 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (BlockBits[7])
        {
            // Left Bottom
            VertexList->insert(VertexList->end(), { 0, 0.5, 0, 0.5, 0.25,   0.5, 0, 0, 0.75, 0,   0, 0, 0, 0.5, 0,   0.5, 0.5, 0, 0.75, 0.25 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
    }

    if (!solidblock)
    {
        // Right
        if (!BlockBits[0] && BlockBits[1])
        {
            // Top Front
            VertexList->insert(VertexList->end(), { 0.5, 1, 0.5, 0.75, 0.5,   0.5, 0.5, 1, 1, 0.25,   0.5, 0.5, 0.5, 0.75, 0.25,   0.5, 1, 1, 1, 0.5 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (!BlockBits[2] && BlockBits[3])
        {
            // Bottom Front
            VertexList->insert(VertexList->end(), { 0.5, 0.5, 0.5, 0.75, 0.25,   0.5, 0, 1, 1, 0,   0.5, 0, 0.5, 0.75, 0,   0.5, 0.5, 1, 1, 0.25 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (!BlockBits[4] && BlockBits[5])
        {
            // Top Back
            VertexList->insert(VertexList->end(), { 0.5, 1, 0, 0.5, 0.5,   0.5, 0.5, 0.5, 0.75, 0.25,   0.5, 0.5, 0, 0.5, 0.25,   0.5, 1, 0.5, 0.75, 0.5 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (!BlockBits[6] && BlockBits[7])
        {
            // Bottom Back
            VertexList->insert(VertexList->end(), { 0.5, 0.5, 0, 0.5, 0.25,   0.5, 0, 0.5, 0.75, 0,   0.5, 0, 0, 0.5, 0,   0.5, 0.5, 0.5, 0.75, 0.25 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }

        // Left
        if (!BlockBits[1] && BlockBits[0])
        {
            // Top Front
            VertexList->insert(VertexList->end(), { 0.5, 1, 1, 0.5, 0.5,   0.5, 0.5, 0.5, 0.75, 0.25,   0.5, 0.5, 1, 0.5, 0.25,   0.5, 1, 0.5, 0.75, 0.5 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (!BlockBits[3] && BlockBits[2])
        {
            // Bottom Front
            VertexList->insert(VertexList->end(), { 0.5, 0.5, 1, 0.5, 0.25,   0.5, 0, 0.5, 0.75, 0,   0.5, 0, 1, 0.5, 0,   0.5, 0.5, 0.5, 0.75, 0.25 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (!BlockBits[5] && BlockBits[4])
        {
            // Top Back
            VertexList->insert(VertexList->end(), { 0.5, 1, 0.5, 0.75, 0.5,   0.5, 0.5, 0, 1, 0.25,   0.5, 0.5, 0.5, 0.75, 0.25,   0.5, 1, 0, 1, 0.5 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (!BlockBits[7] && BlockBits[6])
        {
            // Bottom Back
            VertexList->insert(VertexList->end(), { 0.5, 0.5, 0.5, 0.75, 0.25,   0.5, 0, 0, 1, 0,   0.5, 0, 0.5, 0.75, 0,   0.5, 0.5, 0, 1, 0.25 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }

        // Top
        if (!BlockBits[0] && BlockBits[2])
        {
            // Right Front
            VertexList->insert(VertexList->end(), { 0.5, 0.5, 1, 0.25, 1,   1, 0.5, 0.5, 0.5, 0.75,   0.5, 0.5, 0.5, 0.25, 0.75,   1, 0.5, 1, 0.5, 1 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (!BlockBits[1] && BlockBits[3])
        {
            // Left Front
            VertexList->insert(VertexList->end(), { 0, 0.5, 1, 0, 1,   0.5, 0.5, 0.5, 0.25, 0.75,   0, 0.5, 0.5, 0, 0.75,   0.5, 0.5, 1, 0.25, 1 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (!BlockBits[4] && BlockBits[6])
        {
            // Right Back
            VertexList->insert(VertexList->end(), { 0.5, 0.5, 0.5, 0.25, 0.75,   1, 0.5, 0, 0.5, 0.5,   0.5, 0.5, 0, 0.25, 0.5,   1, 0.5, 0.5, 0.5, 0.75 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (!BlockBits[5] && BlockBits[7])
        {
            // Left Back
            VertexList->insert(VertexList->end(), { 0, 0.5, 0.5, 0, 0.75,   0.5, 0.5, 0, 0.25, 0.5,   0, 0.5, 0, 0, 0.5,   0.5, 0.5, 0.5, 0.25, 0.75 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }

        // Bottom
        if (!BlockBits[2] && BlockBits[0])
        {
            // Right Front
            VertexList->insert(VertexList->end(), { 1, 0.5, 1, 0, 0.5,   0.5, 0.5, 0.5, 0.25, 0.25,   1, 0.5, 0.5, 0, 0.25,   0.5, 0.5, 1, 0.25, 0.5 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (!BlockBits[3] && BlockBits[1])
        {
            // Left Front
            VertexList->insert(VertexList->end(), { 0.5, 0.5, 1, 0.25, 0.5,   0, 0.5, 0.5, 0.5, 0.25,   0.5, 0.5, 0.5, 0.25, 0.25,   0, 0.5, 1, 0.5, 0.5 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (!BlockBits[6] && BlockBits[4])
        {
            // Right Back
            VertexList->insert(VertexList->end(), { 1, 0.5, 0.5, 0, 0.25,   0.5, 0.5, 0, 0.25, 0,   1, 0.5, 0, 0, 0,   0.5, 0.5, 0.5, 0.25, 0.25 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (!BlockBits[7] && BlockBits[5])
        {
            // Left Back
            VertexList->insert(VertexList->end(), { 0.5, 0.5, 0.5, 0.25, 0.25,   0, 0.5, 0, 0.5, 0,   0.5, 0.50, 0, 0.25, 0,   0, 0.5, 0.5, 0.5, 0.25 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }

        // Front
        if (!BlockBits[0] && BlockBits[4])
        {
            // Right Top
            VertexList->insert(VertexList->end(), { 1, 1, 0.5, 0.5, 1,   0.5, 0.5, 0.5, 0.75, 0.75,   1, 0.5, 0.5, 0.5, 0.75,   0.5, 1, 0.5, 0.75, 1 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (!BlockBits[1] && BlockBits[5])
        {
            // Left Top
            VertexList->insert(VertexList->end(), { 0.5, 1, 0.5, 0.75, 1,   0, 0.5, 0.5, 1, 0.75,   0.5, 0.5, 0.5, 0.75, 0.75,   0, 1, 0.5, 1, 1 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (!BlockBits[2] && BlockBits[6])
        {
            // Right Bottom
            VertexList->insert(VertexList->end(), { 1, 0.5, 0.5, 0.5, 0.75,   0.5, 0, 0.5, 0.75, 0.5,   1, 0, 0.5, 0.5, 0.5,   0.5, 0.5, 0.5, 0.75, 0.75 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (!BlockBits[3] && BlockBits[7])
        {
            // Left Bottom
            VertexList->insert(VertexList->end(), { 0.5, 0.5, 0.5, 0.75, 0.75,   0, 0, 0.5, 1, 0.5,   0.5, 0, 0.5, 0.75, 0.5,   0, 0.5, 0.5, 1, 0.75 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }

        // Back
        if (!BlockBits[4] && BlockBits[0])
        {
            // Right Top
            VertexList->insert(VertexList->end(), { 0.5, 1, 0.5, 0.75, 0.5,   1, 0.5, 0.5, 1, 0.25,   0.5, 0.5, 0.5, 0.75, 0.25,   1, 1, 0.5, 1, 0.5 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (!BlockBits[5] && BlockBits[1])
        {
            // Left Top
            VertexList->insert(VertexList->end(), { 0, 1, 0.5, 0.5, 0.5,   0.5, 0.5, 0.5, 0.75, 0.25,   0, 0.5, 0.5, 0.5, 0.25,   0.5, 1, 0.5, 0.75, 0.5 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (!BlockBits[6] && BlockBits[2])
        {
            // Right Bottom
            VertexList->insert(VertexList->end(), { 0.5, 0.5, 0.5, 0.75, 0.25,   1, 0, 0.5, 1, 0,   0.5, 0, 0.5, 0.75, 0,   1, 0.5, 0.5, 1, 0.25 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
        if (!BlockBits[7] && BlockBits[3])
        {
            // Left Bottom
            VertexList->insert(VertexList->end(), { 0, 0.5, 0.5, 0.5, 0.25,   0.5, 0, 0.5, 0.75, 0,   0, 0, 0.5, 0.5, 0,   0.5, 0.5, 0.5, 0.75, 0.25 });
            IndexList->insert(IndexList->end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
            offset += 4;
        }
    }
}