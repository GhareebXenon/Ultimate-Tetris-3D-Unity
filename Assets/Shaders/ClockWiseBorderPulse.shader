Shader "Custom/ClockwiseBorderPulse"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0,0,0,0)
        _PulseColor ("Pulse Color", Color) = (1,1,1,1)
        _BorderSize ("Border Size", Range(0.01, 0.2)) = 0.05
        _PulseWidth ("Pulse Width", Range(0.01, 0.5)) = 0.15
        _Speed ("Pulse Speed", Float) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            float4 _BaseColor;
            float4 _PulseColor;
            float _BorderSize;
            float _PulseWidth;
            float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Convert UV to clockwise perimeter position (0–4)
            float PerimeterPosition(float2 uv)
            {
                if (uv.y <= _BorderSize)             return uv.x;                 // bottom
                if (uv.x >= 1 - _BorderSize)         return 1 + uv.y;             // right
                if (uv.y >= 1 - _BorderSize)         return 2 + (1 - uv.x);       // top
                if (uv.x <= _BorderSize)             return 3 + (1 - uv.y);       // left
                return -1; // not border
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // Check if inside border
                float borderMask =
                    step(uv.x, _BorderSize) +
                    step(uv.y, _BorderSize) +
                    step(1 - uv.x, _BorderSize) +
                    step(1 - uv.y, _BorderSize);

                if (borderMask <= 0)
                    return _BaseColor;

                float p = PerimeterPosition(uv);
                if (p < 0)
                    return _BaseColor;

                // Animate pulse
                float t = frac(_Time.y * _Speed);
                float pulsePos = t * 4.0;

                float d = abs(p - pulsePos);
                d = min(d, 4.0 - d); // wrap around

                float pulse = smoothstep(_PulseWidth, 0.0, d);

                return lerp(_BaseColor, _PulseColor, pulse);
            }
            ENDCG
        }
    }
}
