using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painter : Simulation
{
  


  public bool painting;
  public PaintVerts verts;
  public PaintTris tris;
  public Material windDebugMat;
  public Material grassDebugMat;
  public Material planeDebugMat;


  public Vector3 paintPosition;
  private Vector3 oPP;
  public Vector3 paintDirection;

  public float paintSize;
  public float paintOpacity;
  public float normalOrHeight;

  public Transform paintTip;

  public override void Create(){
    Cycles.Add( tris );
  }

  public override void Bind(){

    life.BindPrimaryForm("_VectorBuffer", verts);
    life.BindAttribute("_PaintPosition", "paintPosition" , this);
    life.BindAttribute("_PaintDirection", "paintDirection" , this);
    life.BindAttribute("_PaintSize", "paintSize" , this);
    life.BindAttribute("_PaintOpacity", "paintOpacity" , this);
    life.BindAttribute("_NormalOrHeight", "normalOrHeight" , this);

    data.BindLandData(life);

  
  }

  public override void WhileDebug(){
    planeDebugMat.SetPass(0);
    planeDebugMat.SetInt("_Dimensions", verts.width );
    Graphics.DrawProceduralNow( MeshTopology.Triangles ,tris.count );
  }

  public void WhileDown(Ray ray){

    paintPosition = data.land.Trace( ray.origin, ray.direction);
    paintTip.position = paintPosition;

    life.YOLO();
    //Save();

  }


   public void Save(){

    float[] values = verts.GetData();

    Color[] colors =  new Color[verts.count];
    for( int i = 0; i < verts.count; i ++ ){
      
      // extracting height
      float h = values[ i * verts.structSize + 1 ] / data.land.height;
      
      // extracting flow verts
      float x = values[ i * verts.structSize + 6 ] * .5f + .5f;
      float z = values[ i * verts.structSize + 8 ] * .5f + .5f;


      float a = values[ i * verts.structSize + 11 ];

      colors[i] = new Color( h,x,z,a);

    }

    data.land.heightMap.SetPixels(colors,0);
    data.land.heightMap.Apply(true);

    SaveTextureAsPNG( data.land.heightMap , "Assets/HeightMap.png");

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