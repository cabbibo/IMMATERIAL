using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairTipParticles : Particles
{
   
  public Hair hair;

  public override void SetCount(){
    count = hair.numHairs;
  }


}
