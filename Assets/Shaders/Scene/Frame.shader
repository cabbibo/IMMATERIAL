Shader "Scene/Frame" {

  Properties {
  
    // This is how many steps the trace will take.
    // Keep in mind that increasing this will increase
    // Cost
    _NumberSteps( "Number Steps", Int ) = 3

    // Total Depth of the trace. Deeper means more parallax
    // but also less precision per step
    _TotalDepth( "Total Depth", Float ) = 0.16


    _NoiseSize( "Noise Size", Float ) = 10
    _HueSize( "Hue Size", Float ) = .3
    _BaseHue( "Base Hue", Float ) = .3

    _FadeMax( "Fade Max", float ) = 25
    _FadeMin( "Fade Min", float ) = 5


    _Cutoff("_Cutoff" , float ) = 0
    _Hovered("_Hovered" , float ) = 0
    _Restricted("_Restricted" , float ) = 0



  }

  SubShader {


    Pass {

      CGPROGRAM

      #pragma vertex vert
      #pragma fragment frag

      #include "../Chunks/frameVSChunk.cginc"
     

      ENDCG
    }


    Pass {
      Tags {
        "LightMode" = "ShadowCaster"
      }

      CGPROGRAM
            #pragma vertex vert
      #pragma fragment frag
      #include "../Chunks/frameVSChunk.cginc"

      ENDCG
    }
    
  }
  FallBack "Diffuse"
}
