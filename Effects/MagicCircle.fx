sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
matrix WorldViewProjection;
texture uTexture;
sampler2D textureSampler = sampler_state
{
    texture = <uTexture>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = wrap;
    AddressV = wrap;
};

float rotation;

struct VertexShaderInput
{
    float2 TextureCoordinates : TEXCOORD0;
    float4 Position : POSITION0;
    float4 Color : COLOR0;
};

struct VertexShaderOutput
{
    float2 TextureCoordinates : TEXCOORD0;
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;
    float4 pos = mul(input.Position, WorldViewProjection);
    output.Position = pos;
    
    output.Color = input.Color;

    output.TextureCoordinates = input.TextureCoordinates;

    return output;
};

float2 Rotate(float2 coords)
{
    float2x2 rotate = float2x2(cos(rotation), -sin(rotation), sin(rotation), cos(rotation));
    return mul((coords + float2(-0.5, -0.5)), rotate) + float2(0.5, 0.5);
}

float4 White(VertexShaderOutput input) : COLOR0
{
    float distFromCenter = sqrt(pow(input.TextureCoordinates.x - 0.5f, 2) + pow(input.TextureCoordinates.y - 0.5f, 2)) * 2;
    if (distFromCenter > 1) //Prevent weirdness from texutre looping after the rotation
        return float4(0, 0, 0, 0);
    
    float4 color = tex2D(textureSampler, Rotate(input.TextureCoordinates));
    
    return color * input.Color;
}

technique Technique1
{
    pass White
    {
        PixelShader = compile ps_2_0 White();
    }
}