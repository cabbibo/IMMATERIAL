using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameParticles : Particles
{
  
  public int size;
  public override void SetCount(){
    print("hellslslo");
    count = size * 4;
  }

}
