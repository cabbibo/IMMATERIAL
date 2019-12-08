
Shader "Debug/BuildConnections" {
    Properties {

    _Color ("Color", Color) = (1,1,1,1)
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
      uniform float3 _Color;





      StructuredBuffer<Vert> _VertBuffer;
      StructuredBuffer<int> _ConnectionBuffer;

      //A simple input struct for our pixel shader step containing a position.
      struct varyings {
          float4 pos : SV_POSITION;
          float which :TEXCOORD1;
      };

      //Our vertex function simply fetches a point from the buffer corresponding to the vertex index
      //which we transform with the view-projection matrix before passing to the pixel program.
      varyings vert (uint id : SV_VertexID){

        varyings o;

        // Getting ID information
        
        int baseConnection = id / 2;
        int alternate = id %2;

        // Making sure we aren't looking up into a bad areas
       // if( baseConnection  < _Count ){
        Vert v1 = _VertBuffer[baseConnection];
        int lookup = _ConnectionBuffer[v1.lookupStart];

         // int t1 = _ConnectionBuffer[baseConnection];
         // Vert v1 = _VertBuffer[baseConnection];
          Vert v2 = _VertBuffer[lookup];

          //if( t1 < 30 && t2 < 30){

          float3 pos;
          if( alternate == 0 ){
            pos = v1.pos;
            //o.which = float(t1);
          }else{
            pos = v2.pos;
            //o.which = float(t2);
          }

          
          //o.which = alternate;
          o.pos = mul (UNITY_MATRIX_VP, float4(pos,1.0f));
        //}else{
        //  o.pos = 0;
        //}
        //}

        return o;

      }


      #include "../Chunks/hsv.cginc"
      //Pixel function returns a solid color for each point.
      float4 frag (varyings v) : COLOR {
          return float4( hsv( .3, 1,1) , 1 );
      }
      ENDCG

    }
  }


}
