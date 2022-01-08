sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
matrix WorldViewProjection;

float progress;

float opacity;

float4 white = float4(1, 1, 1, 1);

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
    float4 color;

    float4 edge = lerp(float4(1, 1, 1, 1), white, input.TextureCoordinates.x);
    if (input.TextureCoordinates.y < 0.94f)
        color = lerp(input.Color, edge, pow(input.TextureCoordinates.y + 0.06f, 4.5f));
    else
        color = lerp(edge, float4(0, 0, 0, 0), (input.TextureCoordinates.y - 0.94f) / 0.06f);
    return (pow(input.TextureCoordinates.x, 2) * input.TextureCoordinates.y) * 4 * color * opacity;
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