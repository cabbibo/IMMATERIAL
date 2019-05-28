using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextParticles : LifeForm{

  public Particles particles;
  public TextAnchor anchor;

  public Body body;

  public Life setAnchor;
  public Life setGlyph;
  public Life setPage;

  public Life simulate;
  public Life transfer;
  private float pageStart;

  public float radius;

  public float scale;

  public void HideShowParticles(bool val){
    body.active = val;
  }

  public override void _Create(){
    SafeInsert(particles);
    SafeInsert(setAnchor);
    SafeInsert(setGlyph);
    SafeInsert(setPage);
    SafeInsert(simulate);
    SafeInsert(transfer);
    SafeInsert(body);
    DoCreate();

  }

  public override void Bind(){

    setGlyph.BindPrimaryForm("_TransferBuffer",body.verts);
    setGlyph.BindForm("_AnchorBuffer",anchor);

    setAnchor.BindPrimaryForm("_VertBuffer",particles);
    setAnchor.BindForm("_AnchorBuffer",anchor);

    setPage.BindPrimaryForm("_VertBuffer",particles);

    simulate.BindPrimaryForm("_VertBuffer",particles);

    transfer.BindPrimaryForm("_TransferBuffer",body.verts);
    transfer.BindForm("_VertBuffer",particles);
    transfer.BindAttribute("_Radius","radius",this);//.BindForm("_VertBuffer",particles);
    transfer.BindAttribute("_Scale","scale",this);//.BindForm("_VertBuffer",particles);


   /* simulate.BindAttribute("_Active","pageActive",story);
    
    simulate.BindAttribute("_Up","up",story.ursula);
    simulate.BindAttribute("_CameraForward","forward",touch);
    simulate.BindAttribute("_CameraUp","up",touch);
    simulate.BindAttribute("_PageAlive","pageAlive",story);
    simulate.BindAttribute("_UrsulaPos","position" , story.ursula );
    
    simulate.BindAttribute("_Fade","fade" , story );

    simulate.BindAttribute("_RayOrigin", "RayOrigin",touch);
    simulate.BindAttribute("_RayDirection", "RayDirection",touch);
    simulate.BindAttribute("_Scale","scale",this);*/




  }

  public void Set(TextAnchor t){
    
    anchor = t;
    scale = t.scale;

    setGlyph.RebindForm("_AnchorBuffer",anchor);
    setAnchor.RebindForm("_AnchorBuffer",anchor);

    setAnchor.YOLO();
    setGlyph.YOLO();
  }

  public override void OnLive(){
    Set( anchor );
  }


  public void PageStart(){
    setPage.YOLO();
  }


}
