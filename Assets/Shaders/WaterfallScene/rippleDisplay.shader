Shader "Scenes/Waterfall/RippleDisp"
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            #include "../Chunks/terrain.cginc"

            float4x4 _Transform;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;

                float dist  : TEXCOORD2;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _Height;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;


                o.vertex = UnityObjectToClipPos(v.vertex);
                
                float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;

                float3 terrainPos = terrainWorldPos( worldPos -.25 );

                float3 dif = terrainPos - worldPos;
                o.dist = abs(dif.y) * 4;
                o.worldPos = worldPos;

                o.uv = v.uv;//TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f v) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, v.uv);
                 //col += 1-tex2D(_Height, v.uv);


                float3 terrainPos = terrainWorldPos( v.worldPos -.5 );

                float3 dif = terrainPos - v.worldPos;
                float dist = abs(dif.y) * 4;

                 //col = 1/dist;// * 1.1;
                return col;
            }
            ENDCG
        }
    }
}
