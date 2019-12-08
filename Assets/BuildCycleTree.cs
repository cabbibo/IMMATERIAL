using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCycleTree :Cycle
{

    public BuildCycleVerts verts;
    public BuildCycleConnections connections;

    public Life sim;

    public override void Create(){
      SafeInsert(verts);
      SafeInsert(connections);
      SafeInsert(sim);
    }

    public override void Bind(){
      sim.BindPrimaryForm("_VertBuffer",verts);
      sim.BindForm("_ConnectionBuffer",connections);
    }

}
