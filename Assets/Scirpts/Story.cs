using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class Story : Cycle
{

  public int id;
   
  public Page[] pages;
  public int currentPage;

  public bool hardcoded;
  public bool cantUnstart;
  public bool spawnFromCamera;
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

   public void CheckForStart(){

    if( !started ){
      RaycastHit hit;

      if (pages[currentPage].frame.collider.Raycast(data.inputEvents.ray, out hit, 100.0f)){
        
        StartStory();
      }else{
        
      }
    }
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

  }

  public void SetColliders( bool val ){
    for( int i = 0; i < pages.Length; i ++ ){
      pages[i].frame.collider.enabled = false; 
    }
    pages[0].frame.collider.enabled = val;
  }

  
  
  public void StartStory(){


    data.state.inStory = true;

    started = true;

    transitioning = true;
    transitionSpeed = pages[currentPage].lerpSpeed;
    if( fast ){ transitionSpeed = 1; }
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
    pages[currentPage].frameMPB.SetFloat("_Cutoff" , 1-2*v);
    pages[currentPage].frame.borderLine.SetPropertyBlock(pages[currentPage].frameMPB);
  
    pages[currentPage].frameMPB.SetFloat("_Cutoff" , 1-2*v);
    pages[currentPage].frame.borderLine.SetPropertyBlock(pages[currentPage].frameMPB);
  
//    print("fadio");
//    print( 1-2*v);
  }

  public void DoBetweenFade(){

    float v = (Time.time - transitionStartTime) / transitionSpeed;
    if( v > 1 && started ){ 
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