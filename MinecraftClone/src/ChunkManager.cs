using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftClone
{
    public class ChunkData
    {
        public BlockData[,,] chunk;

        public ChunkData(Vector3 ChunkOffset)
        {
            chunk = new BlockData[32, 32, 32];

            for (int x = 0; x < 32; x++)
            {
                for (int y = 0; y < 32; y++)
                {
                    for (int z = 0; z < 32; z++)
                    {
                        /*if (MainGame.NoiseTerrain.SamplePoint(x, y, z, ChunkOffset))
                        {
                            chunk[x, y, z] = new BlockData(BlockName.Stone, true);
                        }
                        else
                        {
                            chunk[x, y, z] = new BlockData(BlockName.Air, false);
                        }*/
                    }
                }
            }
        }
    }

    public class BlockData
    {
        public BlockName BlockName;
        public bool BlockIsSolid;

        public BlockData()
        {
            BlockName = BlockName.Air;
            BlockIsSolid = false;
        }

        public BlockData(BlockName NewBlockName, bool IsBlockSolid)
        {
            BlockName = NewBlockName;
            BlockIsSolid = IsBlockSolid;
        }
    }

    public enum BlockName
    {
        Air, Grass, Dirt, Stone, Iron, Bedrock,
    }
}
