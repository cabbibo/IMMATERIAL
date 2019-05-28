using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionVerts : Form {

  public int vertsPerVert;
  public Hair hair;
  public Material lineDebugMaterial;

  public override void SetStructSize(){ structSize = 16; }

  public override void SetCount(){
    count = hair.numHairs * vertsPerVert; 
  }



  public override void WhileDebug(){

    
    lineDebugMaterial.SetPass(0);
    lineDebugMaterial.SetBuffer("_VertBuffer", _buffer);
    lineDebugMaterial.SetBuffer("_SkeletonBuffer", hair._buffer);
    lineDebugMaterial.SetInt("_Count",count);
    lineDebugMaterial.SetInt("_VertsPerVert",vertsPerVert);
    lineDebugMaterial.SetInt("_NumVertsPerHair",hair.numVertsPerHair);
    Graphics.DrawProceduralNow(MeshTopology.Lines, count  * 2 );

    debugMaterial.SetPass(0);
    debugMaterial.SetBuffer("_VertBuffer", _buffer);
    debugMaterial.SetInt("_Count",count);
    Graphics.DrawProceduralNow(MeshTopology.Triangles, count * 3 * 2 );

  }




}
