﻿using System.Collections;
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

public int currentMax;
public int currentMin;

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
    currentMin = 0;
    currentMax = 0;

  }

  public override void Bind(){

    setGlyph.BindForm("_TransferBuffer",body.verts);
    setGlyph.BindPrimaryForm("_AnchorBuffer",anchor);

    setAnchor.BindForm("_VertBuffer",particles);
    setAnchor.BindPrimaryForm("_AnchorBuffer",anchor);

    setPage.BindPrimaryForm("_VertBuffer",particles);

    simulate.BindPrimaryForm("_VertBuffer",particles);

    transfer.BindPrimaryForm("_TransferBuffer",body.verts);
    transfer.BindForm("_VertBuffer",particles);
    transfer.BindAttribute("_Radius","radius",this);//.BindForm("_VertBuffer",particles);
    transfer.BindAttribute("_Scale","scale",this);//.BindForm("_VertBuffer",particles);

    setGlyph.BindAttribute("_BaseID" , "currentMin" , this );
    setAnchor.BindAttribute("_BaseID" , "currentMin" , this );
    setPage.BindAttribute("_BaseID" , "currentMin" , this );
    simulate.BindAttribute("_BaseID" , "currentMin" , this );
    transfer.BindAttribute("_BaseID" , "currentMin" , this );


    setGlyph.BindAttribute(  "_TipID" , "currentMax" , this );
    setAnchor.BindAttribute( "_TipID" , "currentMax" , this );
    setPage.BindAttribute(   "_TipID" , "currentMax" , this );
    simulate.BindAttribute(  "_TipID" , "currentMax" , this );
    transfer.BindAttribute(  "_TipID" , "currentMax" , this );


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

    currentMin = currentMax;

    currentMax = currentMin + t.count; 
    print( currentMax );
    print( t.count );
    
    anchor = t;
    scale = t.scale;

    setGlyph.RebindPrimaryForm("_AnchorBuffer",anchor);
    setAnchor.RebindPrimaryForm("_AnchorBuffer",anchor);

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
