sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float progress;
float3 uColor;
matrix WorldViewProjection;
texture spotTexture;
sampler spotSampler = sampler_state
{
    Texture = (spotTexture);
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

    coords.x = ((coords.x - 0.5f) / 2.0f) + 0.5f;
    float4 color2 = tex2D(noiseSampler, coords);

    return float4((color2.xyz) * input.Color * (1.0 + color2.x * 2.0), color2.x * input.Color.w);
}

technique BasicColorDrawing
{
    pass DefaultPass
	{
		VertexShader = compile vs_2_0 MainVS();
	}
    pass Edge2
    {
        PixelShader = compile ps_2_0 Basic2();
    }
};