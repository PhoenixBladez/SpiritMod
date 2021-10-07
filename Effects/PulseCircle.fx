sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
matrix WorldViewProjection;
float4 RingColor;
float4 BaseColor;

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

float4 LerpColor3(float4 StartColor, float4 MidColor, float progress)
{
    if (progress < 0.5)
        return lerp(StartColor, MidColor, progress * 2);
    
    return lerp(MidColor, float4(0, 0, 0, 0), (progress - 0.5) * 2);
}

float AverageAlpha(float3 inputColor)
{
    return (inputColor.r + inputColor.g + inputColor.b) / 3;
}

const float ringStart = 0.08f;
const float ringEnd = 0.12f;
float4 MainPS(VertexShaderOutput input) : COLOR0
{
    float4 midColor = BaseColor;
    float4 ringColor = RingColor;
    
    float4 finalColor;
    
    float distance = 1 - (2 * sqrt(pow(input.TextureCoordinates.x - 0.5, 2) + pow(input.TextureCoordinates.y - 0.5, 2)));
    if (distance <= 0) //transparent if too much distance from center, as the shader is being applied to a square
        return float4(0, 0, 0, 0);
    else if (distance > ringStart && distance < ringEnd) //always return peak opacity within the specified range
        finalColor = RingColor;
    else if (distance <= ringStart)//interpolate to transparent towards the edge of the coordinates if below the start
    {
        float lerpFactor = pow((ringStart - distance) / (ringStart), 3);
        finalColor = LerpColor3(ringColor, midColor, lerpFactor);
    }
    else //interpolate to transparent towards the center of the coordinates if above the end
    {
        float lerpFactor = pow((distance - ringEnd) / (1 - ringEnd), 0.25f);
        finalColor = LerpColor3(ringColor, midColor, lerpFactor);
    }
    
    finalColor *= input.Color;
    finalColor.a = AverageAlpha(finalColor.rgb);
    return finalColor;
}

technique BasicColorDrawing
{
    pass PrimitiveTextureMap
	{
        VertexShader = compile vs_2_0 MainVS();
        PixelShader = compile ps_2_0 MainPS();
    }
};