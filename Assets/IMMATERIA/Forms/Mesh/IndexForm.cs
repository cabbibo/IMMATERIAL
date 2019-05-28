using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IndexForm : Form {


  public Form toIndex;

  public override void _Create(){
    if( toIndex == null ){ toIndex = GetComponent<Form>(); } 
    DoCreate();
    SetStructSize();
    SetCount();
    SetBufferType();
    Create();
  }
  public override void SetBufferType(){  intBuffer = true; }
  public override void SetStructSize(){ structSize = 1; }
  

  public override void WhileDebug(){
    debugMaterial.SetPass(0);
    debugMaterial.SetBuffer("_VertBuffer", toIndex._buffer);
    debugMaterial.SetBuffer("_TriBuffer", _buffer);
    debugMaterial.SetInt("_Count",count);
    debugMaterial.SetInt("_VertCount",toIndex.count);
    Graphics.DrawProceduralNow(MeshTopology.Lines, (count-1) * 2 );
  }

}
