sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

texture vnoiseTex;
sampler vNoise
{
    Texture = (vnoiseTex);
    AddressU = wrap;
    AddressV = wrap;
};

float timer;
float progress;

float4 Yellow;
float4 Orange;
float4 Pink;

const float fadeDist = 0.1f;

float4 GetColor(float xCoord)
{
    float4 uColor = Yellow;
    float xProgress = (-xCoord + timer + 1) % 1;
    float cutoff1 = 0.33f;
    float cutoff2 = 0.66f;
    if (xProgress < cutoff1)
        return lerp(Yellow, Orange, xProgress * 3);
    
    else if (xProgress < cutoff2)
        return lerp(Orange, Pink, (xProgress - cutoff1) * 3);
    
    return lerp(Pink, Yellow, (xProgress - cutoff2) * 3);
}

float4 MainPS(float2 coords : TEXCOORD0) : COLOR0
{
    float4 texColor = tex2D(uImage0, coords);
    float4 uColor = GetColor(coords.x);
    
    float baseStrength = 1;
    float whiteStrength = 1;
    
    baseStrength *= pow(1 - (abs(coords.y - 0.5f) * 2), 0.75f); //Lower strength when further from vertical center
    
    if (coords.x <= fadeDist)
        baseStrength *= pow(coords.x / fadeDist, 0.25f);
    
    if (coords.x >= progress - fadeDist)
        baseStrength *= 1 - pow((coords.x - (progress - fadeDist)) / fadeDist, 4);
        
    float noisePower = 3;
    float2 noiseCoords = float2((coords.x - timer) * 2, (coords.y + timer) / 3);
    whiteStrength = pow(tex2D(vNoise, noiseCoords).r, 3 + sin(timer * 5) * 0.75f) * noisePower * baseStrength; //Add in noise
    
    baseStrength = min(baseStrength, 1);
    
    uColor = lerp(uColor, float4(0, 0, 0, 1), (1 - baseStrength) * 0.75f); //Darken based on strength
    
    return uColor * texColor * (whiteStrength + 1.5f);
}

technique BasicColorDrawing
{
    pass MainPS
    {
        PixelShader = compile ps_2_0 MainPS();
    }
};