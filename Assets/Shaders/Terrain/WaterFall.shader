Shader "Unlit/waterFall"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {


            Blend OneMinusDstColor One 
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
                o.uv = -v.uv.yx;//TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col1 =  tex2D(_MainTex, float2( (1-i.uv.y) ,1) * i.uv         + float2(  _Time.y  * .3,0));
                fixed4 col2 =  tex2D(_MainTex, float2( (1-i.uv.y) ,1) * i.uv + .7    + float2(  _Time.y * .5,0));
                fixed4 col3 =  tex2D(_MainTex, float2( (1-i.uv.y) ,1) * i.uv + .3    + float2(  _Time.y * .2,0));


                fixed4 col = max(max(col1,col2),col3);
                

                //if( sin(i.uv.x*1000) > 0  || sin(i.uv.y * 1000) > 0 ){discard; }
                //if( length( col ) < .4 ){ discard; }
                // apply fog
                //col = .3;
                return col;
            }
            ENDCG
        }
    }
}

