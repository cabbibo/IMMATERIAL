using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : Cycle
{

  public bool hasFallen;
  public bool hasPickedUpBook;

  public bool[] storiesVisited;

  public int currentStory;
  private int oCurrentStory;

  public float lastTimeStoryVisited;
  
  public bool inStory;
  public bool inBook;
  public bool inPages; 


  public bool isMonolithOn;
  private bool oIsMonolithOn;

  public Transform monolithTarget;
  private Transform oMonolithTarget;

  public string animationState;
  private string oAnimationState;


  //public string 




  public override void WhileLiving(float v){
    
      // TODO
      if( isMonolithOn != oIsMonolithOn ){}
      if( monolithTarget != oMonolithTarget ){}
      
  }




  public override void Create(){
    if( storiesVisited.Length != data.journey.stories.Length ){
      storiesVisited = new bool[ data.journey.stories.Length ];
    }
  }



  

}
