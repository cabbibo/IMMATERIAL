Shader "Unlit/CaveOnTerrain"
{
    Properties
    {
 
    _Color ("Color", Color) = (1,1,1,1)

    _MainTex ("Texture", 2D) = "white" {}


            _ColorMap ("ColorMap", 2D) = "white" {}
    _NormalMap ("NormalMap", 2D) = "white" {}
    _CubeMap( "Cube Map" , Cube )  = "defaulttexture" {}
    _Debug("DEBUG",float) = 0
    _HueStart("_HueStart",float) = 0
    _PlayerFalloff("_PlayerFalloff",float) = 0
    
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
    Cull Off
        Pass
        {
            CGPROGRAM
            
               #pragma target 4.5
      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

      #include "UnityCG.cginc"
      #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 nor : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                float3 eye : TEXCOORD3;
        UNITY_SHADOW_COORDS(4)
            };

 




#include "../Chunks/ComputeTerrainInfo.cginc"

      #include "../Chunks/hsv.cginc"
      #include "../Chunks/noise.cginc"
      float3 _Color;
      float3 _PlayerPosition;
      float3 _TerrainHole;

      bool _Debug;
      float _HueStart;
      float _PlayerFalloff;
      sampler2D _MainTex;
      sampler2D _ColorMap;
      sampler2D _NormalMap;
      samplerCUBE _CubeMap;


float3 tPos( float3 wPos  , float2 uv  ){
    float3 pos = worldPos( wPos);// terrainWorldPos( vWorld );
    float tVal = 2*abs(uv.x-.5);
                tVal = 3 * ( tVal * tVal ) - 2 * ( tVal * tVal * tVal );
                return lerp( pos , wPos ,(1-tVal) * pow( uv.y , 1) );
}

            v2f vert (appdata v)
            {
                v2f o;

                float3 inputPos = v.vertex.xyz;


                    float tVal = (1-v.uv.y);
                tVal = 3 * ( tVal * tVal ) - 2 * ( tVal * tVal * tVal );
                inputPos.x *= tVal + .3;
                inputPos.z += v.uv.y * .4;

                float4 vWorld =   mul(unity_ObjectToWorld, float4(inputPos,1));//terrainWorldPos( v.vertex ) - float4(0,0,_Vertical,0);
           
                float3 fPos = tPos( vWorld , v.uv);

                float3 nor = normalize(cross( 
                       normalize( tPos( vWorld + float3(.1,0,0) , v.uv ) - tPos( vWorld - float3(.1,0,0) , v.uv )), 
                       normalize( tPos( vWorld + float3(0,0,.1) , v.uv ) - tPos( vWorld - float3(0,0,.1) , v.uv )) ));


o.eye = _WorldSpaceCameraPos - fPos;
                o.worldPos = fPos;
                o.nor = -nor;//getNormal(vWorld);
                o.uv = v.uv;//TRANSFORM_TEX(v.uv, _MainTex);
                o.pos = mul (UNITY_MATRIX_VP, float4(fPos,1.0f));
        UNITY_TRANSFER_SHADOW(o,o.worldPos);
                return o;
            }


            fixed4 frag (v2f v) : SV_Target
            {
                // sample the texture

                   float4 color = tex2D(_MainTex,v.worldPos.xz * .1 );
        float4 hCol = sampleColor(v.worldPos );

        float3 fNor = normalize(float3(
            2*noise(v.worldPos* 2 )-1,
            2*noise(v.worldPos* 2 +50)-1,
            2*noise(v.worldPos* 2 +20 )-1
        ));

        fNor += 2*normalize(float3(
            2*noise(v.worldPos* .4 )-1,
            2*noise(v.worldPos* .4 +50)-1,
            2*noise(v.worldPos* .4 +20 )-1
        ));

        fNor += .4 * normalize(float3(
            2*noise(v.worldPos* 10 )-1,
            2*noise(v.worldPos* 10 +50)-1,
            2*noise(v.worldPos* 10 +20 )-1
        ));

        fNor = tex2D(_NormalMap , v.worldPos.xz * .04 );
       // fNor += 2*v.nor;
        

        fNor = normalize(v.nor * fNor.z + float3(1,0,0) * (fNor.x)  + float3(0,0,1) * (fNor.y-.5));//normalize( fNor );

        float3 glint = tex2D(_NormalMap , v.worldPos.xz * .04 ) + tex2D(_NormalMap , v.worldPos.xz * .14 );

        glint = normalize((glint)-1);

        float eyeM = abs(dot(fNor, normalize(v.eye)));
    
        fixed shadow = UNITY_SHADOW_ATTENUATION(v,v.worldPos)  ;
float dif = length( v.worldPos - _PlayerPosition );

float l = saturate( (_PlayerFalloff-dif)/_PlayerFalloff);
        color.xyz = .4*pow(length(color.xyz),4);

        float match = dot( fNor, _WorldSpaceLightPos0 );

        float3 refl = reflect( normalize(v.eye) , fNor );
        float reflM = dot( refl , _WorldSpaceLightPos0 );



        float grassHeight = (hCol.w * 5 + noise( v.worldPos * .2+ float3(0,_Time.y * .2,0) + fNor * _Time.y * .01 ) * .4) / 5;
        color.xyz = tex2D(_ColorMap, float2( reflM * reflM  * .2 + .6 + dif * .03, 0)) * l ;




        color = tex2D(_MainTex,v.worldPos.xz * .1);
        color = tex2D(_ColorMap, float2(color.x * .2 - dif * .01+.6 + grassHeight * .7 + _HueStart, 0)) * l ;



        float3 tCol = texCUBE(_CubeMap,refl) * color;
        color *= ( grassHeight + .5);

        color *= texCUBE( _CubeMap , refl ) * 2;



                fixed3 col = color;//v.nor * .5 + .5;
                return float4(col,1);
            }
            ENDCG
        }
    }
}
