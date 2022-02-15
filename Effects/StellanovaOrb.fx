sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

texture uTexture;
sampler textureSampler
{
    Texture = (uTexture);
    addressU = wrap;
    addressV = wrap;
};

float timer;
float intensity;

const float centerRadius = 0.57f;

float EaseInCirc(float x)
{
    return 1 - sqrt(1 - pow(x, 2));
}

float4 Center(float2 coords, float dist)
{
    float compression = 1 + dist;
    float2 newCoords = float2(((coords.x - 0.5f) * compression) + 0.5f + (timer / 2), ((coords.y - 0.5f) * compression) + 0.5f + timer);
    float4 noise = tex2D(textureSampler, newCoords);

    float strength = noise.r;
    strength = (strength * 0.4f) + 0.6f; //bias towards white
    strength *= intensity * 0.7f; //darker towards center than towards edges
    return float4(strength, strength, strength, 1);
}

float4 MainPS(float2 coords : TEXCOORD0, float4 ocolor : COLOR0) : COLOR0
{
    float4 texColor = tex2D(uImage0, coords);
    
    float distFromCenter = 2 * sqrt(pow(coords.x - 0.5f, 2) + pow(coords.y - 0.5f, 2));
    
    if (distFromCenter <= centerRadius)
    {
        float lerpRate = EaseInCirc(distFromCenter / centerRadius);
        return lerp(Center(coords, lerpRate), intensity * texColor, lerpRate) * ocolor;
    }
    
    return intensity * float4(texColor.rgb, max(texColor.r - 0.1f, 0)) * ocolor; //become more additive as used texture fades out
}

technique BasicColorDrawing
{
    pass MainPS
    {
        PixelShader = compile ps_2_0 MainPS();
    }
};