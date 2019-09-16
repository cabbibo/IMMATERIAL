using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[ExecuteInEditMode]
public class Painter : Simulation
{
  
 [SerializeField]public string[] options;
 public int brushType;


  public string terrainPath;


  public bool painting;
  public PaintVerts verts;
  public PaintTris tris;
  public Material windDebugMat;
  public Material grassDebugMat;
  public Material planeDebugMat;


  public int undoBufferSize;
  public int currentUndoLocation;
  public List<float[]> undoBuffer;

  public bool drawPlane;
  public bool drawGrass;
  public bool drawWind;

  public string safeName;

  public Texture startTexture;
  public Texture undoTexture;


  public Vector3 paintPosition;
  private Vector3 oPP;
  public Vector3 paintDirection;

  public float paintSize;
  public float paintOpacity;
  public float normalOrHeight;
private Color[] colors;
private float[] values;


  public float reset;

  public Transform paintTip;

  public override void Create(){
    SafeInsert( tris );

    if( undoBuffer == null ){ 
      undoBuffer = new List<float[]>(); 
    }//.Clone();
  
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
  public override void Bind(){

    life.BindPrimaryForm("_VectorBuffer", verts);

//    print(this.paintPosition);
    
    
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
    if( drawPlane ){
      planeDebugMat.SetPass(0);
      planeDebugMat.SetInt("_Dimensions", verts.width );
      Graphics.DrawProceduralNow( MeshTopology.Triangles ,tris.count );
    }


    if( drawGrass ){
      grassDebugMat.SetPass(0);

    grassDebugMat.SetBuffer("_VertBuffer", verts._buffer);
    grassDebugMat.SetInt("_Count",verts.count);

      Graphics.DrawProceduralNow( MeshTopology.Triangles ,verts.count * 3 );
    }


    if( drawWind ){

      windDebugMat.SetPass(0);

    windDebugMat.SetBuffer("_VertBuffer", verts._buffer);
    windDebugMat.SetInt("_Count",verts.count);
      Graphics.DrawProceduralNow( MeshTopology.Triangles ,verts.count * 3 );
    }


  }

  public void WhileDown(Ray ray){
    paintDirection = paintPosition;
    paintPosition = data.land.Trace( ray.origin, ray.direction);
    paintTip.position = paintPosition;
    paintTip.localScale = new Vector3( paintSize, paintSize,paintSize);

    paintDirection = -(paintDirection - paintPosition).normalized;

    life.YOLO();
    //Save();

  }


public void ResetToOriginal(){
  reset = 2;
  life.YOLO();
  reset = 0;
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
   public void Save(){

    print("helloooszo");

    ExtractColors();
    propogateUndoBuffer();
    UpdateLand();


  }

  public void UltraSave(){

    ExtractColors();
    propogateUndoBuffer();
    UpdateLand();


    string path = terrainPath + "safe";
    Saveable.Save( verts , path );

    SaveTextureAsPNG( data.land.heightMap , "Assets/"+ path + ".png");
  


  }

  public void Load(){

//    print("wait why am I loading");

    string path = terrainPath + "safe";
    Saveable.Load( verts , path );

//    print("loaded");

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

    //SaveTextureAsPNG( data.land.heightMap , "Assets/" + safeName + ".png");


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
       byte[] _bytes =_texture.EncodeToPNG();
       System.IO.File.WriteAllBytes(_fullPath, _bytes);
       Debug.Log(_bytes.Length/1024  + "Kb was saved as: " + _fullPath);
   }


  


}