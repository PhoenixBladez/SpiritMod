matrix WorldViewProjection;
float progress;
float3 uColor;
float3 outlineColor;
float3 baseColor;
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
}
float4 OutlinePS(VertexShaderOutput input) : COLOR
{
    float2 coordSample = float2(input.TextureCoordinates.x, input.TextureCoordinates.y);
    if (input.TextureCoordinates.x >= 0.9f || input.TextureCoordinates.x <= 0.1f || input.TextureCoordinates.y > 0.85f || input.TextureCoordinates.y < 0.15f)
        return float4(outlineColor, 1);
    return float4(baseColor, 1);
}

technique BasicColorDrawing
{
	pass DefaultPass
	{
		VertexShader = compile vs_2_0 MainVS();
	}
	pass MainPS
	{
		PixelShader = compile ps_2_0 OutlinePS();
	}
}