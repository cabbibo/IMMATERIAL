using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionTransferLifeForm :TransferLifeForm{

  public DandelionVerts dandelion;
 
  public override void Bind(){

    transfer.BindForm("_DandelionBuffer" , dandelion);
    transfer.BindInt("_VertsPerVert" , () => dandelion.vertsPerVert );

    Hair s = (Hair)skeleton;
    
    transfer.BindInt("_NumVertsPerHair" , () => s.numVertsPerHair );
    
  }



}
