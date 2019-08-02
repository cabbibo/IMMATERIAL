// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Final/Water/F1"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BumpMap("BumpMap", 2D) = "white" {}

    _ColorMap ("ColorMap", 2D) = "white" {}
    _NormalMap ("NormalMap", 2D) = "white" {}
    _CubeMap( "Cube Map" , Cube )  = "defaulttexture" {}

        _Color("Color", Color) = (1, 1, 1, 1)
        _EdgeColor("Edge Color", Color) = (1, 1, 1, 1)
        _DepthFactor("Depth Factor", float) = 1.0
        _WaveSpeed("Wave Speed", float) = 1.0
        _WaveAmp("Wave Amp", float) = 0.2
        _DepthRampTex("Depth Ramp", 2D) = "white" {}
        _NoiseTex("Noise Texture", 2D) = "white" {}
        _DistortStrength("Distort Strength", float) = 1.0
        _ExtraHeight("Extra Height", float) = 0.0
    }
SubShader
    {
        Tags
        { 
            "Queue" = "Transparent"
        }

        // Grab the screen behind the object into _BackgroundTexture
        GrabPass
        {
            "_BackgroundTexture"
        }

        Pass
        {
            //Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #include "UnityCG.cginc"
            #include "../Chunks/noise.cginc"

            #pragma vertex vert
            #pragma fragment frag
            


                struct Vert{
      float3 pos;
      float3 vel;
      float3 nor;
      float3 tan;
      float2 uv;
      float2 debug;
    };

      #include "../Chunks/hsv.cginc"
      #include "../Chunks/noise.cginc"


  StructuredBuffer<Vert> _VertBuffer;
  StructuredBuffer<int> _TriBuffer;




      sampler2D _HeightMap;
    float _MapSize;
    float _MapHeight;


            float4 _Color;
            float4 _EdgeColor;
            float  _DepthFactor;
            float  _WaveSpeed;
            float  _WaveAmp;
            float _ExtraHeight;
            sampler2D _BackgroundTexture;
            sampler2D _CameraDepthTexture;
            sampler2D _DepthRampTex;
            sampler2D _BumpMap;
            sampler2D _MainTex;
            sampler2D _NoiseTex;
            samplerCUBE _CubeMap;

            float3 _PlayerPosition;
            float3 _Velocity;
            float3 _TrailPos1;
            float3 _TrailPos2;
            float3 _TrailPos3;

            struct input
            {
                float4 vertex : POSITION;
                float4 texCoord : TEXCOORD1;
            };

            struct varyings
            {
                float4 pos : SV_POSITION;
                float4 texCoord : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                float4 refractedPos : TEXCOORD2;
                float4 rR : TEXCOORD3;
                float4 rG : TEXCOORD4;
                float4 rB : TEXCOORD5;
                float3 world : TEXCOORD6;
                float3 nor : TEXCOORD7;
                float3 eye : TEXCOORD8;
            };



            float3 fPos( float3 pos ){
                float n = noise( float3(pos.xz*.2+_Time.y*.2,_Time.x));
                pos.y += n * .3;
                return pos;
            }

            float3 displace( float3 pos , out float3 nor ){

                float delta = .3;
                float3 pU = fPos( pos + float3(1,0,0)*delta );
                float3 pD = fPos( pos - float3(1,0,0)*delta );
                float3 pL = fPos( pos + float3(0,0,1)*delta );
                float3 pR = fPos( pos - float3(0,0,1)*delta );
                
                nor = normalize(cross(normalize(pU-pD) , normalize(pL-pR)));

                return fPos( pos );

            }

            varyings vert(uint id : SV_VertexID)
            {
                varyings o;

                float3 nor;
                Vert vert = _VertBuffer[_TriBuffer[id]];

                float3 worldPos = vert.pos;
                worldPos.y = _ExtraHeight;


                float3 pos = displace(worldPos,nor);

                float3 eye = _WorldSpaceCameraPos -  pos;

                float3 refr = refract( normalize(eye) , nor , .8 );
                float3 refrR = refract( normalize(eye) , nor , .7 );
                float3 refrG = refract( normalize(eye) , nor , .6 );
                float3 refrB = refract( normalize(eye) , nor , .5 );

                float3 refracted = pos + refr * .1;

                o.eye = eye;

                o.rR  = ComputeGrabScreenPos(mul(UNITY_MATRIX_VP, float4(pos + refrR * .3,1)));
                o.rG  = ComputeGrabScreenPos(mul(UNITY_MATRIX_VP, float4(pos + refrG * .3,1)));
                o.rB  = ComputeGrabScreenPos(mul(UNITY_MATRIX_VP, float4(pos + refrB * .3,1)));

                o.world =pos;

                o.pos = mul( UNITY_MATRIX_VP, float4(pos,1));
                o.nor = nor;

                // compute depth
                o.screenPos = ComputeScreenPos(o.pos);
                o.refractedPos = ComputeScreenPos(mul( UNITY_MATRIX_VP, float4(refracted,1)));

                // texture coordinates 
                o.texCoord.xy = vert.uv;

                return o;
            }




 float3 getBGCol( float3 newPos , float3 worldPos ){


        float4 mp = mul( UNITY_MATRIX_VP , float4( newPos, 1. ) );
        float4 wp = mul( UNITY_MATRIX_VP , float4( worldPos, 1. ) );

        // Getting our screen position
        float4 sp = ComputeGrabScreenPos( mp );
        float4 wsp = ComputeGrabScreenPos( wp );


        //wsp.x += .1*tex2D(_NoiseTex ,float2( wsp.xy + 10 )).r -.01;
        //wsp.y += .1*tex2D(_NoiseTex ,float2( wsp.xy + 20 )).r -.01;

        float3 col =  tex2Dproj(_BackgroundTexture,sp).rgb;

        return col;

      }


float smin( float a, float b, float k )
{
    float h = max( k-abs(a-b), 0.0 );
    return min( a, b ) - h*h*0.25/k;
}
            float4 frag(varyings v) : COLOR
            {
                // apply depth texture
                float4 depthSample = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, v.screenPos);
                float depth = LinearEyeDepth(depthSample).r;

                // create foamline
                float foamLine =(depth - v.screenPos.w);
                float4 foamRamp = float4(tex2D(_DepthRampTex, float2(saturate(foamLine), 0.5)).rgb, 1.0);

                // sample mainput texture
                float4 albedo = tex2D(_MainTex, v.texCoord.xy);
                float r = tex2Dproj(_BackgroundTexture, v.rR).r;
                float g = tex2Dproj(_BackgroundTexture, v.rG).g;
                float b = tex2Dproj(_BackgroundTexture, v.rB).b;

                float pDif  = length( _PlayerPosition.xz - v.world.xz);
                //float p1Dif =  length( _TrailPos1.xz - v.world.xz);
                //float p2Dif =  length( _TrailPos2.xz - v.world.xz);
                //float p3Dif =  length( _TrailPos3.xz - v.world.xz);
//
//
                //pDif = smin(pDif,p1Dif*1,1);
                //pDif = smin(pDif,p2Dif*1.5,1);
                //pDif = smin(pDif,p3Dif*2,1);
                pDif *= 3;


                float2 difP = _PlayerPosition.xz - v.world.xz;

                difP = .1*normalize(difP) * saturate(1/ (10*length(difP)));
                float n = .2*tex2D(_NoiseTex, v.world.xz * .3 +  difP * - (_Time.y* .03)).x;//-_Time,1));
                n += .4*tex2D(_NoiseTex, v.world.xz * .1  +  difP - (_Time.y* .03)).x;//-_Time,1));
                n += tex2D(_NoiseTex, v.world.xz * .05  +  difP - (_Time.y* .03)).x;//-_Time,1));

                
                pDif -=  n*3;


                float4 c = tex2Dlod(_HeightMap , float4(v.world.xz * _MapSize,0,0) );
                
                float dif = v.world.y - c.r * _MapHeight;
               
                //dif -= n;

                float3 shore = tex2D(_DepthRampTex, float2(saturate(dif* 2.1)*.2 + .9, 0)).xyz * (1-2*dif);

                float lookup = saturate(cos(pDif * .3)-(.3*pDif));
                float3 aroundPerson = tex2D(_DepthRampTex,float2(-lookup * .3 + 1 ,0));//aw/(pDif*pDif);

                //saturate(100000/(100000*pDif*pDif*pDif));//tex2D(_DepthRampTex, float2(saturate(pDif* .3) , 0)).xyz;
                //if(depth.r -v.screenPos.w > 1.5){ bg = float4(1,0,0,1); }
                float3 col = shore + aroundPerson + b*saturate(pDif)*.1;//float3(r,g,b)*pDif;//aroundPerson;//bg*float4(1,.5,0,1)+(1-saturate(foamLine)) * foamRamp * saturate(1-.1*dif);//float4(1,0,0,1) * foamLine;//_Color * foamRamp * albedo;
                


                float l1 = (2*tex2D(_NoiseTex, v.world.xz * .2 - 0 - (_Time.y* .03)).x)-1;
                float l2 = (2*tex2D(_NoiseTex, v.world.xz * .2 + 5 - (_Time.y* .03)).x)-1;

                float3 fNor = v.nor + 3*l1 * float3(1,0,0) + 3*l2 * float3(0,0,1);
                fNor = normalize(fNor);

                float3 refr = refract( normalize(v.eye) , fNor   , .6);

                float3 fP = v.world - refr * .1;

                float3 bg = getBGCol( fP , v.world );

                float3 tCol = texCUBE(_CubeMap,refr);

                float shoreLine = saturate(1-floor( (dif - n * .2) * 5));
                col += saturate(1-floor( (dif - n * .2) * 5));//tex2D(_DepthRampTex,float2(bg.b * .2 + .7,0)  ) + aroundPerson ;//tCol;//refl * .5 + .5;
                col = length(bg) + aroundPerson + shore +shoreLine;//tCol;//refl * .5 + .5;
               // col = tex2Dproj(_BackgroundTexture,ComputeGrabScreenPos( mul(UNITY_MATRIX_VP, float4(v.world + v.eye*.9,1)))).rgb;;// + saturate(aroundPerson * (lookup));
                //col *= bg;// tex2Dproj(_BackgroundTexture, v.rR).rgb;;// + saturate(aroundPerson * (lookup));

                col *= (1-lookup);


                //col.xyz = tex2Dproj(_BackgroundTexture, ComputeScreenPos(float4(v.world,1) )).xyz;


                float n2 = tex2D(_NoiseTex, v.world.xz * .3 + foamLine * .2 +  difP * - (_Time.y* .03)).x;
                float fl1 = floor(foamLine * 4+n2*1 ) / 4;
                //col += 4*tex2D(_DepthRampTex,float2(-saturate(1-fl1) * .3 + 1 ,0));//aw/(pDif*pDif);
;
                 //col += 4*tex2D(_DepthRampTex,float2(-saturate(1-10*foamLine) * .3 + 1 ,0))  * (saturate(1-10*foamLine));//aw/(pDif*pDif);
;

                //col += length(bg) * float3(1,.3,.0)  * saturate(1-length(col) * .3 );
        
                //col /= max(1,10.4*pDif) * _PlayerPosition.y * .3;


                return float4(col,1);
            }

            ENDCG
        }
    }
}



