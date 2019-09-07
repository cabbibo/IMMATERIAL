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


      life.BindAttribute("_Size", "size", tile );
      life.BindAttribute("_Offset", "position", tile );
      hair.set.BindAttribute("_Offset" , "position" , tile);
      data.land.BindData(life);

    }

    public void Set(){

      life.YOLO();
      hair.Activate();
    }


}
