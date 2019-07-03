using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryController : Cycle
{


  public void SetPage(){

  }

  public void SetPlayer(){
    data.Controls.SetFollowTarget( data.Player , 1 );
  }

}
