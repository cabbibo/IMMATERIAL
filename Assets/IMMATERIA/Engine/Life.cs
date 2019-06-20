using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public class Life : Cycle {

  [HideInInspector] public string primaryName;
  [HideInInspector] public Form primaryForm;
  
  public ComputeShader shader;
  public string kernelName;
  public int countMultiplier = 1;

  [HideInInspector] public int kernel;
  [HideInInspector] public float executionTime;

  public Dictionary<string, Form> boundForms;
  public Dictionary<string, int> boundInts;

  protected bool allBuffersSet;
  protected int numGroups;
  protected uint numThreads;

  public struct BoundAttribute {
    public string nameInShader;
    public string attributeName;
    public FieldInfo info;
    public System.Object boundObject;
  }

  public List<BoundAttribute> boundAttributes;


  public delegate void SetValues(ComputeShader shader, int kernel);
  public event SetValues OnSetValues;

  public override void _Create(){
    DoCreate();
    boundForms = new Dictionary<string, Form>();
    boundInts = new Dictionary<string, int>();
    boundAttributes = new List<BoundAttribute>();
    FindKernel();
    GetNumThreads();
  }

  public virtual void FindKernel(){
    kernel = shader.FindKernel( kernelName );
  }

  public virtual void GetNumThreads(){
    uint y; uint z;
    shader.GetKernelThreadGroupSizes(kernel, out numThreads , out y, out z);
  }

  public virtual void GetNumGroups(){
    numGroups = (primaryForm.count*countMultiplier+((int)numThreads-1))/(int)numThreads;
  }
 
  public void BindForm( string name , Form form ){
    boundForms.Add( name ,form );
  }

  public void RebindForm(string name , Form form ){
  if( boundForms[name] ){
    boundForms[name] = form;
  }else{
    print("BORKEN no form reset");
  }
  }

    public void RebindPrimaryForm(string name , Form form ){

      if( primaryForm ){

          primaryForm = form;
          primaryName = name;
    }else{

      print("no primary form to reset");
    }
  }

   public void BindInt( string name , int form ){
    boundInts.Add( name ,form );
  }

  public void BindPrimaryForm(string name , Form form){
    primaryForm = form;
    primaryName = name;
  }

  public override void _WhileLiving( float v ){
    if( active ){ Live(); }
  }

  public void Live(){

    if( OnSetValues != null ){ OnSetValues(shader,kernel); }
    
    GetNumGroups();
    SetShaderValues();
    BindAttributes();

    // set this true every frame, 
    // and allow each buffer to make 
    // untrue as needed
    allBuffersSet = true;

    _SetInternal();

    shader.SetInt("_Count", primaryForm.count );


    foreach(KeyValuePair<string,Form> form in boundForms){
      if( debug == true ){ print("Bound Form : " + form.Key );}
      SetBuffer(form.Key , form.Value);
    }

    if( debug == true ){ print("Bound Form : " + primaryName );}
    SetBuffer( primaryName , primaryForm );

    foreach(KeyValuePair<string,int> form in boundInts){
      shader.SetInt(form.Key , form.Value);
    }


    // if its still true than we can dispatch
    if ( allBuffersSet ){
      if( debug ) print( "name : " + kernelName + " Num groups : " + numGroups );
      shader.Dispatch( kernel,numGroups ,1,1);
    }

    AfterDispatch();

  }

  public virtual void _SetInternal(){    
    shader.SetFloat("_Time", Time.time);
    shader.SetFloat("_Delta", Time.deltaTime);
  }


  public virtual void AfterDispatch(){}
  public virtual void SetShaderValues(){}

  private void SetBuffer(string name , Form form){
    //print(form);
    //print(name);


    if( form != null){


      if( form._buffer != null ){
        shader.SetBuffer( kernel , name , form._buffer);
        shader.SetInt(name+"_COUNT" , form.count );
        if( debug == true ){ print("Bound Form : " + form.gameObject );}
      }else{
        allBuffersSet = false;
        print("YOUR BUFFER : " + name +  " IS NULL!");
      }
    }else{
      print("WAHT YR FORM IS NULL");
      print( name );
    }
  }

  public void BindAttribute( string nameInShader, string attributeName , System.Object obj ){
    BoundAttribute a = new BoundAttribute();
    a.nameInShader = nameInShader;
    a.attributeName = attributeName;
    a.boundObject = obj;
    a.info = obj.GetType().GetField(attributeName);

    bool replaced = false;
    int id = 0;
    foreach( BoundAttribute ba in boundAttributes){

      if( ba.nameInShader == nameInShader ){
        boundAttributes[id] = a;
        DebugThis( ba.nameInShader + " is being rebound" );
        replaced = true;
        break;
      }

      id ++;
    }

    if( replaced == false ){  boundAttributes.Add(a); }
  }

  public void BindAttributes(){
    foreach(  BoundAttribute b in boundAttributes ){


      string s = "";
      if( debug == true ){
        s +="UNIFORM : "  + b.nameInShader;
      }

      if( b.info == null ){
        print("------------------------------");
        print("THIS ATTRIBUTE DOESN'T EXIST: ");
        print("Bound Object: " + b.boundObject);
        print("Attribute Name: " + b.attributeName);
        print("Name In Shader: " + b.nameInShader);
        print("I AM ON GAME OBJECT: " + this);
        print("------------------------------");
      }

      if( b.info.FieldType == typeof(float) ){
        float value = (float)b.info.GetValue(b.boundObject);
        if( debug == true ){ print( s + " || VALUE : " + value);}
        shader.SetFloat(b.nameInShader,value);
      }else if(b.info.FieldType == typeof(float[]) ){
        float[] value = (float[])b.info.GetValue(b.boundObject);
        if( debug == true ){ print( s + " || VALUE : " + value);}
        shader.SetFloats(b.nameInShader,value);
      }else if( b.info.FieldType == typeof(int) ){
        int value = (int)b.info.GetValue(b.boundObject);
        if( debug == true ){ print( s + " || VALUE : " + value);}
        shader.SetInt(b.nameInShader,value);
      }else if( b.info.FieldType == typeof(Vector3)){
        Vector3 value = (Vector3)b.info.GetValue(b.boundObject);
        if( debug == true ){ print( s + " || VALUE : " + value);}
        shader.SetVector(b.nameInShader,value);
      }else if( b.info.FieldType == typeof(Vector4)){
        Vector4 value = (Vector4)b.info.GetValue(b.boundObject);
        if( debug == true ){ print( s + " || VALUE : " + value);}
        shader.SetVector(b.nameInShader,value);
      }else if( b.info.FieldType == typeof(Matrix4x4)){
        Matrix4x4 value = (Matrix4x4)b.info.GetValue(b.boundObject);
        if( debug == true ){ print( s + " || VALUE : " + value);}
        shader.SetMatrix(b.nameInShader,value);
      }else if( b.info.FieldType == typeof(Texture) ){
        Texture value = (Texture)b.info.GetValue(b.boundObject);
        if( debug == true ){ print( s + " || VALUE : " + value);}
        shader.SetTexture(kernel,b.nameInShader,value);
      }else if( b.info.FieldType == typeof(Texture2D) ){
        Texture2D value = (Texture2D)b.info.GetValue(b.boundObject);
        if( debug == true ){ print( s + " || VALUE : " + value);}
        shader.SetTexture(kernel,b.nameInShader,value);
      }else if(b.info.FieldType == typeof(ComputeBuffer) ){
        ComputeBuffer value = (ComputeBuffer)b.info.GetValue(b.boundObject);
        if( debug == true ){ print( s + " || VALUE : " + value);}
        shader.SetBuffer(kernel,b.nameInShader,value);
      }

    }
  }

  public void YOLO(){
    active = true;
    _WhileLiving(1);
    active = false;
  }

}
