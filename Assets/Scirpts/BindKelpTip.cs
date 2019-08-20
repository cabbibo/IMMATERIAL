﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindKelpTip :Binder
{

  public Hair hair ;
  public TransformBuffer pullers;


  public override void Bind(){

    data.land.BindData( toBind );
    data.BindPlayerData( toBind );
    data.BindCameraData( toBind );


    toBind.BindAttribute( "_VertsPerHair" , "numVertsPerHair" , hair );
    toBind.BindForm( "_HairBuffer" ,hair );
    toBind.BindForm( "_PullBuffer" ,pullers );


  } 


}