#include "MeshGenerator.h"

#include <iostream>
/*
static void CreateBlock(float X, float Y, float Z, std::vector<float>* VertexList, std::vector<unsigned int>* IndexList, bool* BlockBits)
{
    bool solidblock = true;

    // RightTopFront, LeftTopFront, RightBottomFront, LeftBottomFront,
    // RightTopBack, LeftTopBack, RightBottomBack, LeftBottomBack,

    // Right
    if (BlockBits[0] && BlockBits[2] && BlockBits[4] && BlockBits[6])
    {
        VertexList->insert(VertexList->end(), { 1 + X, 1 + Y, 0 + Z, 0.5, 0.5, 1,   1 + X, 0 + Y, 1 + Z, 1, 0, 1,   1 + X, 0 + Y, 0 + Z, 0.5, 0, 1,   1 + X, 1 + Y, 1 + Z, 1, 0.5, 1 });
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

    if (false && !solidblock)
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
*/

void MeshGenerator::GenerateMesh(Chunk* ChunkReferance)
{
    ChunkReferance->OpaqueMesh.vertices.clear();
    ChunkReferance->OpaqueMesh.indices.clear();

    unsigned int offset = 0;
    for (float x = 0; x < CHUNK_X_SIZE; x++)
    {
        for (float y = 0; y < CHUNK_Y_SIZE; y++)
        {
            for (float z = 0; z < CHUNK_Z_SIZE; z++)
            {
                if (BlockList::Blocks[ChunkReferance->Blocks[Chunk::GetIndex(x, y, z)]].RenderMode == BlockRenderMode::Opaque)
                {
                    float UV1 = BlockList::Blocks[ChunkReferance->Blocks[Chunk::GetIndex(x, y, z)]].Right_TextureID;
                    float UV2 = BlockList::Blocks[ChunkReferance->Blocks[Chunk::GetIndex(x, y, z)]].Left_TextureID;
                    float UV3 = BlockList::Blocks[ChunkReferance->Blocks[Chunk::GetIndex(x, y, z)]].Top_TextureID;
                    float UV4 = BlockList::Blocks[ChunkReferance->Blocks[Chunk::GetIndex(x, y, z)]].Bottom_TextureID;
                    float UV5 = BlockList::Blocks[ChunkReferance->Blocks[Chunk::GetIndex(x, y, z)]].Front_TextureID;
                    float UV6 = BlockList::Blocks[ChunkReferance->Blocks[Chunk::GetIndex(x, y, z)]].Back_TextureID;

                    // Right
                    if (Chunk::GetIndex(x + 1, y, z) != -1 && ChunkReferance->Blocks[Chunk::GetIndex(x + 1, y, z)] == 0)
                    {
                        ChunkReferance->OpaqueMesh.vertices.insert(ChunkReferance->OpaqueMesh.vertices.end(), { 1 + x, 1 + y, 0 + z, 0, 1, UV1, 0.5, 1,   1 + x, 0 + y, 1 + z, 1, 0, UV1, 0.5, 1,   1 + x, 0 + y, 0 + z, 0, 0, UV1, 0.5, 1,   1 + x, 1 + y, 1 + z, 1, 1, UV1, 0.5, 1 });
                        ChunkReferance->OpaqueMesh.indices.insert(ChunkReferance->OpaqueMesh.indices.end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
                        offset += 4;
                    }

                    // Left
                    if (Chunk::GetIndex(x - 1, y, z) != -1 && ChunkReferance->Blocks[Chunk::GetIndex(x - 1, y, z)] == 0)
                    {
                        ChunkReferance->OpaqueMesh.vertices.insert(ChunkReferance->OpaqueMesh.vertices.end(), { 0 + x, 1 + y, 1 + z, 0, 1, UV2, 0.5, 1,   0 + x, 0 + y, 0 + z, 1, 0, UV2, 0.5, 1,   0 + x, 0 + y, 1 + z, 0, 0, UV2, 0.5, 1,   0 + x, 1 + y, 0 + z, 1, 1, UV2, 0.5, 1 });
                        ChunkReferance->OpaqueMesh.indices.insert(ChunkReferance->OpaqueMesh.indices.end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
                        offset += 4;
                    }

                    // Top
                    if (Chunk::GetIndex(x, y + 1, z) != -1 && ChunkReferance->Blocks[Chunk::GetIndex(x, y + 1, z)] == 0)
                    {
                        ChunkReferance->OpaqueMesh.vertices.insert(ChunkReferance->OpaqueMesh.vertices.end(), { 0 + x, 1 + y, 1 + z, 0, 1, UV3, 1.0, 1,   1 + x, 1 + y, 0 + z, 1, 0, UV3, 1.0, 1,   0 + x, 1 + y, 0 + z, 0, 0, UV3, 1.0, 1,   1 + x, 1 + y, 1 + z, 1, 1, UV3, 1.0, 1 });
                        ChunkReferance->OpaqueMesh.indices.insert(ChunkReferance->OpaqueMesh.indices.end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
                        offset += 4;
                    }

                    // Bottom
                    if (Chunk::GetIndex(x, y - 1, z) != -1 && ChunkReferance->Blocks[Chunk::GetIndex(x, y - 1, z)] == 0)
                    {
                        ChunkReferance->OpaqueMesh.vertices.insert(ChunkReferance->OpaqueMesh.vertices.end(), { 1 + x, 0 + y, 1 + z, 0, 1, UV4, 0.0, 1,   0 + x, 0 + y, 0 + z, 1, 0, UV4, 0.0, 1,   1 + x, 0 + y, 0 + z, 0, 0, UV4, 0.0, 1,   0 + x, 0 + y, 1 + z, 1, 1, UV4, 0.0, 1 });
                        ChunkReferance->OpaqueMesh.indices.insert(ChunkReferance->OpaqueMesh.indices.end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
                        offset += 4;
                    }

                    // Front
                    if (Chunk::GetIndex(x, y, z + 1) != -1 && ChunkReferance->Blocks[Chunk::GetIndex(x, y, z + 1)] == 0)
                    {
                        ChunkReferance->OpaqueMesh.vertices.insert(ChunkReferance->OpaqueMesh.vertices.end(), { 1 + x, 1 + y, 1 + z, 0, 1, UV5, 0.75, 1,   0 + x, 0 + y, 1 + z, 1, 0, UV5, 0.75, 1,   1 + x, 0 + y, 1 + z, 0, 0, UV5, 0.75, 1,   0 + x, 1 + y, 1 + z, 1, 1, UV5, 0.75, 1 });
                        ChunkReferance->OpaqueMesh.indices.insert(ChunkReferance->OpaqueMesh.indices.end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
                        offset += 4;
                    }

                    // Back
                    if (Chunk::GetIndex(x, y, z - 1) != -1 && ChunkReferance->Blocks[Chunk::GetIndex(x, y, z - 1)] == 0)
                    {
                        ChunkReferance->OpaqueMesh.vertices.insert(ChunkReferance->OpaqueMesh.vertices.end(), { 0 + x, 1 + y, 0 + z, 0, 1, UV6, 0.25, 1,   1 + x, 0 + y, 0 + z, 1, 0, UV6, 0.25, 1,   0 + x, 0 + y, 0 + z, 0, 0, UV6, 0.25, 1,   1 + x, 1 + y, 0 + z, 1, 1, UV6, 0.25, 1 });
                        ChunkReferance->OpaqueMesh.indices.insert(ChunkReferance->OpaqueMesh.indices.end(), { offset + 0, offset + 1, offset + 2,   offset + 0, offset + 3, offset + 1 });
                        offset += 4;
                    }
                }
            }
        }
    }

    ChunkReferance->OpaqueMesh.vertices.insert(ChunkReferance->OpaqueMesh.vertices.end(), { 0, 0, 0, 0, 0, 0,   0, 0, 0, 0, 0, 0,   0, 0, 0, 0, 0, 0, });
    ChunkReferance->OpaqueMesh.indices.insert(ChunkReferance->OpaqueMesh.indices.end(), { offset + 0, offset + 1, offset + 2});
}