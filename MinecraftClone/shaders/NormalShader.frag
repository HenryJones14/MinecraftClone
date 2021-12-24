#version 330 core
out vec4 FragColor;

in vec3 Normal;
in vec4 Color;
in vec2 UVs;

//uniform sampler2D texture0;

vec3 filmic(vec3 x) {
  vec3 X = max(vec3(0.0), x - 0.004);
  vec3 result = (X * (6.2 * X + 0.5)) / (X * (6.2 * X + 1.7) + 0.06);
  return pow(result, vec3(2.2));
}

vec3 aces(vec3 x) {
  const float a = 2.51;
  const float b = 0.03;
  const float c = 2.43;
  const float d = 0.59;
  const float e = 0.14;
  return clamp((x * (a * x + b)) / (x * (c * x + d) + e), 0.0, 1.0);
}

void main()
{
    if (Color.a == 0)
    {
        discard;
    }

    float lig = (dot(vec3(0, 1, 0), Normal) + 1) * 0.5;
    vec3 col = aces(Color.xyz);

    FragColor = vec4(col * lig, Color.a);
}