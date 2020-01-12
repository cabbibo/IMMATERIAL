using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class StorySetter : Cycle
{


  public Vector2 uv;
  public PerimeterChecker perimeter;
  public int id;

   
  public Story[] stories;
  public int currentStory;

  public StoryAudio audio;

  public Cycle[] localCycles;

  public override void Create(){

    StoryCreate();



  }

  public virtual void StoryCreate(){


    // Turns on and off our story
    SafeInsert( perimeter );

    // Getting Location
    uv =new Vector2( transform.position.x * data.land.size , transform.position.z * data.land.size);

    // Setting up stories
    for( int i = 0; i < stories.Length; i ++ ){
      stories[i].setter = this;
      SafeInsert(stories[i]);
    }

    // Adding any cycles that are just for this set of stories
    for( int i = 0; i < localCycles.Length; i ++ ){
      SafeInsert(localCycles[i]);
    }

    // Setting up audio
    if( audio == null ){ 
      StoryAudio a = GetComponent<StoryAudio>();

      // if we haven't set it up create it
      if( a == null ){ 
        a = gameObject.AddComponent<StoryAudio>();
        a._Create();
      }

      audio = a;
    }

    audio.setter = this;
    SafeInsert( audio );

    //print("Adding listeninters");

    // Setting up our listeners
    perimeter.OnEnterOuter.AddListener(EnterOuter);
    perimeter.OnEnterInner.AddListener(EnterInner);
    perimeter.OnExitOuter.AddListener(ExitOuter);
    perimeter.OnExitInner.AddListener(ExitInner);


  }

  // Unsetting up our listeners!
  public override void Destroy(){
    perimeter.OnEnterOuter.RemoveListener(EnterOuter);
    perimeter.OnEnterInner.RemoveListener(EnterInner);
    perimeter.OnExitOuter.RemoveListener(ExitOuter);
    perimeter.OnExitInner.RemoveListener(ExitInner);
  }


  // This is the function we will override depending 
  // on which set of stories we are looking at
  public virtual void CheckWhichStory(){
    currentStory = 0;
  }

  // This is now the set of stories we are looking at!
  public void EnterOuter(){


    CheckWhichStory();

    if( currentStory < 0 ){

      //This will happen if there is no story to be had!
      data.helper.NoCurrentStory();

    }else{
      

      data.state.SetterEnterOuter( this );

      // Making sure that our story controller has the right story!
      data.journey.controller.EnterOuter(this);


      // TODO: Make it so this is when the individual stories
      // are created and destroyed
      /*for( int i = 0; i < localCycles.Length; i ++ ){
        localCycles[i].SpinDown();
        localCycles[i].SpinUp();
      }*/
    }
 

 

  }

  public void EnterInner(){

     if( currentStory < 0 ){
      //This will happen if there is no story to be had!
      //data.helper.NoCurrentStory();
    }else{
      //using the page turn controller to set all the data!
      data.journey.controller.EnterInner(this);

      // Activating the settter, story and first page BUT NON RECURSIVELY!!!
      _Activate(false);

      // Audio activated non recursively
      audio._Activate(false);
    }


  }

  public void ExitOuter(){
    if( currentStory < 0 ){
      //This will happen if there is no story to be had!
      ///data.helper.NoCurrentStory();
    }else{
      data.journey.controller.ExitOuter(this);
      data.state.SetterExitOuter(this);
    }
  }


  public void ExitInner(){

    if( currentStory < 0 ){
      //This will happen if there is no story to be had!
      //data.helper.NoCurrentStory();
    }else{
      data.state.SetterExitInner(this);
      data.journey.controller.ExitInner(this);
    }
  }


  public void StartStory(){
    print("StartStory Called here");
    data.journey.controller.StartStory();
  }

  public Story CS{
    get{ return stories[currentStory]; }
  }

   public Page CP{
    get{ return CS.pages[CS.currentPage]; }
  }

  public void CheckForStart(){

  }

  public void NextPage(){

  }

  public void PreviousPage(){

  }




  



}