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

    public TransferLifeForm transfer;

    public int locked;
    public float deathTime;

    private Vector3 topLeft;
    private Vector3 topRight;
    private Vector3 bottomLeft;
    private Vector3 bottomRight;

    public override void Create(){

      particles.size = size;
      verts.size = smoothedSize;

      SafeInsert(particles);
      SafeInsert(transfer);
      SafeInsert(set);
      SafeInsert(simulate);

    }

    public override void Bind(){

      set.BindPrimaryForm( "_VertBuffer", particles);
     
      set.BindInt( "_Size" , () => size );

      set.BindVector3( "_TopLeft"     , () => this.topLeft      );
      set.BindVector3( "_TopRight"    , () => this.topRight     );
      set.BindVector3( "_BottomLeft"  , () => this.bottomLeft   );
      set.BindVector3( "_BottomRight" , () => this.bottomRight  );


      simulate.BindPrimaryForm( "_VertBuffer", particles);
      simulate.BindInt( "_Locked" , () => locked );
      simulate.BindFloat( "_DeathTime" , () => deathTime );
      data.BindAllData(simulate);

      data.BindAllData(transfer.transfer);

      transfer.transfer.BindInt("_NumVerts", () => this.size );
      transfer.transfer.BindInt("_NumSmoothedVerts", () => this.smoothedSize );

      transfer.transfer.BindFloat( "_DeathTime" , () => deathTime );
    transfer.transfer.BindInt( "_Locked" , () => locked );


    }


    public void Set(Frame frame){

      topLeft     = frame.topLeft;
      bottomLeft  = frame.bottomLeft;
      topRight    = frame.topRight;
      bottomRight = frame.bottomRight;

      set.Live();
      locked = 1;
    }

    public void Release(){
      locked = 0;
      deathTime = Time.time;
    }


}
