// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Scene/Title/title1" {

  Properties {
  

    _MainTex( "Main Tex" , 2D ) = "white" {}
    _ColorMap( "Color Map" , 2D ) = "white" {}
    _Fade( "Fade" , float ) = 1
    _BaseHue( "BaseHue" , float ) = 1
    _HueSize( "HueSize" , float ) = 1
    _Darkness( "Darkness" , float ) = 0
    _Flip( "Flip" , float ) = 0




  }

  SubShader {

        Tags {"Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha


        Cull off
    Pass {

      CGPROGRAM

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"

      uniform sampler2D _MainTex;
      uniform sampler2D _ColorMap;

      uniform float _Fade;
      uniform float _BaseHue;
      uniform float _HueSize;
      uniform float _Darkness;
      uniform float _Flip;


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
          float3 rd         : TEXCOORD2;
      };

    //From IQ shaders
      float hash( float n )
      {
          return frac(sin(n)*43758.5453);
      }

      float noise( float3 x )
      {
          // The noise function returns a value in the range -1.0f -> 1.0f
          x.z += .2 * _Time.y;

          float3 p = floor(x);
          float3 f = frac(x);

          f       = f*f*(3.0-2.0*f);
          float n = p.x + p.y*57.0 + 113.0*p.z;

          return lerp(lerp(lerp( hash(n+0.0), hash(n+1.0),f.x),
                         lerp( hash(n+57.0), hash(n+58.0),f.x),f.y),
                     lerp(lerp( hash(n+113.0), hash(n+114.0),f.x),
                         lerp( hash(n+170.0), hash(n+171.0),f.x),f.y),f.z);
      }

     
      


float tri( float x ){ 
              return abs( frac(x) - .5 );
            }

            float3 tri3( float3 p ){
             
              return float3( 
                  tri( p.z + tri( p.y * 1. ) ), 
                  tri( p.z + tri( p.x * 1. ) ), 
                  tri( p.y + tri( p.x * 1. ) )
              );

            }
                                             
            float triNoise3D( float3 p, float spd , float time){
              
              float z  = 1.4;
                float rz =  0.;
              float3  bp =   p;

                for( float i = 0.; i <= 3.; i++ ){
               
                float3 dg = tri3( bp * 2. );
                p += ( dg + time * .1 * spd );

                bp *= 1.8;
                    z  *= 1.5;
                    p  *= 1.2; 
                  
                float t = tri( p.z + tri( p.x + tri( p.y )));
                rz += t / z;
                bp += 0.14;

                }

                return rz;

            }
      
      VertexOut vert(VertexIn v) {
        
        VertexOut o;

        o.normal = v.normal;

        o.uv = v.texcoord;
       
  
        // Getting the position for actual position
        o.pos = UnityObjectToClipPos(  v.position );
     
        float3 mPos = mul( unity_ObjectToWorld , v.position );

        // The ray origin will be right where the position is of the surface
        o.ro = v.position.xyz;


        float3 camPos = mul( unity_WorldToObject , float4( _WorldSpaceCameraPos , 1. )).xyz;

        // the ray direction will use the position of the camera in local space, and 
        // draw a ray from the camera to the position shooting a ray through that point
        o.rd = normalize( v.position.xyz - camPos );


        return o;

      }

      // Fragment Shader
      fixed4 frag(VertexOut v) : COLOR {

        float scale = 6.7214 / 1.213;

        // Our color starts off at zero,   
        float3 col = float3( 0.0 , 0.0 , 0.0 );

        float2 newUV = v.uv;//v.uv + sin( v.uv.x * 10  + _Time.y + _BaseHue * 100) * float2(0,0.03);

        float3 p;

         float4 dCol = 0;
        float hit = 0;
        //for(int i = 0; i < 1; i++){

            p = v.ro;// + v.rd;// * float(i)* .0;

            float4 col1 = tex2D( _MainTex ,v.uv);
            //if( col1.a > 1.1-_Fade ){
              dCol += col1;
              hit = 1;
              float3 col2 =tex2D(_ColorMap,float2( col1.x*.2 + col1.a * .2+  noise( p  * 10.3 * float3(6,1,10) + float3(0,0,_Time.y) ) * .2+_BaseHue - p.x * 1.5  + _Time.y * .2, 0)).xyz;// / (.3+.5*float(i));
              col += col1.a * lerp(col1 ,col2, 1-_Darkness);//(_Flip - ((_Flip-.5)*2) *saturate(p.x*100+28))*(1-_Darkness) );
            //}//col += triNoise3D( p  * .3 * float3(6,1,1) , 1, _Time.y * .2);//sin( max(max(abs(p.x * 200) , abs(p.y * 200)), abs(p.z * 200)) );
       // }

        dCol.xyz /= 3;
        //col.xyz /= 4;

        //float dCol = col.x;

        float4 tCol = tex2D( _MainTex , newUV);

        //col = dCol.x * tex2D(_ColorMap,float2(( tCol.x * .2) * _HueSize + _Time.y * .1 + _BaseHue + v.uv.x * 1.1, 0));
        //col = dCol.x * tex2D(_ColorMap,float2(dCol.x * 1.1, 0));
        //col = lerp( col , (tCol.xyz) *tex2D(_ColorMap, float2(dCol * 10.6 * v.uv.x ,0)), _Darkness );
            fixed4 color;

        //if(col1.x < _Fade * 2 ){ discard; }
        color = fixed4( col.xyz ,col1.a * _Fade );
        return color;
      }

      ENDCG
    }
  }
  FallBack "Diffuse"
}