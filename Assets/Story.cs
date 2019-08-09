using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class Story : Cycle
{


  public Vector2 uv;
  public Monolith monolith;

  public int id;

  public float innerRadius;
  public float outerRadius;

  public bool insideInner;
  public bool insideOuter;
   
  public Page[] pages;
  public int currentPage;

  public bool hardcoded;
  public bool cantUnstart;
  public bool spawnFromCamera;




  public EventTypes.StoryEvent OnEnterOuter;
  public EventTypes.StoryEvent OnEnterInner;
  public EventTypes.StoryEvent OnExitOuter;
  public EventTypes.StoryEvent OnExitInner;
  public EventTypes.StoryEvent OnStartStory;
  public EventTypes.StoryEvent OnEndStory;


  public float dif;
  public float oDif;
  public bool started;


  public bool transitioning;
  public float transitionSpeed;
  public float transitionStartTime;
  public Page oldTransitionPage;
  public bool fast;

  public void SpawnFromCamera(){
    spawnFromCamera = true;
  }
  

  public void SpawnFromPlayer(){
    spawnFromCamera = false;
  }

  public override void Create(){


    uv =new Vector2( transform.position.x * data.land.size , transform.position.z * data.land.size);
    dif = 10000000;
    for( int i = 0; i < pages.Length; i ++ ){
      SafeInsert(pages[i]);
    }
    SafeInsert( monolith );
    monolith.story = this;


    for( int i =0; i < data.journey.stories.Length; i++ ){
      if( data.journey.stories[i] == this ){
//        print("IM THIS");
        id = i;
      }
    }

    dif = 1000;
  }

  public override void OnBirthed(){
    for( int i = 0; i < pages.Length; i ++ ){
      pages[i].frameMPB.SetFloat("_Cutoff" , 1);
      pages[i].frame.borderLine.SetPropertyBlock( pages[currentPage].frameMPB );
    }

    monolith.gameObject.SetActive( false );


  }

  public void NextPage(){

    if( started && transitioning == false ){

      currentPage ++;
      
      if( currentPage < pages.Length ){

        transitioning = true;
        transitionSpeed = pages[currentPage].lerpSpeed;
        if( fast ){ transitionSpeed = 1; }
        transitionStartTime = Time.time;

        SetActivePage();
        oldTransitionPage = pages[currentPage-1];
        pages[currentPage].OnStart.Invoke();
        pages[currentPage-1].OnEnd.Invoke();
      }else{
        transitioning = true;
        transitionSpeed = pages[0].lerpSpeed;

        if( fast ){ transitionSpeed = 1; }
        transitionStartTime = Time.time;
        oldTransitionPage = pages[currentPage-1];
        pages[currentPage-1].OnEnd.Invoke();
        currentPage = 0;
        Release();
      }
    }

  }

  public void PreviousPage(){
    
    if( started && transitioning == false ){

      currentPage --;
      
      if( currentPage >= 0 ){


        transitioning = true;
        transitionSpeed = pages[currentPage].lerpSpeed;

        if( fast ){ transitionSpeed = 1; }
        transitionStartTime = Time.time;

        SetActivePage();

        oldTransitionPage = pages[currentPage+1];

         pages[currentPage].OnStart.Invoke();
         pages[currentPage+1].OnEnd.Invoke();

      }else{
        if( !cantUnstart ){
          transitioning = true;
          transitionSpeed = pages[0].lerpSpeed;
          
        if( fast ){ transitionSpeed = 1; }
          transitionStartTime = Time.time;
          oldTransitionPage = pages[currentPage+1];

          pages[currentPage+1].OnEnd.Invoke();
          currentPage = 0;
          Release();
        }else{
          currentPage ++;
        }
      }
    }

  }

  public void SetActivePage(){

    
    data.textParticles.Release();
    
    data.cameraControls.SetLerpTarget( pages[currentPage].transform , transitionSpeed );
   
    if( pages[currentPage].moveTarget ){ data.playerControls.SetMoveTarget( pages[currentPage].moveTarget ); }
    if( pages[currentPage].lerpTarget ){ 
      print("setting Lerp target");
      print(transitionSpeed);

      data.playerControls.SetLerpTarget( pages[currentPage].lerpTarget , transitionSpeed ); }

    if( pages[currentPage].moveTarget &&  pages[currentPage].lerpTarget ){
      Debug.LogError("this page has multiple targets");
    }

  }   


  public void OnLockPage(){
     data.textParticles.Set( pages[currentPage].text );

    if( spawnFromCamera ){
      data.textParticles.SpawnFromCamera();
    }else{
      data.textParticles.PageStart();
    }
  }

  public void Release(){


    started = false;

    data.state.inStory = false;

    data.cameraControls.SetFollowTarget();
    data.textParticles.Release();
    //data.cameraControls.lerping = false;
    data.playerControls.lerping = false;

    SetColliders( true );

    OnEndStory.Invoke( this );

    if( hardcoded ){
      ExitInner();
      ExitOuter();
    }
  }


  public void SetColliders( bool val ){
    for( int i = 0; i < pages.Length; i ++ ){
      pages[i].frame.collider.enabled = false; 
    }
    pages[0].frame.collider.enabled = val;
  }

  public void CheckForStart(){

    if( !started ){
      RaycastHit hit;

      if (pages[currentPage].frame.collider.Raycast(data.inputEvents.ray, out hit, 100.0f)){
        
        StartStory();
      }else{
        
      }
    }
//    print("YA");
  }
  
  public void StartStory(){
//    print("startzio");
 
    if( hardcoded ){
      EnterOuter();
      EnterInner();
      DoFade( 1 );
    }

    data.state.inStory = true;

    OnStartStory.Invoke(this);
    started = true;

      transitioning = true;
        transitionSpeed = pages[currentPage].lerpSpeed;
        if( fast ){ transitionSpeed = 1; }
        transitionStartTime = Time.time;
    SetActivePage(); 
    SetColliders( false );


  }

  public override void WhileLiving( float v){
    
    
    oDif = dif;
    dif = (transform.position - data.player.position).magnitude;

    if( !hardcoded ){

    if( dif < outerRadius && oDif >= outerRadius ){
      EnterOuter();
    }

    if( dif < innerRadius && oDif >= innerRadius ){
      EnterInner();
    }

    if( dif >= outerRadius && oDif < outerRadius ){
      ExitOuter();
    }

    if( dif >= innerRadius && oDif < innerRadius ){
      ExitInner();
    }
    }

    if( insideOuter && !insideInner ){
      DoFade( 1-((dif - innerRadius) / (outerRadius-innerRadius)));
    }



    if( transitioning ){
      DoBetweenFade();
    }


    #if UNITY_EDITOR 
       if( !Application.isPlaying){ 

        for( int i = 0; i < pages.Length; i++ ){
          pages[i].frameMPB.SetFloat("_Cutoff" , 0);
          pages[i].frame.borderLine.SetPropertyBlock(pages[i].frameMPB);
 
        }
      }
    #endif

  }

  public void EnterOuter(){
  
    //Debug.Log("EnterOuttter");
    insideOuter=true;
    data.sceneCircle.Set( this );
    monolith.gameObject.SetActive( true );

    OnEnterOuter.Invoke(this);
  

  }
  public void EnterInner(){
    //Debug.Log("EnterInnerrr");
    insideInner=true;
    data.inputEvents.OnTap.AddListener( CheckForStart );
    data.inputEvents.OnSwipeLeft.AddListener( NextPage );
    data.inputEvents.OnSwipeRight.AddListener( PreviousPage );

    OnEnterInner.Invoke(this);

    DoFade(1);
//    print("fadin");
    

  }

  public void ExitOuter(){
    insideOuter=false;

    data.sceneCircle.Unset( this );
    OnExitOuter.Invoke(this);
    DoFade(0);
  }


  public void ExitInner(){
    insideInner=false;
    data.inputEvents.OnTap.RemoveListener( CheckForStart );
    data.inputEvents.OnSwipeLeft.RemoveListener( NextPage );
    data.inputEvents.OnSwipeRight.RemoveListener( PreviousPage );
    OnExitInner.Invoke(this);
  }


  public void DoFade(float v ){
    pages[currentPage].frameMPB.SetFloat("_Cutoff" , 1-2*v);
    pages[currentPage].frame.borderLine.SetPropertyBlock(pages[currentPage].frameMPB);
  
    pages[currentPage].frameMPB.SetFloat("_Cutoff" , 1-2*v);
    pages[currentPage].frame.borderLine.SetPropertyBlock(pages[currentPage].frameMPB);
  
//    print("fadio");
//    print( 1-2*v);
  }

  public void DoBetweenFade(){

    float v = (Time.time - transitionStartTime) / transitionSpeed;
    if( v > 1 ){ 
      transitioning = false;
      OnLockPage();
    }

  if( oldTransitionPage ){
    oldTransitionPage.frameMPB.SetFloat("_Cutoff" , v);
    oldTransitionPage.frame.borderLine.SetPropertyBlock(oldTransitionPage.frameMPB);
  }
    pages[currentPage].frameMPB.SetFloat("_Cutoff" ,1-v);
    pages[currentPage].frame.borderLine.SetPropertyBlock(pages[currentPage].frameMPB);

//    print("fad btwx");


  }

  public void FadeIn( float v ){

  }

}