sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

texture vnoiseTex;
sampler vNoise
{
    Texture = (vnoiseTex);
};

float timer;

float4 MainPS(float2 coords : TEXCOORD0, float4 ocolor : COLOR0) : COLOR0
{
    float4 texColor = tex2D(uImage0, coords);
    //if (texColor.r < 0.1f)
      //  return float4(0, 0, 0, 0);
    
    float dist = sqrt(pow(coords.x - 0.5f, 2) + pow(coords.y - 0.5f, 2)) * 2;
    float strength = 0.2f + min(pow(dist, 5) * 250, 4);
    
    float4 noiseColor = tex2D(vNoise, float2((0.75f * (coords.x + timer)) % 1, (0.75f * (coords.y + timer)) % 1));
    float4 noiseStrength = (1 - noiseColor.r);
    noiseStrength = pow(noiseStrength, 6) * 6 * pow(1 - dist, 6);
    
    strength += noiseStrength;
    
    return ocolor * texColor * strength;
}

technique BasicColorDrawing
{
    pass MainPS
    {
        PixelShader = compile ps_2_0 MainPS();
    }
};