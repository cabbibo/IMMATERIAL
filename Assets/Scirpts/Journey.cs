using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journey : Cycle
{

  public StorySetter[] stories;

  public int currentStory;
  public int connectedStory;
  public bool inStory;

  public bool startInStory;



  public override void Destroy(){

  }


   public override void Create(){


      for( int i = 0; i < stories.Length; i++ ){
        SafeInsert( stories[i] );
      }

   }


   public override void OnLive(){
    if( startInStory ){

      data.player.position = stories[currentStory].transform.position;

      stories[currentStory].perimeter.EnterOuter();
      stories[currentStory].perimeter.EnterInner();

      stories[currentStory].StartStory();
    }

   }

  public void ConnectMonolith(int id){
    connectedStory = id;
    Shader.SetGlobalInt("_ConnectedStory" , connectedStory );
    data.monolithParticles._Emit = 1;
    //data.monolithParticles._EmitterPosition = stories[id].monolith.transform.position;
  }

  public void DisconnectMonolith(int id){
    connectedStory = -1;
    Shader.SetGlobalInt("_ConnectedStory" , connectedStory );
    data.monolithParticles._Emit = 0;
  }

  public void OnStoryEnter( Story story ){
    currentStory = story.id;
    Shader.SetGlobalInt("_CurrentStory" , currentStory );
    inStory = true;
  }

  public void OnStoryExit( Story story ){
    inStory = false;
  }


}
