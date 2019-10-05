Shader "Scenes/Full/frame"
{

    Properties {
        _Cutoff("_Cutoff" , float ) = 0
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Cull Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"


            #include "../Chunks/Struct16.cginc"

            float _Cutoff;

            float _CanEdgeSwipe;


            struct v2f { float4 pos : SV_POSITION; float3 nor : NORMAL; float2 debug : TEXCOORD0; float3 vel : TEXCOORD1;float3 dif : TEXCOORD2;};
            float4 _Color;

            StructuredBuffer<Vert> _VertBuffer;
            StructuredBuffer<int> _TriBuffer;

            v2f vert ( uint vid : SV_VertexID )
            {
                v2f o;
                Vert v = _VertBuffer[_TriBuffer[vid]];
                o.pos = mul (UNITY_MATRIX_VP, float4(v.pos,1.0f));
                o.nor = v.nor;
                o.vel = v.vel;
                o.dif = v.pos - v.tan;
                o.debug = v.debug;
                return o;
            }

            fixed4 frag (v2f v) : SV_Target
            {
                // sample the texture

                float3 col = .3; float alpha = 1;
                col += length(v.dif);
                if( _CanEdgeSwipe > 0 ){
                    col += length(v.dif);
                }
                return float4(col , alpha);
            }

            ENDCG
        }
    }
}