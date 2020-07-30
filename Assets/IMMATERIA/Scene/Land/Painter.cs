using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;


[ExecuteInEditMode]
public class Painter : Simulation
{
  
 [SerializeField]public string[] options;
 public int brushType;




  public string terrainPath;


  public bool painting;

  public PaintVerts verts;
  public PaintTris tris;

  // how many undos we get!
  public int undoBufferSize;
  public int currentUndoLocation;

  public List<float[]> undoBuffer;

  public Material windDebugMat;
  public Material grassDebugMat;
  public Material planeDebugMat;



  public bool drawPlane;
  public bool drawGrass;
  public bool drawWind;

  public string safeName;


  //Texture taht informs the start
  public Texture startTexture;
  public Texture undoTexture;


  // getting position and direction
  public Vector3 paintPosition;
  private Vector3 oPP;
  public Vector3 paintDirection;

  // brush size
  public float paintSize;
  public float paintOpacity;

  // Which object we are painting
  public float normalOrHeight;


  private Color[] colors;
  private float[] values;


  public float reset;

  public Transform paintTip;

  private MaterialPropertyBlock mpb;




  // to get our data back from gpu
  Queue<AsyncGPUReadbackRequest> _requests = new Queue<AsyncGPUReadbackRequest>();

  public override void Create(){


    if( mpb == null ){ mpb = new MaterialPropertyBlock(); }

    SafeInsert( tris );

    if( undoBuffer == null ){ 
      undoBuffer = new List<float[]>(); 
    }
  
    if( undoTexture == null ){
      undoTexture = new Texture2D(startTexture.width, startTexture.height);
      Graphics.CopyTexture(startTexture, undoTexture);
    }

  }




  // Only Recreate if its not the correct size;
  public override void OnGestated(){


    Load();

    ExtractColors();
    UpdateLand();

    if( undoBuffer.Count != undoBufferSize ){
      undoBuffer = new List<float[]>(undoBufferSize);

      for(int i = 0; i < undoBufferSize;i++ ){
        undoBuffer.Add(values);
      }
    }
  
  }
  


  // binding all our information!
  public override void Bind(){

    life.BindPrimaryForm("_VectorBuffer", verts);

    life.BindVector3( "_PaintPosition"  , () => this.paintPosition  );
    life.BindVector3( "_PaintDirection" , () => this.paintDirection );
    life.BindFloat(   "_PaintSize"      , () => this.paintSize      );
    life.BindFloat(   "_PaintOpacity"   , () => this.paintOpacity   );
    life.BindInt(     "_Brush"          , () => this.brushType      );
    life.BindFloat(   "_Reset"          , () => this.reset          );
    life.BindTexture( "_TextureReset"   , () => this.startTexture   );
    life.BindTexture( "_UndoTexture"    , () => this.undoTexture    );

    data.BindLandData(life);

  
  }

  public override void WhileDebug(){
    
    mpb.SetInt("_Dimensions", verts.width);
    mpb.SetInt("_Count", verts.count);
    mpb.SetBuffer("_VertBuffer", verts._buffer);
    if( drawPlane ){
      Graphics.DrawProcedural(planeDebugMat,  new Bounds(transform.position, Vector3.one * 5000), MeshTopology.Triangles, tris.count , 1, null, mpb, ShadowCastingMode.Off, true, LayerMask.NameToLayer("Debug"));
    }


    if( drawGrass ){
       Graphics.DrawProcedural(grassDebugMat,  new Bounds(transform.position, Vector3.one * 5000), MeshTopology.Triangles, verts.count * 3 , 1, null, mpb, ShadowCastingMode.Off, true, LayerMask.NameToLayer("Debug"));
   
    }


    if( drawWind ){
      Graphics.DrawProcedural(windDebugMat,  new Bounds(transform.position, Vector3.one * 5000), MeshTopology.Triangles, verts.count * 3 , 1, null, mpb, ShadowCastingMode.Off, true, LayerMask.NameToLayer("Debug"));
   
    }


  }



  public void WhileDown(Ray ray){

    print("hiii");
    
    paintDirection = paintPosition;
    paintPosition = data.land.Trace( ray.origin, ray.direction);
    paintTip.position = paintPosition;
    paintTip.localScale = new Vector3( paintSize, paintSize,paintSize);

    paintDirection = -(paintDirection - paintPosition).normalized;

    // update our life
    life.YOLO();

  }


public void ResetToOriginal(){
  Load();
}


public void ResetToUndo(){
  reset = 3;
  life.YOLO();
  reset = 0;
}



public void ResetToFlat(){
  reset = 1;
  life.YOLO();
  reset = 0;
}


