using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;


[ExecuteInEditMode]
public class SoundPainter : Simulation
{


  
[SerializeField]public string[] options;



public int brushType;
public int numberBrushTypes = 16;

public int raiseLowerFlatten;

public int numberTextures{
    get{ return numberBrushTypes/4; }
}
public int whichTexture {
    get{ return brushType/4; }
}





  public string terrainPath;

  public bool painting;

  public LandDataVerts verts;


  public Material debugMaterial;

  public Texture2D[] textures;

  public GameObject[] textureDebuggers;




  // how many undos we get!
  public int undoBufferSize;
  public int currentUndoLocation;
  public List<float[]> undoBuffer;

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



  private Color[] colors;
  private float[] values;


  public float reset;

  public Transform paintTip;

  private MaterialPropertyBlock mpb;




  // to get our data back from gpu
  Queue<AsyncGPUReadbackRequest> _requests = new Queue<AsyncGPUReadbackRequest>();

  public override void Create(){


    if( mpb == null ){ mpb = new MaterialPropertyBlock(); }


    if( undoBuffer == null ){ 
      undoBuffer = new List<float[]>(); 
    }
  
    if( undoTexture == null ){
      undoTexture = new Texture2D(verts.width, verts.width);
      Graphics.CopyTexture(startTexture, undoTexture);
    }

    if( textures.Length  != numberTextures || textures == null){
        textures = new Texture2D[numberTextures];
    }
    for( int i = 0; i < numberTextures; i ++){
        if( textures[i] == null ){
            textures[i] = new Texture2D(verts.width, verts.width);
        }
    }

 

    

  }




  // Only Recreate if its not the correct size;
  public override void OnGestated(){


    Load();

int oBrush = brushType;
for( int i = 0; i < numberTextures; i++ ){
    brushType = i*4;
    ExtractColors();
    UpdateLand();
}  

 for( int i = 0; i < numberTextures; i++){
    var mpb = new MaterialPropertyBlock();
    mpb.SetTexture("_MainTex", data.land.infoTextures[0]);
    textureDebuggers[i].GetComponent<MeshRenderer>().SetPropertyBlock( mpb);
}

brushType = oBrush;

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

    life.BindVector3( "_PaintPosition"      , () => this.paintPosition      );
    life.BindVector3( "_PaintDirection"     , () => this.paintDirection     );
    life.BindFloat(   "_PaintSize"          , () => this.paintSize          );
    life.BindFloat(   "_PaintOpacity"       , () => this.paintOpacity       );
    life.BindInt(     "_Brush"              , () => this.brushType          );
    life.BindInt(     "_RaiseLowerFlatten"  , () => this.raiseLowerFlatten  );
    life.BindFloat(   "_Reset"              , () => this.reset              );
    life.BindTexture( "_TextureReset"       , () => this.startTexture       );
    life.BindTexture( "_UndoTexture"        , () => this.undoTexture        );

    data.BindLandData(life);

  
  }

  public override void WhileDebug(){
    
    mpb.SetInt("_Dimensions", verts.width);
    mpb.SetInt("_Count", verts.count);
    mpb.SetInt("_Brush",brushType);
    mpb.SetBuffer("_VertBuffer", verts._buffer);

    mpb.SetTexture("_Texture0", textures[0]);
    mpb.SetTexture("_Texture1", textures[1]);
    mpb.SetTexture("_Texture2", textures[2]);
    mpb.SetTexture("_Texture3", textures[3]);

    Graphics.DrawProcedural( debugMaterial,  new Bounds(transform.position, Vector3.one * 5000), MeshTopology.Triangles, verts.count * 3 * 2 * numberBrushTypes, 1, null, mpb, ShadowCastingMode.Off, true, LayerMask.NameToLayer("Debug"));

  }




  public void WhileDown(Ray ray){

    print( this.brushType );
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
      float r = values[ i * verts.structSize + 3 + 4 * whichTexture +0 ];
      float g = values[ i * verts.structSize + 3 + 4 * whichTexture +1 ];
      float b = values[ i * verts.structSize + 3 + 4 * whichTexture +2 ];
      float a = values[ i * verts.structSize + 3 + 4 * whichTexture +3 ];

      colors[i] = new Color( r,g,b,a);

    }

  }


  public void ExtractColors( float[] v){
   
    //int count = values.Length / verts.structSize;

    values = v;
    colors =  new Color[verts.count];
    for( int i = 0; i < verts.count; i ++ ){
      
          // extracting flow verts
      float r = values[ i * verts.structSize + 3 + 4 * whichTexture+0 ];
      float g = values[ i * verts.structSize + 3 + 4 * whichTexture+1 ];
      float b = values[ i * verts.structSize + 3 + 4 * whichTexture+2 ];
      float a = values[ i * verts.structSize + 3 + 4 * whichTexture+3 ];

      colors[i] = new Color( r,g,b,a);

    }

  }
   public void Save(){

    ExtractColors();
    propogateUndoBuffer();
    UpdateLand();

  }

  public void UltraSave(){

    ExtractColors();
    propogateUndoBuffer();
    UpdateLand();

    string path = "StreamingAssets/Terrain/terrainData";
    Saveable.Save( verts , path );

    int oBrushType = brushType;
    for( int i = 0; i < numberTextures; i++ ){
        brushType = i * 4;
        ExtractColors();
        UpdateLand();
        SaveTextureAsPNG(textures[i] , Application.dataPath+"/" + path + i );
    }
    brushType = oBrushType;
  
  }

  public void Load(){

    print( "l1 ");



    string path = "StreamingAssets/Terrain/terrainData";
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
print( "APPLUIONG " + whichTexture);
    textures[whichTexture].SetPixels(colors,0);
    textures[whichTexture].Apply(true);

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
                ExtractColors( buffer.ToArray());
                _requests.Dequeue();
            }else{
                break;
            }
        }
   }
  


}