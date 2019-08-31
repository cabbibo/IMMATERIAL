using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePup : Cycle
{

  public Particles particles;
  public Life life;

  public MeshVerts verts;
  public MeshTris tris;

  public Body body;
  public Grass anchors;

  public override void Create(){

    particles.count = verts.meshFilter.sharedMesh.vertices.Length;

    SafeInsert( verts );
    SafeInsert( life );
    SafeInsert( body ); // particles and tris added to body
    SafeInsert( anchors );

  }

  public override void Bind(){

    life.BindPrimaryForm( "_ParticleBuffer" , particles );
    life.BindForm("_VertBuffer" , verts );
  }



}
