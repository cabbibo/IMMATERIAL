Shader "Scenes/Full/frame"
{

    Properties {
        _Cutoff("_Cutoff" , float ) = 0

    _MainTex ("Texture", 2D) = "white" {}
    }
    
    SubShader
    {

                        // inside SubShader

                        // inside SubShader
Tags { "Queue"="Transparent" "RenderType"="Transparent"  }
        LOD 100
// inside Pass
ZWrite On
Blend One One
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


            struct v2f { 
              float4 pos : SV_POSITION; 
              float3 nor : NORMAL; 
              float2 debug : TEXCOORD0; 
              float3 vel : TEXCOORD1;
              float3 dif : TEXCOORD2;
              float2 uv : TEXCOORD3;
            };
            float4 _Color;
            sampler2D _MainTex;

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
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f v) : SV_Target
            {
                // sample the texture

                float3 col = .3; float alpha = 1;
                col += length(v.dif);
               
                col = 1;

                col = tex2D(_MainTex, float2(1-v.uv.y * (1./5.), v.uv.x * .03 * v.debug.y ).yx ).a ;
                if( col.x < .1 ){
                  discard;
                }


                col *= (.3 + 3*length( v.dif ));
                 if( _CanEdgeSwipe > 0 ){
                    col += length(v.dif);
                }
               // col = v.uv.x;
                return float4(col  * v.debug.x, length(col) * v.debug.x);
            }

            ENDCG
        }



                   // SHADOW PASS

    Pass
    {
      Tags{ "LightMode" = "ShadowCaster" }


      Fog{ Mode Off }
      ZWrite On
      ZTest LEqual
      Cull Off
      Offset 1, 1
      CGPROGRAM

      #pragma target 4.5
      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_shadowcaster
      #pragma fragmentoption ARB_precision_hint_fastest

      #include "UnityCG.cginc"
      sampler2D _MainTex;

      float DoShadowDiscard( float3 pos , float2 uv ){
         float lookupVal =  max(min( uv.y * 2,( 1- uv.y ) ) * 1.5,0);//2 * tex2D(_MainTex,uv * float2(4 * saturate(min( uv.y * 4,( 1- uv.y ) )) ,.8) + float2(0,.2));
          float4 tCol = tex2D(_MainTex, uv *   float2( 6,(lookupVal)* 1 ));
          if( ( lookupVal + 1.3) - 1.2*length( tCol ) < .5 ){ return 0;}else{return 1;}
      }
#include "../Chunks/Struct16.cginc"
      #include "../Chunks/ShadowDiscardFunction.cginc"
      ENDCG
    }
  
    }



}