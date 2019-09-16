using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitParticles : Simulation
{
  
  public TransferLifeForm body;
  public TransformBuffer transforms;

  public Vector3 position;


  public override void Create(){
    SafeInsert(body);
  }

  public override void Bind(){
    data.land.BindData(life);
    life.BindVector3( "_EmitterPosition", () => this.position );
    life.BindForm("_TransformBuffer",transforms);

  }

  public override void WhileLiving(float v){
     position = transform.position;
  }




}
