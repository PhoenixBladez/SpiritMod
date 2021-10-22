sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
matrix WorldViewProjection;
float Size;
float RingWidth;
float Angle;

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

float WrapAngle(float angle)
{
    if (angle > 3.14f)
        angle -= 6.28f;
    
    if (angle < -3.14f)
        angle += 6.28f;
    
    return angle;
}

float4 MainPS(VertexShaderOutput input) : COLOR0
{
    float distance = 2 * sqrt(pow(input.TextureCoordinates.x - 0.5, 2) + pow(input.TextureCoordinates.y - 0.5, 2));
    distance *= Size;
    float halfWidth = RingWidth;
    if (distance > Size || distance < (Size - (2 * halfWidth))) //transparent if too much distance from center, as the shader is being applied to a square
        return float4(0, 0, 0, 0);
    
    float distMod = pow((1 - (abs(distance - (Size - halfWidth)) / halfWidth)), 0.75f); //smooth out color to be more transparent towards edge
    float coordAngle = atan2(input.TextureCoordinates.y - 0.5f, input.TextureCoordinates.x - 0.5f);
    
    float angleMod = abs(WrapAngle(Angle - coordAngle)) * 2; //make color more transparent when further from desired angle
    angleMod = pow(abs(min(angleMod, 1)), 3); //cap at 1, then raise to a power to make the fadeout stronger
    return input.Color * distMod * angleMod;
}

technique BasicColorDrawing
{
    pass DefaultPass
	{
        VertexShader = compile vs_2_0 MainVS();
        PixelShader = compile ps_2_0 MainPS();
    }
};