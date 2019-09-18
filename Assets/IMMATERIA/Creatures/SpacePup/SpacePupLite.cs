using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePupLite : Cycle
{

  public Particles particles;
  public Life life;

  public Life triLocation;
  public Life resolve;

  public MeshVerts verts;
  public MeshTris tris;

  public Body body;
  public Simulation anchors;

  public Transform target;

  public Vector3 velocity;
  public Vector3 force;

  public BindTransform transformBinder;

  public override void Create(){

    particles.count = verts.meshFilter.sharedMesh.vertices.Length;

    SafeInsert( verts );
    SafeInsert( life );

    SafeInsert(triLocation);
    SafeInsert( resolve );

    SafeInsert( body ); // particles and tris added to body
    //SafeInsert( anchors );
    SafeInsert( transformBinder );

  }

  public override void Bind(){

    life.BindPrimaryForm( "_ParticleBuffer" , particles );
    life.BindForm("_VertBuffer" , verts );
    
    life.BindVector3("_Velocity" , () => this.velocity );


    triLocation.BindPrimaryForm( "_ParticleBuffer" , particles );
    triLocation.BindForm("_VertBuffer" , verts );

    resolve.BindPrimaryForm( "_ParticleBuffer" , particles );


    //data.BindRayData( life );
  }

  public override void WhileLiving( float v ){

    force = Vector3.zero;

    force += (target.position - transform.position);

    velocity += force;

    velocity  *= .9f;

    transform.position += velocity;



  }





}
