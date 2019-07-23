using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : Cycle
{


  public Vector2 uv;
  public Monolith monolith;


public int storyID;


  public float innerRadius;
  public float outerRadius;

  public bool insideInner;
  public bool insideOuter;
   
  public Page[] pages;
  public int currentPage;

  public EventTypes.BaseEvent OnEnterOuter;
  public EventTypes.BaseEvent OnEnterInner;
  public EventTypes.BaseEvent OnExitOuter;
  public EventTypes.BaseEvent OnExitInner;


  private float dif;
  private float oDif;
  public bool started;


  public bool transitioning;
  public float transitionSpeed;
  public float transitionStartTime;
  public Page oldTransitionPage;
  
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
        print("IM THIS");
        storyID = i;
      }
    }
  }

  public override void OnBirthed(){
    for( int i = 0; i < pages.Length; i ++ ){
      pages[i].frameMPB.SetFloat("_Cutoff" , 1);
      pages[i].frame.borderLine.SetPropertyBlock( pages[currentPage].frameMPB );
    }

    monolith.gameObject.SetActive( false );
  }

  public void NextPage(){

    if( started ){

      currentPage ++;
      
      if( currentPage < pages.Length ){
        SetActivePage();
        transitioning = true;
        transitionSpeed = pages[currentPage].lerpSpeed;
        transitionStartTime = Time.time;
        oldTransitionPage = pages[currentPage-1];
        pages[currentPage].OnStart.Invoke();
        pages[currentPage-1].OnEnd.Invoke();
      }else{
        transitioning = true;
        transitionSpeed = pages[0].lerpSpeed;
        transitionStartTime = Time.time;
        oldTransitionPage = pages[currentPage-1];
        pages[currentPage-1].OnEnd.Invoke();
        currentPage = 0;
        Release();
      }
    }

  }

  public void PreviousPage(){
    
    if( started ){

      currentPage --;
      
      if( currentPage >= 0 ){
        SetActivePage();

        transitioning = true;
        transitionSpeed = pages[currentPage].lerpSpeed;
        transitionStartTime = Time.time;
        oldTransitionPage = pages[currentPage+1];

         pages[currentPage].OnStart.Invoke();
         pages[currentPage+1].OnEnd.Invoke();

      }else{
        
        transitioning = true;
        transitionSpeed = pages[0].lerpSpeed;
        transitionStartTime = Time.time;
        oldTransitionPage = pages[currentPage+1];

        pages[currentPage+1].OnEnd.Invoke();
        currentPage = 0;
        Release();
      }
    }

  }

  public void SetActivePage(){
    data.textParticles.Set( pages[currentPage].text );
    data.textParticles.PageStart();
    data.cameraControls.SetLerpTarget( pages[currentPage].transform ,pages[currentPage].lerpSpeed);
   
    if( pages[currentPage].moveTarget ){ data.playerControls.SetMoveTarget( pages[currentPage].moveTarget.position ); }


  }   

  public void Release(){


    started = false;
    data.cameraControls.SetFollowTarget();
    data.textParticles.Release();
  }



  public void CheckForStart(){

    if( !started ){
      RaycastHit hit;

      if (pages[currentPage].frame.collider.Raycast(data.inputEvents.ray, out hit, 100.0f)){
        started = true;
        SetActivePage(); 
      }else{
        
      }
    }
//    print("YA");
  }
  
  public override void WhileLiving( float v){
    
    
    oDif = dif;
    dif = (transform.position - data.player.position).magnitude;

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

    if( insideOuter && !insideInner ){
      DoFade( 1-((dif - innerRadius) / (outerRadius-innerRadius)));
    }

    if( transitioning ){
      DoBetweenFade();
    }

  }

  public void EnterOuter(){
  
    Debug.Log("EnterOuttter");
    insideOuter=true;
    data.sceneCircle.Set( this );
    monolith.gameObject.SetActive( true );
  

  }
  public void EnterInner(){
    Debug.Log("EnterInnerrr");
    insideInner=true;
    data.inputEvents.OnTap.AddListener( CheckForStart );
    data.inputEvents.OnSwipeLeft.AddListener( NextPage );
    data.inputEvents.OnSwipeRight.AddListener( PreviousPage );
    

  }

  public void ExitOuter(){
    insideOuter=false;

    data.sceneCircle.Unset( this );
  }


  public void ExitInner(){
    insideInner=false;
    data.inputEvents.OnTap.RemoveListener( CheckForStart );
    data.inputEvents.OnSwipeLeft.RemoveListener( NextPage );
    data.inputEvents.OnSwipeRight.RemoveListener( PreviousPage );
  }


  public void DoFade(float v ){
    pages[currentPage].frameMPB.SetFloat("_Cutoff" , 1-2*v);
    pages[currentPage].frame.borderLine.SetPropertyBlock(pages[currentPage].frameMPB);
  }

  public void DoBetweenFade(){

    float v = (Time.time - transitionStartTime) / transitionSpeed;
    if( v > 1 ){ transitioning = false; }

    oldTransitionPage.frameMPB.SetFloat("_Cutoff" , v);
    pages[currentPage].frameMPB.SetFloat("_Cutoff" ,1-v);

    pages[currentPage].frame.borderLine.SetPropertyBlock(pages[currentPage].frameMPB);
    oldTransitionPage.frame.borderLine.SetPropertyBlock(oldTransitionPage.frameMPB);



  }

  public void FadeIn( float v ){

  }

}