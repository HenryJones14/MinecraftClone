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
                        Random random = new Random();
                        if (Terrain.SamplePoint(x, y, z, offset))
                        {
                            chunk[x, y, z] = new BlockData(BlockName.Stone, true, (BlockRotation)random.Next(0, 3));
                        }
                        else
                        {
                            chunk[x, y, z] = new BlockData(BlockName.Air, false, (BlockRotation)random.Next(0, 3));
                        }
                    }
                }
            }

            mesh = GenerateMesh();
            chunk = null;
        }

        private VoxelMesh GenerateMesh()
        {
            List<Vector3> newvertices = new List<Vector3>();
            List<uint> newindices = new List<uint>();
            List<Vector2> newuvs = new List<Vector2>();
            List<float> newlights = new List<float>();

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
                                newvertices.Add(new Vector3(x + 1, y + 1, z));
                                newvertices.Add(new Vector3(x, y + 1, z + 1));
                                newvertices.Add(new Vector3(x + 1, y + 1, z + 1));
                                newvertices.Add(new Vector3(x, y + 1, z));

                                newuvs.Add(new Vector2(0.0f, 1.0f));
                                newuvs.Add(new Vector2(0.5f, 0.5f));
                                newuvs.Add(new Vector2(0.0f, 0.5f));
                                newuvs.Add(new Vector2(0.5f, 1.0f));

                                newlights.Add(1f);
                                newlights.Add(1f);
                                newlights.Add(1f);
                                newlights.Add(1f);

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 1);
                                newindices.Add(offset + 2);

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 3);
                                newindices.Add(offset + 1);

                                offset += 4;
                            }

                            // down
                            if (y - 1 < 0 || (y - 1 >= 0 && !chunk[x, y - 1, z].BlockIsSolid))
                            {
                                newvertices.Add(new Vector3(x + 1, y, z + 1));
                                newvertices.Add(new Vector3(x, y, z));
                                newvertices.Add(new Vector3(x + 1, y, z));
                                newvertices.Add(new Vector3(x, y, z + 1));

                                newuvs.Add(new Vector2(0.0f, 0.5f));
                                newuvs.Add(new Vector2(0.5f, 0.0f));
                                newuvs.Add(new Vector2(0.0f, 0.0f));
                                newuvs.Add(new Vector2(0.5f, 0.5f));

                                newlights.Add(0f);
                                newlights.Add(0f);
                                newlights.Add(0f);
                                newlights.Add(0f);

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
                                newvertices.Add(new Vector3(x, y + 1, z + 1));
                                newvertices.Add(new Vector3(x, y, z));
                                newvertices.Add(new Vector3(x, y, z + 1));
                                newvertices.Add(new Vector3(x, y + 1, z));

                                newuvs.Add(new Vector2(0.5f, 1.0f));
                                newuvs.Add(new Vector2(1.0f, 0.5f));
                                newuvs.Add(new Vector2(0.5f, 0.5f));
                                newuvs.Add(new Vector2(1.0f, 1.0f));

                                newlights.Add(0.5f);
                                newlights.Add(0.5f);
                                newlights.Add(0.5f);
                                newlights.Add(0.5f);

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
                                newvertices.Add(new Vector3(x + 1, y + 1, z));
                                newvertices.Add(new Vector3(x + 1, y, z + 1));
                                newvertices.Add(new Vector3(x + 1, y, z));
                                newvertices.Add(new Vector3(x + 1, y + 1, z + 1));

                                newuvs.Add(new Vector2(0.5f, 1.0f));
                                newuvs.Add(new Vector2(1.0f, 0.5f));
                                newuvs.Add(new Vector2(0.5f, 0.5f));
                                newuvs.Add(new Vector2(1.0f, 1.0f));

                                newlights.Add(0.5f);
                                newlights.Add(0.5f);
                                newlights.Add(0.5f);
                                newlights.Add(0.5f);

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
                                newvertices.Add(new Vector3(x + 1, y + 1, z + 1));
                                newvertices.Add(new Vector3(x, y, z + 1));
                                newvertices.Add(new Vector3(x + 1, y, z + 1));
                                newvertices.Add(new Vector3(x, y + 1, z + 1));

                                newuvs.Add(new Vector2(0.5f, 0.5f));
                                newuvs.Add(new Vector2(1.0f, 0.0f));
                                newuvs.Add(new Vector2(0.5f, 0.0f));
                                newuvs.Add(new Vector2(1.0f, 0.5f));

                                newlights.Add(0.75f);
                                newlights.Add(0.75f);
                                newlights.Add(0.75f);
                                newlights.Add(0.75f);

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
                                newvertices.Add(new Vector3(x, y + 1, z));
                                newvertices.Add(new Vector3(x + 1, y, z));
                                newvertices.Add(new Vector3(x, y, z));
                                newvertices.Add(new Vector3(x + 1, y + 1, z));

                                newuvs.Add(new Vector2(0.5f, 1.0f));
                                newuvs.Add(new Vector2(1.0f, 0.5f));
                                newuvs.Add(new Vector2(0.5f, 0.5f));
                                newuvs.Add(new Vector2(1.0f, 1.0f));

                                newlights.Add(0.25f);
                                newlights.Add(0.25f);
                                newlights.Add(0.25f);
                                newlights.Add(0.25f);

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 1);
                                newindices.Add(offset + 2);

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 3);
                                newindices.Add(offset + 1);

                                offset += 4;
                            }
                        }
                    }
                }
            }
            return new VoxelMesh(newvertices.Count(), newvertices.ToArray(), newindices.ToArray(), newuvs.ToArray(), newlights.ToArray());
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
