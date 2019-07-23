Shader "Scene/Monolith" {

  Properties {
  
 
        _ColorMap("ColorMap", 2D) = "" {}
        _TexMap("TextureMap", 2D) = "" {}


  }

  SubShader {


    Pass {

      CGPROGRAM

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"


      int _NumStories;
      int _ThisStory;
      float3 _StoryPositions[30];
      float3 _PlayerPosition;


      struct VertexIn{
         float4 position  : POSITION; 
         float3 normal    : NORMAL; 
         float4 texcoord  : TEXCOORD0; 
         float4 tangent   : TANGENT;
      };


      struct VertexOut {
          float4 pos        : POSITION; 
          float3 normal     : NORMAL; 
          float4 uv         : TEXCOORD0; 
          float3 ro         : TEXCOORD1;
          float3 rd       : TEXCOORD2;
          float3 player     : TEXCOORD3;
      };


    
            sampler2D _ColorMap;
            sampler2D _TexMap;
      
      VertexOut vert(VertexIn v) {
        
        VertexOut o;

        o.normal = v.normal;

        o.uv = v.texcoord;
       
  
        // Getting the position for actual position
        o.pos = UnityObjectToClipPos(  v.position );
     
        float3 mPos = mul( unity_ObjectToWorld , v.position );

        // The ray origin will be right where the position is of the surface
        o.ro = mPos;


        float3 camPos = mul( unity_WorldToObject , float4( _WorldSpaceCameraPos , 1. )).xyz;

        // the ray direction will use the position of the camera in local space, and 
        // draw a ray from the camera to the position shooting a ray through that point
        o.rd = mPos.xyz - _WorldSpaceCameraPos;

        o.player = mPos.xyz - _PlayerPosition;

        return o;

      }

      // Fragment Shader
      fixed4 frag(VertexOut v) : COLOR {




                // Ray origin 
        float3 ro           = v.ro;

        float dif = 10000;
        int closestID = 1000;

        float4 tCol = tex2D(_TexMap , v.ro.xy * .7 );
        for( int i = 0; i < _NumStories; i++ ){
            float d = length( ro - _StoryPositions[i] );
            if( d < dif - length(tCol) * .001){
                dif = d;
                closestID = i;
            }
        }

        float thisDif = length(_StoryPositions[_ThisStory] - v.ro);

        // Ray direction
        float3 rd           = v.rd;    



        // Our color starts off at zero,   
        float3 col = tex2D(_ColorMap, float2( (((dif * 6 - _Time.y * .3 + length( tCol) *5 ) % 1) * .4) + length( tCol) * .1 ,0 )).xyz / ( .4 + .2*thisDif *thisDif + dif);

        if( thisDif < .45  - .3* length( tCol)){ col = 1;}
        //if( closestID == _ThisStory ){ col *= 4;}
        float4 color = fixed4( col , 1. );
        return color;
      }

      ENDCG
    }
  }
  FallBack "Diffuse"
}
