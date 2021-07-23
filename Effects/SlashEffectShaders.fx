matrix WorldViewProjection;
float progress;
float3 uColor;
texture vnoise;
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
}

float4 ThrustPS(VertexShaderOutput input) : COLOR
{
    float2 coords = input.TextureCoordinates;
    float2 noiseCoords = float2((((2 * coords.y + 1) * 2 * abs(0.5f - coords.x)) % 1) * 0.33f, ((-(coords.y - progress)) % 5) / 5);
    float4 noiseColor = tex2D(voronoiSampler, noiseCoords);
    float4 origColor = input.Color;
    float edgeopacity = (2 * abs(0.5f - (((2 * coords.y + 1) * 2 * abs(0.5f - coords.x)) % 1)));
    return pow(origColor * (max(noiseColor.r * 4, edgeopacity * 1.5f)), 3) * min(4 * coords.y, 2) * edgeopacity;
}

technique BasicColorDrawing
{
	pass DefaultPass
	{
		VertexShader = compile vs_2_0 MainVS();
	}
	pass ThrustPS
	{
		PixelShader = compile ps_2_0 ThrustPS();
	}
}