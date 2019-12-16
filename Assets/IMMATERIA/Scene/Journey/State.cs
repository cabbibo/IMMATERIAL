using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : Cycle
{




  public bool hasFallen;
  public bool hasPickedUpBook;
  
  public int whichStoryLoop;
  
  public bool monolithParticlesEmitting;
  public int  whichMonolithEmitting;

  public int[] storiesVisited;

  public int currentSetter;
  private int oCurrentStory;

  public float lastTimeStoryVisited;
  
  public bool inStory;
  public bool inPages; 
  public bool inBook;
  public bool inBookPages;




  public int currentConnectedMonolith;

  public int startSetter;
  public int startStory;
  public int startPage;

  public bool startInStory;
  public bool startInPages;
  public bool startInBook;
  public bool startInBookPages;




public StorySetter setter;
public Story story;

  public bool fast;



  public bool isMonolithOn;
  private bool oIsMonolithOn;

  public Transform monolithTarget;
  private Transform oMonolithTarget;

  public string animationState;
  private string oAnimationState;

  public Tutorial tutorial;

  public override void Create(){
    
    currentSetter = startSetter;

    if( storiesVisited.Length != data.journey.setters.Length ){
      storiesVisited = new int[ data.journey.setters.Length ];
    }

     SafeInsert(tutorial);
    
  }

  public void SetStartToCurrentStory(){
     startSetter = currentSetter;
     setter = data.journey.setters[startSetter];
     startStory = setter.currentStory;

     startPage = setter.stories[startStory].currentPage;
  }

  
  public override void OnLive(){
    

      data.book.CloseBook();
      
  //public string 
    if( startInStory  || startInPages ){
      
      currentSetter = startSetter;
      setter = data.journey.setters[startSetter];

      data.player.position = data.journey.setters[currentSetter].transform.position;

      data.terrainTap.SetTransform( data.journey.setters[currentSetter].transform );

      if( startStory >= 0 ){
        if(setter.stories[startStory] != null ){

          story = setter.stories[startStory];
          SetStoryState(story);

          setter.perimeter.EnterOuter();
          setter.perimeter.EnterInner();
        
        }else{
          Debug.Log("NO SUB STORY!!!");
        }
      }


      //data.journey.setters[data.journey.currentSetter].
    }

    inStory = startInStory;
    inPages = startInPages;

    //data.journey.inStory = startInStory;
    if( startInStory && !startInPages ){
      data.cameraControls.SetFollowTarget();
    }

    if( startInPages ){
      
      setter.stories[startStory].currentPage = startPage;

      setter.stories[startStory].SetAllEvents();
      setter.StartStory();

    }



    ConnectMonolith( whichMonolithEmitting );
    if( !monolithParticlesEmitting ){
      DisconnectMonolith( whichMonolithEmitting );
    }

    if( hasFallen ){
      print("HAS FALLEN");

    data.playerControls.animator.SetBool("FallAsleep", false);
    data.playerControls.animator.SetBool("Falling", false);
    data.playerControls.animator.SetBool("GetUp", true);
      data.playerControls.animator.Play("Grounded");
    }else{
      //data.playerControls.Fall();
    }


    if( startInBook ){
      print("open books");
      data.book.OpenBook();
    }

    if( hasPickedUpBook ){
      PickUpBook();
    }else{
      PutDownBook();
    }




   /*   if( startInBookPages ){
      data.book.OpenStory(data.book.currentSetter);
    }*/

  }



  public override void WhileLiving(float v){
    
      // TODO
      if( isMonolithOn != oIsMonolithOn ){}
      if( monolithTarget != oMonolithTarget ){}
      
  }





  public void Fall(){

  }

  public void Stand(){
    
  }


  public void PutDownBook(){
    data.playerControls.epiphanyRing.circle.body.mpb.SetFloat("_StartTime" , Time.time );
    data.playerControls.epiphanyRing.circle.body.mpb.SetFloat("_Setting" , 0 );
    //data.playerControls.epiphanyRing.UnSet();
    data.playerControls.OnGroundBook.SetActive(true);
    data.playerControls.InHandBook.SetActive(false);
    data.state.hasPickedUpBook = false;
  }

  public void PickUpBook(){
    //data.playerControls.epiphanyRing.Set();
    data.playerControls.OnGroundBook.SetActive(false);
    data.playerControls.InHandBook.SetActive(true);
    data.state.hasPickedUpBook = true;
  }


  public void SetStoryState( Story s ){

    hasPickedUpBook = s.hasPickedUpBook;
    hasFallen = s.hasFallen;
    monolithParticlesEmitting = s.monolithParticlesEmitting;
    whichMonolithEmitting = s.whichMonolithEmitting;
    
  }
  


  public void ConnectMonolith(int id){
    whichMonolithEmitting = id;
    Shader.SetGlobalInt("_ConnectedStory" , whichMonolithEmitting );
    data.monolithParticles._Emit = 1;
    data.monolithParticles._EmitterPosition = data.journey.monoSetters[id].monolith.transform.position;
  }

  public void DisconnectMonolith(int id){
    whichMonolithEmitting = -1;
    Shader.SetGlobalInt("_ConnectedStory" , -1 );
    data.monolithParticles._Emit = 0;
  }


}
