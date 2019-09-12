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
  private SceneCircleTris tris;

  public float fadeTime;
  private float setTime;
  private float setting;
  
  public override void Create(){
   
    SafeInsert(body);
    SafeInsert(life);

    life.active = false;

    verts = (SceneCircleVerts)(body.verts);

  }

  public override void Bind(){
    life.BindPrimaryForm( "_VertBuffer" , verts);
    
    life.BindVector3("_SetLocation"   , () => this.set    );
    life.BindFloat("_InnerRadius"     , () => this.inner  );
    life.BindFloat("_OuterRadius"     , () => this.outer  );

    data.land.BindData(life);
  }


  public void Set( PerimeterChecker story ){
    set = story.transform.position;
    inner = story.innerRadius;
    outer = story.outerRadius;

    setTime = Time.time;
    setting = 1;

    body.active = true;
    body.mpb.SetFloat("_OuterRadius", story.outerRadius);
    body.mpb.SetFloat("_InnerRadius", story.innerRadius);
    body.mpb.SetFloat("_SetTime", setTime);
    body.mpb.SetFloat("_Setting", setting);

    body.mpb.SetFloat("_FadeTime", fadeTime);

    life.YOLO();

  }


  public void Unset(PerimeterChecker story ){

    setTime = Time.time;
    setting = 0;
    body.mpb.SetFloat("_SetTime", setTime);
    body.mpb.SetFloat("_Setting", setting);
  }

  public override void WhileLiving( float v ){
    if( Time.time - setTime > fadeTime && setting == 0 ){
      body.active = false;
    }
  }


}
