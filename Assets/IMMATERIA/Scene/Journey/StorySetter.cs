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

      audio.setter = this;
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

    data.state.SetSetter( this );

    CheckWhichStory();
    if( currentStory < 0 ){
      data.helper.NoCurrentStory();
    }else{
      
      data.sceneCircle.Set( this.perimeter );
      data.state.lastTimeStoryVisited = Time.time;
      data.state.inStory = true;
      
      CS.DoFade(0);
      CS.OnEnterOuter.Invoke();
      perimeter.OnDoFade.AddListener(CS.DoFade);

      /*for( int i = 0; i < localCycles.Length; i ++ ){
        localCycles[i].SpinDown();
        localCycles[i].SpinUp();
      }*/
    }
 

 

  }

  public void EnterInner(){


    data.inputEvents.OnTap.AddListener( CS.CheckForStart );
    data.inputEvents.OnEdgeSwipeLeft.AddListener(  CS.NextPage );
    data.inputEvents.OnEdgeSwipeRight.AddListener(  CS.PreviousPage );

    data.framer.Set( CS.pages[CS.currentPage] );
    CS.SetColliders(true);
    //CS.pages[CS.currentPage]
    data.textParticles.Release();//.Set( CS.pages[CS.currentPage] );
    CS.OnEnterInner.Invoke();
    CS.DoFade(1);
    
    _Activate(false);
    CS._Activate(false);
    CS.pages[CS.currentPage]._Activate(false);

    audio._Activate(false);


  }

  public void ExitOuter(){

    data.state.UnsetSetter();
    data.sceneCircle.Unset( this.perimeter );
    
    CS.DoFade(0);
    CS.OnExitOuter.Invoke();
    
    perimeter.OnDoFade.RemoveListener(CS.DoFade);

    /*for( int i = 0; i < localCycles.Length; i ++ ){
      localCycles[i].SpinDown();
    }*/
  }


  public void ExitInner(){
    data.inputEvents.OnTap.RemoveListener(  CS.CheckForStart );
    data.inputEvents.OnEdgeSwipeLeft.RemoveListener(  CS.NextPage );
    data.inputEvents.OnEdgeSwipeRight.RemoveListener(  CS.PreviousPage );
    
    data.state.lastTimeStoryVisited = Time.time;
    data.state.inStory = false;
    
    CS.OnExitInner.Invoke();
    CS.DoFade(1);
//    print("exitInner");
  }


  public void StartStory(){
    CS.StartStory();
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