sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float progress;
matrix WorldViewProjection;

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
texture vnoise;
sampler tent = sampler_state
{
    Texture = (vnoise);
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
    input.Color *= input.TextureCoordinates.x;
    float x = (input.TextureCoordinates.x + sin(progress)) % 1;
    float2 noisecoords = float2(x, input.TextureCoordinates.y);
    input.Color *= (1 - (2 * abs(0.5f - input.TextureCoordinates.y))) *tex2D(tent,noisecoords).r * 2;
    
    return input.Color;
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