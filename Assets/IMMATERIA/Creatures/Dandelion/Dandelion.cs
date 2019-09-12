
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dandelion : LifeForm {

  public float armLength;
  public Life force;
  public Life constrain;
  public Hair skeleton;
  public DandelionVerts verts;

  public override void Create(){

    SafeInsert( force     );
    SafeInsert( constrain );
    SafeInsert( verts     );

  }

  public override void Bind(){

    force.BindPrimaryForm("_VertBuffer", verts );
    force.BindForm("_SkeletonBuffer", skeleton );

    force.BindInt( "_NumVertsPerHair" , () => skeleton.numVertsPerHair );
    force.BindInt( "_NumHairs" , () => skeleton.numHairs );
    force.BindInt( "_VertsPerVert" , () => verts.vertsPerVert );

    constrain.BindPrimaryForm("_VertBuffer", verts );
    constrain.BindForm("_SkeletonBuffer", skeleton );

    constrain.BindInt(   "_NumVertsPerHair"  , () => skeleton.numVertsPerHair  );
    constrain.BindInt(   "_NumHairs"         , () => skeleton.numHairs         );
    constrain.BindInt(   "_VertsPerVert"     , () => verts.vertsPerVert        );
    constrain.BindFloat( "_Length"           , () => this.armLength            );

  }


}