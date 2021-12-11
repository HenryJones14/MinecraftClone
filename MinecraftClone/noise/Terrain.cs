using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noise
{
    public static class Terrain
    {
        public static OpenSimplexNoise noise;

        public static TerrainType SamplePoint(float X, float Y, float Z, Vector3 ChunkOffset)
        {
            if (noise == null)
            {
                noise = new OpenSimplexNoise();
            }

            float val = (float)(Math.Abs(noise.Evaluate((X + ChunkOffset.X * 64) / 150f, 0, (Z + ChunkOffset.Z * 64) / 150f)) * 0.7f + ((noise.Evaluate((X + ChunkOffset.X * 64) / 15f, 0, (Z + ChunkOffset.Z * 64) / 15f) + 1) * 0.5f) * 0.3f);
            float hils = (float)(noise.Evaluate((X + ChunkOffset.X * 64) / 250f, 0, (Z + ChunkOffset.Z * 64) / 250f));
            hils = (hils * hils * hils - hils) * 3f + 0.5f;

            if (Y + (ChunkOffset.Y * 64) <= 64 * val * hils)
            {
                val = 1.5f - (float)(Math.Abs(noise.Evaluate((X + ChunkOffset.X * 64) / 25f, (Y + ChunkOffset.Y * 64) / 25f, (Z + ChunkOffset.Z * 64) / 25f)) + ((noise.Evaluate((X + ChunkOffset.X * 64) / 10f, (Y + ChunkOffset.Y * 64) / 10f, (Z + ChunkOffset.Z * 64) / 10f) + 1) * 0.4f));
            }
            else if (Y + (ChunkOffset.Y * 64) < 0)
            {
                return TerrainType.Fluid;
            }
            else
            {
                return TerrainType.None; 
            }

            if (val > 0.5f)
            {
                return TerrainType.Terrain;
            }
            else
            {
                return TerrainType.Space;
            }
        }
    }

    public enum TerrainType
    {
        None, Terrain, Fluid, Space,
    }
}
