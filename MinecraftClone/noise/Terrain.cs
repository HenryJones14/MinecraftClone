using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noise
{
    public class Terrain
    {
        OpenSimplexNoise noise;

        public Terrain(int NewSeed)
        {
            noise = new OpenSimplexNoise();
        }

        public bool SamplePoint(float X, float Y, float Z, Vector3 ChunkOffset)
        {
            float val = 0;

            if (Y + (ChunkOffset.Y * 32) < (noise.Evaluate((X + ChunkOffset.X * 32) / 25f, 0, (Z + ChunkOffset.Z * 32) / 25f) + 1) * 16)
            {
                val = 1 - (float)Math.Abs(noise.Evaluate((X + ChunkOffset.X * 32) / 15f, (Y + ChunkOffset.Y * 32) / 15f, (Z + ChunkOffset.Z * 32) / 15f));
            }

            return val > 0.5f;
        }
    }
}
