using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journey : Cycle
{

  public bool NOSTORIES;


  public StorySetter[] setters;

  public MonolithStorySetter[] monoSetters;
  public StorySetter[] nonMonoSetters;

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
      SafeInsert( setters[currentStory] );
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
    data.monolithParticles._EmitterPosition = monoSetters[id].monolith.transform.position;
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
