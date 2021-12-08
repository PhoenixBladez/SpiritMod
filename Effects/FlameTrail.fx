sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
matrix WorldViewProjection;
texture uTexture;
texture uTexture2;
sampler textureSampler = sampler_state
{
    Texture = (uTexture);
    AddressU = wrap;
    AddressV = wrap;
};
sampler texture2Sampler = sampler_state
{
    Texture = (uTexture2);
};
float Progress;
float4 StartColor;
float4 MidColor;
float4 EndColor;
float xMod;

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
    float4 color;
    if (input.TextureCoordinates.y <= 0.4f)
        color = lerp(StartColor, MidColor, input.TextureCoordinates.y * 2.5f);
    else
        color = lerp(MidColor, EndColor, (input.TextureCoordinates.y - 0.4f) * 1.66f);
        
    float2 texcoords = float2((xMod * (input.TextureCoordinates.y + Progress)) % 1, input.TextureCoordinates.x % 1); //main texture sampling coordinates, scrolls horizontally
    float2 bloomcoords = float2(input.TextureCoordinates.y, input.TextureCoordinates.x); //bloom texture coordinates
    float colorstrength = tex2D(textureSampler, texcoords).r; //use main texture coordinates as base strength
    colorstrength = min(colorstrength * 2 + pow(min(tex2D(texture2Sampler, bloomcoords).r * 3, 1.1f), 2), 2); //add bloom texture whiteness to color multiplier, with a cap
    color *= colorstrength;
    float4 final = color * input.Color;
    if (input.TextureCoordinates.y > 0.7f)
        final *= 1 - (input.TextureCoordinates.y - 0.7f) * 3;
    
    final.a *= (1 - input.TextureCoordinates.y)/2 + 0.5f;

    return final;
}

technique BasicColorDrawing
{
    pass PrimitiveTextureMap
	{
        VertexShader = compile vs_2_0 MainVS();
        PixelShader = compile ps_2_0 MainPS();
    }
};