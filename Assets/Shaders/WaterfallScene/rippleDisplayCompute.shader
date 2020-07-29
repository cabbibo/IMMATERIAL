Shader "Scenes/Waterfall/RippleDispComp"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Height ("Height", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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

            #include "../Chunks/terrain.cginc"
            #include "../Chunks/noise.cginc"

                      
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;

                float dist  : TEXCOORD2;
                float3 worldPos : TEXCOORD1;
                float3 eye : TEXCOORD3;
                float3 nor : NORMAL;
            };



            float4x4 _Transform;

            float4 _Color;

            StructuredBuffer<Vert> _VertBuffer;
            StructuredBuffer<int> _TriBuffer;
 
            sampler2D _MainTex;
            sampler2D _Height;
            float4 _MainTex_ST;
            v2f vert ( uint vid : SV_VertexID )
            {
                v2f o;
                Vert v = _VertBuffer[_TriBuffer[vid]];
                

                o.uv = 1-v.uv;//TRANSFORM_TEX(v.uv, _MainTex);

                float3 delta = float3(.0001 , 0 , 0 );

                fixed4 col = tex2Dlod(_MainTex, float4(o.uv,0,0));
                float colU = tex2Dlod(_MainTex, float4(o.uv + delta.xy,0,0));
                float colD = tex2Dlod(_MainTex, float4(o.uv - delta.xy,0,0));
                float colL = tex2Dlod(_MainTex, float4(o.uv + delta.yx,0,0));
                float colR = tex2Dlod(_MainTex, float4(o.uv - delta.yx,0,0));


                float3 pU  =  v.pos + float3(0,colU*.01,0) + delta.xyy;
                float3 pD  =  v.pos + float3(0,colD*.01,0) - delta.xyy;
                float3 pL  =  v.pos + float3(0,colL*.01,0) + delta.yyx;
                float3 pR  =  v.pos + float3(0,colR*.01,0) - delta.yyx;


                float3 fNor = normalize(cross( pU - pD , pL - pR));

                float3 fPos = v.pos + float3(0,col.x * .1,0);

                o.pos = mul (UNITY_MATRIX_VP, float4(fPos,1.0f));


                o.nor = fNor;
                o.eye = fPos - _WorldSpaceCameraPos;

                float3 terrainPos = terrainWorldPos( fPos - .25 );

                float3 dif = terrainPos - fPos;
                o.dist = abs(dif.y) * 4;
                o.worldPos = fPos;

                return o;

                return o;
            }



    

            fixed4 frag (v2f v) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, v.uv).x;
                 //col += 1-tex2D(_Height, v.uv);

         float3 delta = float3(.004 , 0 , 0 );


                float colU = tex2Dlod(_MainTex, float4(v.uv + delta.xy,0,0));
                float colD = tex2Dlod(_MainTex, float4(v.uv - delta.xy,0,0));
                float colL = tex2Dlod(_MainTex, float4(v.uv + delta.yx,0,0));
                float colR = tex2Dlod(_MainTex, float4(v.uv - delta.yx,0,0));


                float3 pU  =  v.worldPos + float3(0,colU*.01,0) + delta.xyy;
                float3 pD  =  v.worldPos + float3(0,colD*.01,0) - delta.xyy;
                float3 pL  =  v.worldPos + float3(0,colL*.01,0) + delta.yyx;
                float3 pR  =  v.worldPos + float3(0,colR*.01,0) - delta.yyx;


                float3 fNor = normalize(cross( pU - pD , pL - pR));


                col.xyz =fNor * .5 + .5;

                float3 terrainPos = terrainWorldPos( v.worldPos -.5 );

                float3 dif = terrainPos - v.worldPos;
                float dist = 2-((abs(dif.y) * 4 + 2*noise( v.worldPos * 4 + float3(0,0,_Time.y)) + noise( v.worldPos * 10 + float3(0,0,_Time.y))));

                if( dist > 1 - noise( v.worldPos * 20 + col.x) ){ discard; }

                 col.xyz =fNor;// dist + -dot( normalize(v.eye) ,  fNor );//1/dist;// * 1.1;
                return col;
            }
            ENDCG
        }
    }
}
