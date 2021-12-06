sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
matrix WorldViewProjection;
texture uTexture;
sampler2D textureSampler = sampler_state { texture = <uTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };
bool flipCoords = false;
bool additive = false;
bool intensify = false;

float progress;

float repeats = 1;

float scroll = 0;

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

float4 White(VertexShaderOutput input) : COLOR0
{
    float2 coords = float2((input.TextureCoordinates.x * repeats) + scroll, input.TextureCoordinates.y);
    if (flipCoords)
        coords = float2(input.TextureCoordinates.y * repeats, input.TextureCoordinates.x);
    
    float4 color = tex2D(textureSampler, coords);

    if (intensify)
        return float4((color.xyz * 2) * input.Color * (1.0 + color.x * 2.0), color.x * input.Color.w);

    if (additive)
        color *= (color.r + color.g + color.b) / 3;
    
    return color * input.Color;
}

technique BasicColorDrawing
{
    pass DefaultPass
    {
        VertexShader = compile vs_2_0 MainVS();
    }
    pass MainPS
    {
        PixelShader = compile ps_2_0 White();
    }
};