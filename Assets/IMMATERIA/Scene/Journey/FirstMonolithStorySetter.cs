using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMonolithStorySetter : MonolithStorySetter
{
    

    public override void CheckWhichStory(){

      if(data.state.hasPickedUpBook){
        currentStory = 0;
      }else{
        currentStory = 1;
      }
    }
}
