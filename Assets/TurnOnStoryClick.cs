using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnStoryClick : Cycle
{

  public string nameOfStory;
  public Story story;
  public Page  page;

  public void TurnPage(){
    page.locked = false;
    story.NextPage();
    data.helper.OnSuccessUnlock();
  }


  public override void WhileLiving(float v){

    if( active ){
      if( data.inputEvents.hitTag == "StartNode" && data.inputEvents.oDown == 1 && data.inputEvents.Down == 0 ){
        if( data.inputEvents.hit.collider.gameObject.GetComponent<StoryMarker>() != null ){
          if( data.inputEvents.hit.collider.gameObject.GetComponent<StoryMarker>().storyName == nameOfStory ){
            TurnPage();
          }
        }
      }else{

      }
    }
  }




}
