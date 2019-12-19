// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
  // Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
  // Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

  Shader "Scene/wordsLeft" {

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

        _CubeMap( "Cube Map" , Cube )  = "defaulttexture" {}

      _ScaleX("scale x" , Float) = 7.827432
      _ScaleY("scale y" , Float) = 1.957961




    }

    SubShader {

          // inside SubShader
  Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
          LOD 100
  // inside Pass
  ZWrite On
  //Blend One One 
          Cull Back
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

        uniform samplerCUBE _CubeMap;

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



float3 depth( float3 ro , float3 rd ){

float3 n = 0;
float3 p;
 for( int i = 0; i < _NumberSteps; i++ ){

            //  if ( hit == false ){
    float stepVal = float(i)/_NumberSteps;

    p = ro + rd * stepVal * _TotalDepth ;
    n += tex2D(_MainTex, p.yx*300 + float2( sin(100*float(i)) , sin(float(i) * 20))).x;
    //n += noise( p * 2000.01 * float3(1,1,1) );


}

return n;

}
        // Fragment Shader
        fixed4 frag(VertexOut v) : COLOR {

                  // Ray origin 
          float3 ro           = v.ro;

          // Ray direction
          float3 rd           = v.rd;       

          // Our color starts off at zero,   
          float3 col = float3( 0.0 , 0.0 , 0.0 );



          float3 p;


          //float4 aVal1 = tex2D( _AudioMap , float2( tCol.w , 0));


          float3 refrR = refract( rd , v.normal , .9);
          float3 refrG = refract( rd , v.normal , .8);
          float3 refrB = refract( rd , v.normal , .7);

          float3 cR = texCUBE(_CubeMap,normalize(refrR));
          float3 cG = texCUBE(_CubeMap,normalize(refrG));
          float3 cB = texCUBE(_CubeMap,normalize(refrB));

         col = float3(depth(ro,refrR).r,depth(ro,refrG).g,depth(ro,refrB).b);
          col /= _NumberSteps;
         
            //col = float3(cR.r,cG.g,cB.b);
        
           
            fixed4 color;
          color = fixed4( col ,1 );
          return color;
        }

        ENDCG
      }
    }
    FallBack "Diffuse"
  }