sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;

float Radius;

texture NoiseTexture;
sampler NoiseSampler
{
    Texture = (NoiseTexture);
};

const float NoiseFadeDist = 0.75f;
const float WhiteFadeDist = 0.75f;
const float BlackFadeDist = 0.95f;

const float BlackOpacity = 0.825f;


float4 MainPS(float2 coords : TEXCOORD0) : COLOR0
{
    float4 origColor = tex2D(uImage0, coords);
    float2 targetCoords = (uTargetPosition - uScreenPosition) / uScreenResolution;
    float2 vec = (targetCoords - coords) * uScreenResolution;
    float dist = sqrt(pow(vec.x, 2) + pow(vec.y, 2));
    float noiseStrength = 0;
    
    if (dist > Radius * NoiseFadeDist)
    {
        float noiseY = (dist - (Radius * NoiseFadeDist)) / (Radius * (1 - NoiseFadeDist));
        noiseStrength = min(1, noiseY * noiseY) * 0.5f;
        float noiseX = (atan2(vec.y, vec.x) + 3.14f) / 6.28f;
        float2 noiseCoords = float2((noiseX * 12) % 1, (noiseY + uTime * 0.5f) % 1);
        float4 noiseColor = tex2D(NoiseSampler, noiseCoords);

        noiseStrength += noiseColor.r * noiseStrength;
    }
    
    if (dist > Radius * BlackFadeDist)
    {
        float blackStrength = min(1, pow((dist - (Radius * BlackFadeDist)) / (Radius * (1 - BlackFadeDist)), 2)) * BlackOpacity;
        noiseStrength = BlackOpacity - blackStrength;
    }
    
    origColor = lerp(origColor, float4(uColor, 0), noiseStrength * uOpacity);
    return origColor;
}

float4 FadePS(float2 coords : TEXCOORD0) : COLOR0
{
    float4 origColor = tex2D(uImage0, coords);
    float2 targetCoords = (uTargetPosition - uScreenPosition) / uScreenResolution;
    float2 vec = (targetCoords - coords) * uScreenResolution;
    float dist = sqrt(pow(vec.x, 2) + pow(vec.y, 2));
    float whiteStrength = 0;
    float blackStrength = 0;
    
    if (dist > Radius * WhiteFadeDist)
        whiteStrength = min(1, pow((dist - (Radius * WhiteFadeDist)) / (Radius * (BlackFadeDist - WhiteFadeDist)), 6) * 1.15f);
    
    if (dist > Radius * BlackFadeDist)
    {
        blackStrength = min(1, pow((dist - (Radius * BlackFadeDist)) / (Radius * (1 - BlackFadeDist)), 2)) * BlackOpacity;
        whiteStrength = (BlackOpacity - blackStrength) * 1.15f;
    }
    
    origColor = lerp(origColor, float4(1, 1, 1, 1), whiteStrength * uOpacity);
    origColor = lerp(origColor, float4(0, 0, 0, 1), blackStrength * uOpacity);
    return origColor;
}

technique Technique1
{
    pass MainPS
    {
        PixelShader = compile ps_2_0 MainPS();
    }

    pass FadePS
    {
        PixelShader = compile ps_2_0 FadePS();
    }
}