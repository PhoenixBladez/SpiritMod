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

const float maxProgress = 400.0f;
float4 ShockwaveTwo(float2 coords : TEXCOORD0) : COLOR0
{
    float2 targetCoords = (uTargetPosition - uScreenPosition) / uScreenResolution;
    //targetCoords *= (uScreenResolution / uScreenResolution.y);
    
    float2 vec = targetCoords - coords;
    float2 offset = -vec * 0.25;
    vec.x *= uScreenResolution.x;
    vec.y *= uScreenResolution.y;
    float distance = length(vec) / 1.33f;
    
    
    float distFromShockwave = pow(((distance - uProgress) / maxProgress) + 1, 2);

    float4 white = float4(1,1,1,1);
    float4 colortwo = white;
    float lerper = pow(distFromShockwave + 0.1f, 8);
    if (lerper > 1)
    {
        colortwo *= lerp(white, float4(0, 0, 0, 0), lerper - 1);
        offset *= lerper * 1.6;
    }
    else
    {
        colortwo *= lerp(float4(uColor, 0), white, lerper);
        offset *= lerper * lerper;

    }
    float lerper2 = pow(distFromShockwave, 2);
    colortwo = lerp(float4(0,0,0,0), colortwo, lerper2);
    if (lerper > 2)
    {
        colortwo *= 0;
        offset *= 0;
    }
    float prog = (float)max((maxProgress - uProgress), 0) / maxProgress;
    colortwo *= prog;
    offset *= pow(prog, 3)*(1-prog) * 10;
    
    float4 color = float4(tex2D(uImage0, coords + offset * 0.5).r, tex2D(uImage0, coords + offset).g, tex2D(uImage0, coords + offset * 2).b, 1.0);

    return color + (colortwo * 0.9f);
}

technique Technique1
{
    pass PulsarShockwave
    {
        PixelShader = compile ps_2_0 ShockwaveTwo();
    }
}