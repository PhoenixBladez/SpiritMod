sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float progress;
float3 uColor;
matrix WorldViewProjection;
texture spotTexture;
texture vnoise;
sampler spotSampler = sampler_state
{
    Texture = (spotTexture);
};
sampler voronoiSampler = sampler_state
{
    Texture = (vnoise);
};
texture noiseTexture;
sampler noiseSampler = sampler_state
{
    Texture = (noiseTexture);
};
float GetHeight(float2 Coord)
{
    return tex2D(noiseSampler, Coord).r;
}
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
    float2 coords = float2(input.TextureCoordinates.x,input.TextureCoordinates.y);
    float4 spotColor = tex2D(spotSampler, float2(input.TextureCoordinates.x, 0.5f)).r;
    float4 white = float4(1,1,1,1);
    float lerper = pow(coords.y + 0.1f, 8);
    if (lerper > 1)
         input.Color *= lerp(white,float4(uColor, 0), lerper - 1);
    else
        input.Color *= lerp(float4(uColor, 0),white, lerper);
    float lerper2 = pow(coords.y, 2);
    input.Color = lerp(float4(0,0,0,0), input.Color, lerper2);
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