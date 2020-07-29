using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAnchor: Form {


public class glyph{

  public int column;
  public int row;
  public int id;
  public char character;

  public float x;
  public float y;
  public float w;
  public float h;
  public float offsetX;
  public float offsetY;
  public float advance;

  public float left;
  public float top;

  public float textureVal;
  public float scaleOffset;
  public float hueOffset;
  public float special;

  public glyph( float l, float t , int i , float[] vals , float tv , float so , float ho , float s ){
   
    left = l;
    top = t;

    x = vals[0];
    y = vals[1];

    w = vals[2];
    h = vals[3];

    offsetX = vals[4];
    offsetY = vals[5];

    advance = vals[6];
    //column = c;
    //row = r;
    id = i;
    textureVal = tv;
    scaleOffset = so;
    hueOffset = ho;
    special = s;
  }
}


 [TextArea(15,20)]
public string text;
  
  public Frame frame;

  public float characterSize=1;
  public float lineHeight =1;
  public float padding;
  public float advance = 1;

  public List<glyph> glyphs;

  public float scale;
  public float scaledPadding;
  public float scaledCharacterSize;
  public float scaledLineHeight;
  public float scaledAdvance;


  public float currentTextureVal;
  public float currentScaleOffset;
  public float currentHueOffset;
  public float currentSpecial;

  public int row;
  public int column;
  public float locationX;
  public float locationY;

  public override void Create(){
    if( frame == null ){ frame = GetComponent<Frame>(); }
  }

  public override void SetStructSize(){ structSize = 20; }
  
  public override void SetCount(){

  
    currentSpecial       = 0;
    currentHueOffset     = 0;
    currentScaleOffset   = 1;
    currentTextureVal    = 0;

    scaledPadding = scale * padding;
    scaledCharacterSize = scale  * characterSize;
    scaledLineHeight = scale * lineHeight;
    scaledAdvance = scale * advance;

      //print( "setting count");
   // scale = frame.distance / 3;
    row = 0;
    column = 0;
    locationX = scaledPadding;
    locationY = scaledPadding;


   // print("SETTING COUNT + this : " + this.transform.parent);
    //scount = text.length
    glyphs = new List<glyph>();

    count = 0;

    string[] test1 = text.Split('<');

    if( test1.Length > 1 ){
      
      int totalCount = test1.Length;
//      print( totalCount );

      string[] sections = new string[totalCount];
      
      for( int i = 0; i < totalCount-1; i++ ){
        string[] test2 = test1[i+1].Split('>');

        sections[i] = test2[1];
         
        string[] val = test2[0].Split('=');

        if(val[0] == "t" ){ currentTextureVal   = float.Parse(val[1]); }
        if(val[0] == "h" ){ currentHueOffset    = float.Parse(val[1]); }
        if(val[0] == "so" ){ currentScaleOffset = float.Parse(val[1]); }
        if(val[0] == "s"  ){ currentSpecial     = float.Parse(val[1]); }
        
        MakeGlyphs(sections[i]);


      }



    }else{
      MakeGlyphs(text);
    }
  
    //print(words[0]);

  }

  public void MakeGlyphs( string parsedText ){

    string[] words = parsedText.Split(' ');
    

    int first = 0;
    foreach( string word in words ){
 
      
      // makes sure we skip the first space of the section
      if( first != 0 ){
        column ++;
        locationX += scaledAdvance;
      }else{
        first=1;
      }


      char[] letters = word.ToCharArray();

 


 
      float newLine = 0;
      float wordWidth = 0;
      foreach( char c in letters ){ 
        if( c == '\n'){
        }else{
          wordWidth += scaledAdvance;// * scale
        }
      }

      if( locationX + wordWidth >= frame.width - scaledPadding*2 ){
          row ++;
          locationY += scaledLineHeight;
          locationX  = scaledPadding;
          column = 0;
      }


      foreach( char c in letters ){ 

        if( c == '\n'){
          row ++;
          locationY += scaledLineHeight;
          locationX  = scaledPadding;
          column = 0;
        }else{

          if( NovaMono.info.ContainsKey(c)){

          float[] v1 = NovaMono.info[c];


         // print( c );
          glyph g = new glyph(locationX,locationY,count,v1,currentTextureVal,currentScaleOffset,currentHueOffset,currentSpecial);
          glyphs.Add(g);



          locationX += scaledAdvance;
          column ++;
          count ++;
          }else{
            DebugThis("character not found");
            print( c );
            print( row );
            print( column );
          }
        }
      }




    }
    
  }
  

  public override void Embody(){

//    print( "embodying");

    float[] values = new float[count*structSize];
    int index = 0;
    Vector3 dir = (frame.topRight - frame.topLeft).normalized;
    Vector3 down = (frame.bottomRight - frame.topRight).normalized;
    

    Vector3 p;
    for( int i = 0; i < count; i ++ ){

      //print(glyphs[i]);
//      print(glyphs[i]);

      p = frame.topLeft + dir * glyphs[i].left + down * glyphs[i].top;

      // position
      values[ index ++ ] = p.x;
      values[ index ++ ] = p.y;
      values[ index ++ ] = p.z;

      // normal
      values[ index ++ ] = frame.normal.x;
      values[ index ++ ] = frame.normal.y;
      values[ index ++ ] = frame.normal.z;

      //float[] gInfo = getTextCoordinates(glyphs[i].character);

      //Character Info
      values[ index ++ ] = glyphs[i].x;
      values[ index ++ ] = glyphs[i].y;

      values[ index ++ ] = glyphs[i].w;
      values[ index ++ ] = glyphs[i].h;
   
      // debug
      values[ index ++ ] = glyphs[i].offsetX;
      values[ index ++ ] = glyphs[i].offsetY;

      //

      values[ index ++ ] = scaledPadding + glyphs[i].left;
      values[ index ++ ] = scaledPadding + glyphs[i].top;


      values[ index ++ ] = glyphs[i].w;
      values[ index ++ ] = glyphs[i].h;//(scaledPadding + glyphs[i].row * scaledLineHeight)/frame.height;


      values[index++] = glyphs[i].textureVal;// Mathf.Floor(Random.Range(0,1.999f));
      values[index++] = glyphs[i].scaleOffset; //Random.Range(.8f,1.2f );
      values[index++] = glyphs[i].hueOffset;
      values[index++] = glyphs[i].special;

    }

    SetData( values );

  }


  //TODO: Make with and height of letter, for later use
  float[] getTextCoordinates( char letter ){
    
    int  charCode = (int)letter;

    if( charCode == 8216 ){ charCode = 39; }
    if( charCode == 8217 ){ charCode = 39; }
    if( charCode == 8212 ){ charCode = 45; }



    float[] index = ubuntuMono.info[charCode];

    float[] newIndex = new float[index.Length];

    for( int i = 0; i< index.Length; i++ ){
      newIndex[i] = index[i] / 1024;
    }


    return newIndex;//new Vector4(left,top,width,height);

  }

  public override void WhileDebug(){
    //SetCount();
    //Embody();
    debugMaterial.SetPass(0);
    debugMaterial.SetBuffer("_VertBuffer", _buffer);
    debugMaterial.SetInt("_Count",count);
    Graphics.DrawProceduralNow(MeshTopology.Triangles, count * 3 * 2 );
  
  }
}


