

      #include "../Chunks/Struct16.cginc"
      #include "../Chunks/hash.cginc"

      sampler2D _MainTex;
      struct v2f {
        V2F_SHADOW_CASTER;
        float2 uv : TEXCOORD1;
        float2 debug : TEXCOORD2;
      };


      v2f vert(appdata_base v, uint id : SV_VertexID)
      {
        v2f o;
       

               float3 wPos = _TransferBuffer[id].pos;
        float3 wNor = _TransferBuffer[id].nor;

            // Default shadow caster pass: Apply the shadow bias.
    float scos = dot(wNor, normalize(UnityWorldSpaceLightDir(wPos)));
    wPos -= wNor * unity_LightShadowBias.z * sqrt(1 - scos * scos);
    o.pos = UnityApplyLinearShadowBias(UnityWorldToClipPos(float4(wPos, 1)));

        float2 offset = floor( float2(sin(o.debug.x*.04 + 10),sin(o.debug.x * .04)) *2 ) /2; 
        return o;
      }

      float4 frag(v2f v) : COLOR
      {
        SHADOW_CASTER_FRAGMENT(i)
      }