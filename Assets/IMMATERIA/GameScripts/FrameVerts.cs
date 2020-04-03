using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameVerts : Particles
{
    public int size;
    public override void SetCount(){
      count = (size*2)*4;
    }

}
