Shader "Debug/TriConnectedDebug" {
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
      float3 tan;
      float2 uv;
      float2 offset;
      float4 debug;
      float2 connections[16];
    };
      #include "../Chunks/debugVSChunk.cginc"



      //Pixel function returns a solid color for each point.
      float4 frag (varyings v) : COLOR {

          if( length( v.uv2 -.5) > .5 ){ discard;}
          return float4(_Color,1 );
      }

      ENDCG

    }
  }


}
