sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
matrix WorldViewProjection;
texture uTexture;
sampler textureSampler = sampler_state
{
    Texture = (uTexture);
    AddressU = wrap;
    AddressV = wrap;
};
float progress;
float4 uColor;
float4 uSecondaryColor;

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
    VertexShaderOutput output = (VertexShaderOutput)0;
    float4 pos = mul(input.Position, WorldViewProjection);
    output.Position = pos;
    
    output.Color = input.Color;

	output.TextureCoordinates = input.TextureCoordinates;

    return output;
};

const float fadeHeight = 0.95f;
float4 MainPS(VertexShaderOutput input) : COLOR0
{
    //Compresses the noise towards the top of the primitive by dividing based on height, and scrolls vertically
    float4 noise = tex2D(textureSampler, float2((input.TextureCoordinates.x / max(input.TextureCoordinates.y, 0.001f)) % 1, input.TextureCoordinates.y + progress));
    
    //Makes fade out based on the distance horizontally from the center, and stretches out the distance by dividing based on height
    float strength = pow(1 - (abs(input.TextureCoordinates.x - 0.5f) * 2 / max(1 - input.TextureCoordinates.y, 0.001f)), 2);
    strength *= pow(input.TextureCoordinates.y, 2); //Fade out based on how far down the pixel is
    strength *= 1.5f; //Static increase to color strength
    strength += strength * noise.r * input.TextureCoordinates.y; //Add in the noise
    strength = min(strength, 2); //Cap the strength at a certain value
    
    float4 color;
    if (strength < 0.5f) //Interpolate from transparent to first used color if strength is low
         color = lerp(float4(0, 0, 0, 0), uColor, strength * 2);
    else //If strength is high, interpolate from first used color to second
        color = lerp(uColor, uSecondaryColor, (strength - 0.5f) * 2);
    
    if (input.TextureCoordinates.y > fadeHeight) //If above a certain height value, fade out
        strength *= pow(1 - ((input.TextureCoordinates.y - fadeHeight) / (1 - fadeHeight)), 3);
        
    color *= strength;
    return input.Color * color;
}

technique BasicColorDrawing
{
    pass Default
	{
        VertexShader = compile vs_2_0 MainVS();
        PixelShader = compile ps_2_0 MainPS();
    }
};