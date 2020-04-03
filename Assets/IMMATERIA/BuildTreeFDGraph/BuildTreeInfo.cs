using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BuildTreeInfo : Form
{

 // public HELP.CycleInfo c;
  public God god;
  public Material lineDebugMaterial;
  public int selectedVert;
 

  public List<Cycle> allCycles;
  public List<HELP.CycleInfo> allCyclesInfo;

  public int maxChildren;
  public List<int> lookupBuffer;

  public override void SetStructSize(){ structSize = 16; }

  public override void OnGestate(){

      //if( allCycles.Count != god.cycles.Count){
      lookupBuffer = new List<int>();
      allCycles = new List<Cycle>();
      allCyclesInfo = new List<HELP.CycleInfo>();
      allCycles.Add(god);
      god.executionID = 0;
      GetFullCount(god);
      count = allCycles.Count;
      GetCycleInfo();
    //}else{
    //  print("alreadyDone");
    //}

  }

  void GetFullCount(Cycle parent ){

    foreach( Cycle cC in parent.Cycles){

      cC.executionID = allCycles.Count;
      allCycles.Add(cC);
      if( cC.Cycles.Count > maxChildren ){ maxChildren = cC.Cycles.Count; }
      cC.parent = parent;
      GetFullCount(cC);
    }

  }


  public void GetCycleInfo(){

    for( int cycleID =0; cycleID < allCycles.Count; cycleID++ ){

      Cycle cycle = allCycles[cycleID];



      HELP.CycleInfo ci = new HELP.CycleInfo();

      ci.type = 0;
      if( cycle is God                ){ ci.type = -2; }
      if( cycle is StorySetter        ){ ci.type = -1; }
      if( cycle is Form               ){ ci.type = 1;  }
      if( cycle is Life               ){ ci.type = 2;  }
      if( cycle is Binder             ){ ci.type = 3;  }
      if( cycle is Simulation         ){ ci.type = 4;  }
      if( cycle is Page               ){ ci.type = 5;  }
      if( cycle is Body               ){ ci.type = 6;  }
      if( cycle is TransferLifeForm   ){ ci.type = 7;  } // should eventually be simulation?

      ci.id = cycleID;
      ci.cycle = cycle;

      ci.position = cycle.transform.position;
      ci.name = "" + cycle.GetType();
      ci.goName = "" + cycle.gameObject.name;
      ci.go = cycle.gameObject;
//      bool alreadyOne = false;
      if( cycle.parent ){
        
       
        ci.parent = cycle.parent.executionID;
        
        ci.lookupStart = lookupBuffer.Count;
        //ci.position = allCyclesInfo[ci.parent].position;


        int me = -1;
        for( int i = 0; i < cycle.parent.Cycles.Count; i++ ){
          if( cycle.parent.Cycles[i] == cycle){ me = i; }
        }

        ci.siblingID = me;
        ci.siblingCount = cycle.parent.Cycles.Count;

        print( ci.parent );
        print( allCyclesInfo.Count );

        ci.position = allCyclesInfo[ci.parent].position + new Vector3((float)me , 1 , 0 );


        //print( allCyclesInfo.Count );
        //print( parent.executionID );
        //print( allCycles.Count );
        //ci.position = (allCyclesInfo[parent.executionID]).position;
        //ci.position += new Vector3(me , 0,0);

      }else{
        ci.position = new Vector3(0,-2,5);
        ci.parent = 0;
      }
      ci.lookupStart = lookupBuffer.Count;

      ci.children = new int[cycle.Cycles.Count];
      ci.active = 1;

      ci.lookupLength = ci.children.Length;
      for( int i = 0; i < cycle.Cycles.Count; i++ ){
        lookupBuffer.Add( cycle.Cycles[i].executionID );
        ci.children[i] = cycle.Cycles[i].executionID;
      }
      

      allCyclesInfo.Add(ci);
      



    }
  }

  public override void Embody(){

    float[] values = new float[count * structSize];

   //print( count );
   print( allCycles.Count );
   print( allCyclesInfo.Count );
    int index = 0;
    HELP.CycleInfo ci;
    for(int i= 0; i < count; i++ ){
      
      ci = allCyclesInfo[i];

      values[index++] = ci.position.x;
      values[index++] = ci.position.y;
      values[index++] = ci.position.z;

      values[index++] = ci.scenePosition.x;
      values[index++] = ci.scenePosition.y;
      values[index++] = ci.scenePosition.z;
      

      values[index++] = 0;
      values[index++] = 0;
      values[index++] = 0;


      values[index++] = ci.active;
      values[index++] = ci.type;
      values[index++] = ci.siblingCount;

      values[index++] = ci.lookupStart;
      values[index++] = ci.lookupLength;

      //print( ci.parent );
      values[index++] = ci.parent;
      values[index++] = 0;

    }

    SetData(values);

    //pos
    //vel
    //nor
    //tan
    //lookupStart
    //lookupLength
    //parent
    //debug

  }


  void ReassignVertValues(){

  }


}
