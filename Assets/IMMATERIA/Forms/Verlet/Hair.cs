using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hair: Form {

  public Form baseForm;
  public int numVertsPerHair;
  public float length;
  public float variance;
  public Material lineDebugMaterial;
  public int numHairs;

  public float countMultiplier = 1;

  public override void SetStructSize(){ structSize = 16; }

  public override void SetCount(){
    
   float newNum = (float)baseForm.count * (float)countMultiplier;
   if(newNum - Mathf.Floor(newNum) != 0 ){ print("WATISS COUNT MULTIPLIER IS WACKY"); }
   numHairs = (int)newNum;//(float)baseForm.count / (float)countMultiplier;

    count = numHairs * numVertsPerHair; 
  }

  public override void WhileDebug(){
    
    lineDebugMaterial.SetPass(0);
    lineDebugMaterial.SetBuffer("_VertBuffer", _buffer);
    lineDebugMaterial.SetInt("_Count",count);
    lineDebugMaterial.SetInt("_NumVertsPerHair",numVertsPerHair);
    Graphics.DrawProceduralNow(MeshTopology.Lines, count  * 2 );

    debugMaterial.SetPass(0);
    debugMaterial.SetBuffer("_VertBuffer", _buffer);
    debugMaterial.SetInt("_Count",count);
    Graphics.DrawProceduralNow(MeshTopology.Triangles, count  * 2 );

  }


}

