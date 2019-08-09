using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KelpBunch : Cycle
{

  public CircleOnTerrain circle;
  public ParticlesOnCircle baseParticles;
  public HairBasic kelp;
  public TubeTransfer  body;

  public bool constant;

  public override void Create(){
    SafeInsert(circle);
    SafeInsert(baseParticles);
    SafeInsert(kelp);
    SafeInsert(body);
  }

  public void Set(){
    circle.Set();
    baseParticles.Set();
    kelp.Set();
  }

  public override void Activate(){
    print("activado");
    Set();
    body.showBody = true;
  }

  public override void Deactivate(){
    body.showBody = false;
  }

  public override void WhileLiving(float v){
    if( constant ){ Set(); }
  }

}
