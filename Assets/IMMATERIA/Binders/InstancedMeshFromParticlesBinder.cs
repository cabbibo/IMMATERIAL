using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstancedMeshFromParticlesBinder : Binder
{

  public Particles particles;
  public float scale;


  // Use this for initialization
  public override void Bind() {
    toBind.BindForm( "_SkeletonBuffer" , particles );
    toBind.BindForm( "_BaseBuffer" , GetComponent<InstancedMeshVerts>().verts );

    toBind.BindAttribute("_Scale", "scale",this);
    toBind.BindAttribute("_VertsPerMesh", "vertsPerMesh",GetComponent<InstancedMeshVerts>() );
  }
  
}
