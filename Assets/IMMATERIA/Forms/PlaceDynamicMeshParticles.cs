using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceDynamicMeshParticles : Simulation
{
  public Form meshVerts;

  public override void Bind(){

    life.BindForm("_VertBuffer" , meshVerts );

  }
}
