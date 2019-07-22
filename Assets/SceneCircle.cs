using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCircle : Cycle
{

  public Body body;
  public Life life;

  public int rows;
  public int columns;

  public Vector3 set;

  public float inner;
  public float outer;

  private SceneCircleVerts verts;
  private SceneCircleVerts tris;
  
  public override void Create(){
   
    SafeInsert(body);
    SafeInsert(life);

    life.active = false;

    verts = (SceneCircleVerts)(body.verts);

  }

  public override void Bind(){
    life.BindPrimaryForm( "_VertBuffer" , verts);
    life.BindAttribute("_SetLocation", "set", this);
    life.BindAttribute("_InnerRadius", "inner", this);
    life.BindAttribute("_OuterRadius", "outer", this);

    data.land.BindData(life);
  }


  public void Set( Story story ){
    set = story.transform.position;
    inner = story.innerRadius;
    outer = story.outerRadius;

    body.mpb.SetFloat("_OuterRadius", story.outerRadius);
    body.mpb.SetFloat("_InnerRadius", story.innerRadius);
    body.mpb.SetFloat("_SetTime", Time.time);
    



    life.YOLO();
  }


}
