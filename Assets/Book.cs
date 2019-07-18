using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : Cycle
{


    public Frame frame;
    public Transform projectorPoint;

    public float above;

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

      data.PlayerControls.animator.SetTrigger("LiftPhone");

      transform.position = data.PlayerPosition + Vector3.up * above;
      transform.rotation = data.Player.rotation;

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

      data.Controls.SetLerpTarget( transform , 1);
      //started = true;
        //  OpenBook();
    

      data.Events.OnTap.AddListener( TapInBook );
      data.Events.OnSwipeLeft.AddListener( LeftSwipe);
      data.Events.OnSwipeRight.AddListener( RightSwipe);

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

      data.Controls.SetFollowTarget();

      data.PlayerControls.animator.SetTrigger("LowerPhone");

      data.Events.OnTap.RemoveListener( TapInBook );
      data.Events.OnSwipeLeft.RemoveListener( LeftSwipe );
      data.Events.OnSwipeRight.RemoveListener( RightSwipe );

    }


  public void TapInBook(){

    if( data.Events.hitTag == "StartNode"){
      print("YA!");
      for( int i = 0; i < startNodes.Length; i++ ){
         if( data.Events.hit.collider.gameObject == startNodes[i] ){
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
        if( data.Events.hitTag == "Player" ){
          print("Ya Boi");
          OpenBook();
        }
      }


    }



}
