using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideParticles : Simulation
{
  

  public float _Emit;
  public Vector3 _EmitterPosition;
  public TransferLifeForm body;
  private float oldEmit;


  public override void Create(){
    SafeInsert(body);
  }

  public override void Bind(){
    data.land.BindData(life);
    life.BindFloat("_Emit",() => this._Emit);
    life.BindFloat("_OldEmit",() => this.oldEmit);
    life.BindVector3("_EmitterPosition",() => this._EmitterPosition);
  }

  public void SetEmitterPosition( Vector3 position ){
    _EmitterPosition = position;
  }

  public void ToggleEmit(){
    _Emit = 1 - _Emit;
  }

  public void EmitOff(){
    _Emit = 0;
  }

  public void EmitOn(){
    _Emit = 1;
    oldEmit = 2;
  }

  public void EmitAtMonolith( int i ){
    SetEmitterPosition(data.journey.monoSetters[i].monolith.transform.position);
    EmitOn();
  }
  

  // make sure not to update until after shader has run at least once :)
  public override void WhileLiving( float v){
    oldEmit -= 1;
    oldEmit = Mathf.Clamp(oldEmit,0,2);
    
  }

}
