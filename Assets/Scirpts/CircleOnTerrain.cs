using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleOnTerrain : Cycle
{

  public Body body;
  public Life life;

  public int rows;
  public int columns;

  public Vector3 set;

  public float radius;
  public float height = 1;


  private SceneCircleVerts verts;
  private SceneCircleVerts tris;


  
  public override void Create(){
   
    SafeInsert(body);
    SafeInsert(life);


    verts = (SceneCircleVerts)(body.verts);

  }

  public override void Bind(){
    life.BindPrimaryForm( "_VertBuffer" , verts);
    life.BindAttribute("_SetLocation", "set", this);
    life.BindAttribute("_Radius", "radius", this);
    life.BindAttribute("_Height", "height", this);

    data.land.BindData(life);
  }

  public void Embody(){

  }

  public override void Activate(){

    body.active = true;
    body.mpb.SetFloat("_Radius", radius);
  }

  public override void Deactivate(){

    body.active = false;
  }

  public void Set(){

    //print("set");
    
    set = transform.position;


    life.YOLO();

  }




}
