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
        public Vector3 chunkOffset;

        private BlockData[,,] chunk;
        private VoxelMesh mesh;

        public ChunkData(Vector3 ChunkOffset)
        {
            Console.WriteLine("Generating chunk");
            chunkOffset = ChunkOffset;
            chunk = new BlockData[64, 64, 64];
            Random random = new Random();

            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    for (int z = 0; z < 64; z++)
                    {
                        if (Terrain.SamplePoint(x, y, z, chunkOffset))
                        {
                            if (Terrain.SamplePoint(x, y + 3, z, chunkOffset) || y < 0 - chunkOffset.Y * 64)
                            {
                                if (random.Next(0, 250) == 0 && y < 0 - chunkOffset.Y * 64)
                                {
                                    chunk[x, y, z] = new BlockData(BlockName.Diamond, true, (BlockRotation)random.Next(0, 4));
                                }
                                else
                                {
                                    chunk[x, y, z] = new BlockData(BlockName.Stone, true, (BlockRotation)random.Next(0, 4));
                                }
                            }
                            else if (Terrain.SamplePoint(x, y + 2, z, chunkOffset))
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
                            else if (Terrain.SamplePoint(x, y + 1, z, chunkOffset))
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

            GenerateMesh();
        }

        private void GenerateMesh()
        {
            Console.WriteLine("Generating mesh");
            List<Vector3> newvertices = new List<Vector3>();
            List<uint> newindices = new List<uint>();
            List<Vector3> newuvs = new List<Vector3>();
            List<Vector3> newlights = new List<Vector3>();

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
                            if (BlockIsSolid(x, y + 1, z))
                            {
                                newvertices.Add(new Vector3(x + 1, y + 1, z));
                                newvertices.Add(new Vector3(x, y + 1, z + 1));
                                newvertices.Add(new Vector3(x + 1, y + 1, z + 1));
                                newvertices.Add(new Vector3(x, y + 1, z));

                                newuvs.Add(new Vector3(0.0f, 1.0f, (int)chunk[x, y, z].BlockName - 1));
                                newuvs.Add(new Vector3(0.5f, 0.5f, (int)chunk[x, y, z].BlockName - 1));
                                newuvs.Add(new Vector3(0.0f, 0.5f, (int)chunk[x, y, z].BlockName - 1));
                                newuvs.Add(new Vector3(0.5f, 1.0f, (int)chunk[x, y, z].BlockName - 1));

                                newlights.Add(new Vector3(1f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopBask)));
                                newlights.Add(new Vector3(1f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopFront)));
                                newlights.Add(new Vector3(1f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopFront)));
                                newlights.Add(new Vector3(1f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopBask)));

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 1);
                                newindices.Add(offset + 2);

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 3);
                                newindices.Add(offset + 1);

                                offset += 4;
                            }

                            // down
                            if (BlockIsSolid(x, y - 1, z))
                            {
                                newvertices.Add(new Vector3(x + 1, y, z + 1));
                                newvertices.Add(new Vector3(x, y, z));
                                newvertices.Add(new Vector3(x + 1, y, z));
                                newvertices.Add(new Vector3(x, y, z + 1));

                                newuvs.Add(new Vector3(0.0f, 0.5f, (int)chunk[x, y, z].BlockName - 1));
                                newuvs.Add(new Vector3(0.5f, 0.0f, (int)chunk[x, y, z].BlockName - 1));
                                newuvs.Add(new Vector3(0.0f, 0.0f, (int)chunk[x, y, z].BlockName - 1));
                                newuvs.Add(new Vector3(0.5f, 0.5f, (int)chunk[x, y, z].BlockName - 1));

                                newlights.Add(new Vector3(0f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomFront)));
                                newlights.Add(new Vector3(0f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomBask)));
                                newlights.Add(new Vector3(0f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomBask)));
                                newlights.Add(new Vector3(0f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomFront)));

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 1);
                                newindices.Add(offset + 2);

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 3);
                                newindices.Add(offset + 1);

                                offset += 4;
                            }

                            // Left
                            if (BlockIsSolid(x - 1, y, z))
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

                                newlights.Add(new Vector3(0.5f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopFront)));
                                newlights.Add(new Vector3(0.5f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomBask)));
                                newlights.Add(new Vector3(0.5f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomFront)));
                                newlights.Add(new Vector3(0.5f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopBask)));

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 1);
                                newindices.Add(offset + 2);

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 3);
                                newindices.Add(offset + 1);

                                offset += 4;
                            }

                            // Right
                            if (BlockIsSolid(x + 1, y, z))
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

                                newlights.Add(new Vector3(0.5f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopBask)));
                                newlights.Add(new Vector3(0.5f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomFront)));
                                newlights.Add(new Vector3(0.5f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomBask)));
                                newlights.Add(new Vector3(0.5f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopFront)));

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 1);
                                newindices.Add(offset + 2);

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 3);
                                newindices.Add(offset + 1);

                                offset += 4;
                            }

                            // Front
                            if (BlockIsSolid(x, y, z + 1))
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

                                newlights.Add(new Vector3(0.75f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopFront)));
                                newlights.Add(new Vector3(0.75f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomFront)));
                                newlights.Add(new Vector3(0.75f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomFront)));
                                newlights.Add(new Vector3(0.75f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopFront)));

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 1);
                                newindices.Add(offset + 2);

                                newindices.Add(offset + 0);
                                newindices.Add(offset + 3);
                                newindices.Add(offset + 1);

                                offset += 4;
                            }

                            // Back
                            if (BlockIsSolid(x, y, z - 1))
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

                                newlights.Add(new Vector3(0.25f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopBask)));
                                newlights.Add(new Vector3(0.25f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomBask)));
                                newlights.Add(new Vector3(0.25f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomBask)));
                                newlights.Add(new Vector3(0.25f, y + chunkOffset.Y * 64, CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopBask)));

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
            mesh = new VoxelMesh(newvertices.Count(), newvertices.ToArray(), newindices.ToArray(), newuvs.ToArray(), newlights.ToArray());
        }

        private bool BlockIsSolid(int X, int Y, int Z)
        {
            if (X < 0 || X >= 64 || Y < 0 || Y >= 64 || Z < 0 || Z >= 64)
            {
                return true;
            }
            return !chunk[X, Y, Z].BlockIsSolid;
        }

        private float CalculateAO(Vector3 Position, BlockCorner Corner)
        {
            switch (Corner)
            {
                case BlockCorner.RightTopFront:
                    return AmbientOclusion(new bool[7]
                    {
                        BlockIsSolid((int)Position.X, (int)Position.Y + 1, (int)Position.Z),

                        BlockIsSolid((int)Position.X + 1, (int)Position.Y + 1, (int)Position.Z),
                        BlockIsSolid((int)Position.X + 1, (int)Position.Y + 1, (int)Position.Z + 1),
                        BlockIsSolid((int)Position.X, (int)Position.Y + 1, (int)Position.Z + 1),

                        BlockIsSolid((int)Position.X + 1, (int)Position.Y, (int)Position.Z),
                        BlockIsSolid((int)Position.X + 1, (int)Position.Y, (int)Position.Z + 1),
                        BlockIsSolid((int)Position.X, (int)Position.Y, (int)Position.Z + 1)
                    });

                case BlockCorner.LeftTopFront:
                    return AmbientOclusion(new bool[7]
                    {
                        BlockIsSolid((int)Position.X, (int)Position.Y + 1, (int)Position.Z),

                        BlockIsSolid((int)Position.X - 1, (int)Position.Y + 1, (int)Position.Z),
                        BlockIsSolid((int)Position.X - 1, (int)Position.Y + 1, (int)Position.Z + 1),
                        BlockIsSolid((int)Position.X, (int)Position.Y + 1, (int)Position.Z + 1),

                        BlockIsSolid((int)Position.X - 1, (int)Position.Y, (int)Position.Z),
                        BlockIsSolid((int)Position.X - 1, (int)Position.Y, (int)Position.Z + 1),
                        BlockIsSolid((int)Position.X, (int)Position.Y, (int)Position.Z + 1)
                    });

                case BlockCorner.RightBottomFront:
                    return AmbientOclusion(new bool[7]
                    {
                        BlockIsSolid((int)Position.X, (int)Position.Y - 1, (int)Position.Z),

                        BlockIsSolid((int)Position.X + 1, (int)Position.Y - 1, (int)Position.Z),
                        BlockIsSolid((int)Position.X + 1, (int)Position.Y - 1, (int)Position.Z + 1),
                        BlockIsSolid((int)Position.X, (int)Position.Y - 1, (int)Position.Z + 1),

                        BlockIsSolid((int)Position.X + 1, (int)Position.Y, (int)Position.Z),
                        BlockIsSolid((int)Position.X + 1, (int)Position.Y, (int)Position.Z + 1),
                        BlockIsSolid((int)Position.X, (int)Position.Y, (int)Position.Z + 1)
                    });

                case BlockCorner.LeftBottomFront:
                    return AmbientOclusion(new bool[7]
                    {
                        BlockIsSolid((int)Position.X, (int)Position.Y - 1, (int)Position.Z),

                        BlockIsSolid((int)Position.X - 1, (int)Position.Y - 1, (int)Position.Z),
                        BlockIsSolid((int)Position.X - 1, (int)Position.Y - 1, (int)Position.Z + 1),
                        BlockIsSolid((int)Position.X, (int)Position.Y - 1, (int)Position.Z + 1),

                        BlockIsSolid((int)Position.X - 1, (int)Position.Y, (int)Position.Z),
                        BlockIsSolid((int)Position.X - 1, (int)Position.Y, (int)Position.Z + 1),
                        BlockIsSolid((int)Position.X, (int)Position.Y, (int)Position.Z + 1)
                    });

                case BlockCorner.RightTopBask:
                    return AmbientOclusion(new bool[7]
                    {
                        BlockIsSolid((int)Position.X, (int)Position.Y + 1, (int)Position.Z),

                        BlockIsSolid((int)Position.X + 1, (int)Position.Y + 1, (int)Position.Z),
                        BlockIsSolid((int)Position.X + 1, (int)Position.Y + 1, (int)Position.Z - 1),
                        BlockIsSolid((int)Position.X, (int)Position.Y + 1, (int)Position.Z - 1),

                        BlockIsSolid((int)Position.X + 1, (int)Position.Y, (int)Position.Z),
                        BlockIsSolid((int)Position.X + 1, (int)Position.Y, (int)Position.Z - 1),
                        BlockIsSolid((int)Position.X, (int)Position.Y, (int)Position.Z - 1)
                    });

                case BlockCorner.LeftTopBask:
                    return AmbientOclusion(new bool[7]
                    {
                        BlockIsSolid((int)Position.X, (int)Position.Y + 1, (int)Position.Z),

                        BlockIsSolid((int)Position.X - 1, (int)Position.Y + 1, (int)Position.Z),
                        BlockIsSolid((int)Position.X - 1, (int)Position.Y + 1, (int)Position.Z - 1),
                        BlockIsSolid((int)Position.X, (int)Position.Y + 1, (int)Position.Z - 1),

                        BlockIsSolid((int)Position.X - 1, (int)Position.Y, (int)Position.Z),
                        BlockIsSolid((int)Position.X - 1, (int)Position.Y, (int)Position.Z - 1),
                        BlockIsSolid((int)Position.X, (int)Position.Y, (int)Position.Z - 1)
                    });

                case BlockCorner.RightBottomBask:
                    return AmbientOclusion(new bool[7]
                    {
                        BlockIsSolid((int)Position.X, (int)Position.Y - 1, (int)Position.Z),

                        BlockIsSolid((int)Position.X + 1, (int)Position.Y - 1, (int)Position.Z),
                        BlockIsSolid((int)Position.X + 1, (int)Position.Y - 1, (int)Position.Z - 1),
                        BlockIsSolid((int)Position.X, (int)Position.Y - 1, (int)Position.Z - 1),

                        BlockIsSolid((int)Position.X + 1, (int)Position.Y, (int)Position.Z),
                        BlockIsSolid((int)Position.X + 1, (int)Position.Y, (int)Position.Z - 1),
                        BlockIsSolid((int)Position.X, (int)Position.Y, (int)Position.Z - 1)
                    });

                case BlockCorner.LeftBottomBask:
                    return AmbientOclusion(new bool[7]
                    {
                        BlockIsSolid((int)Position.X, (int)Position.Y - 1, (int)Position.Z),

                        BlockIsSolid((int)Position.X - 1, (int)Position.Y - 1, (int)Position.Z),
                        BlockIsSolid((int)Position.X - 1, (int)Position.Y - 1, (int)Position.Z - 1),
                        BlockIsSolid((int)Position.X, (int)Position.Y - 1, (int)Position.Z - 1),

                        BlockIsSolid((int)Position.X - 1, (int)Position.Y, (int)Position.Z),
                        BlockIsSolid((int)Position.X - 1, (int)Position.Y, (int)Position.Z - 1),
                        BlockIsSolid((int)Position.X, (int)Position.Y, (int)Position.Z - 1)
                    });
            }

            return 0;
        }

        private int AmbientOclusion(bool[] Sides)
        {
            int count = -1;

            for (int i = 0; i < Sides.Length; i++)
            {
                if (Sides[i])
                {
                    count++;
                }
            }

            //count -= 1;
            count = Math.Max(count, 0);

            return count;
        }

        public void Render()
        {
            mesh.RenderMesh();
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

    public enum BlockCorner
    {
        RightTopFront, LeftTopFront, RightBottomFront, LeftBottomFront,
        RightTopBask, LeftTopBask, RightBottomBask, LeftBottomBask,
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
