using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaOfStars : Simulation
{
  
  public TransferLifeForm body;
  public float _Emit;
  public Vector3 _EmitterPosition;

  public override void Create(){
    SafeInsert(body);
  }

  public override void Bind(){
    data.land.BindData(life);
    life.BindAttribute("_Emit","_Emit",this);
    life.BindAttribute("_EmitterPosition","_EmitterPosition",this);
  }

  public override void Activate(){
      life.active = true;
      body.transfer.active = true;
      body.showBody = true;
  }

  public override void Deactivate(){
      life.active = false;
      body.transfer.active = false;
      body.showBody = false;

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
  }

  public override void WhileLiving( float v ){
    _EmitterPosition = transform.position;
  }

}
