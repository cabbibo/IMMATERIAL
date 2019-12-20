Shader "Unlit/fadeToBlack"
{
    Properties
    {


        _MainTex ("Texture", 2D) = "white" {}
        _Color ("color", Color) = (1,1,1,1)
    }
    SubShader
    {
        // inside SubShader
Tags { "Queue"="Transparent+3" "RenderType"="Transparent" "IgnoreProjector"="True" }

        LOD 100

        Pass
        {


// inside Pass
ZWrite On
Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float4 _Color;

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col;
                col.xyz = (1-tex2D(_MainTex,i.uv * float2(2,2) - .5).xyz);
                col.a = _Color.a;
                return col;
            }
            ENDCG
        }
    }
}
