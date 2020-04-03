using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BuildTreeVerts : Particles
{


  public Material lineDebugMaterial;

  public BuildTreeInfo info;

  public override void SetCount(){}

  public override void OnGestate(){
    count = info.count;
  }


  public override void Embody(){

    print("EMBODY GETTING CALLLED");
    print( structSize );
    float[] values = new float[count * structSize];

    int index = 0;
    HELP.CycleInfo ci;
    for(int i= 0; i < count; i++ ){
      
      ci = info.allCyclesInfo[i];

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
      
      values[index++] = 0;
      values[index++] = 0;

      //print( ci.parent );
      values[index++] = 0;
      values[index++] = 0;

    }

    SetData(values);

  }

  public override void WhileDebug(){
    
    mpb.SetBuffer("_VertBuffer", _buffer);
    mpb.SetBuffer("_InfoBuffer", info._buffer);
    mpb.SetInt("_Count",count);
    mpb.SetInt("_SelectedVert",info.selectedVert);
    
    Graphics.DrawProcedural(debugMaterial,  new Bounds(transform.position, Vector3.one * 5000), MeshTopology.Triangles, count * 3 * 2 , 0, null, mpb, ShadowCastingMode.Off, true, LayerMask.NameToLayer("Debug")); 
    Graphics.DrawProcedural(lineDebugMaterial,  new Bounds(transform.position, Vector3.one * 5000), MeshTopology.Lines, count  * 2 , 1, null, mpb, ShadowCastingMode.Off, true, LayerMask.NameToLayer("Debug"));

  }


    
}
