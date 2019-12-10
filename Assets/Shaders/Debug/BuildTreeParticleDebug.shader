Shader "Debug/BuildParticles" {
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
  float3 vel;
  float3 nor;
  float3 tang;
  float lookupStart;
  float lookupLength;
  float  parent;
  float  debug;
};
      uniform int _Count;
      int _SelectedVert;
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
          float id        : TEXCOORD5;
          float selected  : TEXCOORD7;
      };

//Our vertex function simply fetches a point from the buffer corresponding to the vertex index
//which we transform with the view-projection matrix before passing to the pixel program.
varyings vert (uint id : SV_VertexID){

  varyings o;

  int base = id / 6;
  int alternate = id %6;

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

      Vert v = _VertBuffer[base];
      o.debug.x = v.tang.y;

      o.selected = 0;
      if( base == _SelectedVert ){ extra *= 5; o.selected = 1;}
      if( v.tang.y == -1 ){ extra *= 2; }
      if( v.tang.y == -2 ){ extra *= 4; }
      o.worldPos = (v.pos) + extra * _Size;
      o.uv2 = uv;
      o.id = base;
      o.pos = mul (UNITY_MATRIX_VP, float4(o.worldPos,1.0f));

  }

  return o;

}

  #include "../Chunks/hsv.cginc"
      //Pixel function returns a solid color for each point.
      float4 frag (varyings v) : COLOR {

          float l =(.5-length( v.uv2 -.5)) * 2;
          if( l<0 ){ discard;}



          float3 col = hsv( v.debug.x * .1,1,1);
          if( v.debug.x < 0 ){ col = 1;}
          if( v.debug.x < -1 ){ col = hsv( l * .2 + _Time.y,1,1);}
          col +=  v.selected;
          return float4(col,1 );
      }

      ENDCG

    }
  }


}
