﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : Cycle
{

  public string nameInBuffer;
  public Form form;
  public Life life;

  public bool skipFormBind;
  

  public Binder[] binders;

  // Use this for initialization
  public override void _Create(){


    if( !skipFormBind) SafeInsert(form);
    SafeInsert(life);

    if( binders == null ){
      binders = GetComponents<Binder>();
    }

    for( int i = 0 ; i < binders.Length; i++ ){
      SafeInsert( binders[i] );
    }

    
    DoCreate();


  }

  public override void _Bind(){

    life.BindPrimaryForm(nameInBuffer, form); 
    
    Bind();

  }



  


}

