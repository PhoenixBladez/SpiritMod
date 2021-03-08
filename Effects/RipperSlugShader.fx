sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float progress;
float3 uColor;
matrix WorldViewProjection;
texture ripperTexture;
texture vnoise;
sampler ripperSampler = sampler_state
{
    Texture = (ripperTexture);
};
sampler voronoiSampler = sampler_state
{
    Texture = (vnoise);
};
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
    float4 color = tex2D(ripperSampler, input.TextureCoordinates);
    float noiseColor = tex2D(voronoiSampler,  input.TextureCoordinates / 3).r;
    color *= sqrt(noiseColor);
	return color;
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