// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/TestShader2"
{
    Properties
    {
        _Position("position", vector) = (0.5,0.5,0)
    }
    SubShader
    {
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            struct Ray {
                float3 origin;
                float3 direction;
            };

            Ray CreateRay(float3 origin, float3 direction) {
                Ray ray;
                ray.origin = origin;
                ray.direction = direction;
                return ray;
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float mod(float a) {
                return a - floor(a);
            }

            float3 mod(float3 a) {
                return float3(mod(a.x), mod(a.y), mod(a.z));
            }

            float dst(float3 a, float3 b) {
                float dx = abs(mod(a.x) - mod(b.x));
                dx = min(dx, 1 - dx);
                float dy = abs(mod(a.y) - mod(b.y));
                dy = min(dy, 1 - dy);
                float dz = abs(mod(a.z) - mod(b.z));
                dz = min(dz, 1 - dz);
                return length(float3(dx, dy, dz));
            }

            float dstToCircle(float3 pos, float3 circMid, float radius) {
                return dst(pos,circMid)-radius;
            }

            float3 _Position;

            float2 dstToObjects(float3 pos) {
                float dstTo1 = dstToCircle(pos,float3(1, 1, 1), 0.07);
                float dstTo2 = dstToCircle(pos, _Position-float3(0,0,0.2),0.01);
                if(dstTo1<dstTo2)
                    return float2(dstTo1,1);
                return float2(dstTo2,2);
            }


            fixed4 frag(v2f i) : SV_Target
            {
                float rayDst = 0;
                float maxDst = 20;
                float epsilon = 0.001;
                float3 pos = _Position;
                float3 dir = float3(i.uv-.5, 1);
                float3 lightDir = float3(0, -1, 0);
                Ray ray = CreateRay(pos, dir / length(dir));
                while (rayDst < maxDst) {
                    float2 dstToObj = dstToObjects(ray.origin);
                    ray.origin = ray.origin + dstToObj.x * ray.direction;
                    rayDst += dstToObj.x;
                    if (dstToObj.x < epsilon)
                        if (dstToObj.y==1) {
                            float3 normal = float3(0.9, 0.9, 0.8) - mod(ray.origin);
                            float lightingCoeff = max(0.1, dot(normal / length(normal), lightDir));
                            return lightingCoeff *((maxDst- rayDst)/maxDst)*float4(0,1,0,0.5);
                        }
                        else {
                            return ((maxDst - rayDst) / maxDst) * float4(0, 0.2, 1, 0);
                        }
                }
                return 0;
            }
            ENDCG
        }
    }
}
