﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePup : Cycle
{

  public Particles particles;
  public Life life;

  public MeshVerts verts;
  public MeshTris tris;

  public Body body;
  public Simulation anchors;

  public Transform target;

  public Vector3 velocity;
  public Vector3 force;

  public override void Create(){

    particles.count = verts.meshFilter.sharedMesh.vertices.Length;

    SafeInsert( verts );
    SafeInsert( life );
    SafeInsert( body ); // particles and tris added to body
    SafeInsert( anchors );

  }

  public override void Bind(){

    life.BindPrimaryForm( "_ParticleBuffer" , particles );
    life.BindForm("_VertBuffer" , verts );
    life.BindAttribute("_Velocity" , "velocity" , this );

    data.BindRayData( life );
  }

  public override void WhileLiving( float v ){

    force = Vector3.zero;

    force += (target.position - transform.position);

    velocity += force;

    velocity  *= .9f;

    transform.position += velocity;



  }



}
