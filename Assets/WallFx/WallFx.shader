Shader "Hidden/Flipper/WallFx"
{
    CGINCLUDE

    #include "UnityCG.cginc"
    #include "SimplexNoise2D.hlsl"

    fixed _Intensity;

    float4 Vertex(float4 position : POSITION, inout float2 uv : TEXCOORD0) : SV_Position
    {
        return UnityObjectToClipPos(position);
    }

    fixed4 FragmentFill(float4 position : SV_Position, float2 uv : TEXCOORD0) : SV_Target
    {
        return fixed4(GammaToLinearSpace(_Intensity), 1);
    }

    fixed4 FragmentVerticalBar(float4 position : SV_Position, float2 uv : TEXCOORD0) : SV_Target
    {
        return abs(uv.x - 0.5) * 2 < _Intensity;
    }

    fixed4 FragmentHorizontalBar(float4 position : SV_Position, float2 uv : TEXCOORD0) : SV_Target
    {
        return abs(uv.y - 0.5) * 2 < _Intensity;
    }

    fixed4 Slits(float x)
    {
        float2 p = float2(x, _Time.y * 2);
        fixed n = snoise(p * float2(8, 1)) + snoise(p * float2(23, 1)) / 2;
        n = (abs(n) / 1.4 + _Intensity / 2 - 1) * 100; 
        return fixed4(GammaToLinearSpace(saturate(n)), 1);
    }

    fixed4 FragmentVerticalSlits(float4 position : SV_Position, float2 uv : TEXCOORD0) : SV_Target
    {
        return Slits(uv.x);
    }

    fixed4 FragmentHorizontalSlits(float4 position : SV_Position, float2 uv : TEXCOORD0) : SV_Target
    {
        return Slits(uv.y);
    }

    fixed4 FragmentDiagonalSlits(float4 position : SV_Position, float2 uv : TEXCOORD0) : SV_Target
    {
        float2 p = uv - 0.5;
        float seed = (p.y > 0) * 2 + (p.x > 0);
        float x = dot(abs(p), 1) + seed * 30;
        return Slits(x * 0.4 - _Time.y * 0.2);
    }

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

    fixed4 FragmentStreamLines(float4 position : SV_Position, float2 uv : TEXCOORD0) : SV_Target
    {
        float speed = lerp(1, 4, Random(uv.y * 10000));
        return frac(uv.x / 8 + speed * _Time.y) < _Intensity * 0.5;
    }

    ENDCG

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment FragmentFill
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment FragmentFill
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment FragmentVerticalBar
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment FragmentHorizontalBar
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment FragmentVerticalSlits
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment FragmentHorizontalSlits
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment FragmentDiagonalSlits
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment FragmentStreamLines
            ENDCG
        }
    }
}
