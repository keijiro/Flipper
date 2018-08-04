Shader "Hidden/Flipper/SimpleBlit"
{
    Properties
    {
        _MainTex("", 2D) = "black"{}
        _Opacity("", Range(0, 1)) = 1
        _Glitch("", Range(0, 1)) = 0
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D _MainTex;
    float4 _MainTex_TexelSize;
    fixed _Opacity;
    fixed _Glitch;
    uint _FrameCount;

    // Hash function from H. Schechter & R. Bridson, goo.gl/RXiKaH
    uint Hash(uint s)
    {
        s ^= 2747636419u;
        s *= 2654435769u;
        s ^= s >> 16;
        s *= 2654435769u;
        s ^= s >> 16;
        s *= 2654435769u;
        return s;
    }

    float Random(uint seed)
    {
        return float(Hash(seed)) / 4294967295.0; // 2^32-1
    }

    float4 Vertex(float4 position : POSITION, inout float2 uv : TEXCOORD0) : SV_Position
    {
        return UnityObjectToClipPos(position);
    }

    fixed4 Fragment(float4 position : SV_Position, float2 uv : TEXCOORD0) : SV_Target
    {
        uint seed = uv.y * _MainTex_TexelSize.w / 8;
        seed += _FrameCount * 2048;

        float delta = (Random(seed) - 0.5) * 2;
        delta = delta * delta * delta * _Glitch;

        uv.x += delta;

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
