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

}
