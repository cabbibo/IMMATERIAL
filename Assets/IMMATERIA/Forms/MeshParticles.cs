using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshParticles : Particles
{
  public MeshVerts verts;

  public override void SetCount(){
    count = verts.count;
  }
}
