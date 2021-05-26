const float4 codedColor = float4(0.0, 1.0, 0.0, 1.0);

sampler2D SpriteTextureSampler;

texture GalaxyTexture0;

sampler2D GalaxySampler0 = sampler_state
{
    Texture = <GalaxyTexture0>;
	AddressU = wrap;
	AddressV = wrap;
};

texture GalaxyTexture1;

sampler2D GalaxySampler1 = sampler_state
{
    Texture = <GalaxyTexture1>;
	AddressU = wrap; 
	AddressV = wrap;
};

texture GalaxyTexture2;

sampler2D GalaxySampler2 = sampler_state
{
    Texture = <GalaxyTexture2>;
	AddressU = wrap;
	AddressV = wrap;
};


float screenWidth;
float screenHeight;
float width;
float height;
float2 offset;
float time;

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);

    float2 coords = input.TextureCoordinates;
    coords.x *= screenWidth / width;
    coords.y *= screenHeight / height;

    float2 newOffset = offset;

    newOffset.x /= width;
    newOffset.y /= height;

    newOffset += float2(cos(time * 0.01), sin(time * 0.01)) * 0.05;

    if (color.r == codedColor.r && color.g == codedColor.g && color.b == codedColor.b && color.a == codedColor.a)
    {
        float4 parallaxColor = tex2D(GalaxySampler0, coords + newOffset * 0.2);

        return parallaxColor + tex2D(GalaxySampler1, coords + newOffset * 0.8) + tex2D(GalaxySampler2, coords + newOffset * 1.3);
    }

    return color;
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile ps_2_0 MainPS();
    }
};
