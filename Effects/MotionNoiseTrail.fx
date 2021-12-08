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
float progress;
float4 uColor;
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

const float yFadeDist = 0.6f;
const float xFadeDist = 0.2f;
float4 MainPS(VertexShaderOutput input) : COLOR0
{    
    float2 textureCoords = float2((input.TextureCoordinates.x * xMod) + progress, input.TextureCoordinates.y * yMod);
    float strength = tex2D(textureSampler, textureCoords).r; //sample texture for base value
    strength = pow(strength, 3) * 2;
    
    //fade out when distance from horizontal center is too low or high
    float absYDist = 1 - (abs(input.TextureCoordinates.y - 0.5f) * 2);
    if (absYDist < yFadeDist)
        strength *= pow(absYDist / yFadeDist, 3);
    
    //fade out when distance from vertical center is too low or high
    float absXDist = 1 - (abs(input.TextureCoordinates.x - 0.5f) * 2);
    if (absXDist < xFadeDist)
        strength *= pow(absXDist / xFadeDist, 3);
    
    strength *= 1 - input.TextureCoordinates.x;
    
    strength = min(strength, 1);
    return strength * uColor * input.Color;
}

technique BasicColorDrawing
{
    pass Default
	{
        VertexShader = compile vs_2_0 MainVS();
        PixelShader = compile ps_2_0 MainPS();
    }
};