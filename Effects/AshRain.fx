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

const float downscale = 3;
float PerlinMod(float2 coords) //2 perlin noise overlays moving at different speeds, with calculations to make one have more binary color values
{
    float noiseA = tex2D(uImage1, (float2(coords.x + uProgress / 5, coords.y) * downscale) % 1).r;
    float noiseB = tex2D(uImage1, (float2(coords.x + uProgress / 15, coords.y) * downscale) % 1).r;

    return 0.1f + min(noiseA + pow(noiseB, 2), 1) * 0.8f;
}

float NoiseMod(float2 coords) //sin wave noise
{
    coords = (coords * downscale * 3) % 1;
    float xcoord = (coords.x + uProgress) % 1;
    float ycoord = (sin(uProgress + xcoord) / 5) + coords.y;
    float4 noiseColor = tex2D(uImage2, float2(xcoord, ycoord) % 1);
    return pow(max(noiseColor.r, noiseColor.b), 3) * 5;
}

float4 MainPS(float2 coords : TEXCOORD0) : COLOR0
{
    float2 targetCoords = (uTargetPosition - uScreenPosition) / uScreenResolution;
    float2 vec = targetCoords - coords;
    
    float4 origColor = tex2D(uImage0, coords);
    float2 resolution = uScreenResolution / (max(uScreenResolution.x, uScreenResolution.y));
    coords = (coords - vec + (0.5f * (uZoom - 1))) * resolution / uZoom;
    float strength = PerlinMod(coords);
    strength += NoiseMod(coords);
    strength = min(strength, 1);
    strength *= uIntensity;
    
    return lerp(origColor, float4(uColor, 1), strength);
}


technique Technique1
{
    pass AshRain
    {
        PixelShader = compile ps_2_0 MainPS();
    }
}