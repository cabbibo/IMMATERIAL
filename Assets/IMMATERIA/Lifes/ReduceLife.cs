using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceLife : Life{
  
  public int numberGroups;
  public float[] values;
  public Vector4 value;
  public ComputeBuffer _buffer;
  public uint count;

  public Vector3 closest;
  public float closestID;
  
  public override void _Create(){
    
    /*
      Normal base class creation stuff!
    */
    
    DoCreate();
    boundForms = new Dictionary<string, Form>();
    boundInts = new Dictionary<string, int>();
    boundAttributes = new List<BoundAttribute>();

    boundIntList = new List<BoundInt>();
    boundFloatList = new List<BoundFloat>();
    boundFloatsList = new List<BoundFloats>();
    boundVector2List = new List<BoundVector2>();
    boundVector3List = new List<BoundVector3>();
    boundVector4List = new List<BoundVector4>();
    boundMatrixList = new List<BoundMatrix>();
    boundTextureList = new List<BoundTexture>();
    boundBufferList = new List<BoundBuffer>();
    FindKernel();
    GetNumThreads();
  
    count = numThreads;
     




  }


  public override void _SetInternal(){
    
    shader.SetFloat("_Time", Time.time);
    shader.SetFloat("_Delta", Time.deltaTime);
    shader.SetBuffer(kernel, "_OutBuffer" , _buffer );///, Time.deltaTime);
    shader.SetInt( "_OutBuffer_COUNT" , (int)count );///, Time.deltaTime);

  }

  public override void Bind(){
    GetNumGroups();
    _buffer = new ComputeBuffer((int)numGroups, 4 * sizeof(float));
    values = new float[numGroups*4];
    _buffer.SetData(values);
  }

  public override void AfterDispatch(){
    numberGroups = numGroups;
    _buffer.GetData(values);
//    float x = 0; float y = 0; float z = 0; float w = 0;

    closest = Vector3.one * 100000;
    closestID = -1;

    for( int i = 0; i < numGroups; i++ ){
      Vector3 v = new Vector3( values[i*4+0]
                             , values[i*4+1]
                             , values[i*4+2] );

      float id = values[i*4+3];

      if( v.magnitude < closest.magnitude ){
        closest = v;
        closestID = id;
      }
    }

    value = new Vector4( closest.x , closest.y , closest.z , closestID );
  
  }

}


