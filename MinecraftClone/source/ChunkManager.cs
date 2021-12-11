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
                                chunk[x, y, z] = new BlockData(BlockName.Air, RenderType.None,  BlockRotation.North, false);
                                break;

                            case TerrainType.Terrain:
                                chunk[x, y, z] = new BlockData(BlockName.Stone, RenderType.Block, BlockRotation.North, false);
                                break;

                            case TerrainType.Fluid:
                                chunk[x, y, z] = new BlockData(BlockName.Water, RenderType.Liquid, BlockRotation.North, false);
                                break;

                            case TerrainType.Space:
                                chunk[x, y, z] = new BlockData(BlockName.Bedrock, RenderType.None, BlockRotation.North, false);
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
                                chunk[x, y, z] = new BlockData(BlockName.Dirt, RenderType.Block, (BlockRotation)random.Next(0, 4), false);

                                for (int i = -1; i < 2; i++)
                                {
                                    for (int j = -1; j < 2; j++)
                                    {
                                        for (int k = -1; k < 2; k++)
                                        {
                                            if (InsideChunk(x + i, y + j + 4, z + k))
                                            {
                                                chunk[x + i, y + j + 4, z + k] = new BlockData(BlockName.Leaves, RenderType.Transparent, BlockRotation.North, false);
                                            }
                                        }
                                    }
                                }

                                if (InsideChunk(x, y + 6, z))
                                {
                                    chunk[x, y + 6, z] = new BlockData(BlockName.Leaves, RenderType.Transparent, BlockRotation.North, false);
                                }
                                if (InsideChunk(x + 1, y + 6, z))
                                {
                                    chunk[x + 1, y + 6, z] = new BlockData(BlockName.Leaves, RenderType.Transparent, BlockRotation.North, false);
                                }
                                if (InsideChunk(x, y + 6, z + 1))
                                {
                                    chunk[x, y + 6, z + 1] = new BlockData(BlockName.Leaves, RenderType.Transparent, BlockRotation.North, false);
                                }
                                if (InsideChunk(x + 1, y + 6, z + 1))
                                {
                                    chunk[x, y + 6, z] = new BlockData(BlockName.Leaves, RenderType.Transparent, BlockRotation.North, false);
                                }
                                if (InsideChunk(x - 1, y + 6, z))
                                {
                                    chunk[x - 1, y + 6, z] = new BlockData(BlockName.Leaves, RenderType.Transparent, BlockRotation.North, false);
                                }
                                if (InsideChunk(x, y + 6, z - 1))
                                {
                                    chunk[x, y + 6, z - 1] = new BlockData(BlockName.Leaves, RenderType.Transparent, BlockRotation.North, false);
                                }

                                for (int i = 1; i < 5; i++)
                                {
                                    if (InsideChunk(x, y + i, z))
                                    {
                                        chunk[x, y + i, z] = new BlockData(BlockName.Log, RenderType.Block, BlockRotation.North, false);
                                    }
                                }
                            }
                            else if (random.Next(0, 100) <= 30 && InsideChunk(x, y + 1, z))
                            {
                                chunk[x, y, z] = new BlockData(BlockName.Grass, RenderType.Block, (BlockRotation)random.Next(0, 4), false);
                                chunk[x, y+1, z] = new BlockData(BlockName.Flowers, RenderType.Model, (BlockRotation)random.Next(0, 4), false);
                            }
                            else
                            {
                                chunk[x, y, z] = new BlockData(BlockName.Grass, RenderType.Block, (BlockRotation)random.Next(0, 4), false);
                            }
                        }
                        else if (chunk[x, y, z].BlockName == BlockName.Stone && InsideChunk(x, y + 1, z) && chunk[x, y + 1, z].BlockName == BlockName.Grass)
                        {
                            chunk[x, y, z] = new BlockData(BlockName.Dirt, RenderType.Block, (BlockRotation)random.Next(0, 4), false);
                        }
                        else if (chunk[x, y, z].BlockName == BlockName.Stone && InsideChunk(x, y + 2, z) && chunk[x, y + 2, z].BlockName == BlockName.Grass)
                        {
                            if (random.Next(0, 100) <= 50)
                            {
                                chunk[x, y, z] = new BlockData(BlockName.Dirt, RenderType.Block, (BlockRotation)random.Next(0, 4), false);
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
                            chunk[x, y, z] = new BlockData(BlockName.Air, RenderType.None, BlockRotation.North, false);
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
                        if ((y == 63 && chunkOffset.Y == -1 && chunk[x, y, z].BlockName == BlockName.Stone) || (chunk[x, y, z].BlockName != BlockName.Water && InsideChunk(x, y + 1, z) && chunk[x, y + 1, z].BlockName == BlockName.Water))
                        {
                            chunk[x, y, z] = new BlockData(BlockName.Sand, RenderType.Block, BlockRotation.North, false);
                        }
                    }
                }
            }

            List<Vector3> lights = new List<Vector3>();

            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    for (int z = 0; z < 64; z++)
                    {
                        if (chunk[x, y, z].BlockName == BlockName.Stone && random.Next(0, 100) > 98)
                        {
                            chunk[x, y, z] = new BlockData(BlockName.Diamond, RenderType.Block, BlockRotation.North, true);
                            if (InsideChunk(x + 1, y, z) && chunk[x + 1, y, z].BlockName == BlockName.Air
                             || InsideChunk(x, y + 1, z) && chunk[x, y + 1, z].BlockName == BlockName.Air
                             || InsideChunk(x, y, z + 1) && chunk[x, y, z + 1].BlockName == BlockName.Air
                             || InsideChunk(x - 1, y, z) && chunk[x - 1, y, z].BlockName == BlockName.Air
                             || InsideChunk(x, y - 1, z) && chunk[x, y - 1, z].BlockName == BlockName.Air
                             || InsideChunk(x, y, z - 1) && chunk[x, y, z - 1].BlockName == BlockName.Air)
                            {
                                lights.Add(new Vector3(x, y, z));
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
                        chunk[x, y, z].BlockLightLevel = CalculateLight(new Vector3(x, y, z), lights);
                    }
                }
            }

            GenerateMesh();
        }

        private void GenerateMesh()
        {
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

                                newlights.Add(new Vector3(2f, chunk[x, y, z].BlockLightLevel[4], CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopBask)));
                                newlights.Add(new Vector3(2f, chunk[x, y, z].BlockLightLevel[1], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopFront)));
                                newlights.Add(new Vector3(2f, chunk[x, y, z].BlockLightLevel[0], CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopFront)));
                                newlights.Add(new Vector3(2f, chunk[x, y, z].BlockLightLevel[5], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopBask)));

                                if (Math.Abs(newlights[newlights.Count - 4].Z - newlights[newlights.Count - 3].Z) < Math.Abs(newlights[newlights.Count - 2].Z - newlights[newlights.Count - 1].Z))
                                {
                                    newindices.Add(offset + 0);
                                    newindices.Add(offset + 1);
                                    newindices.Add(offset + 2);

                                    newindices.Add(offset + 0);
                                    newindices.Add(offset + 3);
                                    newindices.Add(offset + 1);
                                }
                                else
                                {
                                    newindices.Add(offset + 0);
                                    newindices.Add(offset + 3);
                                    newindices.Add(offset + 2);

                                    newindices.Add(offset + 3);
                                    newindices.Add(offset + 1);
                                    newindices.Add(offset + 2);
                                }

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

                                newlights.Add(new Vector3(3f, chunk[x, y, z].BlockLightLevel[2], CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomFront)));
                                newlights.Add(new Vector3(3f, chunk[x, y, z].BlockLightLevel[7], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomBask)));
                                newlights.Add(new Vector3(3f, chunk[x, y, z].BlockLightLevel[6], CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomBask)));
                                newlights.Add(new Vector3(3f, chunk[x, y, z].BlockLightLevel[3], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomFront)));

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

                                newlights.Add(new Vector3(1f, chunk[x, y, z].BlockLightLevel[1], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopFront)));
                                newlights.Add(new Vector3(1f, chunk[x, y, z].BlockLightLevel[7], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomBask)));
                                newlights.Add(new Vector3(1f, chunk[x, y, z].BlockLightLevel[3], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomFront)));
                                newlights.Add(new Vector3(1f, chunk[x, y, z].BlockLightLevel[5], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopBask)));

                                if (Math.Abs(newlights[newlights.Count - 4].Z - newlights[newlights.Count - 3].Z) < Math.Abs(newlights[newlights.Count - 2].Z - newlights[newlights.Count - 1].Z))
                                {
                                    newindices.Add(offset + 0);
                                    newindices.Add(offset + 1);
                                    newindices.Add(offset + 2);

                                    newindices.Add(offset + 0);
                                    newindices.Add(offset + 3);
                                    newindices.Add(offset + 1);
                                }
                                else
                                {
                                    newindices.Add(offset + 0);
                                    newindices.Add(offset + 3);
                                    newindices.Add(offset + 2);

                                    newindices.Add(offset + 3);
                                    newindices.Add(offset + 1);
                                    newindices.Add(offset + 2);
                                }

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

                                newlights.Add(new Vector3(0f, chunk[x, y, z].BlockLightLevel[4], CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopBask)));
                                newlights.Add(new Vector3(0f, chunk[x, y, z].BlockLightLevel[2], CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomFront)));
                                newlights.Add(new Vector3(0f, chunk[x, y, z].BlockLightLevel[6], CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomBask)));
                                newlights.Add(new Vector3(0f, chunk[x, y, z].BlockLightLevel[0], CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopFront)));

                                if (Math.Abs(newlights[newlights.Count - 4].Z - newlights[newlights.Count - 3].Z) < Math.Abs(newlights[newlights.Count - 2].Z - newlights[newlights.Count - 1].Z))
                                {
                                    newindices.Add(offset + 0);
                                    newindices.Add(offset + 1);
                                    newindices.Add(offset + 2);

                                    newindices.Add(offset + 0);
                                    newindices.Add(offset + 3);
                                    newindices.Add(offset + 1);
                                }
                                else
                                {
                                    newindices.Add(offset + 0);
                                    newindices.Add(offset + 3);
                                    newindices.Add(offset + 2);

                                    newindices.Add(offset + 3);
                                    newindices.Add(offset + 1);
                                    newindices.Add(offset + 2);
                                }

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

                                newlights.Add(new Vector3(4f, chunk[x, y, z].BlockLightLevel[0], CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopFront)));
                                newlights.Add(new Vector3(4f, chunk[x, y, z].BlockLightLevel[3], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomFront)));
                                newlights.Add(new Vector3(4f, chunk[x, y, z].BlockLightLevel[2], CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomFront)));
                                newlights.Add(new Vector3(4f, chunk[x, y, z].BlockLightLevel[1], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopFront)));

                                if (Math.Abs(newlights[newlights.Count - 4].Z - newlights[newlights.Count - 3].Z) < Math.Abs(newlights[newlights.Count - 2].Z - newlights[newlights.Count - 1].Z))
                                {
                                    newindices.Add(offset + 0);
                                    newindices.Add(offset + 1);
                                    newindices.Add(offset + 2);

                                    newindices.Add(offset + 0);
                                    newindices.Add(offset + 3);
                                    newindices.Add(offset + 1);
                                }
                                else
                                {
                                    newindices.Add(offset + 0);
                                    newindices.Add(offset + 3);
                                    newindices.Add(offset + 2);

                                    newindices.Add(offset + 3);
                                    newindices.Add(offset + 1);
                                    newindices.Add(offset + 2);
                                }

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

                                newlights.Add(new Vector3(5f, chunk[x, y, z].BlockLightLevel[5], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopBask)));
                                newlights.Add(new Vector3(5f, chunk[x, y, z].BlockLightLevel[6], CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomBask)));
                                newlights.Add(new Vector3(5f, chunk[x, y, z].BlockLightLevel[7], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomBask)));
                                newlights.Add(new Vector3(5f, chunk[x, y, z].BlockLightLevel[4], CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopBask)));

                                if (Math.Abs(newlights[newlights.Count - 4].Z - newlights[newlights.Count - 3].Z) < Math.Abs(newlights[newlights.Count - 2].Z - newlights[newlights.Count - 1].Z))
                                {
                                    newindices.Add(offset + 0);
                                    newindices.Add(offset + 1);
                                    newindices.Add(offset + 2);

                                    newindices.Add(offset + 0);
                                    newindices.Add(offset + 3);
                                    newindices.Add(offset + 1);
                                }
                                else
                                {
                                    newindices.Add(offset + 0);
                                    newindices.Add(offset + 3);
                                    newindices.Add(offset + 2);

                                    newindices.Add(offset + 3);
                                    newindices.Add(offset + 1);
                                    newindices.Add(offset + 2);
                                }

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

                            newlights.Add(new Vector3(1f, chunk[x, y, z].BlockLightLevel[4], CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopBask)));
                            newlights.Add(new Vector3(1f, chunk[x, y, z].BlockLightLevel[1], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopFront)));
                            newlights.Add(new Vector3(1f, chunk[x, y, z].BlockLightLevel[0], CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopFront)));
                            newlights.Add(new Vector3(1f, chunk[x, y, z].BlockLightLevel[5], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopBask)));

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

                            newlights.Add(new Vector3(0f, chunk[x, y, z].BlockLightLevel[2], CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomFront)));
                            newlights.Add(new Vector3(0f, chunk[x, y, z].BlockLightLevel[7], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomBask)));
                            newlights.Add(new Vector3(0f, chunk[x, y, z].BlockLightLevel[6], CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomBask)));
                            newlights.Add(new Vector3(0f, chunk[x, y, z].BlockLightLevel[3], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomFront)));

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

                            newlights.Add(new Vector3(0.5f, chunk[x, y, z].BlockLightLevel[1], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopFront)));
                            newlights.Add(new Vector3(0.5f, chunk[x, y, z].BlockLightLevel[7], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomBask)));
                            newlights.Add(new Vector3(0.5f, chunk[x, y, z].BlockLightLevel[3], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomFront)));
                            newlights.Add(new Vector3(0.5f, chunk[x, y, z].BlockLightLevel[5], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopBask)));

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

                            newlights.Add(new Vector3(0.5f, chunk[x, y, z].BlockLightLevel[4], CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopBask)));
                            newlights.Add(new Vector3(0.5f, chunk[x, y, z].BlockLightLevel[2], CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomFront)));
                            newlights.Add(new Vector3(0.5f, chunk[x, y, z].BlockLightLevel[6], CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomBask)));
                            newlights.Add(new Vector3(0.5f, chunk[x, y, z].BlockLightLevel[0], CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopFront)));

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

                            newlights.Add(new Vector3(0.75f, chunk[x, y, z].BlockLightLevel[0], CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopFront)));
                            newlights.Add(new Vector3(0.75f, chunk[x, y, z].BlockLightLevel[3], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomFront)));
                            newlights.Add(new Vector3(0.75f, chunk[x, y, z].BlockLightLevel[2], CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomFront)));
                            newlights.Add(new Vector3(0.75f, chunk[x, y, z].BlockLightLevel[1], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopFront)));

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

                            newlights.Add(new Vector3(0.25f, chunk[x, y, z].BlockLightLevel[5], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopBask)));
                            newlights.Add(new Vector3(0.25f, chunk[x, y, z].BlockLightLevel[6], CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomBask)));
                            newlights.Add(new Vector3(0.25f, chunk[x, y, z].BlockLightLevel[7], CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomBask)));
                            newlights.Add(new Vector3(0.25f, chunk[x, y, z].BlockLightLevel[4], CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopBask)));

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

            for (int y = 63; y >= 0; y--)
            {
                for (int x = 0; x < 64; x++)
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

                                newlights.Add(new Vector3(1f, 100, 10));
                                newlights.Add(new Vector3(1f, 100, 10));
                                newlights.Add(new Vector3(1f, 100, 10));
                                newlights.Add(new Vector3(1f, 100, 10));

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

                                newlights.Add(new Vector3(0f, 100, 10));
                                newlights.Add(new Vector3(0f, 100, 10));
                                newlights.Add(new Vector3(0f, 100, 10));
                                newlights.Add(new Vector3(0f, 100, 10));

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

                                newlights.Add(new Vector3(0.5f, 100, 10));
                                newlights.Add(new Vector3(0.5f, 100, 10));
                                newlights.Add(new Vector3(0.5f, 100, 10));
                                newlights.Add(new Vector3(0.5f, 100, 10));

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

                                newlights.Add(new Vector3(0.5f, 100, 10));
                                newlights.Add(new Vector3(0.5f, 100, 10));
                                newlights.Add(new Vector3(0.5f, 100, 10));
                                newlights.Add(new Vector3(0.5f, 100, 10));

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

                                newlights.Add(new Vector3(0.75f, 100, 10));
                                newlights.Add(new Vector3(0.75f, 100, 10));
                                newlights.Add(new Vector3(0.75f, 100, 10));
                                newlights.Add(new Vector3(0.75f, 100, 10));

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

                                newlights.Add(new Vector3(0.25f, 100, 10));
                                newlights.Add(new Vector3(0.25f, 100, 10));
                                newlights.Add(new Vector3(0.25f, 100, 10));
                                newlights.Add(new Vector3(0.25f, 100, 10));

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

                                        newlights.Add(new Vector3(1.0f, chunk[x, y, z].BlockLightLevel[0], 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopFront)));
                                        newlights.Add(new Vector3(1.0f, chunk[x, y, z].BlockLightLevel[7], 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomBask)));
                                        newlights.Add(new Vector3(1.0f, chunk[x, y, z].BlockLightLevel[2], 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomFront)));
                                        newlights.Add(new Vector3(1.0f, chunk[x, y, z].BlockLightLevel[5], 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopBask)));
                                        break;

                                    case 1:
                                        newvertices.Add(new Vector3(x, y + 1, z));
                                        newvertices.Add(new Vector3(x + 1, y, z + 1));
                                        newvertices.Add(new Vector3(x, y, z));
                                        newvertices.Add(new Vector3(x + 1, y + 1, z + 1));
                                        
                                        newlights.Add(new Vector3(1.0f, chunk[x, y, z].BlockLightLevel[5], 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopBask)));
                                        newlights.Add(new Vector3(1.0f, chunk[x, y, z].BlockLightLevel[2], 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomFront)));
                                        newlights.Add(new Vector3(1.0f, chunk[x, y, z].BlockLightLevel[7], 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomBask)));
                                        newlights.Add(new Vector3(1.0f, chunk[x, y, z].BlockLightLevel[0], 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopFront)));
                                        break;

                                    case 2:
                                        newvertices.Add(new Vector3(x, y + 1, z + 1));
                                        newvertices.Add(new Vector3(x + 1, y, z));
                                        newvertices.Add(new Vector3(x, y, z + 1));
                                        newvertices.Add(new Vector3(x + 1, y + 1, z));

                                        newlights.Add(new Vector3(1.0f, chunk[x, y, z].BlockLightLevel[1], 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopFront)));
                                        newlights.Add(new Vector3(1.0f, chunk[x, y, z].BlockLightLevel[6], 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomBask)));
                                        newlights.Add(new Vector3(1.0f, chunk[x, y, z].BlockLightLevel[3], 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomFront)));
                                        newlights.Add(new Vector3(1.0f, chunk[x, y, z].BlockLightLevel[4], 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopBask)));
                                        break;

                                    case 3:
                                        newvertices.Add(new Vector3(x + 1, y + 1, z));
                                        newvertices.Add(new Vector3(x, y, z + 1));
                                        newvertices.Add(new Vector3(x + 1, y, z));
                                        newvertices.Add(new Vector3(x, y + 1, z + 1));

                                        newlights.Add(new Vector3(1.0f, chunk[x, y, z].BlockLightLevel[4], 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.RightTopBask)));
                                        newlights.Add(new Vector3(1.0f, chunk[x, y, z].BlockLightLevel[3], 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.LeftBottomFront)));
                                        newlights.Add(new Vector3(1.0f, chunk[x, y, z].BlockLightLevel[6], 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.RightBottomBask)));
                                        newlights.Add(new Vector3(1.0f, chunk[x, y, z].BlockLightLevel[1], 1 + CalculateAO(new Vector3(x, y, z), BlockCorner.LeftTopFront)));
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

        private float[] CalculateLight(Vector3 Position, List<Vector3> Lights)
        {
            float[] dist = new float[8] { 1000000, 1000000, 1000000, 1000000, 1000000, 1000000, 1000000, 1000000, };
            for (int i = 0; i < Lights.Count; i++)
            {
                dist[0] = Math.Min(dist[0], Vector3.Distance(Position + new Vector3(0.5f, 0.5f, 0.5f), Lights[i]));
                dist[1] = Math.Min(dist[1], Vector3.Distance(Position + new Vector3(-0.5f, 0.5f, 0.5f), Lights[i]));
                dist[2] = Math.Min(dist[2], Vector3.Distance(Position + new Vector3(0.5f, -0.5f, 0.5f), Lights[i]));
                dist[3] = Math.Min(dist[3], Vector3.Distance(Position + new Vector3(-0.5f, -0.5f, 0.5f), Lights[i]));

                dist[4] = Math.Min(dist[4], Vector3.Distance(Position + new Vector3(0.5f, 0.5f, -0.5f), Lights[i]));
                dist[5] = Math.Min(dist[5], Vector3.Distance(Position + new Vector3(-0.5f, 0.5f, -0.5f), Lights[i]));
                dist[6] = Math.Min(dist[6], Vector3.Distance(Position + new Vector3(0.5f, -0.5f, -0.5f), Lights[i]));
                dist[7] = Math.Min(dist[7], Vector3.Distance(Position + new Vector3(-0.5f, -0.5f, -0.5f), Lights[i]));
            }
            return dist;
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

        public bool BlockLightSource;
        public float[] BlockLightLevel;

        public BlockData()
        {
            BlockName = BlockName.Air;
            RenderType = RenderType.None;
        }

        public BlockData(BlockName NewBlockName, RenderType NewRenderType, BlockRotation NewBlockRotation, bool NewBlockLightSource)
        {
            BlockName = NewBlockName;
            RenderType = NewRenderType;
            BlockRotation = NewBlockRotation;
            BlockLightSource = NewBlockLightSource;
        }
    }

    public enum BlockCorner
    {
        RightTopFront, LeftTopFront, RightBottomFront, LeftBottomFront,
        RightTopBask, LeftTopBask, RightBottomBask, LeftBottomBask,
    }

    public enum BlockName
    {
        Air, Grass, Dirt, Stone, Diamond, Bedrock, Log, Leaves, Water, Flowers, Sand
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
