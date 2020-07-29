Shader "Scenes/SpacePup/Body"
{
    Properties {

       _ColorMap ("Color", 2D) = "white" {}
       _TextureMap ("Texture", 2D) = "white" {}
       _PainterlyLightMap ("Painterly", 2D) = "white" {}
       _NormalMap ("Normal", 2D) = "white" {}
       _CubeMap( "Cube Map" , Cube )  = "defaulttexture" {}

      _ColorSize("_ColorSize", float ) = 0.5
      _ColorBase("_ColorBase", float ) = 0
    
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Cull Back
        Pass
        {
            CGPROGRAM

            #include "../Chunks/connectionStruct.cginc"

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight


            #include "../Chunks/ShadowVertPassthrough.cginc"


            #include "../Chunks/PainterlyLight.cginc"
            #include "../Chunks/TriplanarTexture.cginc"
            #include "../Chunks/MapNormal.cginc"
            #include "../Chunks/Reflection.cginc"
            #include "../Chunks/SampleAudio.cginc"
            #include "../Chunks/snoise.cginc"

            fixed4 frag (v2f v) : SV_Target
            {

                fixed shadow = UNITY_SHADOW_ATTENUATION(v,v.world) * .5 + .5;

                float3 n = MapNormal( v , .5 , 13.2 );
                float3 r = Reflection( v.pos , n );

                float m = dot( n, _WorldSpaceLightPos0.xyz);
                float baseM = m;
                m = saturate(( m +1 )/2);

                m *= shadow;

                float4 col  = tex2D(_ColorMap , m * _ColorSize  + _ColorBase );
                float3 p = Painterly( m, v.uv.yx * 5) * .7 + .3;

               // col.xyz *= p * .3+ p * r;
                //col *= baseM;

                col = 3.*SampleAudio( snoise(v.world * .5) * .2 + .2) * col;


                //col = shadow;


                return col;
            }

            ENDCG
        }


        // SHADOW PASS

        Pass
        {
          Tags{ "LightMode" = "ShadowCaster" }


          CGPROGRAM

          #pragma target 4.5
          #pragma vertex vert
          #pragma fragment frag

          #pragma multi_compile_shadowcaster
          #pragma fragmentoption ARB_precision_hint_fastest

          #include "UnityCG.cginc"
          sampler2D _MainTex;

          float DoShadowDiscard( float3 pos , float2 uv ){
             return 1;//float lookupVal =  max(min( uv.y * 2,( 1- uv.y ) ) * 1.5,0);//2 * tex2D(_MainTex,uv * float2(4 * saturate(min( uv.y * 4,( 1- uv.y ) )) ,.8) + float2(0,.2));
             // float4 tCol = tex2D(_MainTex, uv *   float2( 6,(lookupVal)* 1 ));
             // if( ( lookupVal + 1.3) - 1.2*length( tCol ) < .5 ){ return 0;}else{return 1;}
          }
          #include "../Chunks/connectionStruct.cginc"
          #include "../Chunks/ShadowDiscardFunction.cginc"
          ENDCG
        }


      
      }


    FallBack "Diffuse"

}