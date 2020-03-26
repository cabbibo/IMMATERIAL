using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandTile : Cycle
{

  public LandTiler tiler;


  public int dimensions;
  public float size;
  public Vector3 position;
  public Body body;
  public LandTileVerts verts;
  public LandTileTris  tris;
  
  public ReRender water;

  public EventTypes.BaseEvent OnSet;

  public HairBasic hairSet;


 
  public override void Create(){

    SafeInsert(body);
    SafeInsert(water);

//    print(verts);

    verts.dimensions = dimensions;
    verts.size = size;
    verts.position = position;

  }


   public void Set(){

    position = transform.position;
    
    tiler._Offset = transform.position;
    tiler.setTile.RebindPrimaryForm( "_VertBuffer" , verts );
    tiler.setTile.YOLO();

    OnSet.Invoke();
    
  }




  
}
