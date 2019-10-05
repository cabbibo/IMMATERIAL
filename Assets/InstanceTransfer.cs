using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceTransfer : TransferLifeForm
{

  public Form baseVerts;

  public override void Bind(){
    transfer.BindForm("_BaseBuffer", baseVerts); 
    transfer.BindInt("_VertsPerMesh" , () => baseVerts.count );
    transfer.BindFloat("_CountMultiplier" , () => ((InstancedMeshVerts)verts).countMultiplier );
  }



}
