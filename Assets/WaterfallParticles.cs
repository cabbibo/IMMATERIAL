﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterfallParticles : Simulation
{
  
  public TransferLifeForm body;
  public Vector3 position;
  public Vector3 direction;
  public float width;


  public override void Create(){
    SafeInsert(body);
  }

  public override void Bind(){
    data.land.BindData(life);
    life.BindAttribute("_EmitterPosition","position",this);
    life.BindAttribute("_EmitterDirection","direction",this);
  }

  public override void WhileLiving(float v){
     position = transform.position;
     direction = transform.right *width;
  }




}
