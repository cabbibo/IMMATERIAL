using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingStorySetter : StorySetter
{


  public float timeSinceLast;
  public float howLongToPlaceIfLost;

  public override void WhileLiving(float v){

  timeSinceLast =  Time.time - data.state.lastTimeStoryVisited;


    if( timeSinceLast > howLongToPlaceIfLost && data.state.inStory == false  ){
      transform.position = data.player.transform.position + data.player.transform.forward * (perimeter.outerRadius -.01f);
      transform.position = new Vector3( transform.position.x, data.land.SampleHeight(transform.position) , transform.position.z );
    }
  }



}
