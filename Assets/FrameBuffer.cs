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
    public InstanceTransfer corners;

    public int locked;
    public float deathTime;
    public float distance;


    public Page currentPage;

    private Vector3 topLeft;
    private Vector3 topRight;
    private Vector3 bottomLeft;
    private Vector3 bottomRight;

    public override void Create(){

      particles.size = size;
      verts.size = smoothedSize;
      ((InstancedMeshVerts)corners.verts).countMultiplier = 1/(float)size;

      SafeInsert(particles);
      SafeInsert(transfer);
      SafeInsert(set);
      SafeInsert(simulate);
      SafeInsert(corners);

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
      simulate.BindFloat( "_Distance" , () => distance );
      simulate.BindFloat( "_CanEdgeSwipe" , () => data.inputEvents.canEdgeSwipe );
      

      data.BindAllData(simulate);
      data.BindAllData(transfer.transfer);

      transfer.transfer.BindInt("_NumVerts", () => this.size );
      transfer.transfer.BindInt("_NumSmoothedVerts", () => this.smoothedSize );

      transfer.transfer.BindFloat( "_DeathTime" , () => deathTime );
      transfer.transfer.BindInt( "_Locked" , () => locked );
      transfer.transfer.BindFloat( "_Distance" , () => distance );
      transfer.transfer.BindFloat( "_CanEdgeSwipe" , () => data.inputEvents.canEdgeSwipe );

      transfer.transfer.BindFloat( "_Fade" , () => currentPage.fade );



      corners.transfer.BindFloat( "_Distance" , () => distance );
      corners.transfer.BindFloat( "_Fade" , () => currentPage.fade );


    }


    public void Set(Page page){

      topLeft     = page.frame.topLeft;
      bottomLeft  = page.frame.bottomLeft;
      topRight    = page.frame.topRight;
      bottomRight = page.frame.bottomRight;

      distance = page.frame.distance;

      transfer.body.mpb = page.frameMPB;
      currentPage = page;

      set.Live();
      locked = 1;
    }

    public void Release(){
      locked = 0;
      deathTime = Time.time;
    }


}
