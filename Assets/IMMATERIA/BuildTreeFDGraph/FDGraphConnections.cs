using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FDGraphConnections : IndexForm
{


  public FDGraph graph;

  public override void Embody(){
    SetData(graph.GetConnectionData());
  }

  public override void WhileDebug(){
   
    mpb.SetBuffer("_VertBuffer", graph.verts._buffer);
    mpb.SetBuffer("_ConnectionBuffer", _buffer);
    mpb.SetInt("_Count",count);
    mpb.SetInt("_VertCount",graph.verts.count);
    
    Graphics.DrawProcedural(debugMaterial,  new Bounds(transform.position, Vector3.one * 5000), MeshTopology.Lines, count , 1, null, mpb, ShadowCastingMode.Off, true, LayerMask.NameToLayer("Debug"));

  }


}
