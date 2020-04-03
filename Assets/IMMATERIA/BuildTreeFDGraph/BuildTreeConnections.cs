using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BuildTreeConnections : IndexForm
{

  int[] values;
  public override void OnGestate(){
    values = ((BuildTreeVerts)toIndex).info.lookupBuffer.ToArray();
    count = values.Length;
  }
  public override void Embody(){

   SetData(values);
  }


  public override void WhileDebug(){
   
    mpb.SetBuffer("_VertBuffer", toIndex._buffer);
    mpb.SetBuffer("_ConnectionBuffer", _buffer);
    mpb.SetInt("_Count",count);
    mpb.SetInt("_VertCount",toIndex.count);
    
    Graphics.DrawProcedural(debugMaterial,  new Bounds(transform.position, Vector3.one * 5000), MeshTopology.Lines, toIndex.count  * 2 , 1, null, mpb, ShadowCastingMode.Off, true, LayerMask.NameToLayer("Debug"));

    //Graphics.DrawProceduralNow(MeshTopology.Lines, count * 2 );
  }

} 
