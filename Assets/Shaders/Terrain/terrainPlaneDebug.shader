
Shader "Debug/TerrainPlaneDebug" {
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
      #include "../Chunks/Struct12.cginc"





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
          float id        : TEXCOORD5;
      };


sampler2D _HeightMap;
float _MapSize;
float _MapHeight;

int _Dimensions;


float3 terrainWorldPos( float4 pos ){
    float3 wp = mul( unity_ObjectToWorld, pos ).xyz;
    float4 c = tex2Dlod(_HeightMap , float4(( wp.xz ) * _MapSize - .5 /1024 ,0,0) );
    wp.xyz += float3(0,1,0) * c.r * _MapHeight;
    return wp;
}

float4 terrainNewPos( float4 pos ){
    float4 wp = float4(terrainWorldPos( pos ) ,1 );
    return mul( unity_WorldToObject, wp);
}



float3 terrainGetNormal( float4 pos ){

  float delta =2;
  float4 dU = terrainNewPos( pos + float4(delta,0,0,0) );
  float4 dD = terrainNewPos( pos + float4(-delta,0,0,0) );
  float4 dL = terrainNewPos( pos + float4(0,0,delta,0) );
  float4 dR = terrainNewPos( pos + float4(0,0,-delta,0) );

    return normalize(cross(normalize(dU.xyz-dD.xyz),normalize(dR.xyz-dL.xyz)));


}


float4 terrainSampleColor( float4 pos ){
  float3 wp = mul( unity_ObjectToWorld, pos ).xyz;
  return tex2Dlod(_HeightMap , float4(wp.xz * _MapSize,0,1) );
}


//Our vertex function simply fetches a point from the buffer corresponding to the vertex index
//which we transform with the view-projection matrix before passing to the pixel program.
varyings vert (uint id : SV_VertexID){

  varyings o;

  int base = id / 6;
  int alternate = id %6;

  float3 extra = float3(0,0,0);

  float3 l = float3(1,0,0);
  float3 u = float3(0,0,1);
  float2 uv = float2(0,0);

  if( alternate == 0 ){ extra =  0; uv = float2(0,0); }
  if( alternate == 1 ){ extra =  l; uv = float2(1,0); }
  if( alternate == 2 ){ extra =  l + u; uv = float2(1,1); }
  if( alternate == 3 ){ extra =  0; uv = float2(0,0); }
  if( alternate == 4 ){ extra =  l + u; uv = float2(1,1); }
  if( alternate == 5 ){ extra =  u; uv = float2(0,1); }

  float x = float( base / _Dimensions);
  float y = float( base % _Dimensions);

  float3 basePos = float3( x , 0 , y ) + extra;
  float4 p = float4( basePos , 1 );


      Vert v = _VertBuffer[base % _Count];
      o.worldPos = terrainNewPos( p ).xyz;//(v.pos) + extra * _Size;
      //o.eye = _WorldSpaceCameraPos - o.worldPos;
      o.nor =terrainGetNormal( p );
      //o.uv = v.uv;
      //o.id = base;
      o.pos = mul (UNITY_MATRIX_VP, float4(o.worldPos,1.0f));

  

  return o;

}




//Pixel function returns a solid color for each point.
float4 frag (varyings v) : COLOR {
    return float4(v.nor * .5 + .5,1 );
}

      ENDCG

    }
  }

  Fallback Off


}
