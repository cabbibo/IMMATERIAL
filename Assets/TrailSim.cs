using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailSim : Simulation
{

  public Form head;

  public override void Bind(){

    life.BindForm("_HeadBuffer", head);

    TrailParticles tp = (TrailParticles)form;
    life.BindInt( "_ParticlesPerTrail" , () => tp.particlesPerTrail );
  }


}
