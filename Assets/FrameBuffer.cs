using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameBuffer : Cycle
{
    public int size;

    public int smoothedSize;

    public FrameParticles particles;

    public FrameVerts verts;
    public FrameTris tris;

    public Life set;
    public Life simulate;
    public Life transfer;

    private Vector3 topLeft;
    private Vector3 topRight;
    private Vector3 bottomLeft;
    private Vector3 bottomRight;

    public override void Create(){

      particles.size = size;
      verts.size = smoothedSize;

      SafeInsert(particles);
      SafeInsert(verts);
      SafeInsert(tris);
      SafeInsert(set);
      SafeInsert(simulate);
      SafeInsert(transfer);

    }

    public override void Bind(){
     
      set.BindInt( "_Size" , () => size );

      set.BindVector3( "_TopLeft"     , () => this.topLeft      );
      set.BindVector3( "_TopRight"    , () => this.topRight     );
      set.BindVector3( "_BottomLeft"  , () => this.bottomLeft   );
      set.BindVector3( "_BottomRight" , () => this.bottomRight  );


    }


    public void Set(Frame frame){

      topLeft     = frame.topLeft;
      bottomLeft  = frame.bottomLeft;
      topRight    = frame.topRight;
      bottomRight = frame.bottomRight;

      set.Live();

    }


}
