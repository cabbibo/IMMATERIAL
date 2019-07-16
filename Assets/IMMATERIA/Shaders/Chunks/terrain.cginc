sampler2D _HeightMap;
float _MapSize;
float _MapHeight;


float3 terrainWorldPos( float4 pos ){
    float3 wp = mul( unity_ObjectToWorld, pos ).xyz;
    float4 c = tex2Dlod(_HeightMap , float4(wp.xz * _MapSize,0,0) );
    wp.xyz += float3(0,1,0) * c.r * _MapHeight;
    return wp;
}

float4 terrainNewPos( float4 pos ){
    float4 wp = float4(terrainWorldPos( pos ) ,1 );
    return mul( unity_WorldToObject, wp);
}



float3 terrainGetNormal( float4 pos ){

  float delta =1.01;
  float4 dU = terrainNewPos( pos + float4(delta,0,0,0) );
  float4 dD = terrainNewPos( pos + float4(-delta,0,0,0) );
  float4 dL = terrainNewPos( pos + float4(0,0,delta,0) );
  float4 dR = terrainNewPos( pos + float4(0,0,-delta,0) );

  //return normalize(cross(dU.xyz-dD.xyz, dL.xyz-dR.xyz));
  return normalize(dU.xyz);

}


float4 terrainSampleColor( float4 pos ){
  float3 wp = mul( unity_ObjectToWorld, pos ).xyz;
  return tex2Dlod(_HeightMap , float4(wp.xz * _MapSize,0,0) );
}
