using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;

public class BodyReRender : ReRender
{

  public Body body;


  public override void _OnGestate(){


    if( mpb == null ){ mpb = new MaterialPropertyBlock(); }

    if( body == null ){ body = GetComponent<Body>(); }    

    verts = body.verts;
    triangles = body.triangles;

    mpb.SetInt("_VertCount", verts.count);
    mpb.SetBuffer("_VertBuffer", verts._buffer );
    mpb.SetBuffer("_TriBuffer", triangles._buffer );

    mpb.SetMatrix("_BodyWorldToLocal" , body.transform.worldToLocalMatrix );

   


  }

  public override void _WhileLiving(float v ){

   
    DoLiving(v);


    if( active ){
      
      mpb.SetInt("_VertCount", verts.count);
      mpb.SetBuffer("_VertBuffer", verts._buffer );
      mpb.SetBuffer("_TriBuffer", triangles._buffer );

      mpb.SetMatrix("_BodyWorldToLocal" , body.transform.worldToLocalMatrix );
      mpb.SetMatrix("_LocalToWorld" , transform.localToWorldMatrix );
      
      // Infinit bounds so its always drawn!
      Graphics.DrawProcedural(material,  new Bounds(transform.position, Vector3.one * 50000), MeshTopology.Triangles, triangles.count , 1, null, mpb, ShadowCastingMode.TwoSided, true, gameObject.layer);
    }
  }

    
}
