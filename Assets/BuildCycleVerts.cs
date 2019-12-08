using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BuildCycleVerts : Particles
{

 // public CycleInfo c;
  public God god;
  public Material lineDebugMaterial;
  public int selectedVert;
  public struct CycleInfo {
    
    public string name;

    public int   type;
    public int[] children;
    public int[] siblings;
    public int parent;

    public int lookupStart;
    public int lookupLength;

    public Vector3 position;

    public int selectedVert;


    /*
    FOR LATER
    public int count;
    public int level;
    public int[] boundForms;
*/

  };

  public List<Cycle> allCycles;
  public List<CycleInfo> allCyclesInfo;

  public int maxChildren;
  public List<int> lookupBuffer;
  public override void SetCount(){

    //lookupBuffer = new List<int>();
    //allCycles = new List<Cycle>();
    //allCyclesInfo = new List<CycleInfo>();
    //allCycles.Add(god);
    //god.executionID = 0;
    //GetFullCount(god.Cycles);
    //count = allCycles.Count;

  }

  public override void OnGestate(){

    print( allCycles.Count-1 );
    print( god.cycles.Count);
    if( allCycles.Count != god.cycles.Count){
      lookupBuffer = new List<int>();
      allCycles = new List<Cycle>();
      allCyclesInfo = new List<CycleInfo>();
      allCycles.Add(god);
      god.executionID = 0;
      GetFullCount(god);
      count = allCycles.Count;
      GetCycleInfo();
    }else{
      print("alreadyDone");
    }

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

      CycleInfo ci = new CycleInfo();

      ci.type = 0;
      if( cycle is Form               ){ ci.type = 1;}
      if( cycle is Life               ){ ci.type = 2;}
      if( cycle is Binder             ){ ci.type = 3;}
      if( cycle is Simulation         ){ ci.type = 4;}
      if( cycle is TransferLifeForm   ){ ci.type = 5;} // should eventually be simulation?

     // ci.position = cycle.transform.position;

      bool alreadyOne = false;
      if( cycle.parent ){
        
       
        ci.parent = cycle.parent.executionID;
        ci.siblings = new int[cycle.parent.Cycles.Count];
        
        ci.lookupStart = lookupBuffer.Count;
        //ci.position = allCyclesInfo[ci.parent].position;


        int me = -1;
        for( int i = 0; i < cycle.parent.Cycles.Count; i++ ){
          if( cycle.parent.Cycles[i] == cycle){ me = i; }
          lookupBuffer.Add( cycle.parent.Cycles[i].executionID );
          //print(cycle.parent.Cycles[i].executionID);
          ci.siblings[i] = cycle.parent.Cycles[i].executionID;
        }

        ci.position = allCyclesInfo[ci.parent].position + new Vector3((float)me , 1 , 0 );

        if( me == -1){ Debug.Log("WE GOT A PROBLEMS"); }
        ci.lookupLength = ci.siblings.Length;

        //print( allCyclesInfo.Count );
        //print( parent.executionID );
        //print( allCycles.Count );
        //ci.position = (allCyclesInfo[parent.executionID]).position;
        //ci.position += new Vector3(me , 0,0);

      }else{
        ci.position = new Vector3(0,-2,5);
        ci.parent = 0;
      }
      
      ci.children = new int[cycle.Cycles.Count];
      for( int i = 0; i < cycle.Cycles.Count; i++ ){
        ci.children[i] = cycle.Cycles[i].executionID;
      }
      

      allCyclesInfo.Add(ci);
      



    }


    print( allCyclesInfo.Count);
  }

  public override void Embody(){

    print( structSize );
    float[] values = new float[count * structSize];

    print( count );
    print( allCycles.Count );
    print( allCyclesInfo.Count );
    int index = 0;
    CycleInfo ci;
    for(int i= 0; i < count; i++ ){
      
      ci = allCyclesInfo[i];

      values[index++] = ci.position.x;
      values[index++] = ci.position.y;
      values[index++] = ci.position.z;

      values[index++] = 0;
      values[index++] = 0;
      values[index++] = 0;
      
      values[index++] = 0;
      values[index++] = 0;
      values[index++] = 0;

      values[index++] = 0;
      values[index++] = 0;
      values[index++] = 0;
      
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

    public override void WhileDebug(){
    mpb.SetBuffer("_VertBuffer", _buffer);
    mpb.SetInt("_Count",count);
    mpb.SetInt("_SelectedVert",selectedVert);
    
    
    Graphics.DrawProcedural(debugMaterial,  new Bounds(transform.position, Vector3.one * 5000), MeshTopology.Triangles, count * 3 * 2 , 0, null, mpb, ShadowCastingMode.Off, true, LayerMask.NameToLayer("Debug"));
    
    Graphics.DrawProcedural(lineDebugMaterial,  new Bounds(transform.position, Vector3.one * 5000), MeshTopology.Lines, count  * 2 , 1, null, mpb, ShadowCastingMode.Off, true, LayerMask.NameToLayer("Debug"));

    //Graphics.DrawProceduralNow(MeshTopology.Lines, count * 2 );
  }


    
}
