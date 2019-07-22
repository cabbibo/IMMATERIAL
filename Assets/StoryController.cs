using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryController : Cycle
{


  public void SetPage(){

  }

  public void SetPlayer(){
    data.cameraControls.SetFollowTarget( data.player , 1 );
  }

}
