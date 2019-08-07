using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformBuffer : Form
{

  public Transform[] transforms;
  public bool dynamic;
  [HideInInspector]public float[] values;
  private float[] tmpVals;

  public override void SetStructSize(){ structSize = 16; }
  public override void SetCount(){ count = transforms.Length; }

  public override void Embody(){
    values = new float[ count * structSize ];
    tmpVals = new float[ 16 ];
    SetInfo();
  }
  public void SetInfo(){
  
    for( int i = 0; i < transforms.Length; i++ ){
      tmpVals = HELP.GetMatrixFloats(transforms[i].worldToLocalMatrix);
      for( int j = 0; j < 16; j++ ){
        values[i * 16 + j ] = tmpVals[j];
      }
    }

    SetData(values);
  }


  public override void WhileLiving( float v ){

    if( dynamic ){
      print("setting");
      SetInfo();
    }
  }
}
