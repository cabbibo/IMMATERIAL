Shader "Scene/SceneCircle/Test1"
{
    Properties{
        _RibbonMap("RibbonMap", 2D) = "" {}
        _RibbonMap2("RibbonMap", 2D) = "" {}
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
            sampler2D _RibbonMap2;

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

                float2 newUV = float2( sin( v.uv.x * 1000) , (2*sin(v.uv.y * 30 + _Time.y * 2 + floor(v.uv.x * 100)%3  + floor( v.uv.x * 200 )%4) + floor( v.uv.x * 150 )%1.5 )-1  );


              //  if( length(t.xyz) < 1){ discard;}
              float3 col;
                if( v.uv.y > _InnerRadius / _OuterRadius ){
                    if( newUV.y < 0){ discard;}
                    if( newUV.x   < 0 ){ discard;}
                      col = 1;

                    col = float3(newUV.x , newUV.y ,0);//tex2D(_RibbonMap , v.uv * 20   ).xyz;//float3(newUV.x ,newUV.y,1);//min( newUV.x+1 , newUV.y+1 );
                    col = tex2D(_RibbonMap , (newUV / 2 )   ).xyz  * tex2D(_RibbonMap2 , (newUV / 2 ) - float2( v.uv.y ,0)  ).xyz;//float3(newUV.x ,newUV.y,1);//min( newUV.x+1 , newUV.y+1 );
                

                if( length(col) < .3 ){ discard; }
                }else{
                    col = tex2D(_RibbonMap2 , float2( v.uv.x * 3 + _Time.y , 0 )).xyz;//float3(1,1,1);

                    if( v.uv.y+.005 < _InnerRadius / _OuterRadius ){
                        discard;//
                    }
                }


                return float4(col,1);
            }

            ENDCG
        }
    }
}