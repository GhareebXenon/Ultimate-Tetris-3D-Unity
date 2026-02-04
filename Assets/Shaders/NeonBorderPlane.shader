Shader "Custom/NeonBorderPlane"
{
    Properties
    {
        _BorderColor ("Border Color (HDR)", Color) = (0, 1, 1, 1)
        _BorderSize ("Border Size", Range(0.001, 0.2)) = 0.05
        _Glow ("Glow Intensity", Range(0, 10)) = 3
    }

    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "RenderPipeline"="UniversalPipeline"
        }

        Blend SrcAlpha One
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BorderColor;
                float _BorderSize;
                float _Glow;
            CBUFFER_END

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;

                // Distance from edges
                float left   = uv.x;
                float right  = 1 - uv.x;
                float bottom = uv.y;
                float top    = 1 - uv.y;

                float edgeDist = min(min(left, right), min(bottom, top));

                // Smooth neon border
                float border = smoothstep(_BorderSize, 0.0, edgeDist);

                // Final color
                float4 color = _BorderColor * border * _Glow;
                color.a = border;

                return color;
            }
            ENDHLSL
        }
    }
}
