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
float FadeDistance;


float4 MainPS(float2 coords : TEXCOORD0) : COLOR0
{
    float4 origColor = tex2D(uImage0, coords);
    float2 targetCoords = (uTargetPosition - uScreenPosition) / uScreenResolution;
    float2 vec = (targetCoords - coords) * uScreenResolution;
    float dist = sqrt(pow(vec.x, 2) + pow(vec.y, 2));
    float lerpStrength = 0;
    
    if (dist > Radius)
    {
        float DistFromRadius = dist - Radius;
        lerpStrength = clamp(DistFromRadius / FadeDistance, 0, 1);
    }
    
    origColor = lerp(origColor, float4(uColor, 0), lerpStrength * uOpacity * uIntensity);
    return origColor;
}

technique Technique1
{
    pass MainPS
    {
        PixelShader = compile ps_2_0 MainPS();
    }
}