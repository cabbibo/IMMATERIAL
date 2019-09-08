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


                float2 c   = tex2D(_LastTex, uv  ).xy;

                float h_n = tex2D(_LastTex, uv + float2(0., 1.) * _TexelSize.xy ).x;
                float h_e = tex2D(_LastTex, uv + float2(1., 0.) * _TexelSize.xy ).x;
                float h_s = tex2D(_LastTex, uv + float2(0., -1.) * _TexelSize.xy ).x;
                float h_w = tex2D(_LastTex, uv + float2(-1., 0.) * _TexelSize.xy ).x;

                float m_n = terrainWorldPos( worldPos  + float3( 0., 0,  1.) * texel ).y - worldPos.y;
                float m_e = terrainWorldPos( worldPos  + float3( 1., 0,  0.) * texel ).y - worldPos.y;
                float m_s = terrainWorldPos( worldPos  + float3( 0., 0, -1.) * texel ).y - worldPos.y;
                float m_w = terrainWorldPos( worldPos  + float3(-1., 0,  0.) * texel ).y - worldPos.y;


                if(m_n > -.5 ){  h_n = h_s; }
                if(m_e > -.5 ){  h_e = h_w; }
                if(m_s > -.5 ){  h_s = h_n; }
                if(m_w > -.5 ){  h_w = h_e; }





                float f = (((h_n + h_e + h_s + h_w)/4) - c.x);
                float newVel = c.y + f ;
                ;// * .3;// * .9f;
                       
                newVel *= .99999;
                
                float newPos = c.x + newVel - .000001;




                return float2( saturate(newPos) , newVel );

            }

            fixed4 frag (v2f v) : SV_Target
            {
        
                    float2 result = newHeight(v.uv , v.worldPos , v.texel);


                    if( length(v.uv-_HitUV) < .01 && _Down == 1 ){
                        result.x += 1;
                    }
                    float tPos = terrainWorldPos( v.worldPos ).y- v.worldPos.y;
                    return fixed4(result.x , result.y ,sin( tPos * 40) * .4, 1.0);
                
            }
            ENDCG
        }
    }
}
