using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : Cycle
{

  public bool hasFallen;
  public bool hasPickedUpBook;

  public bool[] storiesVisited;

  public int currentStory;
  private int oCurrentStory;

  public float lastTimeStoryVisited;
  
  public bool inStory;
  public bool inPages; 
  public bool inBook;
  public bool inBookPages;


  public bool startInStory;
  public bool startInPages;
  public bool startInBook;
  public bool startInBookPages;




  public bool isMonolithOn;
  private bool oIsMonolithOn;

  public Transform monolithTarget;
  private Transform oMonolithTarget;

  public string animationState;
  private string oAnimationState;

  public override void Create(){
    
    data.journey.currentStory = currentStory;
    if( storiesVisited.Length != data.journey.stories.Length ){
      storiesVisited = new bool[ data.journey.stories.Length ];
    }
  }

  
  public override void OnLive(){
    

      data.book.CloseBook();
      
  //public string 
    if( startInStory  || startInPages ){

      data.player.position = data.journey.stories[data.journey.currentStory].transform.position;

      data.journey.stories[data.journey.currentStory].perimeter.EnterOuter();
      data.journey.stories[data.journey.currentStory].perimeter.EnterInner();

    }


    if( startInStory && !startInPages ){
      data.cameraControls.SetFollowTarget();
    }

    if( startInPages ){

      data.journey.stories[data.journey.currentStory].stories[data.journey.stories[data.journey.currentStory].currentStory].SetAllEvents();
      data.journey.stories[data.journey.currentStory].StartStory();

      // Unless we start in the first story, we are going to want our character to
      // have landeded insted of falling
      if( data.journey.currentStory != 0){
        data.playerControls.animator.Play("Grounded");
      }
    }


    if( startInBook ){
      print("open books");
      data.book.OpenBook();
    }



   /*   if( startInBookPages ){
      data.book.OpenStory(data.book.currentStory);
    }*/

  }



  public override void WhileLiving(float v){
    
      // TODO
      if( isMonolithOn != oIsMonolithOn ){}
      if( monolithTarget != oMonolithTarget ){}
      
  }








  public void PickUpBook(){
    data.state.hasPickedUpBook = true;
  }


  

}
