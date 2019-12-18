using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceDynamicMeshParticles : Simulation
{
  public Form baseVerts;
  public MeshVerts meshVerts;

  public override void Create(){
    ((ParticlesOnDynamicMesh)form).mesh = meshVerts;
  }

  public override void Bind(){

    life.BindForm("_VertBuffer" , baseVerts );

  }
}
