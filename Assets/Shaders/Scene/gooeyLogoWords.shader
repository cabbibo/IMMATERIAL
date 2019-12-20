Shader "Scenes/SpacePup/Tencks"
{
    Properties {

       _ColorMap ("Color", 2D) = "white" {}
       _TextureMap ("Texture", 2D) = "white" {}
       _PainterlyLightMap ("Painterly", 2D) = "white" {}
       _NormalMap ("Normal", 2D) = "white" {}
       _CubeMap( "Cube Map" , Cube )  = "defaulttexture" {}

      _ColorSize("_ColorSize", float ) = 0.5
      _ColorBase("_ColorBase", float ) = 0
      _PaintSize("_PaintSize", Vector ) = (1,1,1,1)
      _NormalSize("_NormalSize", Vector ) = (1,1,1,1)
      _NormalDepth("_NormalDepth", float ) = .4
    
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Cull Back//Front
        Pass
        {
            CGPROGRAM

            #include "../Chunks/struct16.cginc"

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

            float _Fade;
            #include "../Chunks/ShadowVertPassthrough.cginc"


            #include "../Chunks/PainterlyLight.cginc"
            #include "../Chunks/TriplanarTexture.cginc"
            #include "../Chunks/MapNormal.cginc"
            #include "../Chunks/Reflection.cginc"
            #include "../Chunks/SampleAudio.cginc"

            fixed4 frag (v2f v) : SV_Target
            {

                fixed shadow = UNITY_SHADOW_ATTENUATION(v,v.world) * .9 + .1;


                float3 fNor = normalize(cross( ddx(v.world) , ddy(v.world)));
                v2f t = v;
                t.nor = fNor;

                float3 n = MapNormal( t , v.uv * _NormalSize , _NormalDepth );
                float3 r = Reflection( v.pos , fNor);

                float m = dot( n, _WorldSpaceLightPos0.xyz);
                float baseM = m;
                m = saturate(( m +1 )/2);

                m *= shadow;

                float4 col  = tex2D(_ColorMap , m * _ColorSize + v.uv.x * _ColorSize  + _ColorBase );
                float3 p = Painterly( m, v.uv.xy * _PaintSize ) * .7 + .3;

                col.xyz *= p * .3+ p * r;
                col *= baseM;

                col.xyz = length(v.vel) * 100 * r + .2 *shadow * r;// m * .3 ;//r* 2*p;//shadow * tex2D(_ColorMap , v.uv );

                //float4 aCol = SampleAudio( length(v.world.xy) * .2  );
                float4 aCol = SampleAudio( saturate(length(v.vel) * 60 + .3 ) * .9);
                col *= 10* aCol.x;

                col *= _Fade;

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
             return 1;
          }
          #include "../Chunks/struct16.cginc"
          #include "../Chunks/ShadowDiscardFunction.cginc"
          ENDCG
        }


      
      }


    FallBack "Diffuse"

}