Shader "Debug/TerrainDataDebug" {
    Properties {

    _Color ("Color", Color) = (1,1,1,1)
    _Size ("Size", float) = .01
    }


  SubShader{
    Cull Off
    Pass{

      CGPROGRAM
      
      #pragma target 4.5

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"

    struct Vert{
      float3 pos;
      float4 info1;
      float4 info2;
      float4 info3;
      float4 info4;
      float  debug;
    };





      uniform int _Count;
      uniform float _Size;
      uniform float3 _Color;

      
      StructuredBuffer<Vert> _VertBuffer;


      //uniform float4x4 worldMat;

      //A simple input struct for our pixel shader step containing a position.
      struct varyings {
          float4 pos      : SV_POSITION;
          float3 nor      : TEXCOORD0;
          float3 worldPos : TEXCOORD1;
          float3 eye      : TEXCOORD2;
          float3 debug    : TEXCOORD3;
          float2 uv       : TEXCOORD4;
          float2 uv2       : TEXCOORD6;
          float which       : TEXCOORD7;
          float id        : TEXCOORD5;
      };

//Our vertex function simply fetches a point from the buffer corresponding to the vertex index
//which we transform with the view-projection matrix before passing to the pixel program.
varyings vert (uint id : SV_VertexID){

  varyings o;

  int base = id / 6;
  int alternate = id %6;

  int which  = base / _Count;
  base %= _Count;

  if( base < _Count ){

      float3 extra = float3(0,0,0);

    float3 l = UNITY_MATRIX_V[0].xyz;
    float3 u = UNITY_MATRIX_V[1].xyz;
    
    float2 uv = float2(0,0);

    if( alternate == 0 ){ extra = -l - u; uv = float2(0,0); }
    if( alternate == 1 ){ extra =  l - u; uv = float2(1,0); }
    if( alternate == 2 ){ extra =  l + u; uv = float2(1,1); }
    if( alternate == 3 ){ extra = -l - u; uv = float2(0,0); }
    if( alternate == 4 ){ extra =  l + u; uv = float2(1,1); }
    if( alternate == 5 ){ extra = -l + u; uv = float2(0,1); }


      Vert v = _VertBuffer[base % _Count];

        if( which == 0 ){
            o.worldPos = (v.pos) + extra * _Size * v.info1.x;
        }else if( which == 1 ){
            o.worldPos = (v.pos) + extra * _Size * v.info1.y;
        }else if( which == 2 ){
            o.worldPos = (v.pos) + extra * _Size * v.info1.z;
        }else if( which == 3 ){
            o.worldPos = (v.pos) + extra * _Size * v.info1.w;
        }else if( which == 4 ){
            o.worldPos = (v.pos) + extra * _Size * v.info2.x;
        }else if( which == 5 ){
            o.worldPos = (v.pos) + extra * _Size * v.info2.y;
        }else if( which == 6 ){
            o.worldPos = (v.pos) + extra * _Size * v.info2.z;
        }else if( which == 7 ){
            o.worldPos = (v.pos) + extra * _Size * v.info2.w;
        }else if( which == 8 ){
            o.worldPos = (v.pos) + extra * _Size * v.info3.x;
        }else if( which == 9 ){
            o.worldPos = (v.pos) + extra * _Size * v.info3.y;
        }else if( which == 10 ){
            o.worldPos = (v.pos) + extra * _Size * v.info3.z;
        }else if( which == 11 ){
            o.worldPos = (v.pos) + extra * _Size * v.info3.w;
        }else if( which == 12 ){
            o.worldPos = (v.pos) + extra * _Size * v.info4.x;
        }else if( which == 13 ){
            o.worldPos = (v.pos) + extra * _Size * v.info4.y;
        }else if( which == 14 ){
            o.worldPos = (v.pos) + extra * _Size * v.info4.z;
        }else if( which == 15 ){
            o.worldPos = (v.pos) + extra * _Size * v.info4.w;
        }

        o.worldPos += float3( 0 , float(which) * _Size , 0);

        o.which =float(which);
      //o.worldPos = extra * _Size;
      o.eye = _WorldSpaceCameraPos - o.worldPos;
      o.uv2 = uv;
      o.id = base;
      o.pos = mul (UNITY_MATRIX_VP, float4(o.worldPos,1.0f));

  }

  return o;

}


#include "../Chunks/hsv.cginc"

      //Pixel function returns a solid color for each point.
      float4 frag (varyings v) : COLOR {

          float3 col = hsv( v.which / 16,1,1);
       //   if( length( v.uv2 -.5) > .5 ){ discard;}
          return float4(col,1 );
      }

      ENDCG

    }
  }


}
