using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterParticles : Particles
{

  public int multiplier;
  public Particles anchors;

  public override void SetCount(){
    count = anchors.count * multiplier;
  }
}
