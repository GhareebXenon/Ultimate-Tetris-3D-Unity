Shader "Custom/URP_NeonEdgeGlow"
{
    Properties
    {
        _TintColor ("Tint Color", Color) = (1, 0.4, 0.4, 0.15)

        _EdgeColor ("Edge Color", Color) = (1, 0.1, 0.1, 1)
        _EdgeWidth ("Edge Width", Range(0.001, 0.02)) = 0.006

        _GlowColor ("Glow Color", Color) = (1, 0.2, 0.2, 1)
        _GlowWidth ("Glow Width", Range(0.01, 0.08)) = 0.03
        _GlowIntensity ("Glow Intensity", Range(0, 5)) = 2
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        // ---------- GLOW PASS ----------
        Pass
        {
            Blend One One
            ZWrite Off
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment fragGlow
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 bary : TEXCOORD1;
                float3 edgeMask : TEXCOORD2;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 bary : TEXCOORD0;
            };

            float4 _GlowColor;
            float _GlowWidth;
            float _GlowIntensity;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.bary = v.bary;
                return o;
            }

            half4 fragGlow (Varyings i) : SV_Target
            {
                float edge = min(min(i.bary.x, i.bary.y), i.bary.z);
                float glow = smoothstep(_GlowWidth, 0.0, edge);

                return _GlowColor * glow * _GlowIntensity;
            }
            ENDHLSL
        }

        // ---------- CORE EDGE + TINT ----------
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment fragCore
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 bary : TEXCOORD1;
                float3 edgeMask : TEXCOORD2;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 bary : TEXCOORD0;
            };

            float4 _TintColor;
            float4 _EdgeColor;
            float _EdgeWidth;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.bary = v.bary;
                return o;
            }

            half4 fragCore (Varyings i) : SV_Target
            {
                float edge = min(min(i.bary.x, i.bary.y), i.bary.z);
                float mask = smoothstep(_EdgeWidth, 0.0, edge);

                return lerp(_TintColor, _EdgeColor, mask);
            }
            ENDHLSL
        }
    }
}