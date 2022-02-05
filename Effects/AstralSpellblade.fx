sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
matrix WorldViewProjection;
texture baseTexture;
sampler baseSampler = sampler_state
{
    Texture = (baseTexture);
    AddressU = wrap;
    AddressV = wrap;
};
float4 baseColorDark;
float4 baseColorLight;

texture overlayTexture;
sampler overlaySampler = sampler_state
{
    Texture = (overlayTexture);
    AddressU = wrap;
    AddressV = wrap;
};
float4 overlayColor;

float Progress;

float xMod;
float yMod;

float2 overlayCoordMods;
float timer;
float progress;

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
    VertexShaderOutput output = (VertexShaderOutput) 0;
    float4 pos = mul(input.Position, WorldViewProjection);
    output.Position = pos;
    
    output.Color = input.Color;

    output.TextureCoordinates = input.TextureCoordinates;

    return output;
};

const float FadeOutRangeX = 1;
const float FadeOutRangeY = 1;

float4 FadeOutColor(float4 inputColor, float fadeProgress)
{
    return float4(inputColor.rgb * lerp(1, pow(inputColor.a, 0.75f), 1 - fadeProgress), inputColor.a); //Raise base color to a power based on fade progress, to make the less opaque parts fade out first
}

float4 MainPS(VertexShaderOutput input) : COLOR0
{
    float4 color = input.Color;
    float4 textureColor;
    float strength = 1;
    
    
    float2 baseTexCoords = float2((input.TextureCoordinates.x - timer) * xMod, input.TextureCoordinates.y * yMod);
    float2 overlayTexCoords = float2((input.TextureCoordinates.x - (timer * 0.33f)) * overlayCoordMods.x, (input.TextureCoordinates.y - (timer * 0.5f)) * overlayCoordMods.y);
    
    //add base texture color
    float samplerStrength = tex2D(baseSampler, baseTexCoords).r;
    textureColor = lerp(baseColorLight, baseColorDark, pow(1 - input.TextureCoordinates.x, 0.33f)) * samplerStrength;
    
    //fade out based on position
    if (input.TextureCoordinates.x + (1 - progress) < FadeOutRangeX) //horizontal
    {
        float fadeProgress = input.TextureCoordinates.x;
        strength *= pow(fadeProgress, 1.5f);
        
        textureColor = FadeOutColor(textureColor, fadeProgress);
    }
    
    //vertical
    float yAbsDist = 1 - (2 * abs(input.TextureCoordinates.y - 0.5f));
    float fadeProgressY = pow(yAbsDist / FadeOutRangeY, 0.66f);
    strength *= pow(fadeProgressY, 1.5f);

    textureColor = FadeOutColor(textureColor, fadeProgressY);
    
    //add overlay color
    textureColor = lerp(textureColor, overlayColor, pow(tex2D(overlaySampler, overlayTexCoords).r, lerp(50, 15, strength))); //raised to absurdly high power to create smaller stars, without making them too close to each other

    float4 finalColor = color * textureColor * strength;
    return pow(finalColor, 1.2f) * 5; //final band-aid fix to make colors more intense
}

technique BasicColorDrawing
{
    pass DefaultPass
	{
        VertexShader = compile vs_2_0 MainVS();
        PixelShader = compile ps_2_0 MainPS();
    }
};