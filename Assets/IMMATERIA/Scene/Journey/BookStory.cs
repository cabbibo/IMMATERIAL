using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookStory : Cycle
{

  public StorySetter story;
  public Page[] pages;
  public int currentPage;
  public GameObject prefab;

  public Vector2 uv;


  private float dif;
  private float oDif;
  public bool started;

  public bool fast;

  // Info for page turning
  public bool transitioning;
  public float transitionSpeed;
  public float transitionStartTime;
  public Page oldTransitionPage;



  public bool forward;

  public override void Create(){

    for( int i = 0; i < pages.Length; i ++ ){
      SafeInsert(pages[i]);
    }



  }



  // populate all the events from this page forward
  public void SetAllEvents(){
    for( int i = 0; i < currentPage-1; i++ ){
      pages[i].OnStartEnter.Invoke();
      pages[i].OnEndExit.Invoke();
    }
  }



  public override void OnBirthed(){
    for( int i = 0; i < pages.Length; i ++ ){
      pages[i].frameMPB.SetFloat("_Cutoff" , 1);
    }



  }

  public void NextPage(){

    if( started ){

      currentPage ++; 
      forward = true;
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

    }

  }

  public void PreviousPage(){
    
    if( started ){

      currentPage --;

      forward = false;
      
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

          transitioning = true;
          transitionSpeed = pages[0].lerpSpeed;
          
          if( fast ){ transitionSpeed = 1; }
          transitionStartTime = Time.time;
          oldTransitionPage = pages[currentPage+1];

          pages[currentPage+1].OnStartExit.Invoke();
          currentPage = 0;
          Release();
       
      }
    }

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


    
    data.textParticles.SpawnFromCamera();
    
  }

  public void SetActivePage(){


    data.textParticles.Release();
    //data.textParticles.Set( pages[currentPage].text );
    //data.textParticles.PageStart();
  }   

  public void Release(){
    started = false;
    data.textParticles.Release();
  }


  public void StartStory(){
    data.state.inBookPages = true;

    started = true;

    oldTransitionPage = null;
    transitioning = true;
    transitionSpeed = pages[currentPage].lerpSpeed;
    pages[currentPage].OnStartEnter.Invoke();

    if( fast ){ transitionSpeed = 1; }
    transitionStartTime = Time.time;
    SetActivePage(); 
    //SetColliders( false );
}


  public override void WhileLiving( float v){
    
    if( transitioning ){
      DoBetweenFade();
    }

  }



  public void DoFade(float v ){
  
    pages[currentPage].frameMPB.SetFloat("_Cutoff" , 1-v);
  
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
      oldTransitionPage.FadeOut.Invoke(v);
      hue = Mathf.Lerp( oldTransitionPage.baseHue , pages[currentPage].baseHue , v);

    }else{

    }


      data.textParticles.body.mpb.SetFloat("_BaseHue" , hue);

      if(started){

    // doing this to make sure the frame doesn't "flash" in 
    pages[currentPage].frameMPB.SetFloat("_Cutoff" ,Mathf.Min
      ((1-v) ,pages[currentPage].frameMPB.GetFloat("_Cutoff")));
    pages[currentPage].FadeIn.Invoke(v);

  }





//    print("fad btwx");


  }

  public void FadeIn( float v ){

  }


}
