using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : Cycle
{

  public bool monolithParticlesEmitting;
  public int  whichMonolithEmitting;
  public bool hasFallen;
  public bool hasPickedUpBook;

  public int[] storiesVisited;

  public float frameBorder = .05f;

  // which id in the set of stories 
  public int id;
   
  public Page[] pages;
  public int currentPage;


  // means we are entering and exiting via code, not by walking closer
  // ( most likely just first page )
  public bool hardcoded;

  // Can't go back past the first page ( most likely just page one)
  public bool cantUnstart;

  // are we spawning particles from camera or from ursula
  public bool spawnFromCamera;

  // Story has started!
  public bool started;


  public StorySetter setter;
  public StateSetter state;



  // Info for page turning
  public bool transitioning;
  public float transitionSpeed;
  public float transitionStartTime;
  public Page oldTransitionPage;
 
  public bool forward;

  public EventTypes.BaseEvent OnEnterOuter;
  public EventTypes.BaseEvent OnExitOuter;


  public EventTypes.BaseEvent OnEnterInner;
  public EventTypes.BaseEvent OnExitInner;
    
  bool s_monolithParticlesOn;
  int  s_whichActiveMonolith;
  bool s_grounded;
  bool s_bookPickedUp;

  int[] s_storiesVisited;
  

  // The words should be coming from the camera
  public void SpawnFromCamera(){
    spawnFromCamera = true;
  }
  
  // the words should be coming from the player
  public void SpawnFromPlayer(){
    spawnFromCamera = false;
  }



  public override void Create(){
    for( int i = 0; i < pages.Length; i ++ ){
      pages[i].setter = setter;
      pages[i].story = this;
      SafeInsert(pages[i]);
    }
    started = false;
    currentPage = 0;
  }

  public override void OnBirthed(){
    for( int i = 0; i < pages.Length; i ++ ){
      pages[i].frameMPB.SetFloat("_Cutoff" , 1);
      pages[i].fade = 1;
      pages[i].frame.borderLeft = frameBorder;
      pages[i].frame.borderRight = frameBorder;
      pages[i].frame.borderTop = frameBorder;
      pages[i].frame.borderBottom = frameBorder;
      pages[i].frame.collider.enabled = false;
    }

    transitioning = false;

  }

   public void CheckForStart(){

    if( !started ){
     
      RaycastHit hit;

      if (pages[currentPage].frame.collider.Raycast(data.inputEvents.ray, out hit, 100.0f)){
        print("HELLLOOOSOSOSOS)");
        StartStory();
      }else{
        
      }

    }

  }

  // populate all the events from this page forward
  public void SetAllEvents(){
    for( int i = 0; i < currentPage-1; i++ ){
      pages[i].OnStartEnter.Invoke();
      pages[i].OnEndExit.Invoke();
    }
  }



  public void NextPage(){

    if( started && transitioning == false && !pages[currentPage].locked ){

      forward = true;

      
      if( currentPage < pages.Length-1 ){

        oldTransitionPage = pages[currentPage];
        currentPage ++;

        PageTurn();

      }else{

        oldTransitionPage = pages[currentPage];
        currentPage = 0;
        LeaveStoryEnd();
        
   
      }
      
    }else{

      if( started && transitioning == false && pages[currentPage].locked ){
        data.helper.OnPageLocked();
      }
    }

  }


  public void SetUpTransition(){
     // Set up transition
     transitioning = true;
     transitionSpeed = pages[currentPage].lerpSpeed;
     if( data.state.fast ){ transitionSpeed = 1; }
     transitionStartTime = Time.time;
  }

  public void PageTurn(){
     
      
    data.audio.Play( setter.audio.endClips[Random.Range(0,setter.audio.endClips.Length)] , 1f , .1f);
      SetUpTransition();
      SetActivePage();
      if( forward ){
        oldTransitionPage.OnEndExit.Invoke();
      }else{
        oldTransitionPage.OnStartExit.Invoke();
      }

     data.framer.Set( pages[currentPage] );

  }

  public void LeaveStoryEnd(){

      
      
    data.audio.Play( setter.audio.endClips[Random.Range(0,setter.audio.endClips.Length)] , 1f , .1f);
      SetUpTransition();
      oldTransitionPage.OnEndExit.Invoke();
      
    setter.audio.Exit();
      Release();
      
  }

  public void PreviousPage(){
    
    if( started && transitioning == false && !pages[currentPage].mustContinue ){

      forward = false;
     
      
      if( currentPage > 0 ){
        
        oldTransitionPage = pages[currentPage];
        currentPage --;
        
        PageTurn();

      }else{
        if( !cantUnstart ){
          transitioning = true;
          transitionSpeed = pages[0].lerpSpeed;
          
          if( data.state.fast ){ transitionSpeed = 1; }
          transitionStartTime = Time.time;
          oldTransitionPage = pages[currentPage+1];

          pages[currentPage+1].OnStartExit.Invoke();
          currentPage = 0;
          Release();
        }else{
          currentPage ++;
        }
      }
    }else{
      if( started && transitioning == false && pages[currentPage].mustContinue ){
        data.helper.OnPageCantGoBack();
      }

    }

  }

  public void SetActivePage(){

    
    data.textParticles.Release();
    
    data.cameraControls.SetLerpTarget( pages[currentPage].transform , transitionSpeed );
    
    if( pages[currentPage].audioInfo.Length == setter.audio.audioInfo.Length ){
      for( int i = 0; i < setter.audio.audioInfo.Length; i++ ){
        setter.audio.audioInfo[i] = pages[currentPage].audioInfo[i];
      }
    }else{
      DebugThis("WHOA WE GOT AN AUDIO PROBLEM");
    }

    if( pages[currentPage].moveTarget ){ data.playerControls.SetMoveTarget( pages[currentPage].moveTarget ); }
    if( pages[currentPage].lerpTarget ){ data.playerControls.SetLerpTarget( pages[currentPage].lerpTarget , transitionSpeed ); }
    if( pages[currentPage].moveTarget &&  pages[currentPage].lerpTarget ){ Debug.LogError("this page has multiple targets"); }

  }   


  public void OnLockPage(){

    data.audio.Play( setter.audio.startClips[Random.Range(0,setter.audio.startClips.Length)] , 1f , .11f);
    transitionSpeed = pages[currentPage].lerpSpeed;
    data.textParticles.Set( pages[currentPage].text );

    if( forward ){
      pages[currentPage].OnStartEnter.Invoke();
    }else{
      pages[currentPage].OnEndEnter.Invoke();
    }

    if( spawnFromCamera ){
      data.textParticles.SpawnFromCamera();
    }else{
      data.textParticles.PageStart();
    }
  }

  public void Release(){


    started = false;

    data.state.inPages = false;

    data.cameraControls.SetFollowTarget();
    data.textParticles.Release();
    //data.cameraControls.lerping = false;
    data.playerControls.lerping = false;

    SetColliders( true );

  }

  public void SetColliders( bool val ){
    
    for( int i = 0; i < pages.Length; i ++ ){
      pages[i].frame.collider.enabled = false; 
    }

    pages[0].frame.collider.enabled = val;
  }

  
  
  public void StartStory(){

    setter.audio.Enter();
    data.state.inPages = true;

    started = true;

    oldTransitionPage = null;
    transitioning = true;
    transitionSpeed = pages[currentPage].lerpSpeed;
    pages[currentPage].OnStartEnter.Invoke();

    if( data.state.fast ){ transitionSpeed = 1; }
    transitionStartTime = Time.time;
    SetActivePage(); 
    SetColliders( false );

    
        data.framer.Set( pages[currentPage] );


//    print("STORY STARTED");

  }

  public override void WhileLiving( float v){
    
    if( transitioning ){
      DoBetweenFade();
    }

  }



  public void DoFade(float v ){
  
    pages[currentPage].frameMPB.SetFloat("_Cutoff" , 1-v);
    pages[currentPage].fade = 1-v;

  }

  public void DoBetweenFade(){

    float v = (Time.time - transitionStartTime) / transitionSpeed;

    if( v > 1){ 

      transitioning = false;
      if( started ){ OnLockPage(); }
    
    }

    float hue = pages[currentPage].baseHue;

    if( oldTransitionPage ){
      oldTransitionPage.frameMPB.SetFloat("_Cutoff" , v);
      oldTransitionPage.fade = v;
      oldTransitionPage.FadeOut.Invoke(v);
      hue = Mathf.Lerp( oldTransitionPage.baseHue , pages[currentPage].baseHue , v);

    }


    data.textParticles.body.mpb.SetFloat("_BaseHue" , hue);


    float m  = Mathf.Min((1-v) ,pages[currentPage].frameMPB.GetFloat("_Cutoff"));
    // doing this to make sure the frame doesn't "flash" in 
    pages[currentPage].frameMPB.SetFloat("_Cutoff" ,m);
    pages[currentPage].fade = m;
    pages[currentPage].FadeIn.Invoke(v);

  }



}