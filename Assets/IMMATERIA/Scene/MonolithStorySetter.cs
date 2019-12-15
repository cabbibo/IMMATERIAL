using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonolithStorySetter : StorySetter
{

  public Monolith monolith;
  

  public override void Create(){


    StoryCreate();

    SafeInsert( monolith );
    perimeter.OnExitOuter.AddListener(TurnOffMonolith);
    perimeter.OnEnterOuter.AddListener(TurnOnMonolith);

    for( int i = 0; i < data.journey.monoSetters.Length; i++ ){
      if( data.journey.monoSetters[i] == this ){
        monolith.whichStory = i;
      }
    }

  
  }

  public void TurnOffMonolith(){
    monolith.gameObject.SetActive(false);
  }

  public void TurnOnMonolith(){
    monolith.gameObject.SetActive(true);
  }

    
}
