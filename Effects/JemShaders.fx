sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float distanceVar;
float rotation;
float opacity2;
float4 colorMod;
float counter;
texture noise;
texture map;

float alpha;
float coloralpha;
float shineSpeed;
float3 lightColour;
float shaderLerp;
sampler noiseSampler = sampler_state
{
    Texture = (noise);
};
sampler tent = sampler_state
{
    Texture = (map);
};
float hue2rgb(float p, float q, float t){
            if(t < 0) t += 1;
            if(t > 1) t -= 1;
            if(t < 0.166f) return p + (q - p) * 6.0f * t;
            if(t < 0.5f) return q;
            if(t < 0.66f) return p + (q - p) * (0.66f - t) * 6.0f;
            return p;
        }
float3 hslToRgb(float h, float s, float l){
    float r, g, b;
        float q = l < 0.5 ? l * (1 + s) : (l + s) - (l * s);
        float p = (2 * l) - q;
        r = hue2rgb(p, q, h + 0.33f); 
        g = hue2rgb(p, q, h);
        b = hue2rgb(p, q, h - 0.33f); 
	return float3(r,g,b);
}
float2 rotate(float2 coords, float delta)
{
    float2 ret;
    ret.x = (coords.x * cos(delta)) - (coords.y * sin(delta));
    ret.y = (coords.x * sin(delta)) + (coords.y * cos(delta));
    return ret;
}
float4 Godrays(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float2 noiseCoords = float2((distanceVar / 10), 0);
    float angle = atan2(coords.x - 0.5f, coords.y - 0.5f) + rotation;
    noiseCoords = rotate(noiseCoords, angle);
    float4 noiseColor = tex2D(noiseSampler, noiseCoords + float2(0.5f,0.5f));
    float2 colorvector = float2(coords.x - 0.5f, coords.y - 0.5f);
    float colordist = opacity2 / sqrt(sqrt((colorvector.x * colorvector.x) + (colorvector.y * colorvector.y)));
    color = colorMod * color.r * noiseColor.r * noiseColor.r * noiseColor.r * colordist;
    return color;
}
float4 PrismShader(float2 coords : TEXCOORD0) : COLOR0
{
    float3 prismColor = hslToRgb(((coloralpha / 9)  + (coords.y / 1.5f)) % 1, 1, 0.7f);
    float4 colour = tex2D(uImage0, coords);
        colour.rgb *= prismColor;
    float4 colour2 = tex2D(tent, coords);
    float pos = alpha - coords.x;
    float4 white = float4(1, 1, 1,1);
    if (colour.a > 0)
    {
        float clamper = clamp(0.8f - distance(alpha* shineSpeed, coords.x)*2,0,1)* colour2.r;
        colour.rgb = lerp(colour,white, clamper);
        colour.rgb *= shaderLerp;
    }
    colour.a *= 0.5f;

    return colour;
}
technique BasicColorDrawing
{
    pass Godrays
    {
        PixelShader = compile ps_2_0 Godrays();
    }
    pass PrismShader
    {
        PixelShader = compile ps_2_0 PrismShader();
    }
};