  public void ExtractColors(){
   
    values = verts.GetData();

    colors =  new Color[verts.count];
    for( int i = 0; i < verts.count; i ++ ){
      
      // extracting height
      float h = values[ i * verts.structSize + 1 ] / data.land.height;
      
      // extracting flow verts
      float x = values[ i * verts.structSize + 6 ] * .5f + .5f;
      float z = values[ i * verts.structSize + 8 ] * .5f + .5f;


      float a = Mathf.Clamp( values[ i * verts.structSize + 11 ], .1f , .9999f);

      colors[i] = new Color( h,x,z,a);

    }

  }


  public void ExtractColors( float[] v){
   
    //int count = values.Length / verts.structSize;

    values = v;
    colors =  new Color[verts.count];
    for( int i = 0; i < verts.count; i ++ ){
      
      // extracting height
      float h = values[ i * verts.structSize + 1 ] / data.land.height;
      
      // extracting flow verts
      float x = values[ i * verts.structSize + 6 ] * .5f + .5f;
      float z = values[ i * verts.structSize + 8 ] * .5f + .5f;

      // extracting grass height
      float a = Mathf.Clamp( values[ i * verts.structSize + 11 ], .1f , .9999f);

      colors[i] = new Color( h,x,z,a);

    }

  }
   public void Save(){

    ExtractColors();
    propogateUndoBuffer();
    UpdateLand();

  }

  public void UltraSave(){

    print("HELLLOSOS ULTA");

    ExtractColors();
    propogateUndoBuffer();
    UpdateLand();

    string path = "StreamingAssets/Terrain/safe";
    Saveable.Save( verts , path );

    SaveTextureAsPNG( data.land.heightMap , Application.dataPath+"/" + path );
  
  }

  public void Load(){
    string path = "StreamingAssets/Terrain/safe";
    Saveable.Load( verts , path );
  }



  public void propogateUndoBuffer(){

     for( int i = undoBuffer.Count-1; i > 0; i-- ){
      undoBuffer[i] = undoBuffer[i-1];
     }

     undoBuffer[0] = values;

     currentUndoLocation = 0;

  }



  public void UpdateLand(){

    data.land.heightMap.SetPixels(colors,0);
    data.land.heightMap.Apply(true);

  }



  public void Undo(){
    currentUndoLocation ++;
    if( currentUndoLocation >= undoBuffer.Count-1 ){
      Debug.Log( "At Oldest");
    }else{
      MakeUndoTexture( undoBuffer[currentUndoLocation] );
    }
  }


  public void Redo(){


    currentUndoLocation --;
    if( currentUndoLocation < 0 ){
      Debug.Log( "AT NEWEST");
    }else{
      MakeUndoTexture( undoBuffer[currentUndoLocation] );
    }

  }


  public void MakeUndoTexture(float[] v){


    verts.SetData(v);
  

    ExtractColors();


    data.land.heightMap.SetPixels(colors,0);
    data.land.heightMap.Apply(true);


  }



public void TogglePaint(){
  normalOrHeight = (normalOrHeight+1)%5;

  string s = "height";
  if( normalOrHeight == 0 ){
    s = "paint vectors";
  }else if(normalOrHeight == 1 ){
    s = "raise terrain";
  }else if(normalOrHeight == 2 ){
    s = "lower terrain";
  }else if(normalOrHeight == 3 ){
    s = "raise grass";
  }else if(normalOrHeight == 4 ){
    s = "lower grass";
  }
//  GameObject.FindWithTag("togglePainting").GetComponent<Text>().text = s;
}

/*public void SetBrushSize(Slider s){
  paintSize = s.value * 100;
}
public void SetBrushOpacity(Slider s){
  paintOpacity = s.value;
}*/

  public static void SaveTextureAsPNG(Texture2D _texture, string _fullPath)
   {
       byte[] _bytes =_texture.EncodeToJPG(1000);
       System.IO.File.WriteAllBytes(_fullPath, _bytes);
       Debug.Log(_bytes.Length/1024  + "Kb was saved as: " + _fullPath + ".jpg");
   }




   public override void WhileLiving( float v ){
      
      while (_requests.Count > 0){
            var req = _requests.Peek();

            if (req.hasError){
                Debug.Log("GPU readback error detected.");
                _requests.Dequeue();
            }else if (req.done){
                var buffer = req.GetData<float>();
                ExtractColors( buffer.ToArray() );
                _requests.Dequeue();
            }else{
                break;
            }
        }
   }
  


}