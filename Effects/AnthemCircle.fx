sampler2D SpriteTextureSampler;

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float offset;

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float2 pos = input.TextureCoordinates;
    float2 cpos = pos - float2(0.5, 0.5);

    float4 color = float4(tex2D(SpriteTextureSampler, cpos * 1.05 + float2(0.5, 0.5)).r,
        tex2D(SpriteTextureSampler, cpos + float2(0.5, 0.5)).g,
        tex2D(SpriteTextureSampler, cpos * 0.95 + float2(0.5, 0.5)).b,
        0.5 + (1.0 - pos.x) * 0.5);

    return color * input.Color;
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile ps_2_0 MainPS();
    }
};
