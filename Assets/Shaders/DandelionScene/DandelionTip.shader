Shader "Scenes/DandelionScene/dandyTip"
{
    Properties {

    _Color ("Color", Color) = (1,1,1,1)
    _MainTex ("Texture", 2D) = "white" {}
    _ColorMap ("Color Map", 2D) = "white" {}
    _HueStart ("HueStart", Float) = 0

  }
    SubShader
    {
        
        Pass
        {
Tags { "RenderType"="Opaque" }
        LOD 100

        Cull Off

          Tags{ "LightMode" = "ForwardBase" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5
            // make fog work
            #pragma multi_compile_fogV
 #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

      #include "UnityCG.cginc"
      #include "AutoLight.cginc"
    


            #include "../Chunks/Struct16.cginc"
            #include "../Chunks/hash.cginc"

            sampler2D _MainTex;
            sampler2D _ColorMap;

            struct v2f { 
              float4 pos : SV_POSITION; 
              float3 nor : NORMAL;
              float2 uv :TEXCOORD0; 
              float3 worldPos :TEXCOORD1;
              float2 debug :TEXCOORD3;
              float id :TEXCOORD4;
              float4 whereInTip : TEXCOORD5;
              UNITY_SHADOW_COORDS(2)
            };
            float4 _Color;
            float _HueStart;

            StructuredBuffer<Vert> _VertBuffer;
            StructuredBuffer<int> _TriBuffer;

            v2f vert ( uint vid : SV_VertexID )
            {
                v2f o;


                UNITY_INITIALIZE_OUTPUT(v2f, o);
                Vert v = _VertBuffer[_TriBuffer[vid]];
                o.pos = mul (UNITY_MATRIX_VP, float4(v.pos,1.0f));


                o.whereInTip.x = vid / 9;
                o.whereInTip.y = vid % 9;
                o.whereInTip.z = hash(o.whereInTip.x);


                o.nor = v.nor;

                float oX = floor(hash( (o.whereInTip.x) * 10 )*6)/6;
                float oY = floor(hash( (o.whereInTip.x) * 51 )*6)/6;
                o.uv = (v.uv * 1/6) + float2(oX,oY);
                o.worldPos = v.pos;
                o.debug = v.debug;
                o.id = vid / 12;


        UNITY_TRANSFER_SHADOW(o,o.worldPos);

                return o;
            }

      float DoShadowDiscard( float3 pos , float2 uv , float3 nor ){
        float v = dot(normalize(_WorldSpaceLightPos0.xyz), normalize(nor));
        return v;//sin( uv.y * 100 + _Time.y);
      }

            fixed4 frag (v2f v) : SV_Target
            {
                // sample the texture
                fixed shadow = UNITY_SHADOW_ATTENUATION(v,v.worldPos) * .5 + .5;
                float val = -dot(normalize(_WorldSpaceLightPos0.xyz),normalize(v.nor));// -DoShadowDiscard( i.worldPos , i.uv , i.nor );

                 float4 tCol = tex2D(_MainTex, v.uv );
                 float vL = length(v.uv-.5) ;
                 if( length( tCol ) < .5 && v.whereInTip.y < 6 ){ discard; }

                  float match = dot(normalize(_WorldSpaceLightPos0.xyz), normalize(v.nor));
                 //if( vL > .4 ){ discard; }
                fixed4 col =  saturate(match+.5)*1.1*tCol*tex2D(_ColorMap , float2( length( tCol) * .1 + sin(vL*10 + length(tCol)*10 - _Time.y*10  * sin(v.id/300)) * .04 + sin(v.id/1000) * .2+  _HueStart   + match *.2, 0) );//saturate(((_Time-v.debug.y) * 1 )) *  tex2D(_ColorMap , float2( length( tCol) * length( tCol ) * .1  + _HueStart , 0) )  * tCol* tCol;//* 20-10;//*tCol* lookupVal*4;//* 10 - 1;
                    
            col = tex2D(_ColorMap , float2(tCol.x * .3 + .2 + v.whereInTip.z  * .3,0)) * shadow;



            if( v.whereInTip.y >= 6 ){ col = tex2D(_ColorMap , float2(v.uv.y * 1.3 + .2 + v.whereInTip.z  * .3,0)) * shadow;}
                 // if( v.debug.x > .5 ){ col =float4(1,0,0,1);}
                return col;
            }

            ENDCG
        

        }

                  // SHADOW PASS

    Pass
    {
      Tags{ "LightMode" = "ShadowCaster" }


      Fog{ Mode Off }
      ZWrite On
      ZTest LEqual
      Cull Off
      Offset 1, 1
      CGPROGRAM

      #pragma target 4.5
      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_shadowcaster
      #pragma fragmentoption ARB_precision_hint_fastest

      #include "UnityCG.cginc"
      sampler2D _MainTex;

     

      #include "../Chunks/Struct16.cginc"

      #include "../Chunks/hash.cginc"
      #include "../Chunks/ShadowCasterPos.cginc"
   

      StructuredBuffer<Vert> _VertBuffer;
      StructuredBuffer<int> _TriBuffer;

      struct v2f {
        V2F_SHADOW_CASTER;
        float3 nor : NORMAL;
        float3 worldPos : TEXCOORD1;
        float2 uv : TEXCOORD0;
        float  idInTip : TEXCOORD2;
      };


      v2f vert(appdata_base input, uint id : SV_VertexID)
      {
        v2f o;
        Vert v = _VertBuffer[_TriBuffer[id]];

        float4 position = ShadowCasterPos(v.pos, -v.nor);
        o.pos = UnityApplyLinearShadowBias(position);
        o.worldPos = v.pos;
       
        int tipID = id / 9;
        int idInTip = id % 9;

        float oX = floor(hash( (tipID) * 10 )*6)/6;
        float oY = floor(hash( (tipID) * 51 )*6)/6;
        o.uv = (v.uv * 1/6) + float2(oX,oY);
        o.idInTip = idInTip;

        return o;
      }

      float4 frag(v2f i) : COLOR
      {

         float4 tCol = tex2D(_MainTex, i.uv );
        

        if( tCol.a < .5 && i.idInTip < 6 ){ discard; }

        SHADOW_CASTER_FRAGMENT(i)
      }


      ENDCG
    }
  
    




    }




}