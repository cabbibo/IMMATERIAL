using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionScene : Cycle
{

  public MeshVerts  planeVerts;
  public MeshTris   planeTris;

  public Dandelion dandelions;


  public Life SetPlane;
  public Life SetDandelionsOnPlane;


  public override void Create(){

    SafeInsert( SetPlane );
    SafeInsert( SetDandelionsOnPlane );
  }


  public override void Bind(){

  }

  public void SceneStart(){



    SetPlane.YOLO();
    SetDandelionsOnPlane.YOLO();

  }

  public void SceneEnd(){

  }

}
