sampler s0 : register(s0);

texture noiseTexture;
sampler noiseSampler = sampler_state
{
    Texture = (noiseTexture);
};
float time;
float yCoord;
float movement;
float3 colour1;
float3 colour2;
float opacity;

float CalcBand(float2 coord, float speed)
{
    float2 noiseCoord = float2(coord.x + time * speed, yCoord);
    float height = tex2D(noiseSampler, noiseCoord).r;
    height *= height;
    if (coord.y < height)
    {
        return float4(0.0, 0.0, 0.0, 0.0);
    }
    float dist = 0.88 - height;
    float alpha = (coord.y - height) / dist;
    float bottomHeight = 0.88 + 0.06 * height;
    float bottomHeight2 = bottomHeight + 0.06;
    if (coord.y > bottomHeight)
    {
        if (coord.y > bottomHeight2)
        {
            return float4(0.0, 0.0, 0.0, 0.0);
        }
        alpha *= (bottomHeight2 - coord.y) / 0.06;
    }
    noiseCoord.y += 0.6;
    return alpha * alpha;
}

float4 Aurora(float2 coord : TEXCOORD0) : COLOR
{
    float value = 0.0;
    float speed = 0.0;

    for (int i = 0; i < 2; i++)
    {
        value += CalcBand(coord, speed);
        coord.x += 0.2;
        speed += movement;
    }

    value /= 2.0;

    float3 diff = colour2 - colour1;
    float3 withDiff = colour1 + diff * pow(coord.y, 4);
	
    return float4(withDiff.r, withDiff.g, withDiff.b, 1.0) * value;
}

float4 MainPS(float2 coord : TEXCOORD0) : COLOR
{
    float2 coordSample = float2(coord.x * 0.08 + 0.00035 * time, 0.1);
    float value = tex2D(noiseSampler, coordSample);
    float2 newCoord = float2(coord.x, coord.y + 0.3 * value);

    return Aurora(newCoord) * opacity;
}

technique BasicColorDrawing
{
    pass P0
    {
        PixelShader = compile ps_2_0 MainPS();
    }
};