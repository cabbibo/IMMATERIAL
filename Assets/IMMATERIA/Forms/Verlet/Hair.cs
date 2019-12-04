using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hair: Form {

  public Form baseForm;
  public int numVertsPerHair;
  public float length;
  public float variance;
  //public Material lineDebugMaterial;
  public int numHairs;

  public float countMultiplier = 1;

  public override void SetStructSize(){ structSize = 16; }

  public override void SetCount(){
    
   float newNum = (float)baseForm.count * (float)countMultiplier;
   if(newNum - Mathf.Floor(newNum) != 0 ){ print("WATISS COUNT MULTIPLIER IS WACKY"); }
   numHairs = (int)newNum;//(float)baseForm.count / (float)countMultiplier;

    count = numHairs * numVertsPerHair; 
  }




}

