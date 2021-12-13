sampler uImage0 : register(s0); // The contents of the screen
sampler uImage1 : register(s1);

float rotation;

float4 White(float2 uv : TEXCOORD0) : COLOR0
{
    float2x2 rotate = float2x2(cos(rotation), -sin(rotation), sin(rotation), cos(rotation));

    float4 color = tex2D(uImage0, mul((uv + float2(-0.5, -0.5)), rotate) + float2(0.5, 0.5));
    return color;
}

technique Technique1
{
    pass White
    {
        PixelShader = compile ps_2_0 White();
    }
}