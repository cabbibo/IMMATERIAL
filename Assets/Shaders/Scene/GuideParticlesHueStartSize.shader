Shader "Final/GuideParticlesHueStartSize/16" {
  
  Properties {
  
    _HueStart ("_HueStart", float) = .3
    _HueSize ("_HueSize", float) = .3
    _MainTex ("Texture", 2D) = "white" {}
    _ColorMap ("Texture", 2D) = "white" {}
    _CubeMap( "Cube Map" , Cube )  = "defaulttexture" {}
  }

    SubShader {

        // COLOR PASS

        Pass {
            Tags{ "LightMode" = "ForwardBase" }
            Cull Off

            CGPROGRAM
            #pragma target 4.5
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"    

            struct Vert{
          float3 pos;
          float3 vel;
          float3 nor;
          float3 tan;
          float2 uv;
          float2 debug;
        };


        StructuredBuffer<Vert> _VertBuffer;
        StructuredBuffer<int> _TriBuffer;

        uniform sampler2D _MainTex;
        uniform sampler2D _ColorMap;
        uniform samplerCUBE _CubeMap;
    
        float3 _Color;
        float _HueStart;
        float _HueSize;
    
        float3 _Player;


        struct varyings {
            float4 pos      : SV_POSITION;
            float3 nor      : TEXCOORD0;
            float2 uv       : TEXCOORD1;
            float3 eye      : TEXCOORD5;
            float3 worldPos : TEXCOORD6;
            float3 debug    : TEXCOORD7;
            float3 closest    : TEXCOORD8;
            float3 tan    : TEXCOORD9;
            float3 vel    : TEXCOORD10;
            UNITY_SHADOW_COORDS(2)
        };

        #include "../Chunks/hsv.cginc"
        #include "../Chunks/hash.cginc"

            varyings vert(uint id : SV_VertexID) {


                Vert v = _VertBuffer[_TriBuffer[id]];

                float3 fPos     =  v.pos;
                float3 fNor     =  v.nor;
                float2 fUV      =  v.uv;
                float3 fTan     =  v.tan;
                float2 debug    =  v.debug;

                varyings o;

                UNITY_INITIALIZE_OUTPUT(varyings, o);

                o.pos = mul(UNITY_MATRIX_VP, float4(fPos,1));
                o.worldPos = fPos;
                o.eye = _WorldSpaceCameraPos - fPos;
                o.nor = fNor;

                float2 center = fUV-float2(.5 , .5);
                float r = length(center);
                float a =  atan2(center.x, center.y);
                o.tan = normalize(normalize(fTan) *  -cos(a) +   normalize(cross(fNor,fTan))  * sin(a)) * r;
                o.uv =  fUV * (1./6.)+ floor(float2(hash(debug.x*10), hash(debug*20)) * 6)/6;
                o.debug = float3(debug.x,debug.y,a);
                o.vel = v.vel;

                UNITY_TRANSFER_SHADOW(o,o.worldPos);

                return o;
            }

            float4 frag(varyings v) : COLOR {
        
                fixed shadow = UNITY_SHADOW_ATTENUATION(v,v.worldPos ) * .9 + .1 ;
                float4 d = tex2D(_MainTex,v.uv);
        //if( d.a < .9 ){discard;}
        if( length(d.xyz) > 1.5 ){discard;}

       // if( length(v.tan)> .5){ discard;}
        
        float3 fNor = normalize(v.nor + 1*sin(length(v.tan) * 30) *normalize(v.tan) );

        float3 lDir = normalize(_Player - v.worldPos);
        float3 refl = reflect( lDir , v.nor );
        float rM  = dot( normalize( v.eye) , refl );
        float3 col = normalize(lDir) * .5 + .5;

        float3 eyeRefl = reflect( normalize(v.eye),v.nor);
        //float3 eyeRefl = refract( normalize(v.eye), fNor , .8 );

        float3 tCol = texCUBE( _CubeMap , eyeRefl );

        col = 2*tCol* tex2D(_ColorMap, float2(v.debug.y * _HueSize + _HueStart ,0));//-rM;//v.nor * .5 + .5;// tCol;//*tCol * tex2D(_ColorMap,float2(rM*.1+.7 - v.debug.y * .1 ,.5 )).rgb;// *(1-rM);//hsv(rM*rM*rM * 2.1,.5,rM);// + normalize(refl) * .5+.5;
        //col = v.tan * .5 + .5;

        //col += hsv(dot(v.eye,v.nor) * -.1,.6,1) * (1-length(col));
        return float4( col , 1.);
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
#include "../Chunks/hash.cginc"

sampler2D _MainTex;
  struct v2f {
        V2F_SHADOW_CASTER;
        float2 uv : TEXCOORD1;
      };

            struct Vert{
          float3 pos;
          float3 vel;
          float3 nor;
          float3 tan;
          float2 uv;
          float2 debug;
        };


        StructuredBuffer<Vert> _VertBuffer;
        StructuredBuffer<int> _TriBuffer;


      v2f vert(appdata_base i, uint id : SV_VertexID)
      {
        v2f o;


        Vert v = _VertBuffer[_TriBuffer[id]];
       
        o.uv =  v.uv;

        float2 debug = v.debug;

        o.uv = o.uv * (1./6.)+ floor(float2(hash(debug.x*10), hash(debug.x*20)) * 6)/6;
        o.pos = mul(UNITY_MATRIX_VP, float4(v.pos, 1));
        return o;
      }

      float4 frag(v2f i) : COLOR
      {
        float4 col = tex2D(_MainTex,i.uv);
        //if( col.a < .4){discard;}
         if( length(col.xyz) > 1 ){discard;}
        SHADOW_CASTER_FRAGMENT(i)
      }
      ENDCG
    }
  


    }

}
