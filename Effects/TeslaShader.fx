sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float progress;
matrix WorldViewProjection;
texture baseTexture;
sampler baseSampler = sampler_state
{
    Texture = (baseTexture);
};

texture vnoise;
sampler vnoiseSampler = sampler_state
{
    Texture = (vnoise);
};

texture pnoise;
sampler pnoiseSampler = sampler_state
{
    Texture = (pnoise);
};


texture wnoise;
sampler wnoiseSampler = sampler_state
{
    Texture = (wnoise);
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
	float2 uv = float2(input.TextureCoordinates.x, input.TextureCoordinates.y);
	float time3 = floor(progress * 2) / 2;
	float time2 = time3 / 15;

	time2 += floor(time3 / 5) * 0.5f;

	//uv = floor(uv * 48) / 48;

	uv.x += pow(abs(uv.y - 0.5f) * 2, 2) * clamp(tex2D(pnoiseSampler, (uv + float2(time3 / 6, 0)) % 1).r - 0.5f, -0.35f, 0.35f) * 2.5f;

	uv.y += 0.05f;

	float2 coords2 = uv;

	float offset = tex2D(pnoiseSampler, float2((uv.x * 1) % 1, time2 % 1)).r - 0.5f;
	offset += tex2D(vnoiseSampler, float2((uv.x * 1) % 1, time2 % 1)).r - 0.5f;
	coords2.y += (offset) / 2.66f;

	float absOffset = abs(offset);
	float column = float2(0, tex2D(baseSampler, coords2).r * (absOffset + 0.25f) * 1.5f);

	column *= tex2D(wnoiseSampler, uv / 4 + (float2(time2, int(time3) / 150) * 0.5f)).r * (1 - absOffset) * 2;
	column += tex2D(baseSampler, float2(0, 0.5f) + ((coords2 - 0.5f) * 2.0f)); //adds a second, tighter band

	column = pow(column, lerp(3, 6, tex2D(pnoiseSampler, float2(uv.x, time2 % 1))));

	//column.x = 0.1f + floor((time3 / 2.0f + uv.x) % 1.5f) / 2; //white flashes
	column = clamp(column, 0, 1);

	float4 color = lerp(float4(1,1,1,1), float4(33.0f / 255.0f, 1, 211.0f / 255.0f, 1), 1 - column);
	if (column > 0.7f)
		return float4(1, 1, 1, 1);
	if (column > 0.5f)
		return float4(33.0f / 255.0f, 1, 211.0f / 255.0f, 1);
	return (color * column);
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