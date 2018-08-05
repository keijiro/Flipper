Shader "Hidden/Flipper/SimpleBlit"
{
    Properties
    {
        _MainTex("", 2D) = "black"{}
        _Overlay("", 2D) = "black"{}
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D _MainTex;
    sampler2D _Overlay;

    float4 _MainTex_TexelSize;

    fixed2 _Opacity;
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

    float2 GlitchUV(float2 uv)
    {
        uint seed = uv.y * _MainTex_TexelSize.w / 8;
        seed += _FrameCount * 2048;

        float delta = (Random(seed) - 0.5) * 2;
        delta = delta * delta * delta * _Glitch;

        uv.x += delta;
        return uv;
    }

    float4 Vertex(float4 position : POSITION, inout float2 uv : TEXCOORD0) : SV_Position
    {
        return UnityObjectToClipPos(position);
    }

    fixed4 FragmentSingle(float4 position : SV_Position, float2 uv : TEXCOORD0) : SV_Target
    {
        fixed4 c = tex2D(_MainTex, GlitchUV(uv));
        c.rgb = LinearToGammaSpace(c.rgb);
        c.rgb *= _Opacity.x;
        c.rgb = GammaToLinearSpace(c.rgb);
        return c;
    }

    fixed4 FragmentDual(float4 position : SV_Position, float2 uv : TEXCOORD0) : SV_Target
    {
        uv = GlitchUV(uv);

        fixed4 c1 = tex2D(_MainTex, uv);
        fixed4 c2 = tex2D(_Overlay, uv);

        c1.rgb = LinearToGammaSpace(c1.rgb);
        c2.rgb = LinearToGammaSpace(c2.rgb);

        half3 c = c1.rgb * _Opacity.x + c2.rgb * _Opacity.y;

        return half4(GammaToLinearSpace(c), c1.a);
    }

    ENDCG

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment FragmentSingle
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment FragmentDual
            ENDCG
        }
    }
}
