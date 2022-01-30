#pragma once

class RandomNoise {
public:
    // 1D Random noise
    static float noise(float x);

    // 2D Random noise
    static float noise(float x, float y);

    // 3D Random noise
    static float noise(float x, float y, float z);
};