matrix WorldViewProjection;
float progress;
float3 uColor;
texture noise;
texture circle;
sampler noiseSampler = sampler_state
{
    Texture = (noise);
};
sampler circleSampler = sampler_state
{
    Texture = (circle);
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
float4 OutlinePS(VertexShaderOutput input) : COLOR
{ 
    float4 origColor = float4(uColor, 1);
    float2 coords = float2(input.TextureCoordinates.x, input.TextureCoordinates.y);
    float4 color = tex2D(circleSampler, coords);

    if (color.a == 0)
        return float4(0, 0, 0, 0);
    
    float xdistfromcenter = min(abs(0.5f - coords.x) * 3, 1);
    float noiseR = pow(tex2D(noiseSampler, float2(progress % 1, coords.y)), sqrt(coords.y));
    float4 transparency = (0, 0, 0, 0);
    transparency = lerp(color, transparency, xdistfromcenter * noiseR);
    
    float4 newColor = float4(origColor.r, origColor.g, origColor.b, max(origColor.a - transparency.a, 0)) * (1 - xdistfromcenter) * noiseR;
    return floor(newColor * 4) / 4;
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