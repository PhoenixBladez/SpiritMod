sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
texture vnoise;
sampler vnoisesampler = sampler_state
{
    Texture = vnoise;
};

float4 colorMod;
float timer;


float4 Center(float2 coords, float4 ocolor)
{
    float4 noise = tex2D(vnoisesampler, float2(((coords.x + (timer / 2)) % 3) / 3, (coords.y + timer) % 1));
    float distFromCenter = 4 * sqrt(pow(coords.x - 0.5, 2) + pow((0.5 * coords.y) - 0.25, 2));
    
    return colorMod * max(distFromCenter, pow(1 - noise.r, 5)) * ocolor.a;
}

float4 MainPS(float2 coords : TEXCOORD0, float4 ocolor : COLOR0) : COLOR0
{
    float4 texColor = tex2D(uImage0, coords);
    
    if (texColor.r == 1)
        return Center(coords, ocolor);
    
    if (texColor.g == 1)
        return ocolor;
    
    return float4(0, 0, 0, 0);
}

technique BasicColorDrawing
{
    pass MainPS
    {
        PixelShader = compile ps_2_0 MainPS();
    }
};