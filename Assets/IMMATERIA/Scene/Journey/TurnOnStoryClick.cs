using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnStoryClick : Cycle
{

  public string nameOfStory;
  public Story story;
  public Page  page;
  public Monolith monolith;

  public void TurnPage(){
    page.locked = false;
    story.NextPage();
    data.helper.OnSuccessUnlock();
  }


  public override void WhileLiving(float v){

    if( active ){
      if( data.inputEvents.hitTag == "StartNode" && data.inputEvents.oDown == 1 && data.inputEvents.Down == 0 ){
        print(monolith);
        for( int i = 0; i< monolith.storyMarkers.Length; i++ ){
          if( data.inputEvents.hit.collider.gameObject == monolith.storyMarkers[i] ){
            if( data.journey.monoSetters[i].gameObject.name  == nameOfStory){
              TurnPage();
            }
          }
        }
      }else{

      }
    }
  }




}
