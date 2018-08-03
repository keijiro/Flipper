Shader "Hidden/Flipper/SimpleBlit"
{
    Properties
    {
        _MainTex("", 2D) = "black"{}
        _Opacity("", Range(0, 1)) = 1
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D _MainTex;
    fixed _Opacity;

    float4 Vertex(float4 position : POSITION, inout float2 uv : TEXCOORD0) : SV_Position
    {
        return UnityObjectToClipPos(position);
    }

    fixed4 Fragment(float4 position : SV_Position, float2 uv : TEXCOORD0) : SV_Target
    {
        fixed4 c = tex2D(_MainTex, uv);
        c.rgb = LinearToGammaSpace(c.rgb);
        c.rgb *= _Opacity;
        c.rgb = GammaToLinearSpace(c.rgb);
        return c;
    }

    ENDCG

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            ENDCG
        }
    }
}
