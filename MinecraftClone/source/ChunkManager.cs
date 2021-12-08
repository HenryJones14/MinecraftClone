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
                        switch (Terrain.SamplePoint(x, y, z, chunkOffset))
                        {
                            case TerrainType.None:
                                chunk[x, y, z] = new BlockData(BlockName.Air, RenderType.None,  BlockRotation.North);
                                break;

                            case TerrainType.Terrain:
                                chunk[x, y, z] = new BlockData(BlockName.Stone, RenderType.Block, BlockRotation.North);
                                break;

                            case TerrainType.Fluid:
                                chunk[x, y, z] = new BlockData(BlockName.Water, RenderType.Liquid, BlockRotation.North);
                                break;

                            case TerrainType.Space:
                                chunk[x, y, z] = new BlockData(BlockName.Bedrock, RenderType.None, BlockRotation.North);
                                break;
                        }
                    }
                }
            }

            List<Vector2> points = new List<Vector2>();
            bool canspawntree = false;

            for (int y = 63; y >= 0; y--)
            {
                for (int x = 0; x < 64; x++)
                {
                    for (int z = 0; z < 64; z++)
                    {
                        if (chunk[x, y, z].BlockName == BlockName.Stone && InsideChunk(x, y + 1, z) && chunk[x, y + 1, z].BlockName == BlockName.Air)
                        {
                            canspawntree = true;

                            for (int i = 0; i < points.Count; i++)
                            {
                                if (Vector2.Distance(points[i], new Vector2(x, z)) < 3)
                                {
                                    canspawntree = false;
                                }
                            }

                            if (canspawntree && random.Next(0, 100) <= 3 && Terrain.noise.Evaluate((chunkOffset.X * 64) + x / 25f, (chunkOffset.X * 64) + z / 25f) <= 0)
                            {
                                points.Add(new Vector2(x, z));
                                chunk[x, y, z] = new BlockData(BlockName.Dirt, RenderType.Block, (BlockRotation)random.Next(0, 4));

                                for (int i = -1; i < 2; i++)
                                {
                                    for (int j = -1; j < 2; j++)
                                    {
                                        for (int k = -1; k < 2; k++)
                                        {
                                            if (InsideChunk(x + i, y + j + 4, z + k))
                                            {
                                                chunk[x + i, y + j + 4, z + k] = new BlockData(BlockName.Leaves, RenderType.Transparent, BlockRotation.North);
                                            }
                                        }
                                    }
                                }

                                if (InsideChunk(x, y + 6, z))
                                {
                                    chunk[x, y + 6, z] = new BlockData(BlockName.Leaves, RenderType.Transparent, BlockRotation.North);
                                }
                                if (InsideChunk(x + 1, y + 6, z))
                                {
                                    chunk[x + 1, y + 6, z] = new BlockData(BlockName.Leaves, RenderType.Transparent, BlockRotation.North);
                                }
                                if (InsideChunk(x, y + 6, z + 1))
                                {
                                    chunk[x, y + 6, z + 1] = new BlockData(BlockName.Leaves, RenderType.Transparent, BlockRotation.North);
                                }
                                if (InsideChunk(x + 1, y + 6, z + 1))
                                {
                                    chunk[x, y + 6, z] = new BlockData(BlockName.Leaves, RenderType.Transparent, BlockRotation.North);
                                }
                                if (InsideChunk(x - 1, y + 6, z))
                                {
                                    chunk[x - 1, y + 6, z] = new BlockData(BlockName.Leaves, RenderType.Transparent, BlockRotation.North);
                                }
                                if (InsideChunk(x, y + 6, z - 1))
                                {
                                    chunk[x, y + 6, z - 1] = new BlockData(BlockName.Leaves, RenderType.Transparent, BlockRotation.North);
                                }

                                for (int i = 1; i < 5; i++)
                                {
                                    if (InsideChunk(x, y + i, z))
                                    {
                                        chunk[x, y + i, z] = new BlockData(BlockName.Log, RenderType.Block, BlockRotation.North);
                                    }
                                }
                            }
                            else if (random.Next(0, 100) <= 30 && InsideChunk(x, y + 1, z))
                            {
                                chunk[x, y, z] = new BlockData(BlockName.Grass, RenderType.Block, (BlockRotation)random.Next(0, 4));
                                chunk[x, y+1, z] = new BlockData(BlockName.Flowers, RenderType.Model, (BlockRotation)random.Next(0, 4));
                            }
                            else
                            {
                                chunk[x, y, z] = new BlockData(BlockName.Grass, RenderType.Block, (BlockRotation)random.Next(0, 4));
                            }
                        }
                        else if (chunk[x, y, z].BlockName == BlockName.Stone && InsideChunk(x, y + 1, z) && chunk[x, y + 1, z].BlockName == BlockName.Grass)
                        {
                            chunk[x, y, z] = new BlockData(BlockName.Dirt, RenderType.Block, (BlockRotation)random.Next(0, 4));
                        }
                        else if (chunk[x, y, z].BlockName == BlockName.Stone && InsideChunk(x, y + 2, z) && chunk[x, y + 2, z].BlockName == BlockName.Grass)
                        {
                            if (random.Next(0, 100) <= 50)
                            {
                                chunk[x, y, z] = new BlockData(BlockName.Dirt, RenderType.Block, (BlockRotation)random.Next(0, 4));
                            }
                        }
                    }
                }
            }

            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    for (int z = 0; z < 64; z++)
                    {
                        if (chunk[x, y, z].BlockName == BlockName.Bedrock)
                        {
                            chunk[x, y, z] = new BlockData(BlockName.Air, RenderType.None,  BlockRotation.North);
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
                        if (chunk[x, y, z].RenderType == RenderType.Block)
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
            };

            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    for (int z = 0; z < 64; z++)
                    {
                        if (chunk[x, y, z].RenderType == RenderType.Transparent)
                        {
                            // up
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

                            // down
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

                            // Left
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

                            // Right
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

                            // Front
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

                            // Back
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
            };

            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    for (int z = 0; z < 64; z++)
                    {
                        if (chunk[x, y, z].RenderType == RenderType.Liquid)
                        {
                            // up
                            if (W_BlockIsSolid(x, y + 1, z))
                            {
                                newvertices.Add(new Vector3(x + 1, y + 0.78125f, z));
                                newvertices.Add(new Vector3(x, y + 0.78125f, z + 1));
                                newvertices.Add(new Vector3(x + 1, y + 0.78125f, z + 1));
                                newvertices.Add(new Vector3(x, y + 0.78125f, z));

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
                            if (W_BlockIsSolid(x, y - 1, z))
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
                            if (W_BlockIsSolid(x - 1, y, z))
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
                            if (W_BlockIsSolid(x + 1, y, z))
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
                            if (W_BlockIsSolid(x, y, z + 1))
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
                            if (W_BlockIsSolid(x, y, z - 1))
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
            };

            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    for (int z = 0; z < 64; z++)
                    {
                        if (chunk[x, y, z].RenderType == RenderType.Model)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        newvertices.Add(new Vector3(x + 1, y + 1, z + 1));
                                        newvertices.Add(new Vector3(x, y, z));
                                        newvertices.Add(new Vector3(x + 1, y, z + 1));
                                        newvertices.Add(new Vector3(x, y + 1, z));

                                        newlights.Add(new Vector3(1.0f, y + chunkOffset.Y * 64, 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopFront)));
                                        newlights.Add(new Vector3(1.0f, y + chunkOffset.Y * 64, 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomBask)));
                                        newlights.Add(new Vector3(1.0f, y + chunkOffset.Y * 64, 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomFront)));
                                        newlights.Add(new Vector3(1.0f, y + chunkOffset.Y * 64, 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopBask)));
                                        break;

                                    case 1:
                                        newvertices.Add(new Vector3(x, y + 1, z));
                                        newvertices.Add(new Vector3(x + 1, y, z + 1));
                                        newvertices.Add(new Vector3(x, y, z));
                                        newvertices.Add(new Vector3(x + 1, y + 1, z + 1));
                                        
                                        newlights.Add(new Vector3(1.0f, y + chunkOffset.Y * 64, 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopBask)));
                                        newlights.Add(new Vector3(1.0f, y + chunkOffset.Y * 64, 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomFront)));
                                        newlights.Add(new Vector3(1.0f, y + chunkOffset.Y * 64, 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomBask)));
                                        newlights.Add(new Vector3(1.0f, y + chunkOffset.Y * 64, 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopFront)));
                                        break;

                                    case 2:
                                        newvertices.Add(new Vector3(x, y + 1, z + 1));
                                        newvertices.Add(new Vector3(x + 1, y, z));
                                        newvertices.Add(new Vector3(x, y, z + 1));
                                        newvertices.Add(new Vector3(x + 1, y + 1, z));

                                        newlights.Add(new Vector3(1.0f, y + chunkOffset.Y * 64, 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopFront)));
                                        newlights.Add(new Vector3(1.0f, y + chunkOffset.Y * 64, 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomBask)));
                                        newlights.Add(new Vector3(1.0f, y + chunkOffset.Y * 64, 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomFront)));
                                        newlights.Add(new Vector3(1.0f, y + chunkOffset.Y * 64, 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopBask)));
                                        break;

                                    case 3:
                                        newvertices.Add(new Vector3(x + 1, y + 1, z));
                                        newvertices.Add(new Vector3(x, y, z + 1));
                                        newvertices.Add(new Vector3(x + 1, y, z));
                                        newvertices.Add(new Vector3(x, y + 1, z + 1));

                                        newlights.Add(new Vector3(1.0f, y + chunkOffset.Y * 64, 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopBask)));
                                        newlights.Add(new Vector3(1.0f, y + chunkOffset.Y * 64, 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomFront)));
                                        newlights.Add(new Vector3(1.0f, y + chunkOffset.Y * 64, 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomBask)));
                                        newlights.Add(new Vector3(1.0f, y + chunkOffset.Y * 64, 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopFront)));
                                        break;

                                }

                                newuvs.Add(new Vector3(-0.207105f, 1.0f, (int)chunk[x, y, z].BlockName - 1));
                                newuvs.Add(new Vector3(1.207105f, 0.0f, (int)chunk[x, y, z].BlockName - 1));
                                newuvs.Add(new Vector3(-0.207105f, 0.0f, (int)chunk[x, y, z].BlockName - 1));
                                newuvs.Add(new Vector3(1.207105f, 1.0f, (int)chunk[x, y, z].BlockName - 1));

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
            return chunk[X, Y, Z].RenderType != RenderType.Block;
        }

        private bool W_BlockIsSolid(int X, int Y, int Z)
        {
            if (X < 0 || X >= 64 || Y < 0 || Y >= 64 || Z < 0 || Z >= 64)
            {
                return true;
            }
            return chunk[X, Y, Z].RenderType != RenderType.Liquid;
        }

        private bool AO_BlockIsSolid(int X, int Y, int Z)
        {
            if (X < 0 || X >= 64 || Y < 0 || Y >= 64 || Z < 0 || Z >= 64)
            {
                return true;
            }
            return !(chunk[X, Y, Z].RenderType == RenderType.Block || chunk[X, Y, Z].RenderType == RenderType.Transparent);
        }

        private bool InsideChunk(int X, int Y, int Z)
        {
            if (X < 0 || X >= 64 || Y < 0 || Y >= 64 || Z < 0 || Z >= 64)
            {
                return false;
            }
            return true;
        }

        private float CalculateAO(Vector3 Position, BlockCorner Corner)
        {
            switch (Corner)
            {
                case BlockCorner.RightTopFront:
                    return AmbientOclusion(new bool[7]
                    {
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y + 1, (int)Position.Z),

                        AO_BlockIsSolid((int)Position.X + 1, (int)Position.Y + 1, (int)Position.Z),
                        AO_BlockIsSolid((int)Position.X + 1, (int)Position.Y + 1, (int)Position.Z + 1),
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y + 1, (int)Position.Z + 1),

                        AO_BlockIsSolid((int)Position.X + 1, (int)Position.Y, (int)Position.Z),
                        AO_BlockIsSolid((int)Position.X + 1, (int)Position.Y, (int)Position.Z + 1),
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y, (int)Position.Z + 1)
                    });

                case BlockCorner.LeftTopFront:
                    return AmbientOclusion(new bool[7]
                    {
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y + 1, (int)Position.Z),

                        AO_BlockIsSolid((int)Position.X - 1, (int)Position.Y + 1, (int)Position.Z),
                        AO_BlockIsSolid((int)Position.X - 1, (int)Position.Y + 1, (int)Position.Z + 1),
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y + 1, (int)Position.Z + 1),

                        AO_BlockIsSolid((int)Position.X - 1, (int)Position.Y, (int)Position.Z),
                        AO_BlockIsSolid((int)Position.X - 1, (int)Position.Y, (int)Position.Z + 1),
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y, (int)Position.Z + 1)
                    });

                case BlockCorner.RightBottomFront:
                    return AmbientOclusion(new bool[7]
                    {
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y - 1, (int)Position.Z),

                        AO_BlockIsSolid((int)Position.X + 1, (int)Position.Y - 1, (int)Position.Z),
                        AO_BlockIsSolid((int)Position.X + 1, (int)Position.Y - 1, (int)Position.Z + 1),
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y - 1, (int)Position.Z + 1),

                        AO_BlockIsSolid((int)Position.X + 1, (int)Position.Y, (int)Position.Z),
                        AO_BlockIsSolid((int)Position.X + 1, (int)Position.Y, (int)Position.Z + 1),
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y, (int)Position.Z + 1)
                    });

                case BlockCorner.LeftBottomFront:
                    return AmbientOclusion(new bool[7]
                    {
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y - 1, (int)Position.Z),

                        AO_BlockIsSolid((int)Position.X - 1, (int)Position.Y - 1, (int)Position.Z),
                        AO_BlockIsSolid((int)Position.X - 1, (int)Position.Y - 1, (int)Position.Z + 1),
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y - 1, (int)Position.Z + 1),

                        AO_BlockIsSolid((int)Position.X - 1, (int)Position.Y, (int)Position.Z),
                        AO_BlockIsSolid((int)Position.X - 1, (int)Position.Y, (int)Position.Z + 1),
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y, (int)Position.Z + 1)
                    });

                case BlockCorner.RightTopBask:
                    return AmbientOclusion(new bool[7]
                    {
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y + 1, (int)Position.Z),

                        AO_BlockIsSolid((int)Position.X + 1, (int)Position.Y + 1, (int)Position.Z),
                        AO_BlockIsSolid((int)Position.X + 1, (int)Position.Y + 1, (int)Position.Z - 1),
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y + 1, (int)Position.Z - 1),

                        AO_BlockIsSolid((int)Position.X + 1, (int)Position.Y, (int)Position.Z),
                        AO_BlockIsSolid((int)Position.X + 1, (int)Position.Y, (int)Position.Z - 1),
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y, (int)Position.Z - 1)
                    });

                case BlockCorner.LeftTopBask:
                    return AmbientOclusion(new bool[7]
                    {
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y + 1, (int)Position.Z),

                        AO_BlockIsSolid((int)Position.X - 1, (int)Position.Y + 1, (int)Position.Z),
                        AO_BlockIsSolid((int)Position.X - 1, (int)Position.Y + 1, (int)Position.Z - 1),
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y + 1, (int)Position.Z - 1),

                        AO_BlockIsSolid((int)Position.X - 1, (int)Position.Y, (int)Position.Z),
                        AO_BlockIsSolid((int)Position.X - 1, (int)Position.Y, (int)Position.Z - 1),
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y, (int)Position.Z - 1)
                    });

                case BlockCorner.RightBottomBask:
                    return AmbientOclusion(new bool[7]
                    {
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y - 1, (int)Position.Z),

                        AO_BlockIsSolid((int)Position.X + 1, (int)Position.Y - 1, (int)Position.Z),
                        AO_BlockIsSolid((int)Position.X + 1, (int)Position.Y - 1, (int)Position.Z - 1),
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y - 1, (int)Position.Z - 1),

                        AO_BlockIsSolid((int)Position.X + 1, (int)Position.Y, (int)Position.Z),
                        AO_BlockIsSolid((int)Position.X + 1, (int)Position.Y, (int)Position.Z - 1),
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y, (int)Position.Z - 1)
                    });

                case BlockCorner.LeftBottomBask:
                    return AmbientOclusion(new bool[7]
                    {
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y - 1, (int)Position.Z),

                        AO_BlockIsSolid((int)Position.X - 1, (int)Position.Y - 1, (int)Position.Z),
                        AO_BlockIsSolid((int)Position.X - 1, (int)Position.Y - 1, (int)Position.Z - 1),
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y - 1, (int)Position.Z - 1),

                        AO_BlockIsSolid((int)Position.X - 1, (int)Position.Y, (int)Position.Z),
                        AO_BlockIsSolid((int)Position.X - 1, (int)Position.Y, (int)Position.Z - 1),
                        AO_BlockIsSolid((int)Position.X, (int)Position.Y, (int)Position.Z - 1)
                    });
            }

            return 0;
        }

        private int AmbientOclusion(bool[] Sides)
        {
            int count = 0;

            for (int i = 0; i < Sides.Length; i++)
            {
                if (Sides[i])
                {
                    count++;
                }
            }

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
        public RenderType RenderType;
        public BlockRotation BlockRotation;

        public BlockData()
        {
            BlockName = BlockName.Air;
            RenderType = RenderType.None;
        }

        public BlockData(BlockName NewBlockName, RenderType NewRenderType, BlockRotation NewBlockRotation)
        {
            BlockName = NewBlockName;
            RenderType = NewRenderType;
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
        Air, Grass, Dirt, Stone, Diamond, Bedrock, Log, Leaves, Water, Flowers
    }

    public enum RenderType
    {
        None, Block, Transparent, Foliage, Model, Liquid
    }

    public enum BlockRotation
    {
        North, East, South, West,
    }
}
