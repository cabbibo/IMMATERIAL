Shader "Scenes/GoldenPit/AmbiClusio"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorMap ("Color Map", 2D) = "white" {}
    }
    SubShader
    {
        LOD 100

        Pass
        {


         Tags {"LightMode" = "ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase

            #include "UnityCG.cginc"// after CGPROGRAM;
            #include "AutoLight.cginc"
 

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {

                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD3;
                float3 worldNor : TEXCOORD4;
               //// float3 nor : NORMAL;
                float ao : TEXCOORD2;// in v2f struct;
                LIGHTING_COORDS(5,6) 

            };

            sampler2D _MainTex;
            sampler2D _ColorMap;
            float4 _MainTex_ST;


struct Transform {
  float4x4 localToWorld;
  float4x4 worldToLocal;
};



StructuredBuffer<Transform> _TransformBuffer;
int _TransformBuffer_COUNT;

float sdSphere( float3 p, float s )
{
  return length(p)-s;
}

// from https://www.shadertoy.com/view/4djSDy
// Sphere occlusion
float sphOcclusion( in float3 pos, in float3 nor, in float4 sph )
{
    float3  di = sph.xyz - pos;
    float l  = length(di);
    float nl = dot(nor,di/l);
    float h  = l/sph.w;
    float h2 = h*h;
    float k2 = 1.0 - h2*nl*nl;

    // above/below horizon: Quilez - http://iquilezles.org/www/articles/sphereao/sphereao.htm
    float res = max(0.0,nl)/h2;
    // intersecting horizon: Lagarde/de Rousiers - http://www.frostbite.com/wp-content/uploads/2014/11/course_notes_moving_frostbite_to_pbr.pdf
    if( k2 > 0.0 ) 
    {
       // #if 1
       //     res = nl*acos(-nl*sqrt( (h2-1.0)/(1.0-nl*nl) )) - sqrt(k2*(h2-1.0));
       //     res = res/h2 + atan( sqrt(k2/(h2-1.0)));
       //     res /= 3.141593;
       // #else
       //     // cheap approximation: Quilez
       //     res = pow( clamp(0.5*(nl*h+1.0)/h2,0.0,1.0), 1.5 );
       // #endif
    res = pow( clamp(0.5*(nl*h+1.0)/h2,0.0,1.0), 1.5 );
    }

    return res;
}

float map( float3 pos ){
    
    float minDist = 100000;
    
    for( int i = 0; i < _TransformBuffer_COUNT; i++ ){

      if( mul( _TransformBuffer[i].localToWorld[0] , float4(0,0,0,1)).x != mul( unity_ObjectToWorld , float4(0,0,0,1)).x ){
      float3 newPos = mul( _TransformBuffer[i].worldToLocal , float4( pos ,1)).xyz;
      float  dist = sdSphere( newPos , .5);
      minDist = min( minDist ,dist);
  }
    }

    return minDist;
}

float calcAO( float3 pos, float3 nor )
{
    float occ = 0.0;
    float sca = 1.0;
    for( int i=0; i<5; i++ )
    {
        float hr = 0.01 + 0.612*float(i)/4.0;
        float3 aopos =  nor * hr + pos;
        float dd = map( aopos ).x;
        occ += -(dd-hr)*sca;
        sca *= 0.5;
    }
    return clamp( 1.0 - 3.0*occ, 0.0, 1.0 );    
}


float calcAO2( float3 pos, float3 nor )
{

    float occ = 0;
    for( int i = 0; i < _TransformBuffer_COUNT; i++ ){

 if( mul( _TransformBuffer[i].localToWorld[0] , float4(0,0,0,1)).x != mul( unity_ObjectToWorld , float4(0,0,0,1)).x ){
        Transform t =  _TransformBuffer[i];

        float3 spherePos = mul(_TransformBuffer[i].localToWorld , float4(0,0,0,1));
        float sphereRad =.5*length(mul(_TransformBuffer[i].localToWorld , float4(1,0,0,0)));

        float ao = sphOcclusion( pos , nor , float4( spherePos , sphereRad ));

        //occ = ao * occ;//
        //if( (1-ao) < occ ){ occ += (1-ao); }

        occ += (1-ao);
    }
    }
    occ/=_TransformBuffer_COUNT;
    return occ;    
}




            v2f vert (appdata v)
            {
                v2f o;

                o.pos = UnityObjectToClipPos(v.vertex);

                  o.worldPos = mul(unity_ObjectToWorld,v.vertex).xyz;
              //  o.nor = v.normal;
             o.worldNor = UnityObjectToWorldNormal(v.normal).xyz;
//
                  o.ao = calcAO2(o.worldPos,o.worldNor);
                 o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // in vert shader;

                    TRANSFER_VERTEX_TO_FRAGMENT(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

               float a = calcAO2(i.worldPos,i.worldNor);
                //in frag shader;
                half atten = LIGHT_ATTENUATION(i);
                // sample the texture

                float4 c = clamp( pow((1-a),2) * 50 ,0,2) *tex2D(_ColorMap , float2( a * 1  , 0 ));
                fixed4 col = c;// * atten;//i.ao * ( atten * .5 + .5);//a;//1-(i.ao*4);//((atten  * .5+.5)  * (1-i.ao)); //atten;// * atten;//tex2D(_MainTex, i.uv);
          
                return col;
            }
            ENDCG
        }
    }
    
    Fallback "Diffuse"

}
