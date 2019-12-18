using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstancedMeshVerts : Form {

  public MeshVerts verts;
  public Form baseForm;
  public int numMesh;
  public int vertsPerMesh;

  public float countMultiplier;
  
  /*struct Vert{
    public Vector3 pos;
    public Vector3 nor;
    public Vector3 tan;
    public Vector2 uv;
    public float debug;
  };*/

  public override void SetStructSize(){ structSize = 16; }

  public override void SetCount(){ 

    vertsPerMesh = verts.count;
    if( baseForm != null ){ numMesh = (int)((float)baseForm.count * countMultiplier); }

    count = vertsPerMesh * numMesh;

  }

  public override void Embody(){


    float[] values = new float[count*structSize];
    float[] data = verts.GetData();

    int index = 0;
    for( int i = 0; i < vertsPerMesh; i ++ ){
      for(int j = 0; j < numMesh; j++ ){

        values[ index ++ ] = data[0+i *structSize];
        values[ index ++ ] = data[1+i *structSize];
        values[ index ++ ] = data[2+i *structSize];

        values[ index ++ ] = data[3+i *structSize];
        values[ index ++ ] = data[4+i *structSize];
        values[ index ++ ] = data[5+i *structSize];

        values[ index ++ ] = data[6+i *structSize];
        values[ index ++ ] = data[7+i *structSize];
        values[ index ++ ] = data[8+i *structSize];

        values[ index ++ ] = data[9+i *structSize];
        values[ index ++ ] = data[10+i *structSize];
        values[ index ++ ] = data[11+i *structSize];


        values[ index ++ ] = data[12+i *structSize];
        values[ index ++ ] = data[13+i *structSize];
        values[ index ++ ] = data[14+i *structSize];
        values[ index ++ ] = data[15+i *structSize];

      }
    }
    SetData( values );
  }
}