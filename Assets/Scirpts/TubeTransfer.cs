using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeTransfer : TransferLifeForm {
  
  
  public override void Bind(){
    transfer.BindAttribute( "_TubeLength" , "length" , verts );
    transfer.BindAttribute( "_TubeWidth" , "width" , verts );
    transfer.BindAttribute( "_NumVertsPerHair" , "numVertsPerHair" , skeleton );
  }

}