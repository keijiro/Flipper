Shader "Hidden/Flipper/WallFx"
{
    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            CGPROGRAM

            #pragma vertex Vertex
            #pragma fragment Fragment

            #include "UnityCG.cginc"

            float4 Vertex(float4 position : POSITION, inout float2 uv : TEXCOORD0) : SV_Position
            {
                return UnityObjectToClipPos(position);
            }

            fixed4 Fragment(float4 position : SV_Position, float2 uv : TEXCOORD0) : SV_Target
            {
                return frac(_Time.y);
            }

            ENDCG
        }
    }
}
