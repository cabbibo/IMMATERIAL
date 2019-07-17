﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LandTileTris : IndexForm {

  [ HideInInspector ] public int width;
  [ HideInInspector ] public int length;
  [ HideInInspector ] public int numTubes;
  public LandTile tile;

  public override void SetCount(){
    toIndex = tile;
    count = (tile.dimensions-1) * (tile.dimensions-1)  * 3 * 2;
  }

  public override void Embody(){

    int[] values = new int[count];
    int index = 0;
    for( int i = 0; i < tile.dimensions-1; i++ ){
    for( int j = 0; j < tile.dimensions-1; j++   ){
        int id1 = i * tile.dimensions + j + 0 ;
        int id2 = i * tile.dimensions + j + 1 ;
        int id3 = i * tile.dimensions + j + tile.dimensions;
        int id4 = i * tile.dimensions + j + tile.dimensions + 1;

        values[ index ++ ] = id1;
        values[ index ++ ] = id4;
        values[ index ++ ] = id2;
        values[ index ++ ] = id1;
        values[ index ++ ] = id3;
        values[ index ++ ] = id4;

    }
    }
    SetData(values);
  }

}
