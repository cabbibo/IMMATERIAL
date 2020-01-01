using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;    
 using System;

public class FDGraph : Cycle
{

    public FDGraphVerts verts;
    public FDGraphConnections connections;
    public Life simulate;
    public Life resolve;

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


    [HideInInspector]public int[] connectionData;

    [HideInInspector]public LifeJSON allData;

    public override void Create(){
      print("hello");
      verts.graph = this;
      connections.graph = this;
      ParseData();
      SafeInsert(verts);
      SafeInsert(connections);
      SafeInsert(simulate);
      SafeInsert(resolve);
    }


    public override void Bind(){

      simulate.BindInt( "_Frame" , () => frame );
      simulate.BindPrimaryForm( "_VertBuffer" , verts );
      simulate.BindForm( "_ConnectionBuffer" , connections );
   
      resolve.BindPrimaryForm( "_VertBuffer" , verts );
      resolve.BindForm( "_ConnectionBuffer" , connections );


    }

    public override void WhileLiving( float v ){
      frame ++;
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
