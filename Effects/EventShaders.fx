sampler uImage0 : register(s0);

float4x4 MATRIX;

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
};

texture NoiseTexture;
sampler NoiseSampler = sampler_state
{
    Texture = (NoiseTexture);
    AddressU = Wrap;
    AddressV = Wrap;
};

/* BLOOD BLOSSOM EVENT SHADER */
float FieldOpacity;
float FieldRadius; 
float4 FieldColour;
float2 FieldCenter;
float2 ScreenPosition;
float2 ScreenSize;
float4 BloodBlossom(VertexShaderOutput input) : COLOR0
{
    float4 original = float4(0.0, 0.0, 0.0, 0.0);

    float2 worldPos = ScreenPosition + ScreenSize * input.TexCoord;
    float2 vec = worldPos - FieldCenter;
    float angle = atan2(vec.y, vec.x);
    float radiusNoise = tex2D(NoiseSampler, float2(angle * 0.4f, 0.2f)).r - 0.5f * 2.0f; //todo: add time
    float worldNoiseR = tex2D(NoiseSampler, (worldPos / ScreenSize) * 3.0f).r;

    //float worldNoiseG = tex2D(NoiseSampler, (worldPos / ScreenSize) * 3.0f + float2(0.0f, 0.33f)).r;
    //float worldNoiseB = tex2D(NoiseSampler, (worldPos / ScreenSize) * 3.0f + float2(0.0f, 0.66f)).r;

    float dist = length(vec);
    float2 pixelSize = float2(1.0f / ScreenSize.x, 1.0f / ScreenSize.y);
    float radiusOffset = radiusNoise * 16.0f * (FieldRadius / 256.0f);
    float radius = FieldRadius + radiusOffset;
    if (dist < radius)
    {
        float op = dist / radius;
        op *= 1.0f - (max(0.0f, (dist - (radius - 8.0f))) / 8.0f);
        float globalStrength = FieldOpacity * op;
        float noiseAsAngleR = -3.14159f * worldNoiseR + 6.2831f;

        //float noiseAsAngleG = -3.14159f * worldNoiseG + 6.2831f;
        //float noiseAsAngleB = -3.14159f * worldNoiseB + 6.2831f;
        //original.r = tex2D(uImage0, input.TexCoord + float2(cos(noiseAsAngleR), sin(noiseAsAngleR)) * globalStrength * pixelSize * 25.0f).r;
        //original.g = tex2D(uImage0, input.TexCoord + float2(cos(noiseAsAngleG), sin(noiseAsAngleG)) * globalStrength * pixelSize * 25.0f).g;
        //original.b = tex2D(uImage0, input.TexCoord + float2(cos(noiseAsAngleB), sin(noiseAsAngleB)) * globalStrength * pixelSize * 25.0f).b;

        original = tex2D(uImage0, input.TexCoord + float2(cos(noiseAsAngleR), sin(noiseAsAngleR)) * globalStrength * pixelSize * 25.0f);
        original = lerp(original, lerp(original, FieldColour, 0.2f), globalStrength);
    }
    else
    {
        original = tex2D(uImage0, input.TexCoord);
    }

    return original;
}

/*
VertexShaderOutput BasicVShader(float4 position : SV_POSITION, float4 color : COLOR0, float2 texCoord : TEXCOORD0)
{
    VertexShaderOutput output = (VertexShaderOutput)0;

    output.Position = mul(position, MATRIX);
    output.TexCoord = texCoord;

    return output;
}
*/

technique Technique1
{
    pass BloodBlossomScreen
    {
        //VertexShader = compile vs_3_0 BasicVShader();
        PixelShader = compile ps_2_0 BloodBlossom();
    }
}
