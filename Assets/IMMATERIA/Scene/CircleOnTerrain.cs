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

    verts.rows = rows;
    verts.cols = columns;

  }

  public override void Bind(){
    life.BindPrimaryForm( "_VertBuffer" , verts);
    life.BindVector3( "_SetLocation" , () => this.set );
    life.BindFloat("_Radius", () => this.radius );
    life.BindFloat("_Height", () => this.height );

    data.land.BindData(life);
  }

  public void Embody(){

  }

  public override void Activate(){
    
    //Debug.Log("no body for some reason" , this.gameObject );
    body.active = true;
    body.mpb.SetFloat("_Radius", radius);
  
    Set();
    
  }

  public override void Deactivate(){

    //DebugThis("DEACTIVATEING");

    body.active = false;
  }

  public void Set(){

    set = transform.position;
    life.active = true;

  }




}
