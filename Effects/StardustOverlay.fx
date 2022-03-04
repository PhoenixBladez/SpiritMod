sampler2D image0 : register(S0);

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 color = tex2D(image0, uv);
    color.b = color.r + color.g;
    color.rg *= 0.5;
    return color;
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile ps_2_0 main();
    }
};
