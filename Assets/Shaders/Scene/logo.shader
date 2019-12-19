  // Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
  // Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
  // Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

  Shader "Custom/DepthText2" {

    Properties {
    
          // This is how many steps the trace will take.
          // Keep in mind that increasing this will increase
          // Cost
      _NumberSteps( "Number Steps", Int ) = 4

      // Total Depth of the trace. Deeper means more parallax
      // but also less precision per step
      _TotalDepth( "Total Depth", Float ) = 0.16


      _NoiseSize( "Noise Size", Float ) = 10
      _NoiseSpeed( "Noise Speed", Float ) = 10
      _HueSize( "Hue Size", Float ) = .3
      _BaseHue( "Base Hue", Float ) = .3

      _MainTex( "Main Tex" , 2D ) = "white" {}
      _MainTex2( "Main Tex2" , 2D ) = "white" {}
      _ColorMap( "Color Map" , 2D ) = "white" {}

      _ScaleX("scale x" , Float) = 7.827432
      _ScaleY("scale y" , Float) = 1.957961




    }

    SubShader {

          // inside SubShader
  Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
          LOD 100
  // inside Pass
  ZWrite Off
  Blend One One 
          Cull off
      Pass {

        CGPROGRAM

        #pragma vertex vert
        #pragma fragment frag

        #include "UnityCG.cginc"


        uniform int _NumberSteps;
        uniform float _TotalDepth;
        uniform float _NoiseSize;
        uniform float _NoiseSpeed;
        uniform float _HueSize;
        uniform float _BaseHue;
        uniform float _Cutoff;
        uniform float _Fade;



        uniform sampler2D _AudioMap;
        uniform sampler2D _MainTex;
        uniform sampler2D _MainTex2;
        uniform sampler2D _ColorMap;

        uniform float _ScaleX;
        uniform float _ScaleY;

        uniform float _CurrTexture;

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


              float3 hsv(float h, float s, float v){
          return lerp( float3( 1.0,1,1 ), clamp(( abs( frac(h + float3( 3.0, 2.0, 1.0 ) / 3.0 )
                               * 6.0 - 3.0 ) - 1.0 ), 0.0, 1.0 ), s ) * v;
        }

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

        float getFogVal( float3 pos ){

          pos *= _NoiseSize * .3;

          float patternVal = 1;//sin( length( pos )  * _PatternSize )+1;
          float noiseVal = triNoise3D( pos * .1 , 1 , _Time.y * _NoiseSpeed )+1.6 + triNoise3D( pos * .5 , 1 , _Time.y* _NoiseSpeed ) * .5 + triNoise3D( pos * 2, 1 , _Time.y* _NoiseSpeed ) * .1;
          return patternVal * noiseVal;
        }
        

        /*float getFogVal( float3 pos ){
          pos *= _NoiseSize;
          float oct1 = noise( pos * 3 + _Time.y * .3 * _NoiseSpeed );
          float oct2 =.5 * noise( pos * 8 + _Time.y * .2 * _NoiseSpeed ) ;
          float oct3 =.25 * noise( pos * 20 + _Time.y * .1 *_NoiseSpeed);
          float v =  oct1 + oct2 + oct3;
          return  v * v * v * .4 ;
        }*/
        

        
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


        float3 doCol( float3 ro , float3 rd){
          float3 p;
          float3 col = 0;
                   bool hit = false;
          for( int i = 0; i < _NumberSteps; i++ ){

                       float stepVal = float(i)/_NumberSteps;

            p = ro + rd * stepVal * _TotalDepth * .5 ;

            float2 xy =  float2( p.x / _ScaleX , p.y / _ScaleY ) * 1.3    + float2( .5 , .5);

           float n2 = tex2D( _MainTex2 , xy.yx + stepVal * .4 ).b;

            xy = clamp( xy + n2 * .01 * _NoiseSize, float2(0,0) , float2(1,1));
            float4 tColStep = tex2D( _MainTex , xy  ); 

            float opacity = tColStep.a;

            if( opacity > 0.2  && xy.x > 0 && xy.x < 1 && xy.y > 0 && xy.y < 1){
      
               col +=  n2;
              hit = true;
            }
          
          }

              col /=  float( _NumberSteps);

              if( hit == false ){
                discard;
              }

              return col;
        }

        // Fragment Shader
        fixed4 frag(VertexOut v) : COLOR {

                  // Ray origin 
          float3 ro           = v.ro;

          // Ray direction
          float3 rd           = v.rd;       

        



          float3 p;

          float4 tCol = tex2D( _MainTex , v.uv);

          float fV = getFogVal( ro ); 
          float fade = 1;

          float4 aVal1 = tex2D( _AudioMap , float2( tCol.w , 0));

          


 
          float3 colR = doCol( ro , refract( rd , v.normal , .9) );
          float3 colG = doCol( ro , refract( rd , v.normal , .7) );
          float3 colB = doCol( ro , refract( rd , v.normal , .5) );

          float3 col = float3(colR.r , colG.g ,colB.b);
         


              fixed4 color;
          color = fixed4( col* _Fade , _Fade );
          return color;
        }

        ENDCG
      }
    }
    FallBack "Diffuse"
  }