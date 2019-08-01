Shader "Final/Body/F1" {
  Properties {

    _Color ("Color", Color) = (1,1,1,1)

    _MainTex ("Texture", 2D) = "white" {}

    _ColorMap ("ColorMap", 2D) = "white" {}
    _NormalMap ("NormalMap", 2D) = "white" {}
    _CubeMap( "Cube Map" , Cube )  = "defaulttexture" {}
    
    [Toggle(Enable12Struct)] _Struct12("12 Struct", Float) = 0
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
    
        #include "../Chunks/Struct36.cginc"
            #include "../Chunks/hsv.cginc"

            int _SubMeshID;
            int _BaseID;
      #include "../Chunks/noise.cginc"

      float3 _Color;
      float3 _PlayerPosition;
      sampler2D _MainTex;
      sampler2D _ColorMap;
      sampler2D _NormalMap;
      samplerCUBE _CubeMap;


  StructuredBuffer<Vert> _VertBuffer;
  StructuredBuffer<int> _TriBuffer;



      sampler2D _HeightMap;
    float _MapSize;
    float _MapHeight;



  float4 sampleColor( float3 pos ){
        return tex2Dlod(_HeightMap , float4(pos.xz * _MapSize,0,0) );
    }

      struct varyings {
        float4 pos    : SV_POSITION;
        float3 nor    : TEXCOORD0;
        float2 uv     : TEXCOORD1;
        float3 eye      : TEXCOORD5;
        float3 worldPos : TEXCOORD6;
        float3 debug    : TEXCOORD7;
        float3 closest    : TEXCOORD8;
        UNITY_SHADOW_COORDS(2)
      };

      varyings vert(uint id : SV_VertexID) {
Vert v = _VertBuffer[_TriBuffer[id + _BaseID]];
        
        float3 fPos   = v.pos;
        float3 fNor   = v.nor;
        float2 fUV    = v.uv;
        float2 debug  = v.debug;

        varyings o;

        UNITY_INITIALIZE_OUTPUT(varyings, o);

        //fPos -= float3(0,1,0) * .3  * (1-saturate(.3*length( fPos - _PlayerPosition)));

        o.worldPos = fPos;



        o.pos = mul(UNITY_MATRIX_VP, float4(fPos,1));
        o.eye = _WorldSpaceCameraPos - fPos;
        o.nor = fNor;
        o.uv =  float2(.9,1)-fUV;
        o.debug = float3(debug.x,debug.y,0);

        UNITY_TRANSFER_SHADOW(o,o.worldPos);

        return o;
      }


               float3 nor;

            float3 DoBeltBuckle(){
                return float3(0,0,1);
            }

            float3 DoBelt(){
                return float3(0,.3,.5);
            }

            float3 DoHair(){
                return float3(0,.5,1);//nor * .5 + .5;
            }

            float3 DoEyes(){
                return 1;
            }
            float3 DoPants(){
                return float3(1,.4,.2);
            }

            float3 DoShirtShoes(){
                return float3( .4, .8,1);
            }
            
            float3 DoSkin(){

                return float3( .5 , .3,.2);
            }

            float3 DoSocks(){
                return DoSkin();//float3( .5 , .3,.2);
            }


      float4 frag(varyings v) : COLOR {

                fixed shadow = UNITY_SHADOW_ATTENUATION(v,v.worldPos) * .5 + .5 ;
float dif = length( v.worldPos - _PlayerPosition );

float l = saturate( (20-dif)/20);

        float3 fNor = v.nor;
        //col.xyz = .4*pow(length(color.xyz),4);

        float match = dot( fNor, _WorldSpaceLightPos0 );

        float3 refl = reflect( normalize(v.eye) , fNor );
        float reflM = dot( refl , _WorldSpaceLightPos0 );

        float3 tCol = texCUBE(_CubeMap,refl);
        float3 tCol2 = tex2D( _MainTex , v.uv  );

        float3 col = 0;

        col = tCol * 2;
        if( _SubMeshID == 0 ){
            col *= DoEyes();
        }else if( _SubMeshID == 1 ){
            col *= DoHair();
        }else if( _SubMeshID == 2 ){
            col *= DoBeltBuckle();
        }else if( _SubMeshID == 3 ){
            col *= DoBelt();
        }else if( _SubMeshID == 4 ){
            col *= DoPants();
        }else if( _SubMeshID == 5 ){
            col = DoShirtShoes();
        }else if( _SubMeshID == 6 ){
            col = DoSocks();
        }else if( _SubMeshID == 7 ){
            col = DoSkin();


        }






       // col = tCol * 2 * reflM * reflM;


        //tCol *=color;// pow(eyeM,100)  * 20;
        //tCol = 1;

        //tCol *= shadow;

        //tCol = dif;

        //tCol = grassHeight;
        return float4( col.xyz * shadow  , 1.);
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


 
        #include "../Chunks/Struct36.cginc"

            int _SubMeshID;
            int _BaseID;


float4 ShadowCasterPos (float3 vertex, float3 normal) {
  float4 clipPos;
    
    // Important to match MVP transform precision exactly while rendering
    // into the depth texture, so branch on normal bias being zero.
    if (unity_LightShadowBias.z != 0.0) {
    float3 wPos = vertex.xyz;
    float3 wNormal = normal;
    float3 wLight = normalize(UnityWorldSpaceLightDir(wPos));

  // apply normal offset bias (inset position along the normal)
  // bias needs to be scaled by sine between normal and light direction
  // (http://the-witness.net/news/2013/09/shadow-mapping-summary-part-1/)
  //
  // unity_LightShadowBias.z contains user-specified normal offset amount
  // scaled by world space texel size.

    float shadowCos = dot(wNormal, wLight);
    float shadowSine = sqrt(1 - shadowCos * shadowCos);
    float normalBias = unity_LightShadowBias.z * shadowSine;

    wPos -= wNormal * normalBias;

    clipPos = mul(UNITY_MATRIX_VP, float4(wPos, 1));
    }
    else {
        clipPos = UnityObjectToClipPos(vertex);
    }
  return clipPos;
}



  StructuredBuffer<Vert> _VertBuffer;
  StructuredBuffer<int> _TriBuffer;

      struct v2f {
        V2F_SHADOW_CASTER;
        float3 nor : NORMAL;
      };


      v2f vert(appdata_base input, uint id : SV_VertexID)
      {
        v2f o;
        Vert v = _VertBuffer[_TriBuffer[id+_BaseID]];

        float4 position = ShadowCasterPos(v.pos, 1*v.nor);
        o.pos = UnityApplyLinearShadowBias(position);
        return o;
      }

      float4 frag(v2f i) : COLOR
      {
        SHADOW_CASTER_FRAGMENT(i)
      }
      ENDCG
    }
  


  }

}
