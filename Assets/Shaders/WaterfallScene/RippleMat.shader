Shader "Scenes/Waterfall/RippleMat"
{
    Properties
    {
        _LastTex ("tex2D", 2D) = "white" {}
        _Height ("tex2D", 2D) = "white" {}
        _TexelSize ("TexelSize", Vector) = (1.0, 1.0, 0.0, 0.0) 
        _Resolution ("Resolution", Vector) = (1.0, 1.0, 0.0, 0.0)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            #include "../Chunks/terrain.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float texel : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };


            float4x4 _Transform;

                sampler2D _LastTex;
            sampler2D _Height;
            float4 _TexelSize;
            float4 _Resolution;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul( _Transform , float4(0, 0 , 0 , 1 )).xyz;
                float scale = mul( _Transform , float4(1, 0 , 0 , 0 )).x * 10;

                o.worldPos -= float3(v.uv.x -.5 , 0,v.uv.y-.5)* scale;
                o.texel = _TexelSize * scale;

                return o;
            }
            
        


            float2 _HitUV;
            int _Down;

            float2 newHeight(float2 uv , float3 worldPos ,float texel ) {

              float dif = terrainWorldPos( worldPos ).y - worldPos.y;

                float2 c   = tex2D(_LastTex, uv  ).xy;

                float h_n = tex2D(_LastTex, uv + float2(0., 1.) * _TexelSize.xy ).x;
                float h_e = tex2D(_LastTex, uv + float2(1., 0.) * _TexelSize.xy ).x;
                float h_s = tex2D(_LastTex, uv + float2(0., -1.) * _TexelSize.xy ).x;
                float h_w = tex2D(_LastTex, uv + float2(-1., 0.) * _TexelSize.xy ).x;

                float m_n = terrainWorldPos( worldPos  + float3( 0., 0,  1.) * texel* .1).y - worldPos.y;
                float m_e = terrainWorldPos( worldPos  + float3( 1., 0,  0.) * texel* .1).y - worldPos.y;
                float m_s = terrainWorldPos( worldPos  + float3( 0., 0, -1.) * texel* .1).y - worldPos.y;
                float m_w = terrainWorldPos( worldPos  + float3(-1., 0,  0.) * texel* .1).y - worldPos.y;

               // if( )

               float edge = 0;
                if(m_n > 0 ){  h_n = 0;edge = 1;}
                if(m_e > 0 ){  h_e = 0;edge = 1;}
                if(m_s > 0 ){  h_s = 0;edge = 1;}
                if(m_w > 0 ){  h_w = 0;edge = 1;}





                float f = (((h_n + h_e + h_s + h_w)/4) - c.x);
                float newVel = c.y + f ;
                ;// * .3;// * .9f;
                       
                newVel *= .996;
                
                float newPos = c.x/(edge+1) + newVel / (edge+1) ;

                //newPos *= dif;

                return float2( clamp( newPos , -2 ,2) , newVel );

            }

            fixed4 frag (v2f v) : SV_Target
            {
        
                    float2 result = newHeight(v.uv , v.worldPos - float3(.5,0,.5) , v.texel);


                    if( length(v.uv-_HitUV) < .01 && _Down == 1 ){
                        result.x += 1;
                    }

                    float tPos = terrainWorldPos( v.worldPos ).y- v.worldPos.y;

                    return fixed4(result.x , result.y ,sin( tPos * 20 + _Time.y * .1) * 1.4, 1.0);
                
            }
            ENDCG
        }
    }
}
