using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSceneObjects : Cycle
{


  public TransformBuffer coral;
  public Simulation[] fish;

  public override void Create(){
    SafeInsert( coral );
    for( int i = 0; i  < fish.Length; i++ ){
      SafeInsert(fish[i]);
    }

  }


  public override void Activate(){

    print("HELLLOSSS");
  }

  public override void Deactivate(){

  }


  public void Test(){
    Debug.Log("WTHAHASHSDSHSHDESHDSHS");  
  }

}
