sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
matrix WorldViewProjection;
texture uTexture;
texture uTexture2;
sampler textureSampler = sampler_state
{
    Texture = (uTexture);
};
sampler texture2Sampler = sampler_state
{
    Texture = (uTexture2);
};
float Progress;
float4 StartColor;
float4 EndColor;

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
    float4 color = lerp(StartColor, EndColor, input.TextureCoordinates.y);
    float2 texcoords = float2((input.TextureCoordinates.y + Progress) % 1, input.TextureCoordinates.x % 1); //main texture sampling coordinates, scrolls horizontally
    float2 bloomcoords = float2(input.TextureCoordinates.y, input.TextureCoordinates.x); //bloom texture coordinates
    float colorstrength = tex2D(textureSampler, texcoords).r; //use main texture coordinates as base strength
    colorstrength = min(colorstrength + min(tex2D(texture2Sampler, bloomcoords).r * 2, 0.5f), 1); //add bloom texture whiteness to color multiplier, with a cap
    color *= colorstrength;
	return color * input.Color;
}

technique BasicColorDrawing
{
    pass PrimitiveTextureMap
	{
        VertexShader = compile vs_2_0 MainVS();
        PixelShader = compile ps_2_0 MainPS();
    }
};