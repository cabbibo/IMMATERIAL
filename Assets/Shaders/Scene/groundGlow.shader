Shader "Scenes/pickup/glowGround"
{
    Properties {

    _Color ("Color", Color) = (1,1,1,1)
    _MainTex ("Texture", 2D) = "white" {}
    _ColorMap ("Color Map", 2D) = "white" {}
    _HueSize("Hue Size", float) = .5
    _HueOffset("Hue Size", float) = .5
    

  }
    SubShader
    {
        
        Pass
        {
Tags { "RenderType"="Opaque" }
        LOD 100

        Cull Off

          Tags{ "LightMode" = "ForwardBase" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5
            // make fog work
            #pragma multi_compile_fogV
 #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

      #include "UnityCG.cginc"
      #include "AutoLight.cginc"
    


            #include "../Chunks/Struct16.cginc"
            #include "../Chunks/noise.cginc"

            sampler2D _MainTex;
            sampler2D _ColorMap;
            sampler2D _AudioMap;

            float _StartTime;
            float _Setting;
            float _HueOffset;
            float _HueSize;

            struct v2f { 
              float4 pos : SV_POSITION; 
              float3 nor : NORMAL;
              float2 uv :TEXCOORD0; 
              float3 worldPos :TEXCOORD1;
              UNITY_SHADOW_COORDS(2)
            };
            float4 _Color;

            StructuredBuffer<Vert> _VertBuffer;
            StructuredBuffer<int> _TriBuffer;

            v2f vert ( uint vid : SV_VertexID )
            {
                v2f o;

        UNITY_INITIALIZE_OUTPUT(v2f, o);
                Vert v = _VertBuffer[_TriBuffer[vid]];
                o.pos = mul (UNITY_MATRIX_VP, float4(v.pos,1.0f));


                o.nor = v.nor;
                o.uv = v.uv;
                o.worldPos = v.pos;


        UNITY_TRANSFER_SHADOW(o,o.worldPos);

                return o;
            }

      float DoShadowDiscard( float3 pos , float2 uv , float3 nor ){
        float v = dot(normalize(_WorldSpaceLightPos0.xyz), normalize(nor));
        return v;//sin( uv.y * 100 + _Time.y);
      }

            fixed4 frag (v2f v) : SV_Target
            {
                // sample the texture
fixed shadow = UNITY_SHADOW_ATTENUATION(v,v.worldPos) * .5 + .5;
                float val = -dot(normalize(_WorldSpaceLightPos0.xyz),normalize(v.nor));// -DoShadowDiscard( i.worldPos , i.uv , i.nor );

                float lookupVal =  max(min( v.uv.y  * .01,( 1- v.uv.y ) ),0);//2 * tex2D(_MainTex,v.uv * float2(4 * saturate(min( v.uv.y * 4,( 1- v.uv.y ) )) ,.8) + float2(0,.2));
               

                float4 tCol = tex2D(_MainTex, v.uv *   float2( 10, 1 ) + .1 - float2(0 , .05)*_Time.y);


                float tVal = tCol.x * tCol.x * tCol.x *30 + .3*abs(noise( float3(v.uv.x * 1000,v.uv.y * 20, 0)));
                float cutoff = saturate(((_Time.y - _StartTime - 8 ) / 2) * _Setting);
                if( ( lookupVal + 1.3) - 20*tVal  <  cutoff * .3   ){ discard;}

               // if( v.uv.y + tCol.x * .1> .9){ discard;}

                fixed4 col =   tex2D(_ColorMap , float2( tVal * 1 + (1-shadow  * .05)+ .0 + lookupVal* lookupVal * .1  + v.uv.y * _HueSize + _HueOffset , 0) );//* 20-10;//*tCol* lookupVal*4;//* 10 - 1;
                    col *= tex2D(_AudioMap , tVal );
                    
                   // col = 1;
                return col;
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
    float _StartTime;
            float _Setting;
      float DoShadowDiscard( float3 pos , float2 uv ){
        
                float lookupVal =  max(min( uv.y * 2,( 1- uv.y ) ) * 1.5,0);//2 * tex2D(_MainTex,v.uv * float2(4 * saturate(min( v.uv.y * 4,( 1- v.uv.y ) )) ,.8) + float2(0,.2));
               

                float4 tCol = tex2D(_MainTex, uv *   float2( 10, 1 ) + .1- float2(0 , .05)*_Time.y);

                 float cutoff = saturate(((_Time.y - _StartTime - 8 ) / 2) * _Setting);
                if( ( lookupVal + 1.3) - 1.2*length( tCol ) < .8  + cutoff * .3){ return 0;}else{ return 1;}//     if( ( lookupVal + 1.3) - 1.2*length( tCol ) < .5 ){ return 0;}else{return 1;}
      }

      #include "../Chunks/Struct16.cginc"

      #include "../Chunks/ShadowDiscardFunction.cginc"
      ENDCG
    }
  
    }




}
