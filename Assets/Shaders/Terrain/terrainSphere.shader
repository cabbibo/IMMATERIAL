
// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Debug/TerrainSphere" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Vertical("_Vertical",float)= 0
    }

    SubShader{
        Cull Off

    Tags { "RenderType"="Opaque" }
        LOD 200


        CGPROGRAM


        #pragma target 4.5

        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard
        #pragma vertex vert 


        #include "UnityCG.cginc"


        struct Input {
            float2 uv_MainTex;
            float3 worldPosition : TEXCOORD5;
            float4 color : TEXCOORD3;
            float3 nor : TEXCOORD4;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
sampler2D _HeightMap;
float _MapSize;
float _MapHeight;
float _Vertical;


float3 terrainWorldPos( float4 pos ){
    float3 wp = mul( unity_ObjectToWorld, pos ).xyz;
    float4 c = tex2Dlod(_HeightMap , float4(wp.xz * _MapSize,0,0) );
    wp.xyz += float3(0,1,0) * c.r * _MapHeight;
    return wp;
}

float4 terrainNewPos( float4 pos ){
    float4 wp = float4(terrainWorldPos( pos ) ,1 );
    return mul( unity_WorldToObject, wp);
}



float3 terrainGetNormal( float4 pos ){

  float delta =.0001;
  float4 dU = terrainNewPos( pos + float4(delta,0,0,0) );
  float4 dD = terrainNewPos( pos + float4(-delta,0,0,0) );
  float4 dL = terrainNewPos( pos + float4(0,delta,0,0) );
  float4 dR = terrainNewPos( pos + float4(0,-delta,0,0) );

    return normalize(cross(normalize(dU.xyz-dD.xyz),normalize(dR.xyz-dL.xyz)));


}


float4 terrainSampleColor( float4 pos ){
  float3 wp = mul( unity_ObjectToWorld, pos ).xyz;
  return tex2Dlod(_HeightMap , float4(wp.xz * _MapSize,0,0) );
}

        #include "../Chunks/noise.cginc"
        #include "../Chunks/hsv.cginc"


        void vert (inout appdata_full v,out Input o) {
                UNITY_INITIALIZE_OUTPUT(Input,o);
                //o.nor = terrainGetNormal( v.vertex );
                o.worldPosition = mul(unity_ObjectToWorld, float4(v.vertex.xyz,1)).xyz;//terrainWorldPos( v.vertex ) - float4(0,0,_Vertical,0);
               // v.vertex = terrainNewPos( v.vertex )- float4(0,0,_Vertical,0);//mul( unity_WorldToObject, worldPos);
                //o.color = terrainSampleColor( v.vertex );
      }



        void surf (Input IN, inout SurfaceOutputStandard o) {

            
            // Albedo comes from a texture tinted by color
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Emission.xyz = IN.worldPosition.y * 1;//* (IN.nor * .5 + .5);// hsv(IN.normal.y * .5,1,1);


            float h =  tex2D(_HeightMap , float2(IN.worldPosition.xz * _MapSize) ).x *  _MapHeight;
            float dif = abs(IN.worldPosition.y - h);
o.Emission.xyz = 1-dif;

if( dif> 1){ discard; }
            //if( ((IN.worldPosition.y * .3)+ noise( IN.worldPosition * .2 ) * .1)  % 1 < .8 ){ discard; }
            //IN.color.w * 1;//float3(1,1,1);//c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
    
        }

            ENDCG


    }

    FallBack "Diffuse"
}
