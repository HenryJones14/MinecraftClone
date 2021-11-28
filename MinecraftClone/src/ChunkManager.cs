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
            Random random = new Random();

            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    for (int z = 0; z < 64; z++)
                    {
                        if (Terrain.SamplePoint(x, y, z, offset))
                        {
                            if (Terrain.SamplePoint(x, y + 3, z, offset) || y < 0 - offset.Y * 64)
                            {
                                if (random.Next(0, 250) == 0 && y < 0 - offset.Y * 64)
                                {
                                    chunk[x, y, z] = new BlockData(BlockName.Diamond, true, (BlockRotation)random.Next(0, 4));
                                }
                                else
                                {
                                    chunk[x, y, z] = new BlockData(BlockName.Stone, true, (BlockRotation)random.Next(0, 4));
                                }
                            }
                            else if (Terrain.SamplePoint(x, y + 2, z, offset))
                            {
                                if (random.Next(0, 2) > 0)
                                {
                                    chunk[x, y, z] = new BlockData(BlockName.Dirt, true, (BlockRotation)random.Next(0, 4));
                                }
                                else
                                {
                                    chunk[x, y, z] = new BlockData(BlockName.Stone, true, (BlockRotation)random.Next(0, 4));
                                }
                            }
                            else if (Terrain.SamplePoint(x, y + 1, z, offset))
                            {
                                chunk[x, y, z] = new BlockData(BlockName.Dirt, true, (BlockRotation)random.Next(0, 4));
                            }
                            else
                            {
                                chunk[x, y, z] = new BlockData(BlockName.Grass, true, (BlockRotation)random.Next(0, 4));
                            }
                        }
                        else
                        {
                            chunk[x, y, z] = new BlockData(BlockName.Air, false, BlockRotation.North);
                        }
                    }
                }
            }

            mesh = GenerateMesh(offset * 64);
            chunk = null;
        }

        private VoxelMesh GenerateMesh(Vector3 Offset)
        {
            List<Vector3> newvertices = new List<Vector3>();
            List<uint> newindices = new List<uint>();
            List<Vector3> newuvs = new List<Vector3>();
            List<float> newlights = new List<float>();

            uint offset = 0;
            float light = 1;

            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    light = (y + Offset.Y + 128) / 128f;
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

                                newuvs.Add(new Vector3(0.0f, 1.0f, (int)chunk[x, y, z].BlockName - 1));
                                newuvs.Add(new Vector3(0.5f, 0.5f, (int)chunk[x, y, z].BlockName - 1));
                                newuvs.Add(new Vector3(0.0f, 0.5f, (int)chunk[x, y, z].BlockName - 1));
                                newuvs.Add(new Vector3(0.5f, 1.0f, (int)chunk[x, y, z].BlockName - 1));

                                newlights.Add(1f * light);
                                newlights.Add(1f * light);
                                newlights.Add(1f * light);
                                newlights.Add(1f * light);

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

                                newuvs.Add(new Vector3(0.0f, 0.5f, (int)chunk[x, y, z].BlockName - 1));
                                newuvs.Add(new Vector3(0.5f, 0.0f, (int)chunk[x, y, z].BlockName - 1));
                                newuvs.Add(new Vector3(0.0f, 0.0f, (int)chunk[x, y, z].BlockName - 1));
                                newuvs.Add(new Vector3(0.5f, 0.5f, (int)chunk[x, y, z].BlockName - 1));

                                newlights.Add(0f * light);
                                newlights.Add(0f * light);
                                newlights.Add(0f * light);
                                newlights.Add(0f * light);

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


                                if (chunk[x, y, z].BlockRotation == BlockRotation.West)
                                {
                                    newuvs.Add(new Vector3(0.5f, 0.5f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(1.0f, 0.0f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(0.5f, 0.0f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(1.0f, 0.5f, (int)chunk[x, y, z].BlockName - 1));
                                }
                                else
                                {
                                    newuvs.Add(new Vector3(0.5f, 1.0f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(1.0f, 0.5f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(0.5f, 0.5f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(1.0f, 1.0f, (int)chunk[x, y, z].BlockName - 1));
                                }

                                newlights.Add(0.5f * light);
                                newlights.Add(0.5f * light);
                                newlights.Add(0.5f * light);
                                newlights.Add(0.5f * light);

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

                                if (chunk[x, y, z].BlockRotation == BlockRotation.East)
                                {
                                    newuvs.Add(new Vector3(0.5f, 0.5f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(1.0f, 0.0f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(0.5f, 0.0f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(1.0f, 0.5f, (int)chunk[x, y, z].BlockName - 1));
                                }
                                else
                                {
                                    newuvs.Add(new Vector3(0.5f, 1.0f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(1.0f, 0.5f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(0.5f, 0.5f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(1.0f, 1.0f, (int)chunk[x, y, z].BlockName - 1));
                                }

                                newlights.Add(0.5f * light);
                                newlights.Add(0.5f * light);
                                newlights.Add(0.5f * light);
                                newlights.Add(0.5f * light);

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

                                if (chunk[x, y, z].BlockRotation == BlockRotation.North)
                                {
                                    newuvs.Add(new Vector3(0.5f, 0.5f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(1.0f, 0.0f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(0.5f, 0.0f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(1.0f, 0.5f, (int)chunk[x, y, z].BlockName - 1));
                                }
                                else
                                {
                                    newuvs.Add(new Vector3(0.5f, 1.0f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(1.0f, 0.5f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(0.5f, 0.5f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(1.0f, 1.0f, (int)chunk[x, y, z].BlockName - 1));
                                }

                                newlights.Add(0.75f * light);
                                newlights.Add(0.75f * light);
                                newlights.Add(0.75f * light);
                                newlights.Add(0.75f * light);

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

                                if (chunk[x, y, z].BlockRotation == BlockRotation.South)
                                {
                                    newuvs.Add(new Vector3(0.5f, 0.5f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(1.0f, 0.0f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(0.5f, 0.0f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(1.0f, 0.5f, (int)chunk[x, y, z].BlockName - 1));
                                }
                                else
                                {
                                    newuvs.Add(new Vector3(0.5f, 1.0f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(1.0f, 0.5f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(0.5f, 0.5f, (int)chunk[x, y, z].BlockName - 1));
                                    newuvs.Add(new Vector3(1.0f, 1.0f, (int)chunk[x, y, z].BlockName - 1));
                                }

                                newlights.Add(0.25f * light);
                                newlights.Add(0.25f * light);
                                newlights.Add(0.25f * light);
                                newlights.Add(0.25f * light);

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
        Air, Grass, Dirt, Stone, Diamond, Bedrock,
    }

    public enum BlockRotation
    {
        North, East, South, West,
    }
}
