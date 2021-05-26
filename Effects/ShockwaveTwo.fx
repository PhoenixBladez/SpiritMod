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
    float4 color = tex2D(uImage0, coords);
    float2 vec = targetCoords - coords;
    vec.x *= uScreenResolution.x;
    vec.y *= uScreenResolution.y;
    float distance = length(vec) / 2.0f;
    
    float distFromShockwave = pow(((distance - uProgress) / maxProgress) + 1, 2);
    float4 white = float4(1,1,1,1);
    float4 colortwo = white;
    float lerper = pow(distFromShockwave + 0.1f, 8);
    if (lerper > 1)
         colortwo *= lerp(white,float4(0,0,0,0), lerper - 1);
    else
        colortwo *= lerp(float4(uColor, 0),white, lerper);
    float lerper2 = pow(distFromShockwave, 2);
    colortwo = lerp(float4(0,0,0,0), colortwo, lerper2);
    if (lerper > 2)
        colortwo *= 0;
    colortwo *= (float)max((maxProgress - uProgress), 0) / maxProgress;
    return color + colortwo;
}

technique Technique1
{
    pass ShockwaveTwo
    {
        PixelShader = compile ps_2_0 ShockwaveTwo();
    }
}