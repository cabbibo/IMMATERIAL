using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;    
 using System;

using UnityEngine.Networking;

public class FDGraph : Cycle
{

    public FDGraphVerts verts;
    public FDGraphConnections connections;
    public Life simulate;
    public Life resolve;
    public ClosestLife closest;


    public TextMesh text;
    public Renderer showImg;

    public int frame;

    public string fileName; 



//{"CONFIDENCE": "0", "LEAF": "0", "CHILDCOUNT": "4", "PHYLESIS": "0", "HASPAGE": "2", "EXTINCT": "0", "ID": "1", "name": "Life on Earth", "id": "1"}
[Serializable]
public class nodeInfo{
  public int CONFIDENCE;
  public int LEAF;
  public int CHILDCOUNT;
  public int PHYLESIS;
  public int HASPAGE;
  public int EXTINCT;
  public int ID;
  public string name;
  public int id;
  public int startConnectionID;
  public int totalConnections;
  public int parentID;
}

//{"weight": 1, "source": 0, "target": 1}
[Serializable]
public class connectionInfo{
  public int weight;
  public int source;
  public int target;
}


    [Serializable]
public class LifeJSON
{
    public bool directed;
    public bool multigraph;
    public nodeInfo[] nodes;
    public connectionInfo[] links;
}
/*
{
  "batchcomplete":"","
  query":{
    "pages":{
      "175040":{
        "pageid":175040,
        "ns":0,"title":
        "Al-Farabi",
        "thumbnail":{
              "source":"https://upload.wikimedia.org/wikipedia/commons/thumb/5/55/Al-Farabi.jpg/80px-Al-Farabi.jpg",
              "width":80,
              "height":100
            },
          "pageimage":"Al-Farabi.jpg"
          }
          }
          }
        }
*/
[Serializable]
public class imageData
{
    public int level;
    public float timeElapsed;
    public string playerName;
}

    [HideInInspector]private int[] connectionData;

    [HideInInspector]private LifeJSON allData;


    public int[] GetConnectionData(){
      return connectionData;
    }

    public nodeInfo[] GetNodes(){
      return allData.nodes;
    }
    public override void Create(){
      print("hello");
      verts.graph = this;
      connections.graph = this;
      ParseData();
      SafeInsert(verts);
      SafeInsert(connections);
      SafeInsert(simulate);
      SafeInsert(resolve);
      SafeInsert(closest);
    }


    public override void Bind(){

      simulate.BindInt( "_Frame" , () => frame );
      simulate.BindPrimaryForm( "_VertBuffer" , verts );
      simulate.BindForm( "_ConnectionBuffer" , connections );
   
      resolve.BindPrimaryForm( "_VertBuffer" , verts );
      resolve.BindForm( "_ConnectionBuffer" , connections );


      closest.Set(verts);
    }

    public void OnTapCheck(){
        nodeInfo ni = allData.nodes[(int)closest.closestID];
        text.text = ni.name;
        //string s1 = "http://en.wikipedia.org/w/api.php?action=query&titles=Acanthosoma_labiduroides&prop=pageimages&format=json";
        string s1 = "http://en.wikipedia.org/w/api.php?action=query&titles=Acanthosoma_labiduroides&prop=pageimages&format=json";
       
        s1 = "https://en.wikipedia.org/w/api.php?action=opensearch&search="+ni.name+"&limit=1&namespace=0&format=json";
        string word = s1;//"https://upload.wikimedia.org/wikipedia/commons/5/51/Acanthosoma_labiduroides_%28male%29.jpg";
        //UnityWebRequest www = UnityWebRequest.Post(s1);

        if( ni.name != "None" ){
          StartCoroutine(GetText(s1));
        }else{
          print("THIS HAS NO NAME");
        }
        
        //StartCoroutine(GetTexture(word));
    }

    IEnumerator GetText( string request ) {
        UnityWebRequest www = UnityWebRequest.Get(request);
        yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        } else {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
            string n_s = www.downloadHandler.text;


string[] result = n_s.Split(new string[] {"["}, System.StringSplitOptions.None);


if( result.Length > 4 && result[4].Length > 2 ){
string[] s2;
string s1 = result[4];
s2 = s1.Split(new string[] {"\""}, System.StringSplitOptions.None);
//if( s2)
print( s2[0]);
print( s2[1]);

string[] justTitle = s2[1].Split(new string[] {"https://en.wikipedia.org/wiki/"}, System.StringSplitOptions.None);

print( justTitle[1] );
Application.OpenURL(s2[1]);
StartCoroutine(GetImageURL(s2[1]));
}else{

  print("NO WIKI PAGE");
}




            //string[] splitArray =  n_s.Split(n_s,"source"); //Here we assing the splitted string to array by that char
            // name = splitArray[0];
 
            // Or retrieve results as binary data
            //byte[] results = www.downloadHandler.data;
        }
    }




  IEnumerator GetImageURL( string request ) {
        UnityWebRequest www = UnityWebRequest.Get(request);
        yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        } else {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
            string n_s = www.downloadHandler.text;


          string[] result = n_s.Split(new string[] {"<img"}, System.StringSplitOptions.None);

          print(result[1]);

          if( result.Length > 4 && result[4].Length > 2 ){

              //StartCoroutine(GetTextureL(s2[1]));
          }else{

            print("NO WIKI PAGE");
          }

        }
    }

    IEnumerator GetTexture( string request ) {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(request);
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
          print("NO ERROR");
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            print( myTexture );
            print( myTexture.width );
            print( myTexture.height );
            float r = (float)myTexture.width / (float)myTexture.height;

            showImg.transform.localScale = new Vector3( r , 1  , 1) * .2f;
            showImg.material.SetTexture("_MainTex", myTexture);

        }
    }

    public override void WhileLiving( float v ){
      frame ++;
      Shader.SetGlobalInt("_ClosestID", (int)closest.closestID);
    }

    public void ParseData(){

      //print("hello1");
       string filePath = Path.Combine(Application.streamingAssetsPath,"" + fileName + ".json");

        if(File.Exists(filePath))
        {
            // Read the json from the file into a string
            string data = File.ReadAllText(filePath);    

            allData = JsonUtility.FromJson<LifeJSON>( data);

            verts.count = allData.nodes.Length;
            connections.count = allData.links.Length * 2;


            connectionData = new int[ allData.links.Length  * 2 ];

            int total = 0;

            int currentSource = 0;
            int reset = 0;
            int index = 0;

            for(int i = 0; i < allData.links.Length; i++ ){
              if( allData.links[i].source != currentSource ){ 
                
                allData.nodes[currentSource].totalConnections = reset;
                reset = 0;
                total ++; 
                currentSource = allData.links[i].source; 

                allData.nodes[currentSource].startConnectionID = i;
              }
              
              reset ++;

              connectionData[index++] = allData.links[i].source;
              connectionData[index++] = allData.links[i].target;
              allData.nodes[allData.links[i].target].parentID = allData.links[i].source;

            }

            int v = UnityEngine.Random.Range( 0, 10000);

            for( int i = 0; i < allData.nodes.Length; i++ ){

              if( i == 10000 ){print(allData.nodes[i].name); }

              // ?????
              if( allData.nodes[i].parentID == null ){
                print("Img no");
              }else{

                if( allData.nodes[i].parentID == 0 ){
                  print(allData.nodes[i].name);
                }
              }
            }





        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

}
