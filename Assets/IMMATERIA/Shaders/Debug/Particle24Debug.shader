Shader "Debug/Particles24" {
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
      #include "../Chunks/Struct24.cginc"
      #include "../Chunks/debugVSChunk.cginc"

      ENDCG

    }
  }

  Fallback Off


}
