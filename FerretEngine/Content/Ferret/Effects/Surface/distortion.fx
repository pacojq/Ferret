//------------------------------ TEXTURE PROPERTIES ----------------------------

texture ScreenTexture;
sampler TextureSampler = sampler_state { Texture = <ScreenTexture>; };

texture _MaskTexture;
sampler MaskSampler = sampler_state { Texture = <_MaskTexture>; };

float _MaskBlend = 0.1f;
float _MaskSize = 1;

float _DistortionStrength = 0.01f;
 
 
float2 CRTCurveUV( float2 coord, float str )
{
    // put in symmetrical coords
    coord = (coord - 0.5) * 2.0;

    coord *= 1.05;	

    // deform coords
    coord.x *= 1.0 + pow((abs(coord.y) / str), 2.0);
    coord.y *= 1.0 + pow((abs(coord.x) / str), 2.0);

    // transform back to 0.0 - 1.0 space
    coord  = (coord / 2.0) + 0.5;

    return coord;
}
 
//------------------------ PIXEL SHADER ----------------------------------------
// This pixel shader will simply look up the color of the texture at the
// requested point
float4 PixelShaderFunction(float2 TextureCoordinate : TEXCOORD0) : COLOR0
{        
    TextureCoordinate = CRTCurveUV(TextureCoordinate, 6);
    
    if (TextureCoordinate.x < 0 || TextureCoordinate.x > 1)
            return float4(0, 0, 0, 1);
            
    if (TextureCoordinate.y < 0 || TextureCoordinate.y > 1)
        return float4(0, 0, 0, 1);
            
    
    float4 mask = tex2D(MaskSampler, TextureCoordinate * _MaskSize);
    float4 base = tex2D(TextureSampler, TextureCoordinate);
    
    float4 col = lerp(base, mask, _MaskBlend);
    
    return col;
}

//-------------------------- TECHNIQUES ----------------------------------------
// This technique is pretty simple - only one pass, and only a pixel shader
technique Plain
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}