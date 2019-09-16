using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KelpBunch : Cycle
{

  public CircleOnTerrain circle;
  public ParticlesOnCircle baseParticles;
  public HairBasic kelp;
  public TubeTransfer  body;
  public KelpTips tips;

  public bool constant;

  public override void Create(){
    SafeInsert(circle);
    SafeInsert(baseParticles);
    SafeInsert(kelp);
    SafeInsert(body);
    SafeInsert(tips);
  }

  public void Set(){
    circle.Set();
    baseParticles.Set();
    //kelp.Set();
  }

  public override void Activate(){
//    print("activado");
    Set();
    body.showBody = true;
    //circle.body.active = true;
  }

  public override void Deactivate(){
//    print("what");
    body.showBody = false;
  //  circle.body.active = false;
  }

  public override void WhileLiving(float v){
    if( constant ){ Set(); }
  }

}
