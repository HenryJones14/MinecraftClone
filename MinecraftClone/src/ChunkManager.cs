using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftClone;
using Noise;

namespace MinecraftClone
{
    public class ChunkData
    {
        public BlockData[,,] chunk;
        public VoxelMesh mesh;
        public Vector3 offset;

        public ChunkData(Vector3 ChunkOffset)
        {
            offset = ChunkOffset;
            chunk = new BlockData[64, 64, 64];

            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    for (int z = 0; z < 64; z++)
                    {
                        if (Terrain.SamplePoint(x, y, z, offset))
                        {
                            chunk[x, y, z] = new BlockData(BlockName.Stone, true, BlockRotation.North);
                        }
                        else
                        {
                            chunk[x, y, z] = new BlockData(BlockName.Air, false, BlockRotation.North);
                        }
                    }
                }
            }

            mesh = GenerateMesh();
        }

        private VoxelMesh GenerateMesh()
        {
            List<uint> newindices = new List<uint>();
            List<uint> newvertices = new List<uint>();

            uint offset = 0;

            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    for (int z = 0; z < 64; z++)
                    {
                        if (chunk[x, y, z].BlockIsSolid)
                        {
                            // up
                            if (y + 1 >= 64 || (y + 1 < 64 && !chunk[x, y + 1, z].BlockIsSolid))
                            {
                                newvertices.Add(VoxelMesh.PackPosition(new Vector3(x + 1, y + 1, z)));
                                newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.0f, 1.0f)));
                                newvertices.Add(100);

                                newvertices.Add(VoxelMesh.PackPosition(new Vector3(x, y + 1, z + 1)));
                                newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.5f, 0.5f)));
                                newvertices.Add(100);

                                newvertices.Add(VoxelMesh.PackPosition(new Vector3(x + 1, y + 1, z + 1)));
                                newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.0f, 0.5f)));
                                newvertices.Add(100);

                                newvertices.Add(VoxelMesh.PackPosition(new Vector3(x, y + 1, z)));
                                newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.5f, 1.0f)));
                                newvertices.Add(100);

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 1);
                                newindices.Add(offset + 2);

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 3);
                                newindices.Add(offset + 1);

                                offset += 4;
                            }

                            /*
                            // down
                            if (y - 1 < 0 || (y - 1 >= 0 && !chunk[x, y - 1, z].BlockIsSolid))
                            {
                                newvertices.Add(VoxelMesh.PackPosition(new Vector3(x + 1, y, z + 1)));
                                newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.0f, 0.5f)));
                                newvertices.Add(0);

                                newvertices.Add(VoxelMesh.PackPosition(new Vector3(x, y, z)));
                                newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.5f, 0.0f)));
                                newvertices.Add(0);
                                
                                newvertices.Add(VoxelMesh.PackPosition(new Vector3(x + 1, y, z)));
                                newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.0f, 0.0f)));
                                newvertices.Add(0);
                                
                                newvertices.Add(VoxelMesh.PackPosition(new Vector3(x, y, z + 1)));
                                newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.5f, 0.5f)));
                                newvertices.Add(0);

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 1);
                                newindices.Add(offset + 2);

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 3);
                                newindices.Add(offset + 1);

                                offset += 4;
                            }

                            // Left
                            if (x - 1 < 0 || (x - 1 >= 0 && !chunk[x - 1, y, z].BlockIsSolid))
                            {
                                if (chunk[x, y, z].BlockRotation == BlockRotation.West)
                                {
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x, y + 1, z + 1)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.5f, 0.5f)));
                                    newvertices.Add(50);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x, y, z)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(1.0f, 0.0f)));
                                    newvertices.Add(50);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x, y, z + 1)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.5f, 0.0f)));
                                    newvertices.Add(50);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x, y + 1, z)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(1.0f, 0.5f)));
                                    newvertices.Add(50);
                                }
                                else
                                {
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x, y + 1, z + 1)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.5f, 1.0f)));
                                    newvertices.Add(50);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x, y, z)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(1.0f, 0.5f)));
                                    newvertices.Add(50);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x, y, z + 1)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.5f, 0.5f)));
                                    newvertices.Add(50);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x, y + 1, z)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(1.0f, 1.0f)));
                                    newvertices.Add(50);
                                }

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 1);
                                newindices.Add(offset + 2);

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 3);
                                newindices.Add(offset + 1);

                                offset += 4;
                            }

                            // Right
                            if (x + 1 >= 64 || (x + 1 < 64 && !chunk[x + 1, y, z].BlockIsSolid))
                            {
                                if (chunk[x, y, z].BlockRotation == BlockRotation.East)
                                {
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x + 1, y + 1, z)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.5f, 0.5f)));
                                    newvertices.Add(50);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x + 1, y, z + 1)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(1.0f, 0.0f)));
                                    newvertices.Add(50);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x + 1, y, z)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.5f, 0.0f)));
                                    newvertices.Add(50);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x + 1, y + 1, z + 1)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(1.0f, 0.5f)));
                                    newvertices.Add(50);
                                }
                                else
                                {
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x + 1, y + 1, z)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.5f, 1.0f)));
                                    newvertices.Add(50);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x + 1, y, z + 1)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(1.0f, 0.5f)));
                                    newvertices.Add(50);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x + 1, y, z)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.5f, 0.5f)));
                                    newvertices.Add(50);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x + 1, y + 1, z + 1)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(1.0f, 1.0f)));
                                    newvertices.Add(50);
                                }

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 1);
                                newindices.Add(offset + 2);

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 3);
                                newindices.Add(offset + 1);

                                offset += 4;
                            }

                            // Front
                            if (z + 1 >= 64 || (z + 1 < 64 && !chunk[x, y, z + 1].BlockIsSolid))
                            {
                                if (chunk[x, y, z].BlockRotation == BlockRotation.North)
                                {
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x + 1, y + 1, z + 1)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.5f, 0.5f)));
                                    newvertices.Add(75);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x, y, z + 1)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(1.0f, 0.0f)));
                                    newvertices.Add(75);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x + 1, y, z + 1)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.5f, 0.0f)));
                                    newvertices.Add(75);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x, y + 1, z + 1)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(1.0f, 0.5f)));
                                    newvertices.Add(75);
                                }
                                else
                                {
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x + 1, y + 1, z + 1)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.5f, 1.0f)));
                                    newvertices.Add(75);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x, y, z + 1)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(1.0f, 0.5f)));
                                    newvertices.Add(75);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x + 1, y, z + 1)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.5f, 0.5f)));
                                    newvertices.Add(75);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x, y + 1, z + 1)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(1.0f, 1.0f)));
                                    newvertices.Add(75);
                                }

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 1);
                                newindices.Add(offset + 2);

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 3);
                                newindices.Add(offset + 1);

                                offset += 4;
                            }

                            // Back
                            if (z - 1 < 0 || (z - 1 >= 0 && !chunk[x, y, z - 1].BlockIsSolid))
                            {
                                if (chunk[x, y, z].BlockRotation == BlockRotation.South)
                                {
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x, y + 1, z)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.5f, 0.5f)));
                                    newvertices.Add(25);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x + 1, y, z)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(1.0f, 0.0f)));
                                    newvertices.Add(25);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x, y, z)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.5f, 0.0f)));
                                    newvertices.Add(25);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x + 1, y + 1, z)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(1.0f, 0.5f)));
                                    newvertices.Add(25);
                                }
                                else
                                {
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x, y + 1, z)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.5f, 1.0f)));
                                    newvertices.Add(25);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x + 1, y, z)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(1.0f, 0.5f)));
                                    newvertices.Add(25);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x, y, z)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(0.5f, 0.5f)));
                                    newvertices.Add(25);

                                    newvertices.Add(VoxelMesh.PackPosition(new Vector3(x + 1, y + 1, z)));
                                    newvertices.Add(VoxelMesh.PackPosition(new Vector2(1.0f, 1.0f)));
                                    newvertices.Add(25);
                                }

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 1);
                                newindices.Add(offset + 2);

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 3);
                                newindices.Add(offset + 1);

                                offset += 4;
                            }*/
                        }
                    }
                }
            }
            return new VoxelMesh(newindices.ToArray(), newvertices.ToArray());
        }
    }

    public class BlockData
    {
        public BlockName BlockName;
        public bool BlockIsSolid;
        public BlockRotation BlockRotation;

        public BlockData()
        {
            BlockName = BlockName.Air;
            BlockIsSolid = false;
        }

        public BlockData(BlockName NewBlockName, bool IsBlockSolid, BlockRotation NewBlockRotation)
        {
            BlockName = NewBlockName;
            BlockIsSolid = IsBlockSolid;
            BlockRotation = NewBlockRotation;
        }
    }

    public enum BlockName
    {
        Air, Grass, Dirt, Stone, Iron, Bedrock,
    }

    public enum BlockRotation
    {
        North, East, South, West,
    }
}
