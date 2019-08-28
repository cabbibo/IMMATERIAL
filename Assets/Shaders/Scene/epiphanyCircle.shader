Shader "Scenes/pickup/Epiphany"
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
            #include "../Chunks/terrain.cginc"

            sampler2D _RibbonMap;
            sampler2D _RibbonMap2;

           

            struct v2f { 
                float4 pos : SV_POSITION; 
                float3 nor : NORMAL;
                float2 uv : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                float  dist : TEXCOORD3;
            };



            float _InnerRadius;
            float _OuterRadius;
            float _StartTime;
            float3 _SetPosition;
            float _ID;

            float3 _PlayerPosition;

            float4 _Color;

            StructuredBuffer<Vert> _VertBuffer;
            StructuredBuffer<int> _TriBuffer;

            v2f vert ( uint vid : SV_VertexID )
            {
                v2f o;
                Vert v = _VertBuffer[_TriBuffer[vid]];

                float angle = atan2( v.pos.z , v.pos.x );

                float distance = (_ID + 4 ) * .2 *  pow( ( _Time.y  - _StartTime) , 2);
                float r =  (length( v.pos )* ( distance/3)) + distance;



                float3 p = sin(angle) * r * float3(1,0,0) + cos(angle) * r * float3(0,0,1);

                float3 fPos =_SetPosition + p;

                o.nor = terrainGetNormal( fPos );
                //o.uv = v.texcoord.xy;

                o.worldPos = terrainWorldPos( fPos ) + float3(0,distance * .01 + .3,0);// - float4(0,0,_Vertical,0);
               // v.vertex = terrainNewPos( v.vertex )- float4(0,0,_Vertical,0);//mul( unity_WorldToObject, worldPos);
               

                //fPos = // + v.pos * length( v.pos) * 10 + float3(0,1,0) * angle;// .000000001 ;//- sin(a) * ( r  )  * float3(1,0,0)  + cos(a) * ( r )  * float3(0,0,1);
                //float3 fPos = v.pos * 1 + _SetPosition + normalize(v.pos ) * 10;
                o.pos = mul(UNITY_MATRIX_VP, float4(o.worldPos,1.0f));
       
                o.dist = length(o.worldPos - _PlayerPosition);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f v) : SV_Target
            {


                float3 col;

                col = tex2D(_RibbonMap , (v.uv * 2 + 1 + _ID + float2(_Time.y  * .05 * (_ID+4),0) ) * float2( 10,1) );

                float cutoff = saturate(((_Time.y - _StartTime)) / 10);
                if( col.x > abs(v.uv.y) - cutoff * .6 ){ 
                    discard; 
                }else{
                    col = tex2D(_RibbonMap2, float2(sin(col.x * 2 + _ID * .1) * .3 + .7,0));
                }

                col /= (1 + .01 * v.dist);// (.5+ .1*dif);




                return float4(col,1);
            }

            ENDCG
        }
    


  }
}