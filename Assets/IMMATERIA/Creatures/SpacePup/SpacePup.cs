using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePup : Cycle
{

  public TriConnectedParticles particles;
  public Life life;

  public Life triLocation;
  public Life resolve;

  public MeshVerts verts;
  public MeshTris tris;

  public Body body;
  public PlaceDynamicMeshParticles anchors;

  public Transform target;

  public Vector3 velocity;
  public Vector3 force;

  public BindTransform transformBinder;

  public override void Create(){

    particles.count = verts.meshFilter.sharedMesh.vertices.Length;

    particles.verts = verts;
    particles.tris = tris;
    anchors.meshVerts = verts;
    body.triangles = tris;

    SafeInsert( life );
    SafeInsert( particles );

    SafeInsert(triLocation);
    SafeInsert( resolve );

    SafeInsert( body ); 
    SafeInsert( anchors );
    SafeInsert( transformBinder );

  }

  public override void Bind(){

    life.BindPrimaryForm( "_ParticleBuffer" , particles );
    life.BindForm("_VertBuffer" , verts );
    
    life.BindVector3("_Velocity" , () => this.velocity );


    triLocation.BindPrimaryForm( "_ParticleBuffer" , particles );
    triLocation.BindForm("_VertBuffer" , verts );

    resolve.BindPrimaryForm( "_ParticleBuffer" , particles );


    data.BindRayData( life );
  }

  public override void WhileLiving( float v ){

    force = Vector3.zero;

    force += (target.position - transform.position) * 1;

    velocity += force;

    velocity  *= .9f;

    transform.position += velocity;



  }



}
