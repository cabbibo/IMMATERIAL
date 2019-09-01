using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailSim : Simulation
{

  public Form head;

  public override void Bind(){

    life.BindForm("_HeadBuffer", head);
    life.BindAttribute("_ParticlesPerTrail" , "particlesPerTrail" , form );
  }


}
