using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTree :Cycle
{

    public Camera camera;

    public BuildTreeVerts verts;
    public BuildTreeInfo info;
    public BuildTreeConnections connections;

    public Life sim;
    public Life resolve;
    public Life closest;

    public override void Create(){
      SafeInsert(info);
      SafeInsert(verts);
      SafeInsert(connections);
      SafeInsert(sim);
      SafeInsert(resolve);
     // SafeInsert(closest);
      AddBinders();
    }

    public override void Bind(){
      sim.BindPrimaryForm("_VertBuffer",verts);
      sim.BindForm("_InfoBuffer",info);
      sim.BindForm("_ConnectionBuffer",connections);

      resolve.BindPrimaryForm("_VertBuffer",verts);
      resolve.BindForm("_ConnectionBuffer",connections);

     // closest.BindForm("_VertBuffer",verts);

    }

}
