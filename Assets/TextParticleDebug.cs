﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering;

public class TextParticleDebug : Cycle
{


  public Vector3 offset;
  public Material GlyphMaterial;
  public Material LocationMaterial;
  public Texture GlyphTexture;

  private MaterialPropertyBlock glyphMPB;
  private MaterialPropertyBlock locationMPB;

  public override void Create(){

  if( glyphMPB == null ){ glyphMPB = new MaterialPropertyBlock(); }
  if( locationMPB == null ){ locationMPB = new MaterialPropertyBlock(); }
    
  }
  public override void  Bind(){

    print("binding");

    locationMPB.SetBuffer("_ParticleBuffer" , data.text.particles._buffer );
    

    glyphMPB.SetBuffer("_ParticleBuffer" , data.text.particles._buffer );

    glyphMPB.SetTexture("_TextMap" , GlyphTexture );
    glyphMPB.SetBuffer("_VertBuffer" , data.text.body.verts._buffer );
    glyphMPB.SetBuffer("_TriBuffer" , data.text.body.triangles._buffer );


  }
  
  public override void WhileLiving(float v){

//    print( "helol");

    transform.position = Camera.main.transform.position + offset.z * Camera.main.transform.forward  + Camera.main.transform.right * offset.x + Camera.main.transform.up * offset.y;
    transform.rotation = Camera.main.transform.rotation;

    glyphMPB.SetMatrix( "_World" , transform.localToWorldMatrix );
    glyphMPB.SetInt("_BaseID" , data.text.currentMin);
    glyphMPB.SetInt("_TipID" , data.text.currentMax);
    glyphMPB.SetInt("_TextCount" , data.text.particles.count );



    locationMPB.SetMatrix( "_World" , transform.localToWorldMatrix );
    locationMPB.SetInt("_BaseID" , data.text.currentMin);
    locationMPB.SetInt("_TipID" , data.text.currentMax);
    locationMPB.SetInt("_TextCount" , data.text.particles.count );



    Graphics.DrawProcedural(GlyphMaterial,  new Bounds(transform.position, Vector3.one * 50000), MeshTopology.Triangles, data.text.body.triangles.count , 1, null, glyphMPB, ShadowCastingMode.TwoSided, true, gameObject.layer);
    Graphics.DrawProcedural(LocationMaterial,  new Bounds(transform.position, Vector3.one * 50000), MeshTopology.Triangles, data.text.particles.count * 3 , 1, null, locationMPB, ShadowCastingMode.TwoSided, true, gameObject.layer);

  }


}
