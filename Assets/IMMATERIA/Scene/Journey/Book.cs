using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : Cycle
{


    public Frame frame;
    public Transform projectorPoint;

    public float above;
    public float forward;

    public bool started;

    public BookStory[] bookStories;
    public GameObject nodePrefab;

    public GameObject[] startNodes; 

    public bool inStory = false;
    public int storyID = 0; 

    private float transitionToStoryTime;
    public float transitionToStoryRate;

    private float transitionOutOfStoryTime;
    public float transitionOutOfStoryRate;


    public override void Destroy(){
      for( int i = 0; i < startNodes.Length; i++ ){
        DestroyImmediate(startNodes[i]);
      }
    }

    public override void Create(){
      frame = GetComponent<Frame>();  
      SafeInsert(frame);

      for( int i = 0; i < startNodes.Length; i++ ){
        DestroyImmediate(startNodes[i]);
      }

      startNodes = new GameObject[ bookStories.Length ];
      
      for(int i = 0; i < bookStories.Length; i++ ){
      
        SafeInsert(bookStories[i]);
        
        startNodes[i] = GameObject.Instantiate( nodePrefab );
        startNodes[i].transform.parent = transform;

      }


    }

    

    public void OpenBook(){

      started = true;

      data.playerControls.animator.SetTrigger("LiftPhone");

      transform.position = data.playerPosition + Vector3.up * above +  data.player.forward * forward;
      transform.rotation = data.player.rotation;

      for(int i = 0; i < bookStories.Length; i++ ){

          print(startNodes[i]);
          frame.SetFrame();
          startNodes[i].transform.position = frame.topLeft + frame.right * bookStories[i].uv.x * frame.width - frame.up * bookStories[i].uv.y * frame.height;

       
        for( int j = 0; j < bookStories[i].pages.Length; j ++ ){

          bookStories[i].pages[j].transform.position = transform.position; 
          bookStories[i].pages[j].transform.rotation = transform.rotation;
          bookStories[i].pages[j].frame.distance = frame.distance;
        }

      }

      data.cameraControls.SetLerpTarget( transform , 1);
      //started = true;
        //  OpenBook();
    

      data.inputEvents.OnTap.AddListener( TapInBook );
      data.inputEvents.OnSwipeLeft.AddListener( LeftSwipe);
      data.inputEvents.OnSwipeRight.AddListener( RightSwipe);

    }

    public void LeftSwipe(){
      if( inStory ){
        bookStories[storyID].NextPage();

        if( bookStories[storyID].started == false ){
          CloseStory();
        }
      }else{
        CloseBook();
      }
    }

    public void RightSwipe(){
        if( inStory ){
          
          bookStories[storyID].PreviousPage();
          if( bookStories[storyID].started == false ){
            CloseStory();
        }
        }else{
          CloseBook();
        }
    }

    public void CloseBook(){
      
      started = false;

      data.cameraControls.SetFollowTarget();

      data.playerControls.animator.SetTrigger("LowerPhone");

      data.inputEvents.OnTap.RemoveListener( TapInBook );
      data.inputEvents.OnSwipeLeft.RemoveListener( LeftSwipe );
      data.inputEvents.OnSwipeRight.RemoveListener( RightSwipe );

    }


  public void TapInBook(){

    if( data.inputEvents.hitTag == "StartNode"){
      print("YA!");
      for( int i = 0; i < startNodes.Length; i++ ){
         if( data.inputEvents.hit.collider.gameObject == startNodes[i] ){
            print( "node + " +  i );
            OpenStory(i);
         }
      }
    }

  }

  public override void WhileLiving(float f){

    if( Time.time - transitionOutOfStoryTime < transitionOutOfStoryRate ){
      DoTransitionOutOf( (Time.time - transitionOutOfStoryTime) / transitionOutOfStoryRate );
    }

    if( Time.time - transitionToStoryTime < transitionToStoryRate ){
      DoTransitionTo( (Time.time - transitionToStoryTime) / transitionToStoryRate );
    }


  }

  public void DoTransitionTo(float v){
    for( int i = 0; i<startNodes.Length; i++){
    //print("TransitioningTo");
      Vector3 start = frame.topLeft + frame.right * bookStories[i].uv.x * frame.width - frame.up * bookStories[i].uv.y * frame.height;
      Vector3 end = frame.bottomLeft + frame.right * bookStories[i].uv.x * frame.width ;
        startNodes[i].transform.position = Vector3.Lerp( start, end , v );
    }
  }


  public void DoTransitionOutOf(float v){
    

    //print("TransitioningOutOff");
    for( int i = 0; i<startNodes.Length; i++){
      Vector3 start = frame.topLeft + frame.right * bookStories[i].uv.x * frame.width - frame.up * bookStories[i].uv.y * frame.height;
      Vector3 end = frame.bottomLeft + frame.right * bookStories[i].uv.x * frame.width ;
        startNodes[i].transform.position = Vector3.Lerp( start, end , 1-v );
    }
  }

  public void OpenStory(int id){

    if( !inStory ){
      transitionToStoryTime = Time.time;
    }

    inStory = true;
    storyID = id;
    bookStories[id].started = true;
    bookStories[id].SetActivePage();
    
  }

  public void CloseStory(){
    inStory = false;
    transitionOutOfStoryTime = Time.time;
  }


    public void CheckForStart(){

      if( !started ){
        RaycastHit hit;
        if( data.inputEvents.hitTag == "Player" && data.state.inPages == false ){
          print("Ya Boi");
          OpenBook();
        }
      }


    }



}
