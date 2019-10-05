using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedMeshes : Cycle
{

  public GameObject[] meshes;
  public Form[] verts;
  public IndexForm[] tris;

  public override void Create(){

    verts = new Form[meshes.Length];
    tris = new IndexForm[meshes.Length];
    
    for( int i = 0; i < meshes.Length; i++ ){
      verts[i] = meshes[i].GetComponent<Form>();
      tris[i] = meshes[i].GetComponent<IndexForm>();

      SafeInsert(verts[i]);
      SafeInsert(tris[i]);
    }

  }


}
