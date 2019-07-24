﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonolithParticles : GuideParticles
{
  

  public float _Emit;
  public Vector3 _EmitterPosition;
  public Texture directionTexture;

  public override void Bind(){
    data.land.BindData(life);
    life.BindAttribute("_Emit","_Emit",this);
    life.BindAttribute("_EmitterPosition","_EmitterPosition",this);
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
        
}
