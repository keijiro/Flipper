Shader "Hidden/Flipper/Recolor"
{
    HLSLINCLUDE

    #include "PostProcessing/Shaders/StdLib.hlsl"
    #include "PostProcessing/Shaders/Colors.hlsl"

    TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);

    half3 _Color;
    half2 _Levels;

    half4 Frag(VaryingsDefault i) : SV_Target
    {
        half4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
        half lum = Luminance(LinearToSRGB(c));
        lum = saturate((lum - _Levels.x) / (_Levels.y - _Levels.x));
        c.rgb = _Color * lum;
        return SRGBToLinear(c);
    }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment Frag
            ENDHLSL
        }
    }
}
