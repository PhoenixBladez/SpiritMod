sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

texture uTexture;
sampler textureSampler
{
    Texture = (uTexture);
    addressU = wrap;
    addressV = wrap;
};

texture distortTexture;
sampler distortSampler
{
    Texture = (distortTexture);
    addressU = wrap;
    addressV = wrap;
};

float timer;
float intensity;

float4 lightColor;
float4 darkColor;
float coordMod;

const float centerRadius = 0.6f;

float EaseInCirc(float x)
{
    return 1 - sqrt(1 - pow(x, 2));
}

float4 Center(float2 coords, float dist)
{
    float compression = coordMod * (1 + dist);
    float2 newCoords = float2(((coords.x - 0.5f) * compression) + 0.5f + (timer / 2), ((coords.y - 0.5f) * compression) + 0.5f + timer);
    float4 noise = tex2D(textureSampler, newCoords);

    float strength = pow(noise.r, 0.5f);
    float4 color = lerp(darkColor, lightColor, (strength + dist) / 2);
    
    strength = (strength * 0.4f) + 0.6f; //bias towards white
    strength *= intensity * 0.8f; //darker towards center than towards edges
    return float4(color.rgb * strength, 1);
}

float4 MainPS(float2 coords : TEXCOORD0, float4 ocolor : COLOR0) : COLOR0
{
    float2 distortionFactor = tex2D(distortSampler, float2(coords.x / 3, (coords.y + timer) / 3)).rg - float2(0.5f, 0.5f);
    distortionFactor *= 0.07f;
    
    float2 distortedCoords = float2(coords.x + distortionFactor.x, coords.y + distortionFactor.y);
    float4 texColor = tex2D(uImage0, distortedCoords);
    
    float distFromCenter = 2 * sqrt(pow(distortedCoords.x - 0.5f, 2) + pow(distortedCoords.y - 0.5f, 2));
    
    if (distFromCenter <= centerRadius)
    {
        float lerpRate = EaseInCirc(distFromCenter / centerRadius);
        return lerp(Center(distortedCoords, lerpRate), intensity * texColor * lightColor, lerpRate);
    }
    
    float4 final = intensity * float4(texColor.rgb, texColor.r) * lightColor; //become more additive as used texture fades out
    return final;
}

technique BasicColorDrawing
{
    pass MainPS
    {
        PixelShader = compile ps_2_0 MainPS();
    }
};