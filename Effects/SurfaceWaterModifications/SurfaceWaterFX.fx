float transparency;
texture sampleTexture;
sampler2D samplerTex = sampler_state { texture = <sampleTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

float4 PixelShaderFunction(float4 screenSpace : TEXCOORD0) : COLOR0
{
    float2 st = screenSpace.xy;
    float4 color = tex2D(samplerTex, st);
    float luminosity = ((0.3 * color.r) + (0.59 * color.g) + (0.11 * color.b));
    
    //mb + (1-m)c
    float realLuminosity = (transparency * luminosity) * (1 - transparency);
    color.a *= 1 - (luminosity * 1.8);
    return color;
}

technique Technique1
{
    pass PrimitivesPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
};