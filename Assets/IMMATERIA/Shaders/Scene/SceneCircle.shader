Shader "Scene/SceneCircle/Test1"
{
    Properties{
        _RibbonMap("RibbonMap", 2D) = "" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Cull Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"


            #include "../Chunks/Struct16.cginc"

            sampler2D _RibbonMap;

            struct v2f { 
                float4 pos : SV_POSITION; 
                float3 nor : NORMAL;
                float2 uv : TEXCOORD1;
            };



            float _InnerRadius;
            float _OuterRadius;
            float _StartTime;

            float4 _Color;

            StructuredBuffer<Vert> _VertBuffer;
            StructuredBuffer<int> _TriBuffer;

            v2f vert ( uint vid : SV_VertexID )
            {
                v2f o;
                Vert v = _VertBuffer[_TriBuffer[vid]];
                o.pos = mul (UNITY_MATRIX_VP, float4(v.pos,1.0f));
                o.nor = v.nor;
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f v) : SV_Target
            {

                float offset = floor( sin( v.uv.y * 10) );

                //float4 t = tex2D(_RibbonMap , v.uv   );

                float2 newUV = float2( sin( v.uv.x * 1000 ) , (floor(sin(v.uv.y * 30 + _Time.y * 2) * 4)-1)/3);


              //  if( length(t.xyz) < 1){ discard;}

              if( newUV.y < 0){ discard;}
                if( newUV.x   < 0  ){ discard;}


                // sample the texture
                fixed4 col =newUV.y* newUV.x*2;// float4( v.nor * .5 + .5 , 1);//tex2D(_MainTex, i.uv);
                
                col = 1;

                if( v.uv.y < _InnerRadius / _OuterRadius ){ col = float4(0,.3,1,1);}
                return col;
            }

            ENDCG
        }
    }
}