Shader "Flipper/FloorFx"
{
    Properties
    {
        _Cutoff("Alpha Cutoff", Range(0, 1)) = 0
    }

    CGINCLUDE

    #include "SimplexNoise2D.hlsl"

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

    ENDCG

    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" }

        CGPROGRAM

        #pragma surface surf Standard vertex:vert alphatest:_Cutoff addshadow noshadowmask nodynlightmap nodirlightmap nolightmap
        #pragma multi_compile FLOORFX0 FLOORFX1 FLOORFX2 FLOORFX3 FLOORFX4

        struct Input { float2 texcoord; };

        float _LocalTime;

        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.texcoord = v.texcoord.xy;
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = 0;
            o.Metallic = 0;
            o.Smoothness = 0;

            float2 uv = IN.texcoord;
            float t = _LocalTime;

            #if defined(FLOORFX0)

            o.Alpha = 1;

            #elif defined(FLOORFX1)

            uv *= 10;
            float n = snoise(floor(uv) * 0.15 + t * 0.15);
            uv = abs(frac(uv) - 0.5) * 2;
            o.Alpha = 1 - max(uv.x, uv.y) + n * 0.5;

            #elif defined(FLOORFX2)

            float n = snoise(float2(uv.y *  8 + t * 0.8, t * 0.1)) +
                      snoise(float2(uv.y * 23 + t * 0.8, t * 0.1));
            o.Alpha = n + 0.5;

            #elif defined(FLOORFX3)

            half alpha = 1;

            for (uint i = 0; i < 2; i++)
            {
                float2 org = snoise_grad(float2(i, t * 0.05)).xy * 0.1 + 0.5;
                alpha = min(alpha, frac(length(uv - org) * 5 - t));
            }

            o.Alpha = alpha;

            #else // FLOORFX4

            uv -= 0.5;

            float r = atan2(uv.x, uv.y) / (2 * UNITY_PI);
            float l = length(uv);

            float sp = lerp(0.05, 0.2, Random(l * 100));
            sp *= Random(l * 100 + 100) < 0.5 ? -1 : 1;

            o.Alpha = frac(r + sp * t);

            #endif
        }

        ENDCG
    }
    FallBack "Diffuse"
}
