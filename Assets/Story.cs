using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : Cycle
{


  public float innerRadius;
  public float outerRadius;

  public bool insideInner;
  public bool insideOuter;
   
  public Page[] pages;
  public int currentPage;


  private float dif;
  private float oDif;
  public bool started;


  public bool transitioning;
  public float transitionSpeed;
  public float transitionStartTime;
  public Page oldTransitionPage;
  
  public override void Create(){

    for( int i = 0; i < pages.Length; i ++ ){
      SafeInsert(pages[i]);
    }

  }

  public override void OnBirthed(){
    for( int i = 0; i < pages.Length; i ++ ){
      pages[i].frameMPB.SetFloat("_Cutoff" , 1);
      pages[i].frame.borderLine.SetPropertyBlock( pages[currentPage].frameMPB );
    }
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
      }else{
        transitioning = true;
        transitionSpeed = pages[0].lerpSpeed;
        transitionStartTime = Time.time;
        oldTransitionPage = pages[currentPage-1];
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

      }else{
        
        transitioning = true;
        transitionSpeed = pages[0].lerpSpeed;
        transitionStartTime = Time.time;
        oldTransitionPage = pages[currentPage+1];
        currentPage = 0;
        Release();
      }
    }

  }

  public void SetActivePage(){
    data.Text.Set( pages[currentPage].text );
    data.Text.PageStart();
    data.Controls.SetLerpTarget( pages[currentPage].transform ,pages[currentPage].lerpSpeed);
    if( pages[currentPage].moveTarget ){ data.PlayerControls.SetMoveTarget( pages[currentPage].moveTarget.position ); }


  }   

  public void Release(){


    started = false;
    data.Controls.SetFollowTarget();
    data.Text.Release();
  }



  public void CheckForStart(){

    if( !started ){
      RaycastHit hit;

      if (pages[currentPage].frame.collider.Raycast(data.Events.ray, out hit, 100.0f)){
        started = true;
        SetActivePage(); 
      }else{
        
      }
    }
//    print("YA");
  }
  
  public override void WhileLiving( float v){
    
    
    oDif = dif;
    dif = (transform.position - data.Player.position).magnitude;

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
  

  }
  public void EnterInner(){
    Debug.Log("EnterInnerrr");
    insideInner=true;
    data.Events.OnTap.AddListener( CheckForStart );
    data.Events.OnSwipeLeft.AddListener( NextPage );
    data.Events.OnSwipeRight.AddListener( PreviousPage );

  }

  public void ExitOuter(){
    insideOuter=false;
  }


  public void ExitInner(){
    insideInner=false;
    data.Events.OnTap.RemoveListener( CheckForStart );
    data.Events.OnSwipeLeft.RemoveListener( NextPage );
    data.Events.OnSwipeRight.RemoveListener( PreviousPage );
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
