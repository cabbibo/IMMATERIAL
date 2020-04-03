using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameBuffer : Cycle
{
    public int size;

    public int smoothedSize;


    public ToggleFrame closeButton;

    public FrameParticles particles;
    public FrameVerts verts;
    public FrameTris tris;

    public Life set;
    public Life simulate;
    public ClosestLife checkClosest;

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

    public SampleSynth instrument;
    public int closestID;
    
    public override void Create(){

      particles.size = size;
      verts.size = smoothedSize;
      ((InstancedMeshVerts)corners.verts).countMultiplier = 1/(float)size;

      SafeInsert(particles);
      SafeInsert(transfer);
      SafeInsert(set);
      SafeInsert(simulate);
      SafeInsert(corners);
      SafeInsert(checkClosest);
      SafeInsert(closeButton);

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
      simulate.BindInt("_ClosestID" , () => (int)checkClosest.closestID );
      

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

      corners.transfer.BindInt( "_Locked" , () => locked );
      corners.transfer.BindFloat( "_DeathTime" , () => deathTime );



      checkClosest.Set( particles );
      data.BindAllData(checkClosest);


    }


    public void Set(Page page){

      //print("Page FRame Set :" + gameObject);
      topLeft     = page.frame.topLeft;
      bottomLeft  = page.frame.bottomLeft;
      topRight    = page.frame.topRight;
      bottomRight = page.frame.bottomRight;

      closeButton.transform.position = page.frame.bottomLeft;
      closeButton.transform.position += page.frame.right * .5f * page.frame.width ;
      closeButton.transform.position += -page.frame.up * .05f * page.frame.distance;
      closeButton.transform.LookAt( closeButton.transform.position + page.transform.forward );
      closeButton.transform.localScale = new Vector3(3,1,1) * .02f * page.frame.distance;

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

    public void ImmediateDeath(){
      locked = 0;
      deathTime = Time.time-100;
    }




}
