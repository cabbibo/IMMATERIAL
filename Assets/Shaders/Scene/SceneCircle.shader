Shader "Scene/SceneCircle/Test1"
{
    Properties{
        _RibbonMap("RibbonMap", 2D) = "" {}
        _RibbonMap2("RibbonMap", 2D) = "" {}
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

            sampler2D _RibbonMap;
            sampler2D _RibbonMap2;

            float _SetTime;
            float _Setting;
float3 _PlayerPosition;

            float _FadeTime;

            struct v2f { 
                float4 pos : SV_POSITION; 
                float3 nor : NORMAL;
                float2 uv : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };



            float _InnerRadius;
            float _OuterRadius;
            float _StartTime;

            float4 _Color;

            StructuredBuffer<Vert> _VertBuffer;
            StructuredBuffer<int> _TriBuffer;

            v2f vert ( uint vid : SV_VertexID )
            {
                v2f o;
                Vert v = _VertBuffer[_TriBuffer[vid]];
                o.pos = mul (UNITY_MATRIX_VP, float4(v.pos,1.0f));
                o.nor = v.nor;
                o.uv = v.uv;
                o.worldPos = v.pos;
                return o;
            }

            fixed4 frag (v2f v) : SV_Target
            {


              float fade = clamp( (_Time.y - _SetTime)/_FadeTime , 0 ,1 );

              if( _Setting == 1 ){ fade = 1-fade; }
                float offset = floor( sin( v.uv.y * 10) );

                //float4 t = tex2D(_RibbonMap , v.uv   );

                float dif = length(_PlayerPosition - v.worldPos);


                float2 newUV = float2( sin( v.uv.x * 1000) , (2*sin(v.uv.y * 30 + _Time.y * 2 + floor(v.uv.x * 100)%3  + floor( v.uv.x * 200 )%4) + floor( v.uv.x * 150 )%1.5 )-1  );


              //  if( length(t.xyz) < 1){ discard;}
                 float3 col;
               
                    if( newUV.y < 0){ discard;}
                    if( newUV.x   < 0 ){ discard;}
                      col = 1;

                    col = float3(newUV.x , newUV.y ,0);//tex2D(_RibbonMap , v.uv * 20   ).xyz;//float3(newUV.x ,newUV.y,1);//min( newUV.x+1 , newUV.y+1 );
                    col = tex2D(_RibbonMap , (newUV / 2 )   ).xyz  * tex2D(_RibbonMap2 , (newUV / 2 ) - float2( v.uv.y * .3 + .5,0)  ).xyz;//float3(newUV.x ,newUV.y,1);//min( newUV.x+1 , newUV.y+1 );
                
                    float v2 = clamp( (_InnerRadius / _OuterRadius)-v.uv.y , 0 ,1) ;
                    
                    float v3 = clamp( (v.uv.y -.9)*20 , 0 , 1 ); 
                    if( length(col) < .3 + 2*fade  + v3){ discard; }
              


                col /= (.5+ .1*dif);




                return float4(col,1);
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


      struct Vert{
      float3 pos;
      float3 vel;
      float3 nor;
      float3 tan;
      float2 uv;
      float2 debug;
    };


     sampler2D _RibbonMap;
            sampler2D _RibbonMap2;
float3 _PlayerPosition;



float _SetTime;
            float _Setting;
            float _FadeTime;
            float _InnerRadius;
            float _OuterRadius;

  StructuredBuffer<Vert> _VertBuffer;
  StructuredBuffer<int> _TriBuffer;

      struct v2f {
        V2F_SHADOW_CASTER;
        float2 uv: TEXCOORD1;
      };


      v2f vert(appdata_base v, uint id : SV_VertexID)
      {
        v2f o;
        Vert v1 = _VertBuffer[_TriBuffer[id]];
        o.pos = mul(UNITY_MATRIX_VP, float4(v1.pos, 1));
        o.uv = v1.uv;
        return o;
      }

      float4 frag(v2f v) : COLOR
      {



          
              float fade = clamp( (_Time.y - _SetTime)/_FadeTime , 0 ,1 );

              if( _Setting == 1 ){ fade = 1-fade; }
                float offset = floor( sin( v.uv.y * 10) );

                //float4 t = tex2D(_RibbonMap , v.uv   );

                float2 newUV = float2( sin( v.uv.x * 1000) , (2*sin(v.uv.y * 30 + _Time.y * 2 + floor(v.uv.x * 100)%3  + floor( v.uv.x * 200 )%4) + floor( v.uv.x * 150 )%1.5 )-1  );


              //  if( length(t.xyz) < 1){ discard;}
                 float3 col;
               
                    if( newUV.y < 0){ discard;}
                    if( newUV.x   < 0 ){ discard;}
                      col = 1;

                    col = float3(newUV.x , newUV.y ,0);//tex2D(_RibbonMap , v.uv * 20   ).xyz;//float3(newUV.x ,newUV.y,1);//min( newUV.x+1 , newUV.y+1 );
                     col = tex2D(_RibbonMap , (newUV / 2 )   ).xyz  * tex2D(_RibbonMap2 , (newUV / 2 ) - float2( v.uv.y * .3 + .5,0)  ).xyz;//float3(newUV.x ,newUV.y,1);//min( newUV.x+1 , newUV.y+1 );
                
                    float v2 = clamp( (_InnerRadius / _OuterRadius)-v.uv.y , 0 ,1) ;
                    float v3 = clamp( (v.uv.y -.9)*20 , 0 , 1 ); 
                    if( length(col) < .3 + 2*fade   + v3){ discard; }


              

        SHADOW_CASTER_FRAGMENT(i)
      }
      ENDCG
    }
  


  }
}