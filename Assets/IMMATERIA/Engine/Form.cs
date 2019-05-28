using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Form : Cycle {


  public int count;

  [HideInInspector] public bool intBuffer;

  [HideInInspector] public int structSize;
  [HideInInspector] public ComputeBuffer _buffer;

//  [HideInInspector] public string name;
  [HideInInspector] public string description;
  [HideInInspector] public float timeToCreate;
  [HideInInspector] public int totalMemory;  

  public Material debugMaterial;

  public override void _Create(){
    DoCreate();
    SetStructSize();
    SetCount();
    SetBufferType();
    Create();
  }

  public override void _OnGestate(){ 
    DoGestate();
    _buffer = MakeBuffer();
    Embody();
  }

  
  public virtual void Embody(){}
  public virtual void SetCount(){}
  public virtual void SetStructSize(){}
  public virtual void SetBufferType(){}

  public override void _Destroy(){
    DoDestroy();
    ReleaseBuffer();
  }

  public int[] GetIntData(){
    int[] val = new int[count];
    GetData(val);
    return val;
  }

  public float[] GetFloatData(){
    float[] val = new float[count*structSize];
    GetData(val);
    return val;
  }

  public float[] GetData(){
    return GetFloatData();
  }

  public void GetData( int[] values ){ _buffer.GetData(values); }
  public void GetData( float[] values ){ _buffer.GetData(values); }

  public void SetData( float[] values ){ _buffer.SetData( values );}
  public void SetData( int[] values ){ _buffer.SetData( values ); }

  public ComputeBuffer MakeBuffer(){

    if( intBuffer == true ){
      return new ComputeBuffer( count, sizeof(int) * structSize );
    }else{
      return new ComputeBuffer( count, sizeof(float) * structSize );
    }
  }

  public virtual int[] GetIntDNA(){
    return GetIntData();
  }

  public virtual float[] GetDNA(){
    return GetData();
  }


  public virtual void SetDNA(int[] dna){
    SetData(dna);
  }

  public virtual void SetDNA(float[] dna){
    SetData(dna);
  }

  public void ReleaseBuffer(){
   if(_buffer != null){ _buffer.Release(); }
  }

  public override void WhileDebug(){
    debugMaterial.SetPass(0);
    debugMaterial.SetBuffer("_VertBuffer", _buffer);
    debugMaterial.SetInt("_Count",count);
    Graphics.DrawProceduralNow(MeshTopology.Triangles, count * 3 * 2 );
  }

}