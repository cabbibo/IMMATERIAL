Shader "Scenes/Full/frameCorners"
{    Properties {
        _Cutoff("_Cutoff" , float ) = 0

    _MainTex ("Texture", 2D) = "white" {}
    }
    
    
    SubShader
    {
                // inside SubShader
Tags { "Queue"="Transparent" "RenderType"="Transparent"  }
        LOD 100
// inside Pass
ZWrite On
Blend SrcAlpha OneMinusSrcAlpha
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
            #include "../Chunks/hash.cginc"


            struct v2f { 
                float4 pos      : SV_POSITION; 
                float3 nor      : NORMAL; 
                float2 uv       : TEXCOORD0; 
                float2 debug    : TEXCOORD1; 

            };
            float4 _Color;

            StructuredBuffer<Vert> _VertBuffer;
            StructuredBuffer<int> _TriBuffer;
            float _CanEdgeSwipe;


            int _TotalFrames;


            sampler2D _MainTex;
            v2f vert ( uint vid : SV_VertexID )
            {
                v2f o;
                Vert v = _VertBuffer[_TriBuffer[vid]];
                o.pos = mul (UNITY_MATRIX_VP, float4(v.pos,1.0f));
                o.nor = v.nor;

                float2 fUV = v.uv * ( 1./6);
                fUV.x += floor((hash( v.debug.x* 121 + float(_TotalFrames * 20)) * 6))/6;
                fUV.y += floor((hash( v.debug.x* 213 + float(_TotalFrames * 33)) * 6))/6;
                o.uv = fUV;
                o.debug = v.debug;
                
                return o;
            }

            fixed4 frag (v2f v) : SV_Target
            {


                // sample the texture
                fixed4 col = float4( v.nor * .5 + .5 , 1);//tex2D(_MainTex, i.uv);

                col = _CanEdgeSwipe;
                if( _CanEdgeSwipe > 0 ){
                    col += 1;//length(v.dif);
                }

                col = tex2D( _MainTex , v.uv );
                if( col.a < .3 ){ discard; }
                col *= v.debug.y;
                
                return col;
            }

            ENDCG
        }
    }
}