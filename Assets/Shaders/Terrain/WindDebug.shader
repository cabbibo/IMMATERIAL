
Shader "Debug/WindDebug" {
    Properties {

    _Color ("Color", Color) = (1,1,1,1)
    _Size ("Size", float) = .01
    _Up ("Up", float) = .01
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
      uniform float _Up;
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



float3 terrainNewPos( float3 pos ){
    float3 wp = pos;
    float4 c = tex2Dlod(_HeightMap , float4(( wp.xz ) * _MapSize + .5 /1024 ,0,0) );
    wp.y = float3(0,1,0) * c.r * _MapHeight;
    return wp;
}



float3 terrainGetNormal( float3 pos ){

  float delta =.1;
  float3 dU = terrainNewPos( pos + float3(delta,0,0) );
  float3 dD = terrainNewPos( pos + float3(-delta,0,0) );
  float3 dL = terrainNewPos( pos + float3(0,0,delta) );
  float3 dR = terrainNewPos( pos + float3(0,0,-delta) );

    return normalize(cross(normalize(dU.xyz-dD.xyz),normalize(dR.xyz-dL.xyz)));


}


float4 terrainSampleColor( float3 pos ){
  return tex2Dlod(_HeightMap , float4(pos.xz * _MapSize+ .5 /1024,0,0) );
}


//Our vertex function simply fetches a point from the buffer corresponding to the vertex index
//which we transform with the view-projection matrix before passing to the pixel program.
varyings vert (uint id : SV_VertexID){

  varyings o;

  int base = id / 3;
  int alternate = id %3;

  float3 extra;

  float2 uv = float2(0,0);

      Vert v = _VertBuffer[base];
      float4 t = terrainSampleColor( v.pos );

    float3 tan = normalize(float3( t.y -.5 ,0, t.z -.5 ));

float3 dir = tan; 

float3 viewDir = UNITY_MATRIX_IT_MV[2].xyz;

float3 yVal = normalize( cross( dir , float3(0,1,0) ));

  if( alternate == 0 ){ extra =  -yVal * .1; uv = float2(0,0); }
  if( alternate == 1 ){ extra =  +yVal  * .1; uv = float2(1,0); }
  if( alternate == 2 ){ extra =  dir; uv = float2(.5,1); }


      o.worldPos = v.pos.xyz + float3(0,_Up,0) +extra * _Size;
      o.debug = normalize(dir);
      o.uv = uv * v.debug.x * 3;

      o.pos = mul (UNITY_MATRIX_VP, float4(o.worldPos,1.0f));

  

  return o;

}




//Pixel function returns a solid color for each point.
float4 frag (varyings v) : COLOR {
   return float4(v.debug * .5 + .5,1 );
    return 1;
}

      ENDCG

    }
  }

  Fallback Off


}
