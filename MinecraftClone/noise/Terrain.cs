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
        static OpenSimplexNoise noise;

        public static bool SamplePoint(float X, float Y, float Z, Vector3 ChunkOffset)
        {
            if (noise == null)
            {
                noise = new OpenSimplexNoise(0);
            }

            float val = (float)(Math.Abs(noise.Evaluate((X + ChunkOffset.X * 64) / 25f, 0, (Z + ChunkOffset.Z * 64) / 25f)) * 0.7f + ((noise.Evaluate((X + ChunkOffset.X * 64) / 5f, 0, (Z + ChunkOffset.Z * 64) / 5f) + 1) * 0.5f) * 0.3f);
            float hils = (float)Math.Pow((noise.Evaluate((X + ChunkOffset.X * 64) / 75f, 0, (Z + ChunkOffset.Z * 64) / 75f) + 1), 4) * 0.0625f;

            if (Y + (ChunkOffset.Y * 64) < val * 64 * hils)
            {
                val = (val * 0.5f + 1) - ((float)Math.Abs(noise.Evaluate((X + ChunkOffset.X * 64) / 20f, (Y + ChunkOffset.Y * 64) / 20f, (Z + ChunkOffset.Z * 64) / 20f)) * 0.7f + (float)Math.Abs(noise.Evaluate((X + ChunkOffset.X * 64) / 10f, (Y + ChunkOffset.Y * 64) / 10f, (Z + ChunkOffset.Z * 64) / 10f)) * 0.3f);
            }
            else
            {
                val = 0;
            }

            return val > 0.7f;
        }
    }
}
