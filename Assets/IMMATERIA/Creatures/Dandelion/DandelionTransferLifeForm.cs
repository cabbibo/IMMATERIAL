using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionTransferLifeForm :TransferLifeForm{

  public DandelionVerts dandelion;
 
  public override void Bind(){

    transfer.BindForm("_DandelionBuffer" , dandelion);
    transfer.BindAttribute("_VertsPerVert" , "vertsPerVert" , dandelion);
    transfer.BindAttribute("_NumVertsPerHair" , "numVertsPerHair" , skeleton);
    
  }



}
