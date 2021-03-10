matrix WorldViewProjection;
texture noiseTexture;
float progress;
float3 uColor;
float3 arcLashColorTwo;
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
sampler noiseSampler = sampler_state
{
    Texture = (noiseTexture);
};

float CalcBand(float2 coord, float speed)
{
    float2 noiseCoord = float2((coord.x + progress * speed) * (progress), progress / 5);
	float2 noiseCoord2 = float2((coord.x + progress * speed) * (progress), 1 - (progress / 5));
    float height = tex2D(noiseSampler, noiseCoord).r;
	float height2 = tex2D(noiseSampler, noiseCoord2).r;
	height2 += 0.5;
	height *= height;
    if (coord.y < height || coord.y > height2)
    {
        return float4(0.0, 0.0, 0.0, 0.0);
    }
    float dist = 0.88 - height;
    float alpha = (coord.y - height) / dist;
    float bottomHeight = 0.88 + 0.18 * height;
    float bottomHeight2 = bottomHeight + 0.18;
    if (coord.y > bottomHeight)
    {
        if (coord.y > bottomHeight2)
        {
            return float4(0.0, 0.0, 0.0, 0.0);
        }
        alpha *= (bottomHeight2 - coord.y) / 0.18;
    }
    noiseCoord.y += 0.6;
    return alpha;
}

float4 Aurora(float2 coord : TEXCOORD0) : COLOR
{
    float value = 0.0;
    float speed = 0.0;

    for (int i = 0; i < 2; i++)
    {
        value += CalcBand(coord, speed);
        coord.x += 0;
        speed += 0;
    }

    value /= 2.0;

	 float3 diff = arcLashColorTwo - uColor;
    float3 withDiff = uColor + diff * (coord.y * 3);
    return float4(withDiff.r, withDiff.g, withDiff.b, 1.0) * value;
}

float4 AuroraPS(VertexShaderOutput input) : COLOR
{
    float2 coordSample = float2(input.TextureCoordinates.x * 0.08 + 0.00035 * progress, 0.1);
    float value = tex2D(noiseSampler, coordSample);
    float2 newCoord = float2(input.TextureCoordinates.x, input.TextureCoordinates.y + 0.3 * value);

    return Aurora(newCoord) * 0.7;
}

technique BasicColorDrawing
{
	pass DefaultPass
	{
		VertexShader = compile vs_2_0 MainVS();
	}
	pass MainPS
	{
		PixelShader = compile ps_2_0 AuroraPS();
	}
}