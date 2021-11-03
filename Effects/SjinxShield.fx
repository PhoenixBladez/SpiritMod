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
    float strength = 0.2f + min(pow(dist, 5) * 250, 4); //Change the opacity based on how close the pixel is to the center
    
    float4 noiseColor = tex2D(vNoise, float2((1.2f * (coords.x + timer)) % 1, (1.2f * (coords.y + timer)) % 1)); //Use a voronoi noise texture for spots, scrolling vertically and horizontally
    float4 noiseStrength = noiseColor.r;
    noiseStrength = pow(noiseStrength, 5) * 4 * pow(1 - dist, 6); //Change the strength of the noise by raising it to a power to make spots smaller, multiplying to make them brighter, and making it more transparent when further from center
    
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