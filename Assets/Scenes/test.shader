// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/TestShader"
{
    Properties
    {
        _Area("Area", vector) = (0,0,4,4)
        _ColorModifier("ColorModifier", float) = 0.9
        _ColorGradient("ColorGradient", vector) = (.3,.45,.6,.9)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 _Area;
            float4 _ColorGradient;
            float _ColorModifier;

            fixed4 frag(v2f i) : SV_Target
            {
                float2 c = _Area.xy + (i.uv-.5)*_Area.zw;
                float2 z;
                float iter;
                float esc = 5;
                for (iter = 0; iter < 1000; iter++) {
                    z = float2(z.x * z.x - z.y * z.y, 2 * z.x * z.y) + c;
                    if (length(z) > esc)break;
                }
                float fracIter = log2(log(length(z)) / log(esc))-1;
                if (iter > 499)
                    return 0;
                iter -= fracIter;
                float m = sqrt(iter / 255);
                float4 color = sin(_ColorGradient * (m+_ColorModifier)* 20)* .5 + .5;
                return color;
            }
            ENDCG
        }
    }
}
