using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : Cycle
{




  public bool hasFallen;
  public bool hasPickedUpBook;
  public int whichStoryLoop;
  public bool monolithParticlesEmitting;
  public int  whichMonolithEmitting;


/*
  public bool spacePuppyVisited;
  public bool crystalsVisited;
  public bool spacePuppyContent;
  public bool dandelionsVisited;
  public bool lighthouseVisited;
  public bool lighthouseContent;
  public bool pitOfGoldVisited;
  public bool kelpTonguesVisited;
*/



  public bool[] storiesVisited;

  public int currentStory;
  private int oCurrentStory;

  public float lastTimeStoryVisited;
  
  public bool inStory;
  public bool inPages; 
  public bool inBook;
  public bool inBookPages;




  public int currentConnectedMonolith;

  public int startStory;
  public int startPage;

  public bool startInStory;
  public bool startInPages;
  public bool startInBook;
  public bool startInBookPages;




  public bool fast;



  public bool isMonolithOn;
  private bool oIsMonolithOn;

  public Transform monolithTarget;
  private Transform oMonolithTarget;

  public string animationState;
  private string oAnimationState;

  public override void Create(){
    
    data.journey.currentStory = startStory;
    if( storiesVisited.Length != data.journey.stories.Length ){
      storiesVisited = new bool[ data.journey.stories.Length ];
    }
  }

  public void SetStartToCurrentStory(){
     startStory = data.journey.currentStory;
     startPage = data.journey.stories[data.journey.currentStory].stories[data.journey.stories[data.journey.currentStory].currentStory].currentPage;
  }

  
  public override void OnLive(){
    

      data.book.CloseBook();
      
  //public string 
    if( startInStory  || startInPages ){

      data.player.position = data.journey.stories[data.journey.currentStory].transform.position;

      data.journey.stories[data.journey.currentStory].perimeter.EnterOuter();
      data.journey.stories[data.journey.currentStory].perimeter.EnterInner();

      //data.journey.stories[data.journey.currentStory].
    }


    data.journey.inStory = startInStory;
    if( startInStory && !startInPages ){
      data.cameraControls.SetFollowTarget();

    }

    if( startInPages ){
data.journey.stories[data.journey.currentStory].stories[data.journey.stories[data.journey.currentStory].currentStory].currentPage = startPage;

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

    if( hasPickedUpBook ){
      PickUpBook();
    }else{
      PutDownBook();
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







  public void PutDownBook(){
    data.playerControls.epiphanyRing.circle.body.mpb.SetFloat("_StartTime" , Time.time );
    data.playerControls.epiphanyRing.circle.body.mpb.SetFloat("_Setting" , 0 );
    data.playerControls.epiphanyRing.UnSet();
    data.playerControls.OnGroundBook.SetActive(true);
    data.playerControls.InHandBook.SetActive(false);
    data.state.hasPickedUpBook = false;
  }

  public void PickUpBook(){
    data.playerControls.epiphanyRing.Set();
    data.playerControls.OnGroundBook.SetActive(false);
    data.playerControls.InHandBook.SetActive(true);
    data.state.hasPickedUpBook = true;
  }


  

}
