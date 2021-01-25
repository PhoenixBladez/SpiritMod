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
float4 Basic2(VertexShaderOutput input) : COLOR
{
    float2 coords = float2(input.TextureCoordinates.x,input.TextureCoordinates.y);
    float4 spotColor = tex2D(spotSampler, coords).r;
    float lerper = pow(sin(input.TextureCoordinates.y * 3.14f - 1.2f),50 + GetHeight(input.TextureCoordinates.x) * 50 - 25) + pow(sin(input.TextureCoordinates.x * 3.14f - 1.2f + (progress / 3 * progress / 3)), 30 + GetHeight(input.TextureCoordinates.y / 2 + progress) * 50 - 25);
    input.Color *= lerp(float4(0,0,0,0),float4(uColor, 0) * 10, lerper);
    input.Color *= sin(input.TextureCoordinates.x * 3.14f);
    return input.Color;
}
float4 Basic3(VertexShaderOutput input) : COLOR
{
    float2 coords = float2(input.TextureCoordinates.x,input.TextureCoordinates.y);
    float4 spotColor = tex2D(spotSampler, coords).r;
    float lerper = pow(sin(input.TextureCoordinates.y * 3.14f - 1.2f),25 + GetHeight(float2(input.TextureCoordinates.x, progress % 1)) * 25 - 12.5f);
    if (input.TextureCoordinates.y < 0.1f)
        lerper = 0;
    input.Color *= lerp(float4(0,0,0,0),float4(uColor, 0) * 10, lerper);
    input.Color *= sin(input.TextureCoordinates.x * 3.14f);
    return input.Color;
}
float4 White(VertexShaderOutput input) : COLOR0
{
    input.Color *= input.TextureCoordinates.x;
    input.Color *= (1 - (2 * abs(0.5f - input.TextureCoordinates.y)));
    return input.Color;
}
float4 WhiteNoise(VertexShaderOutput input) : COLOR0
{
    float2 coords = float2(input.TextureCoordinates.x, input.TextureCoordinates.y);
    input.Color *= input.TextureCoordinates.x;
    float4 noiseColor = tex2D(noiseSampler, float2((coords.x + (progress * 0.1f)) % 1, (coords.y + (progress * 0.1f)) % 1)).r;
    input.Color *= (1 - (2 * abs(0.5f - input.TextureCoordinates.y)));
    input.Color *= noiseColor * 2;
    return input.Color * 2;
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
    pass NoiseTrail
    {
        PixelShader = compile ps_2_0 WhiteNoise();
    }
    pass Edge2
    {
        PixelShader = compile ps_2_0 Basic2();
    }
    pass Edge3
    {
        PixelShader = compile ps_2_0 Basic3();
    }
};