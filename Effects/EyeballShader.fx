sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float progress;
float3 uColor;
matrix WorldViewProjection;
texture tentacleTexture;
sampler vineSampler = sampler_state
{
    Texture = (tentacleTexture);
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
    float4 color = tex2D(vineSampler, float2((input.TextureCoordinates.x * (progress + 0.1f)) % 1, input.TextureCoordinates.y));
	return color * float4(uColor,1);
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