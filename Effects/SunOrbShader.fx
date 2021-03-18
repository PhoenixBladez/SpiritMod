sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float4 colorMod;
float4 colorMod2;
float timer;

float4 White(float2 coords : TEXCOORD0, float4 ocolor : COLOR0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    if (color.a > 0.9f)
        return float4(0, 0, 0, 0);
    
    color = float4(1 - color.r, 1 - color.g, 1 - color.b, 0.9f - color.a);
    
    float dist = 1 - (sqrt(pow(coords.x - 0.5f, 2) + pow(coords.y - 0.5f, 2)) * 2);
    
    float4 finalcolor = colorMod * ((dist + timer) % 1) + colorMod2 * (1 - ((dist + timer) % 1));
    finalcolor.a *= color.a * ocolor.a;
    finalcolor.rgb *= 0.2f + ((color.a * ocolor.a) * 0.8f);
    return finalcolor * dist * 1.25f;
}

technique BasicColorDrawing
{
    pass WhiteSprite
    {
        PixelShader = compile ps_2_0 White();
    }
};