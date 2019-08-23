using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTap : Cycle
{
   
  public float emitTime;
  public float tapTime;
   
   public void Tap(){

      if( data.inputEvents.hitTag == "Untagged" && !data.state.inPages ){
   
      transform.position = data.land.Trace( data.inputEvents.ray.origin , data.inputEvents.ray.direction );
      data.playerControls.SetMoveTarget( transform );
      data.guideParticles.SetEmitterPosition( transform.position );

      data.guideParticles.EmitOn();
      tapTime = Time.time;
    }
   }

  


  public override void WhileLiving(float f){



    if( Time.time - tapTime > emitTime ){
      data.guideParticles.EmitOff();
    }else{
     
    }

  }



}
