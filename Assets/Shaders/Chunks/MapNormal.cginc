float3 MapNormal(  v2f v ){
   float3 tnormal = UnpackNormal(tex2D(_NormalMap, v.uv));
   // transform normal from tangent to world space
  float3 n;
  n.x = dot(v.t1, tnormal);
  n.y = dot(v.t2, tnormal);
  n.z = dot(v.t3, tnormal);

  return normalize(n);
}

float3 MapNormal(  v2f v , float size ){
   float3 tnormal = UnpackNormal(tex2D(_NormalMap, v.uv  * size));
   // transform normal from tangent to world space
  float3 n;
  n.x = dot(v.t1, tnormal);
  n.y = dot(v.t2, tnormal);
  n.z = dot(v.t3, tnormal);

  return normalize(n);


}

float3 MapNormal(  v2f v , float size , float val ){
   float3 tnormal = UnpackNormal(tex2D(_NormalMap, v.uv  * size));
   // transform normal from tangent to world space
  float3 n;
  n.x = dot(v.t1, tnormal);
  n.y = dot(v.t2, tnormal);
  n.z = dot(v.t3, tnormal);

  return normalize(lerp(v.nor , n, val));
}