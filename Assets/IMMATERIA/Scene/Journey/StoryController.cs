using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryController : Cycle
{
  
  public StorySetter setter;
  public Story story;
  public int currentPageID;
  public Page currentPage;
  public Page oldTransitionPage;

  public Page[] pages;

  public bool started;
  public bool transitioning;

  public bool forward;
  public float transitionSpeed;

  public float transitionStartTime;

  // The first page's collider needs to be clicked 
  // in order for the story to start
  // The collider will be turned on when we enter the inner circle?
  public void CheckForStart(){

    if( !started ){
     
      RaycastHit hit;

      if (pages[currentPageID].frame.collider.Raycast(data.inputEvents.ray, out hit, 100.0f)){
        StartStory();
      }else{
        
      }

    }

  }


  // Need to make sure that if we start in the middle 
  // of a story, we update all events that set state 
  // before it
  public void SetAllEvents(){
    
    for( int i = 0; i < currentPageID; i++ ){
      //print( "setting all events");
      pages[i].OnStartEnter.Invoke();
      pages[i].OnEndExit.Invoke();
    }
  }




  // Sets up our transition!
  public void SetUpTransition(){
     // Set up transition
     transitioning = true;
     transitionSpeed = pages[currentPageID].lerpSpeed;
     if( data.state.fast ){ transitionSpeed = 1; }
     transitionStartTime = Time.time;
  }





  public void NextPage(){

    // Only Turn page IF:
    // the story is started
    // we are not currently transitioning
    // the current page isn't locked!
    // the frame is currently being shown instead of all hidden
    if( started && transitioning == false && !pages[currentPageID].locked  && data.state.frameShown ){

      forward = true;
      
      // We are NOT at the end of the story!
      if( currentPageID < pages.Length-1 ){

        oldTransitionPage = pages[currentPageID];
        currentPageID ++;

        PageTurn();

      // We ARE at the end of the story
      }else{

        oldTransitionPage = pages[currentPageID];
        currentPageID = 0;
        LeaveStory();

      }
      
    }else{

      // If its locked, then we are going to do something to let
      // the user know that the page is currently locked
      if( started && transitioning == false && pages[currentPageID].locked ){
        data.helper.OnPageLocked();
      }
    }

  }


  public void PreviousPage(){

    // Only Turn page IF:
    // the story is started
    // we are not currently transitioning
    // we are allowed to go back!
    // the frame is currently being shown instead of all hidden
    if( started && transitioning == false && !pages[currentPageID].mustContinue && data.state.frameShown ){

      forward = false;
     
      if( currentPageID > 0 ){
        
        oldTransitionPage = pages[currentPageID];
        currentPageID --;
        
        PageTurn();

      }else{
        // Can NEVER unstart a story!
        data.helper.OnPageCantGoBack();
        //currentPageID ++;
      }

    }else{

      // Need to have some way of letting the user know they can't go backwards!
      if( started && transitioning == false && pages[currentPageID].mustContinue ){
        data.helper.OnPageCantGoBack();
      }
    }

  }





  // How we turn the Page!
  public void PageTurn(){

    data.framer.frames[data.framer.currentFrame].closeButton.gameObject.GetComponent<FadeMaterial>().FadeOut();

    if( setter.audio.startClips.Length == 0 ){ 
      DebugThis("This story doesn't have audio yet");
    }else{
     data.sound.Play( setter.audio.endClips[Random.Range(0,setter.audio.endClips.Length)] , 1f , .1f);
    }    
    SetUpTransition();
    SetActivePage();
    

    if( forward ){
      oldTransitionPage.OnEndExit.Invoke();
    }else{
      oldTransitionPage.OnStartExit.Invoke();
    }

   data.framer.Set( pages[currentPageID] );

  }


  // Leaving the Story!
  public void LeaveStory(){
 if( setter.audio.startClips.Length == 0 ){ 
      DebugThis("This story doesn't have audio yet");
    }else{
    data.sound.Play( setter.audio.endClips[Random.Range(0,setter.audio.endClips.Length)] , 1f , .1f);
      }   
      SetUpTransition();
      oldTransitionPage.OnEndExit.Invoke();
      data.framer.Release();//( pages[currentPageID] );
      setter.audio.Exit();
      Release();
      story.OnStoryEnd.Invoke();
      
  }



  public void SetActivePage(){

    // letting go of our current pages
    data.textParticles.Release();
    
    // Setting the next page for our camera to move to
    data.cameraControls.SetLerpTarget( pages[currentPageID].transform , transitionSpeed );
    
    // Setting up the audio for each page
    if( pages[currentPageID].audioInfo.Length == setter.audio.audioInfo.Length ){
      for( int i = 0; i < setter.audio.audioInfo.Length; i++ ){
        setter.audio.audioInfo[i] = pages[currentPageID].audioInfo[i];
      }
    }else{
      DebugThis("WHOA WE GOT AN AUDIO PROBLEM");
    }

    if( pages[currentPageID].moveTarget ){ data.playerControls.SetMoveTarget( pages[currentPageID].moveTarget ); }
    if( pages[currentPageID].lerpTarget ){ data.playerControls.SetLerpTarget( pages[currentPageID].lerpTarget , transitionSpeed ); }
    if( pages[currentPageID].moveTarget &&  pages[currentPageID].lerpTarget ){ Debug.LogError("this page has multiple targets"); }


    pages[currentPageID]._Activate();
  }   


  // We have now gotten to the page and fullty transitioned
  public void OnLockPage(){

    if( oldTransitionPage ){ oldTransitionPage._Deactivate(); }


    // fade in our little 'close button'
    data.framer.frames[data.framer.currentFrame].closeButton.gameObject.GetComponent<FadeMaterial>().FadeIn();
    
    if( setter.audio.startClips.Length == 0 ){ 
      DebugThis("This story doesn't have audio yet");
    }else{
      data.sound.Play( setter.audio.startClips[Random.Range(0,setter.audio.startClips.Length)] , 1f , .11f);
    }

    transitionSpeed = pages[currentPageID].lerpSpeed;
    

    // Set up the text for this page
    data.textParticles.Set( pages[currentPageID].text );

    // Than spawn the particles
    if( story.spawnFromCamera ){
      data.textParticles.SpawnFromCamera();
    }else{
      data.textParticles.PageStart();
    }


    // CALL EVENTS!
    if( forward ){
      pages[currentPageID].OnStartEnter.Invoke();
    }else{
      pages[currentPageID].OnEndEnter.Invoke();
    }

   
  }

  public void Release(){


    print( "Release" );
    started = false;

    data.state.inPages = false;

    data.cameraControls.SetFollowTarget();
    data.textParticles.Release();
    data.playerControls.lerping = false;

    //SetColliders( true );

  }


  // Makes it so that our first collider is on 
  // To make sure that we can click it to start the story!
  public void SetColliders( bool val ){
      

//    print("SETGTING COLLIDERS : " + val );
    for( int i = 0; i < pages.Length; i ++ ){
      pages[i].frame.collider.enabled = false; 
    }

    pages[0].frame.collider.enabled = val;
  }

  
  
  public void StartStory(){
  

    story.OnStoryStart.Invoke();

    setter.audio.Enter();
    data.state.inPages = true;

    started = true;

    oldTransitionPage = null;
    transitioning = true;
    transitionSpeed = pages[currentPageID].lerpSpeed;
    pages[currentPageID].OnStartEnter.Invoke();

    // Hacking it so that if we just started the app,
    // It will transition instantly
    if( data.state.fast ){ transitionSpeed = 1; }
    if( data.state.firstStory ){ transitionSpeed = .000f; }


    transitionStartTime = Time.time;
    SetActivePage(); 
    SetColliders( false );

  }

  public override void WhileLiving( float v){
    
    if( transitioning ){
      DoBetweenFade();
    }

  }



  public void DoFade(float v ){
    pages[currentPageID].frameMPB.SetFloat("_Cutoff" , 1-v);
    pages[currentPageID].fade = 1-v;
  }

  public void DoBetweenFade(){

    float v = (Time.time - transitionStartTime) / transitionSpeed;


    if( transitionSpeed  == 0 ){
      v = 1.01f;
    }

    if( v > 1){ 

      transitioning = false;
      if( started ){ OnLockPage(); }
    
    }

    float hue = pages[currentPageID].baseHue;

    if( oldTransitionPage ){
      oldTransitionPage.frameMPB.SetFloat("_Cutoff" , v);
      oldTransitionPage.fade = v;
      oldTransitionPage.FadeOut.Invoke(v);
      hue = Mathf.Lerp( oldTransitionPage.baseHue , pages[currentPageID].baseHue , v);

    }

    data.textParticles.body.mpb.SetFloat("_BaseHue" , hue);

    float m  = Mathf.Min((1-v) ,pages[currentPageID].frameMPB.GetFloat("_Cutoff"));
    // doing this to make sure the frame doesn't "flash" in 
    pages[currentPageID].frameMPB.SetFloat("_Cutoff" ,m);
    pages[currentPageID].fade = m;
    pages[currentPageID].FadeIn.Invoke(v);

  }


  /*


_______  __    _  _______  _______  ______      _______  __   __  _______  __    _  _______  _______ 
|       ||  |  | ||       ||       ||    _ |    |       ||  | |  ||       ||  |  | ||       ||       |
|    ___||   |_| ||_     _||    ___||   | ||    |    ___||  |_|  ||    ___||   |_| ||_     _||  _____|
|   |___ |       |  |   |  |   |___ |   |_||_   |   |___ |       ||   |___ |       |  |   |  | |_____ 
|    ___||  _    |  |   |  |    ___||    __  |  |    ___||       ||    ___||  _    |  |   |  |_____  |
|   |___ | | |   |  |   |  |   |___ |   |  | |  |   |___  |     | |   |___ | | |   |  |   |   _____| |
|_______||_|  |__|  |___|  |_______||___|  |_|  |_______|  |___|  |_______||_|  |__|  |___|  |_______|



  */


  public void EnterOuter( StorySetter s ){

    // Set up all our fun stuff 
    setter = s;
    story = s.CS;
    currentPageID = story.currentPage;
    pages = story.pages;
    currentPage = story.pages[currentPageID];
    data.sceneCircle.Set( setter.perimeter );
    setter.perimeter.OnDoFade.AddListener(DoFade);

    DoFade(0);

    story.OnEnterOuter.Invoke();


    data.inputEvents.OnTap.AddListener( CheckForStart );
    data.inputEvents.OnEdgeSwipeLeft.AddListener( NextPage );
    data.inputEvents.OnEdgeSwipeRight.AddListener( PreviousPage );


  }

   // We will have already set the  current story etc. here because we will always call:
    // enter outer THAN enter inner
  public void EnterInner(StorySetter s){


//    print("entering inner of controller");

    data.framer.Set( currentPage );

    // Making it so the colliders
    SetColliders(true);
    data.textParticles.Release();//.Set( CS.pages[CS.currentPageID] );
    story.OnEnterInner.Invoke();
    
    DoFade(1);

    story._Activate(false);
    currentPage._Activate(false);

  }



  public void ExitOuter(StorySetter s){

    DoFade(0);
    story.OnExitOuter.Invoke();

    setter.perimeter.OnDoFade.RemoveListener(DoFade);
    
    /*for( int i = 0; i < localCycles.Length; i ++ ){
      localCycles[i].SpinDown();
    }*/
  }


  public void ExitInner(StorySetter s){
    //print("exiting the inner");

    data.sceneCircle.Unset( s.perimeter );
    data.inputEvents.OnTap.RemoveListener(CheckForStart);
    data.inputEvents.OnEdgeSwipeLeft.RemoveListener(NextPage);
    data.inputEvents.OnEdgeSwipeRight.RemoveListener(PreviousPage);
    story.OnExitInner.Invoke();
    DoFade(1);
  }



   public Page CP{
    get{ return pages[currentPageID]; }
  }

}
