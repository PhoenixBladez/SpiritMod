sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
matrix WorldViewProjection;
texture uTexture;
sampler textureSampler = sampler_state
{
    Texture = (uTexture);
    AddressU = wrap;
    AddressV = wrap;
};

float Progress;

float xMod;
float yMod;

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
    VertexShaderOutput output = (VertexShaderOutput)0;
    float4 pos = mul(input.Position, WorldViewProjection);
    output.Position = pos;
    
    output.Color = input.Color;

	output.TextureCoordinates = input.TextureCoordinates;

    return output;
};

float4 MainPS(VertexShaderOutput input) : COLOR0
{
    float4 color = input.Color;
    float strength = 1;
    float halfYMod = yMod / 2;
    
    float2 texCoords = float2((input.TextureCoordinates.x * xMod) + Progress, (input.TextureCoordinates.y / yMod) + (0.5f - 0.5f / yMod));
    float2 texCoords2 = float2((input.TextureCoordinates.x * xMod) + Progress/2, (input.TextureCoordinates.y / halfYMod) + (0.5f - 0.5f / halfYMod));
    
    strength = pow(1 - abs(input.TextureCoordinates.y - 0.5f) * 2, 2) * 1.25f;
    strength += pow(tex2D(textureSampler, texCoords).r, 2) * 3;
    strength += pow(tex2D(textureSampler, texCoords2).r, 2);
    strength *= 1.5f;

    return color * strength;
}

technique BasicColorDrawing
{
    pass DefaultPS
	{
        VertexShader = compile vs_2_0 MainVS();
        PixelShader = compile ps_2_0 MainPS();
    }
};