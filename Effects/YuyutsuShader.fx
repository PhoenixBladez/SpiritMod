sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float progress;
float4 color1;
float4 color2;
texture noise;
sampler noiseSampler = sampler_state
{
    Texture = (noise);
};
float4 White(float2 coords : TEXCOORD0) : COLOR0
{

    float4 color = tex2D(uImage0, coords);
    float4 noisecolor = tex2D(noiseSampler, coords / 2);
    color *= color.a * noisecolor.r;
    color = lerp(color1,color2,coords.x) * color.a;
    color *= sin(progress / 30);
    return color;
}

technique BasicColorDrawing
{
    pass WhiteSprite
    {
        PixelShader = compile ps_2_0 White();
    }
};