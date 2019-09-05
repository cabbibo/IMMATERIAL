﻿Shader "Scenes/GoldenPit/pitStencil"
{
    
    SubShader
    {
            Tags { "RenderType"="Opaque" "Queue"="Geometry+1"}
        //ColorMask 0
        ZWrite Off
        Stencil {
            Ref 1
            Comp Always 
            Pass Replace
        }
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


            struct v2f { float4 pos : SV_POSITION; float3 nor : NORMAL; };
            float4 _Color;

            StructuredBuffer<Vert> _VertBuffer;
            StructuredBuffer<int> _TriBuffer;

            v2f vert ( uint vid : SV_VertexID )
            {
                v2f o;
                Vert v = _VertBuffer[_TriBuffer[vid]];
                o.pos = mul (UNITY_MATRIX_VP, float4(v.pos,1.0f));
                o.nor = v.nor;
                return o;
            }


struct FragOut {
         float4 color :    COLOR;
         float depth :    DEPTH;
     };
            FragOut frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = float4( i.nor * .5 + .5 , 1);//tex2D(_MainTex, i.uv);
                
                FragOut o;

                o.color = col;
                o.depth = 10000;
                return o;
            }

            ENDCG
        }
    }
}