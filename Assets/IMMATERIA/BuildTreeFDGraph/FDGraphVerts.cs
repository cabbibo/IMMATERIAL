using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FDGraphVerts : Particles
{

  public FDGraph graph;



  public override void Embody(){

    float[] values = new float[count * structSize];
    int index = 0;

    FDGraph.nodeInfo[] nodes = graph.GetNodes();

    for( int i = 0; i < count; i++ ){
      
      values[index++] = Random.Range(-10.00f, 10.00f);
      values[index++] = Random.Range(-10.00f, 10.00f);
      values[index++] = Random.Range(-10.00f, 10.00f);
      
      values[index++] = 0;
      values[index++] = 0;
      values[index++] = 0;
      
      values[index++] = 0;
      values[index++] = 0;
      values[index++] = 0;

      values[index++] = nodes[i].startConnectionID;
      values[index++] = nodes[i].totalConnections;
      values[index++] = nodes[i].parentID;

      values[index++] = i;
      values[index++] = (float)i / (float)count;


      values[index++] = nodes[i].EXTINCT;
      values[index++] = 0;


    }






    SetData( values );

  }

}
