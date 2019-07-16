using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class TextAnchor: Form {

public class glyph{

  public int column;
  public int row;
  public int id;
  public char character;

  public glyph( int c, int r , int i , char ch ){
    column = c;
    row = r;
    id = i;
    character = ch;
  }
}


[TextArea]
public string text;
  
  public Frame frame;

  public float characterSize;
  public float lineHeight;
  public float padding;

  public List<glyph> glyphs;

  public float scale;
  public float scaledPadding;
  public float scaledCharacterSize;
  public float scaledLineHeight;

  public override void Create(){
    if( frame == null ){ frame = GetComponent<Frame>(); }
  }

  public override void SetStructSize(){ structSize = 12; }
  
  public override void SetCount(){

    //print( "setting count");
   // scale = frame.distance / 3;

    scaledPadding = scale * padding;
    scaledCharacterSize = scale  * characterSize;
    scaledLineHeight = scale * lineHeight;

   // print("SETTING COUNT + this : " + this.transform.parent);
    //scount = text.length
    glyphs = new List<glyph>();

    count = 0;
    
    string[] words = text.Split(' ');
    

    int column = 0;
    int row = 0;
    float value = scaledPadding * 3;

    foreach( string word in words ){
      
      char[] letters = word.ToCharArray();
      if( value + letters.Length * scaledCharacterSize >= frame.width - scaledPadding){
          row ++;
          value = scaledPadding;
          column = 0;
      }

      foreach( char c in letters ){ 


        if( c == '\n'){
          
          row ++;
          value = scaledPadding;
          column = 0;

        }else{
         // print( c );
          glyph g = new glyph(column,row,count,c);
          glyphs.Add(g);
          value += scaledCharacterSize;
          column ++;
          count ++;
        }
        //print((int)c);
        //print(column);
      }

      column ++;
      value += scaledCharacterSize;

    }
    
    //print(words[0]);

  }
  

  public override void Embody(){

    print( "embodying");

    float[] values = new float[count*structSize];
    int index = 0;
    Vector3 dir = (frame.topRight - frame.topLeft).normalized;
    Vector3 down = (frame.bottomRight - frame.topRight).normalized;
    

//    print(scaledPadding);
    Vector3 p;
    for( int i = 0; i < count; i ++ ){

      //print(glyphs[i]);
//      print(glyphs[i]);

      p = frame.topLeft + dir * ( scaledPadding + glyphs[i].column * scaledCharacterSize ) + down * (scaledPadding + glyphs[i].row * scaledLineHeight );

      // position
      values[ index ++ ] = p.x;
      values[ index ++ ] = p.y;
      values[ index ++ ] = p.z;

//      print(frame.normal.x);
      // normal
      values[ index ++ ] = frame.normal.x;
      values[ index ++ ] = frame.normal.y;
      values[ index ++ ] = frame.normal.z;

      float[] gInfo = getTextCoordinates(glyphs[i].character);

      //Character Info
      values[ index ++ ] = gInfo[0];
      values[ index ++ ] = gInfo[1];        
      values[ index ++ ] = gInfo[2];
      values[ index ++ ] = gInfo[3];
   
      // debug
      values[ index ++ ] = gInfo[4];
      values[ index ++ ] = gInfo[5];

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


