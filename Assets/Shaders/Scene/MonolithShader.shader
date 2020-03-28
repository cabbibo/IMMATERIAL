Shader "Scene/Monolith" {

  Properties {
  
 
        _ColorMap("ColorMap", 2D) = "" {}
        _TexMap("TextureMap", 2D) = "" {}
        _HueStart("Hue start", float ) = 1

  }

  SubShader {


    Pass {

      CGPROGRAM

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"
      #include "../Chunks/noise.cginc"


      int _NumStories;
      int _WhichStory;
      int _ConnectedStory;
      float _HueStory;
      float3 _StoryPositions[30];
      float3 _PlayerPosition;

      float _FadeValue;


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
          float3 thisDif : TEXCOORD4;
      };


    
            sampler2D _ColorMap;
            sampler2D _TexMap;
            float _HueStart;


//https://stackoverflow.com/questions/9246100/how-can-i-implement-the-distance-from-a-point-to-a-line-segment-in-glsl
            float DistToLine(float2 pt1, float2 pt2, float2 testPt)
{
  float2 lineDir = pt2 - pt1;
  float2 perpDir = float2(lineDir.y, -lineDir.x);
  float2 dirToPt1 = pt1 - testPt;
  return abs(dot(normalize(perpDir), dirToPt1));
}



float PointOnLine(float2 p1, float2 p2, float2 p)
{


  return  (dot(p1-p,(p1-p2)) / dot((p1-p2),(p1-p2)));

}
      
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
        o.thisDif = mPos.xyz - _StoryPositions[_WhichStory];

        return o;

      }


      sampler2D _AudioMap;

      // SDF for capsule from  https://www.iquilezles.org/www/articles/distfunctions/distfunctions.htm
float sdCapsule( float3 p, float3 a, float3 b, float r )
{
    float3 pa = p - a, ba = b - a;
    float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
    return length( pa - ba*h ) - r;
}

      // Fragment Shader
      fixed4 frag(VertexOut v) : COLOR {




                // Ray origin 
        float3 ro           = v.ro;

        float dif = 10000;
        int closestID = 1000;

        float4 tCol = tex2D(_TexMap , v.ro.xy * float2( 1,.7) );
        for( int i = 0; i < _NumStories; i++ ){
            float d = length( ro - _StoryPositions[i] );
            if( d < dif - length(tCol) * .0001){
                dif = d;
                closestID = i;
            }
        }

        float3 thisDifVec = (_StoryPositions[_WhichStory] - v.ro);
        float3 connectedDifVec = (_StoryPositions[_ConnectedStory]- v.ro);

        float thisDif = length(thisDifVec);
        float connectedDif = length(connectedDifVec);

        // Ray direction
        float3 rd           = v.rd;    



        // Our color starts off at zero,   
        //float3 col = tex2D(_ColorMap, float2( _HueStart + (((dif * 6 - _Time.y * .3 + length( tCol) *5 ) % 1) * .4) + length( tCol) * .01 ,0 )).xyz / ( .4 + .2*thisDif *thisDif + dif);
        float3 col = tex2D(_ColorMap, float2(thisDif * .1 + dif * .3+ _HueStart,0)).xyz / ( .4 + .2*thisDif *thisDif + dif);

         //if( closestID == _WhichStory ){ col *= 4;}

         float distToLine = DistToLine( _StoryPositions[_WhichStory].xy , _StoryPositions[_ConnectedStory] , ro.xy );
         float linePoint = PointOnLine( _StoryPositions[_WhichStory].xy , _StoryPositions[_ConnectedStory] , ro.xy );
        

         if( _WhichStory == _ConnectedStory || _ConnectedStory < 0 ){
          distToLine = 10000000;
         }
       
        col *= tex2D(_AudioMap , float2(dif * .1 + tCol.x * .01 ,0)) / (10 *dif);
        if( thisDif < .18 + .005 * sin(_Time.y*4) + tCol.x * .1  ){ col *= 2;}
        if( connectedDif < .135 + .005 * sin(_Time.y*2)+ tCol.x * .1   && _ConnectedStory >= 0){ col *= .1;}
        
        if( distToLine * 3 + tCol.x * .2 + noise( ro + _Time.y ) * .1 < .3 || thisDif < .18 ||  connectedDif < .18 ){
          //col = 1;
        }

        if(linePoint > 0 && linePoint < 1 && distToLine  - tCol.x * .1< .02 && dif- tCol.x * .05 > .15 ){
            col *= 2 + length(tCol);
          }

        float4 color = fixed4(col, 1. ) * _FadeValue;
        //float4 color = _FadeValue;
        return color;
      }

      ENDCG
    }
  }
  FallBack "Diffuse"
}
