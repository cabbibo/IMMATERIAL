using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonolithStorySetter : StorySetter
{

  public Monolith monolith;
  

  public override void Create(){


    SafeInsert( perimeter );
    SafeInsert( monolith );
    uv =new Vector2( transform.position.x * data.land.size , transform.position.z * data.land.size);

    for( int i = 0; i < stories.Length; i ++ ){
      SafeInsert(stories[i]);
    }


    perimeter.OnEnterOuter.AddListener(CheckWhichStory);
    perimeter.OnEnterOuter.AddListener(EnterOuter);
    perimeter.OnEnterInner.AddListener(EnterInner);
    perimeter.OnExitOuter.AddListener(ExitOuter);
    perimeter.OnExitInner.AddListener(ExitInner);

    perimeter.OnExitOuter.AddListener(TurnOffMonolith);
    perimeter.OnEnterOuter.AddListener(TurnOnMonolith);

  
  }

  public void TurnOffMonolith(){
    monolith.gameObject.SetActive(false);
  }

  public void TurnOnMonolith(){
    monolith.gameObject.SetActive(true);
  }

    
}
