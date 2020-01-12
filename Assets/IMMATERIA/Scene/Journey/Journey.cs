using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journey : Cycle
{

  public StoryController controller;
  public bool NOSTORIES;


  public StorySetter[] setters;

  public MonolithStorySetter[] monoSetters;
  public StorySetter[] nonMonoSetters;

// public int currentSetter;
// public int connectedStory;
// public int activatedMonolith;



  public override void Destroy(){

  }


   public override void Create(){

    SafeInsert( controller );
   
    if( !NOSTORIES ){

      int numMonoSetters = 0;

      for( int i = 0; i < setters.Length; i++ ){
        SafeInsert( setters[i] );
        if( setters[i] is MonolithStorySetter ){
          numMonoSetters ++;
        }
      }

      int monoIndex = 0;
      int nonMonoIndex = 0;

//      print( numMonoSetters );

      monoSetters = new MonolithStorySetter[ numMonoSetters ];
      nonMonoSetters = new StorySetter[ setters.Length - numMonoSetters ];

      for( int i = 0; i < setters.Length; i++ ){
        if( setters[i] is MonolithStorySetter ){
          setters[i].id = monoIndex;
          monoSetters[monoIndex] = (MonolithStorySetter)setters[i];
          monoIndex ++;
        }else{
          nonMonoSetters[nonMonoIndex] = setters[i];
          nonMonoIndex ++;
        }
      }


    }else{

      Cycles.Clear();
      SafeInsert( setters[data.state.currentSetter] );
    }


   }

  public void OnStoryEnter( Story story ){
    data.state.currentSetter = story.id;
    Shader.SetGlobalInt("_CurrentStory" , data.state.currentSetter );
    data.state.inStory = true;
  }

  public void OnStoryExit( Story story ){
    data.state.inStory = false;
  }



}
