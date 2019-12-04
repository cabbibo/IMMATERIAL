using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : Simulation
{
   
    public LandTile tile;
    public HairBasic hair;

    public override void Create(){
      SafeInsert( hair );
    }

    public override void Bind(){


      life.BindFloat("_Size", () => tile.size );
      life.BindVector3("_Offset", () => tile.position );
      hair.set.BindVector3("_Offset" , () => tile.position );
      data.land.BindData(life);

    }

    public void Set(){

      hair.set.YOLO();
      life.YOLO();
    }


}
