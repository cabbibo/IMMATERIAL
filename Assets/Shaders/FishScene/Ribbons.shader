Shader "Scenes/FishScene/Ribbons1" {
  Properties {

    _Color ("Color", Color) = (1,1,1,1)
    _FalloffRadius ("Falloff", float) = 20

    _MainTex ("Texture", 2D) = "white" {}
    _BumpMap ("Bumpy", 2D) = "white" {}

       _ColorMap ("ColorMap", 2D) = "white" {}
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
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"

      #include "../Chunks/hsv.cginc"
            #include "../Chunks/hash.cginc"


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


      float3 _Color;
      float3 _PlayerPosition;
      float _FalloffRadius;
      sampler2D _MainTex;
      sampler2D _ColorMap;
      sampler2D _BumpMap;


            struct varyings {
                float4 pos      : SV_POSITION;
                float3 nor      : TEXCOORD0;
                float2 uv       : TEXCOORD1;
                float3 eye      : TEXCOORD5;
                float3 worldPos : TEXCOORD6;
        float3 debug    : TEXCOORD7;
                float3 vel    : TEXCOORD9;
                float3 closest    : TEXCOORD8;
                   half3 tspace0 : TEXCOORD11; // tangent.x, bitangent.x, normal.x
                half3 tspace1 : TEXCOORD12; // tangent.y, bitangent.y, normal.y
                half3 tspace2 : TEXCOORD13; // tangent.z, bitangent.z, normal.z
                half3 tang : TEXCOORD14; // tangent.z, bitangent.z, normal.z
                UNITY_SHADOW_COORDS(2)
            };

            varyings vert(uint id : SV_VertexID) {

                   Vert v = _VertBuffer[_TriBuffer[id]];
        
        float3 fPos   = v.pos;
        float3 fNor   = v.nor;
        float3 fVel   = v.vel;
        float2 fUV    = v.uv;
        float2 debug  = v.debug;

                varyings o;

                UNITY_INITIALIZE_OUTPUT(varyings, o);

                o.pos = mul(UNITY_MATRIX_VP, float4(fPos,1));
                o.worldPos = fPos;
                o.eye = _WorldSpaceCameraPos - fPos;
        o.nor = fNor;
                o.vel = fVel;

        float offset = floor(hash(debug.x) * 6) /6;
                o.uv =  fUV * float2(1,1./6.) + float2(-.1,offset);
                o.debug = float3(debug.x,debug.y,0);

                o.tang = v.tan;

                half3 wNormal = v.nor;
                half3 wTangent = v.tan;
                // compute bitangent from cross product of normal and tangent
                //half tangentSign = tangent.w * unity_WorldTransformParams.w;
                half3 wBitangent = cross(wNormal, wTangent);// * tangentSign;
                // output the tangent space matrix
                o.tspace0 = half3(wTangent.x, wBitangent.x, wNormal.x);
                o.tspace1 = half3(wTangent.y, wBitangent.y, wNormal.y);
                o.tspace2 = half3(wTangent.z, wBitangent.z, wNormal.z);

                UNITY_TRANSFER_SHADOW(o,o.worldPos);

                return o;
            }

            float4 frag(varyings v) : COLOR {



              float4 color = float4(0,0,0,0);// = tex2D(_MainTex,v.uv );
              float4 tCol = tex2D(_MainTex,v.uv );


 // sample the normal map, and decode from the Unity encoding
                half3 tnormal =UnpackNormal(tex2D(_BumpMap, v.uv));// lerp( i.norm ,  , specMap.x);
                // transform normal from tangent to world space
                half3 worldNormal;
                worldNormal.x = dot(v.tspace0, tnormal);
                worldNormal.y = dot(v.tspace1, tnormal);
                worldNormal.z = dot(v.tspace2, tnormal);

               worldNormal = lerp( v.nor , worldNormal , tCol.x);
          half3 worldViewDir = normalize(UnityWorldSpaceViewDir(v.worldPos));
                //half3 worldRefl = reflect(-worldViewDir, worldNormal);
                half3 worldRefl = refract(worldViewDir, worldNormal,.8);
                half4 skyData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, worldRefl);
                half3 skyColor = DecodeHDR (skyData, unity_SpecCube0_HDR);

              float4 cCol = tex2D(_ColorMap,float2(tCol.x * .3 - tCol.a*.3,0) );
        
              fixed shadow = UNITY_SHADOW_ATTENUATION(v,v.worldPos  ) * .7 + .3 ;
              
              color.xyz = skyColor  * cCol ;// * tCol;;//worldNormal * .5 + .5;//tCol;
             // color =  float4(v.nor * .5 + .5,1);//v.uv.x;
              if( tCol.a < .3 ){ discard; }    
              return float4( color.xyz * shadow, 1.);
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

      #include "../Chunks/ShadowCasterPos.cginc"
sampler2D _MainTex;
  struct v2f {
        V2F_SHADOW_CASTER;
        float2 uv : TEXCOORD1;
        float2 debug : TEXCOORD2;
      };


      v2f vert( uint id : SV_VertexID)
      {

        Vert v = _VertBuffer[_TriBuffer[id]];
        
        float3 fPos   = v.pos;
        float3 fNor   = v.nor;
        float2 fUV    = v.uv;
        float2 debug  = v.debug;

        v2f o;
       
        float offset = floor(hash(debug.x) * 6) /6;
               o.uv =  fUV * float2(1,1./6.) + float2(-.1,offset);
        //o.uv = fUV.xy  -float2(0.1,0);// *float2(1./6.,1);;
        float4 position = ShadowCasterPos(v.pos, -v.nor);
        o.pos = UnityApplyLinearShadowBias(position);
        o.debug = debug;
        return o;
      }

      float4 frag(v2f i) : COLOR
      {
        float4 col = tex2D(_MainTex,i.uv);

        //if( i.debug.y < .3 ){ discard; }
        if( col.a < .3){discard;}
        SHADOW_CASTER_FRAGMENT(i)
      }
      ENDCG
    }
  


    }

}
