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


    SafeInsert( perimeter );
    uv =new Vector2( transform.position.x * data.land.size , transform.position.z * data.land.size);

    for( int i = 0; i < stories.Length; i ++ ){
      stories[i].setter = this;
      SafeInsert(stories[i]);
    }


    for( int i = 0; i < localCycles.Length; i ++ ){
      SafeInsert(localCycles[i]);
    }

    if( audio == null ){ 
      StoryAudio a = GetComponent<StoryAudio>();
      if( a == null ){ 
        a = gameObject.AddComponent<StoryAudio>();
        a._Create();
      }
      audio = a;
    }

    SafeInsert( audio );

    perimeter.OnEnterOuter.AddListener(EnterOuter);
    perimeter.OnEnterInner.AddListener(EnterInner);
    perimeter.OnExitOuter.AddListener(ExitOuter);
    perimeter.OnExitInner.AddListener(ExitInner);


  }

  public override void Destroy(){
    perimeter.OnEnterOuter.RemoveListener(EnterOuter);
    perimeter.OnEnterInner.RemoveListener(EnterInner);
    perimeter.OnExitOuter.RemoveListener(ExitOuter);
    perimeter.OnExitInner.RemoveListener(ExitInner);
  }


  public virtual void CheckWhichStory(){
    currentStory = 0;
  }



  

  public override void WhileLiving( float v){

  }

  public void EnterOuter(){
    CheckWhichStory();

    stories[currentStory].DoFade(0);
    stories[currentStory].OnEnterOuter.Invoke();


    perimeter.OnDoFade.AddListener(stories[currentStory].DoFade);
 

    /*for( int i = 0; i < localCycles.Length; i ++ ){
       localCycles[i].SpinDown();
      localCycles[i].SpinUp();
    }*/

  }

  public void EnterInner(){
    
    print("DOBLSLSW");
    data.inputEvents.OnTap.AddListener( stories[currentStory].CheckForStart );
    data.inputEvents.OnEdgeSwipeLeft.AddListener(  stories[currentStory].NextPage );
    data.inputEvents.OnEdgeSwipeRight.AddListener(  stories[currentStory].PreviousPage );

    stories[currentStory].OnEnterInner.Invoke();
    stories[currentStory].DoFade(1);
    
    _Activate();


  }

  public void ExitOuter(){

//    print("StorySetter exiting outer : " + gameObject.name );
    stories[currentStory].DoFade(0);

    stories[currentStory].OnExitOuter.Invoke();
    perimeter.OnDoFade.RemoveListener(stories[currentStory].DoFade);

    /*for( int i = 0; i < localCycles.Length; i ++ ){
      localCycles[i].SpinDown();
    }*/
  }


  public void ExitInner(){
    data.inputEvents.OnTap.RemoveListener(  stories[currentStory].CheckForStart );
    data.inputEvents.OnEdgeSwipeLeft.RemoveListener(  stories[currentStory].NextPage );
    data.inputEvents.OnEdgeSwipeRight.RemoveListener(  stories[currentStory].PreviousPage );

    stories[currentStory].OnExitInner.Invoke();
    stories[currentStory].DoFade(1);
//    print("exitInner");
  }


  public void StartStory(){
    stories[currentStory].StartStory();
  }
  



}