using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : Cycle
{

  public void OnPageCantGoBack(){
    print("Sorry but you can't go back from this position");
  }


  public void OnPageLocked(){
    print("Sorry but this page is locked! Youll have to do something to fix it! ");
  }

  public void OnSuccessUnlock(){
    print("GOOD JOB U ONLOCKTIOD!");  
  }

  public void NoCurrentStory(){
    print("NO CURRENT STORY");
  }

  public GameObject nodePrefab;
  public GameObject[] nodes;


  public override void OnLive(){

    if( debug ){
      for( int i = 0; i < nodes.Length; i++ ){
        DestroyImmediate( nodes[i] );
      }


    }

  }

}
