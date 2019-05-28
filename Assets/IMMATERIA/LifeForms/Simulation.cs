using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : LifeForm
{

  public string nameInBuffer;
  public Form form;
  public Life life;
  

  private Binder[] binders;

  // Use this for initialization
  public override void Create(){

    SafeInsert(form);
    SafeInsert(life);

    binders = GetComponents<Binder>();

    for( int i = 0 ; i < binders.Length; i++ ){
      SafeInsert( binders[i] );
    }


  }

  public override void _Bind(){

    life.BindPrimaryForm(nameInBuffer, form); 
    
    Bind();

  }



  


}

