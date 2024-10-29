sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;
float2 uTargetPosition;
float4 uLegacyArmorSourceRect;
float2 uLegacyArmorSheetSize;
    
float4 ArmorBasic(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    color.rgb *= uColor;
    return color * sampleColor;
}
float4 ArmorColorGradient(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float luminosity = (color.r + color.g + color.b) / 10;
    color.rgb *= ((coords.x * uColor) + ((1 - coords.x) * uSecondaryColor)) * luminosity;
    return color * sampleColor;
}
float4 ArmorRadar(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    //float wave = 1 - frac(coords.x + uTime);
    float wave = 1 - frac(coords.x + uTime * 0.5f); // This would be half as fast.
    //float wave = 1 - frac(coords.x + uTime * 2); // This would be twice as fast. Or half as slow. Whatever takes your fancy.
    color.rgb = color.rgb * wave;
    return color * sampleColor;
}
float4 ArmorCrystalEffect(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float luminosity = (color.r + color.g + color.b) / 10;
    color.rgb *= (((coords.x * uColor) + ((1 - coords.x) * uSecondaryColor)) * luminosity);

    //float wave = 1 - frac(coords.x + uTime * 2); // This would be twice as fast. Or half as slow. Whatever takes your fancy.
    //color.rgb = color.rgb * wave;
    
    float altTime = sin(uTime) * 0.3f + .75f;
    color.rgb *= altTime;
    
    return color * sampleColor;
}
    
technique Technique1
{
    pass ArmorBasic
    {
        PixelShader = compile ps_2_0 ArmorBasic();
    }
    pass ArmorColorGradient
    {
        PixelShader = compile ps_2_0 ArmorColorGradient();
    }
    pass ArmorRadar
    {
        PixelShader = compile ps_2_0 ArmorRadar();
    }
    pass ArmorCrystalEffect
    {
        PixelShader = compile ps_2_0 ArmorCrystalEffect();
    }
}