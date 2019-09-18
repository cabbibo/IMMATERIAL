using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class Story : Cycle
{

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



  // Info for page turning
  public bool transitioning;
  public float transitionSpeed;
  public float transitionStartTime;
  public Page oldTransitionPage;
 

  // Makes it so that we can move faster
  public bool fast;

  public bool forward;

  public EventTypes.BaseEvent OnEnterOuter;
  public EventTypes.BaseEvent OnExitOuter;


  public EventTypes.BaseEvent OnEnterInner;
  public EventTypes.BaseEvent OnExitInner;


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
      SafeInsert(pages[i]);
    }
    started = false;
  }

  public override void OnBirthed(){
    for( int i = 0; i < pages.Length; i ++ ){
      pages[i].frameMPB.SetFloat("_Cutoff" , 1);
      pages[i].frame.borderLine.SetPropertyBlock( pages[currentPage].frameMPB );
      pages[i].frame.borderLeft = frameBorder;
      pages[i].frame.borderRight = frameBorder;
      pages[i].frame.borderTop = frameBorder;
      pages[i].frame.borderBottom = frameBorder;
    }

    transitioning = false;

  }

   public void CheckForStart(){

    if( !started ){
     
      RaycastHit hit;

      if (pages[currentPage].frame.collider.Raycast(data.inputEvents.ray, out hit, 100.0f)){
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


//    print("NEXXX");
    if( started && transitioning == false && !pages[currentPage].locked ){

      forward = true;
      currentPage ++;
      
      if( currentPage < pages.Length ){

        transitioning = true;
        transitionSpeed = pages[currentPage].lerpSpeed;
        if( fast ){ transitionSpeed = 1; }
        transitionStartTime = Time.time;

        SetActivePage();
        oldTransitionPage = pages[currentPage-1];
        pages[currentPage-1].OnEndExit.Invoke();

      }else{
        
        transitioning = true;
        transitionSpeed = pages[0].lerpSpeed;

        if( fast ){ transitionSpeed = 1; }
        transitionStartTime = Time.time;
        oldTransitionPage = pages[currentPage-1];
        pages[currentPage-1].OnEndExit.Invoke();
        currentPage = 0;
        Release();
      
      }
      
    }else{

      if( started && transitioning == false && pages[currentPage].locked ){
        data.helper.OnPageLocked();
      }
    }

  }

  public void PreviousPage(){
    
    if( started && transitioning == false && !pages[currentPage].mustContinue ){

      forward = false;
      currentPage --;
      
      if( currentPage >= 0 ){

        transitioning = true;
        //transitionSpeed = pages[currentPage].lerpSpeed;

        if( fast ){ transitionSpeed = 1; }
        transitionStartTime = Time.time;

        SetActivePage();

        oldTransitionPage = pages[currentPage+1];

         //pages[currentPage].OnEndEnter.Invoke();
         pages[currentPage+1].OnStartExit.Invoke();

      }else{
        if( !cantUnstart ){
          transitioning = true;
          transitionSpeed = pages[0].lerpSpeed;
          
          if( fast ){ transitionSpeed = 1; }
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
   
    if( pages[currentPage].moveTarget ){ data.playerControls.SetMoveTarget( pages[currentPage].moveTarget ); }
    if( pages[currentPage].lerpTarget ){ data.playerControls.SetLerpTarget( pages[currentPage].lerpTarget , transitionSpeed ); }
    if( pages[currentPage].moveTarget &&  pages[currentPage].lerpTarget ){ Debug.LogError("this page has multiple targets"); }

  }   


  public void OnLockPage(){

//    print("ON LOCK PAGE");
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


    data.state.inPages = true;

    started = true;

    oldTransitionPage = null;
    transitioning = true;
    transitionSpeed = pages[currentPage].lerpSpeed;
    pages[currentPage].OnStartEnter.Invoke();

    if( fast ){ transitionSpeed = 1; }
    transitionStartTime = Time.time;
    SetActivePage(); 
    SetColliders( false );


//    print("STORY STARTED");

  }

  public override void WhileLiving( float v){
    
    if( transitioning ){
      DoBetweenFade();
    }

  }



  public void DoFade(float v ){
  
    pages[currentPage].frameMPB.SetFloat("_Cutoff" , 1-v);
    pages[currentPage].frame.borderLine.SetPropertyBlock(pages[currentPage].frameMPB);
  
 //   print("fadio");
//    print( 1-2*v);
  }

  public void DoBetweenFade(){

    float v = (Time.time - transitionStartTime) / transitionSpeed;


    if( v > 1){ 

      transitioning = false;

      if( started ){ 
        OnLockPage();
      }
    
    }

    float hue = pages[currentPage].baseHue;

    if( oldTransitionPage ){
      oldTransitionPage.frameMPB.SetFloat("_Cutoff" , v);
      oldTransitionPage.frame.borderLine.SetPropertyBlock(oldTransitionPage.frameMPB);
      oldTransitionPage.FadeOut.Invoke(v);
      hue = Mathf.Lerp( oldTransitionPage.baseHue , pages[currentPage].baseHue , v);

    }else{

    }


      data.textParticles.body.mpb.SetFloat("_BaseHue" , hue);

    // doing this to make sure the frame doesn't "flash" in 
    pages[currentPage].frameMPB.SetFloat("_Cutoff" ,Mathf.Min
      ((1-v) ,pages[currentPage].frameMPB.GetFloat("_Cutoff")));
    pages[currentPage].frame.borderLine.SetPropertyBlock(pages[currentPage].frameMPB);
    pages[currentPage].FadeIn.Invoke(v);





//    print("fad btwx");


  }

  public void FadeIn( float v ){

  }

}