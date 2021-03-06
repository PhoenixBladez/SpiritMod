sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float4 colorMod;
float breakCounter;
float2 center;
texture noise;
float rotation;
sampler tent = sampler_state
{
    Texture = (noise);
};
float2 rotate(float2 coords, float delta)
{
    float2 ret;
    ret.x = (coords.x * cos(delta)) - (coords.y * sin(delta));
    ret.y = (coords.x * sin(delta)) + (coords.y * cos(delta));
    return ret;
}
float4 White(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float2 noiseCoords = float2(0.49f, 0);
    float angle = atan2(coords.x - 0.5f, coords.y - 0.5f) + (rotation / 2);
    noiseCoords = rotate(noiseCoords, angle);
    float4 noiseColor = tex2D(tent, noiseCoords);
    if (noiseColor.r > 0.15f + (breakCounter / 5) || breakCounter < 0)
    {
        color *= 0;
    }
    float4 white = float4(1,1,1,1);
    color = white * color.r;
    return color;
}

technique BasicColorDrawing
{
    pass WhiteSprite
    {
        PixelShader = compile ps_2_0 White();
    }
};