using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTextAnchors : Cycle
{
   public TextParticles particles;
   public TextAnchor anchor;

   public override void OnBirthed(){
    print("helllooo");
    particles.Set(anchor);
   }
}
