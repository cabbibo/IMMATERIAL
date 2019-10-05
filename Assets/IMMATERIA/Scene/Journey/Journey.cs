﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journey : Cycle
{

  public bool NOSTORIES;


  public StorySetter[] stories;

  public MonolithStorySetter[] monoStories;
  public StorySetter[] nonMonoStories;

  public int currentStory;
  public int connectedStory;
  public int activatedMonolith;
  public bool inStory;


  public Tutorial tutorial;



  public override void Destroy(){

  }


   public override void Create(){

    SafeInsert(tutorial);
    
    if( !NOSTORIES ){


      int numMonoStories = 0;

      for( int i = 0; i < stories.Length; i++ ){
        SafeInsert( stories[i] );
        if( stories[i] is MonolithStorySetter ){
          numMonoStories ++;
        }
      }

      int monoIndex = 0;
      int nonMonoIndex = 0;

//      print( numMonoStories );

      monoStories = new MonolithStorySetter[ numMonoStories ];
      nonMonoStories = new StorySetter[ stories.Length - numMonoStories ];

      for( int i = 0; i < stories.Length; i++ ){
        if( stories[i] is MonolithStorySetter ){
          stories[i].id = monoIndex;
          monoStories[monoIndex] = (MonolithStorySetter)stories[i];
          monoIndex ++;
        }else{
          nonMonoStories[nonMonoIndex] = stories[i];
          nonMonoIndex ++;
        }
      }


    }else{

      Cycles.Clear();
      SafeInsert( stories[currentStory] );
    }


   }


   public override void OnLive(){
    
    DisconnectMonolith(0);

   }

  public void ConnectMonolith(int id){
    connectedStory = id;
    activatedMonolith = id;
    Shader.SetGlobalInt("_ConnectedStory" , connectedStory );
    data.monolithParticles._Emit = 1;
    data.monolithParticles._EmitterPosition = monoStories[id].monolith.transform.position;
  }

  public void DisconnectMonolith(int id){
    connectedStory = -1;
    Shader.SetGlobalInt("_ConnectedStory" , connectedStory );
    data.monolithParticles._Emit = 0;
  }

  public void OnStoryEnter( Story story ){
    currentStory = story.id;
    Shader.SetGlobalInt("_CurrentStory" , currentStory );
    inStory = true;
  }

  public void OnStoryExit( Story story ){
    inStory = false;
  }



}