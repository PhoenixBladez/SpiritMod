sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float4 colorMod;
float distance;
float rotation;
float opacity2;

float counter;
texture noise;
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
float lengthSquared(float2 colorvector)
{
    float ret = (colorvector.x * colorvector.x) + (colorvector.y * colorvector.y);
    return ret;
}
float4 White(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float2 noiseCoords = float2((distance / 10), 0);
    float angle = atan2(coords.x - 0.5f, coords.y - 0.5f) + rotation;
    noiseCoords = rotate(noiseCoords, angle);
    float4 noiseColor = tex2D(tent, noiseCoords + float2(0.5f,0.5f));
    float2 colorvector = float2(coords.x - 0.5f, coords.y - 0.5f);
    float colordist = opacity2 / sqrt(sqrt((colorvector.x * colorvector.x) + (colorvector.y * colorvector.y)));
    color = colorMod * color.r * noiseColor.r * noiseColor.r * noiseColor.r * colordist;
    return color;
}
float4 WhiteTwo(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float noiseR = pow(tex2D(tent, float2(counter % 1, coords.y)), sqrt(coords.y));
    
    float2 colorvector = float2(coords.x - 0.85f, coords.y - 0.5f);
    float colorDist = opacity2 / (colorvector.x * colorvector.x) + (colorvector.y * colorvector.y);
    color = lerp(float4(0,0,0,0), colorMod, color.r * noiseR * colorDist);
    color = lerp(color, float4(1,1,1,1), pow(clamp(sqrt(sqrt(color.a)) / 2, 0, 1), 2));
    return color;
}

float4 WhiteThree(float2 coords : TEXCOORD0, float4 origcolor : COLOR0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float noiseR = pow(tex2D(tent, float2(counter % 1, coords.y)), sqrt(coords.y));
    
    float2 colorvector = float2(coords.x - 0.85f, coords.y - 0.5f);
    float colorDist = opacity2 / (colorvector.x * colorvector.x) + (colorvector.y * colorvector.y);
    color = lerp(float4(0, 0, 0, 0), colorMod, color.r * noiseR * colorDist);
    color = lerp(color, float4(1, 1, 1, 1), pow(clamp(sqrt(sqrt(color.a)) / 2, 0, 1), 2));
    color /= 2;
    return origcolor * color;
}
float4 Comet(float2 coords : TEXCOORD0, float4 origcolor : COLOR0) :COLOR0
{
    float4 color = tex2D(uImage0, coords);
    if (color.a == 0)
        return float4(0, 0, 0, 0);
    
    float xdistfromcenter = min(abs(0.5f - coords.x) * 3, 1);
    float noiseR = pow(tex2D(tent, float2(counter % 1, coords.y)), sqrt(coords.y));
    float4 transparency = (0, 0, 0, 0);
    transparency = lerp(color, transparency, xdistfromcenter * noiseR);
    
    float4 newColor = float4(origcolor.r, origcolor.g, origcolor.b, max(origcolor.a - transparency.a, 0)) * (1 - xdistfromcenter) * noiseR;
    return floor(newColor * 4) / 4;
}

technique BasicColorDrawing
{
    pass WhiteSprite
    {
        PixelShader = compile ps_2_0 White();
    }
    pass WhiteSpriteTwo
    {
        PixelShader = compile ps_2_0 WhiteTwo();
    }
    pass WhiteSpriteThree
    {
        PixelShader = compile ps_2_0 WhiteThree();
    }
    pass Comet
    {
        PixelShader = compile ps_2_0 Comet();
    }
};