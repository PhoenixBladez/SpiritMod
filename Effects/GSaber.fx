sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
matrix WorldViewProjection;
texture baseTexture;
sampler baseSampler = sampler_state
{
    Texture = (baseTexture);
    AddressU = wrap;
    AddressV = wrap;
};
float4 baseColor;

texture overlayTexture;
sampler overlaySampler = sampler_state
{
    Texture = (overlayTexture);
    AddressU = wrap;
    AddressV = wrap;
};
float4 overlayColor;

float Progress;

float xMod;
float yMod;
float timer;
float progress;

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

const float FadeOutRangeX = 1;
const float FadeOutRangeY = 0.4f;
float4 MainPS(VertexShaderOutput input) : COLOR0
{
    float4 color = input.Color;
    float4 textureColor;
    float strength = 1;
    
    //fade out based on position
    if (input.TextureCoordinates.x + (1 - progress) < FadeOutRangeX)
        strength *= pow(input.TextureCoordinates.x / FadeOutRangeX, 2);
    
    float yAbsDist = 1 - (2 * abs(input.TextureCoordinates.y - 0.5f));
    if (yAbsDist < FadeOutRangeY)
        strength *= pow(yAbsDist / FadeOutRangeY, 2);
    
    float2 baseTexCoords = float2((input.TextureCoordinates.x * xMod) - timer, input.TextureCoordinates.y * yMod);
    float2 overlayTexCoords = baseTexCoords / 2;
    
    //add base texture color
    textureColor = baseColor * tex2D(baseSampler, baseTexCoords).r;
    
    //add overlay color
    textureColor = lerp(textureColor, overlayColor, tex2D(overlaySampler, overlayTexCoords).r);

    return color * textureColor * strength;
}

technique BasicColorDrawing
{
    pass DefaultPass
	{
        VertexShader = compile vs_2_0 MainVS();
        PixelShader = compile ps_2_0 MainPS();
    }
};