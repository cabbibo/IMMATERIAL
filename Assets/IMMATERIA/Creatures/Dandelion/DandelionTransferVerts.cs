using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionTransferVerts: Form {

  public DandelionVerts verts;
  public Hair hair;
  public override void SetStructSize(){ structSize = 16; }
  public override void SetCount(){ count = verts.count * 7; }

}



