using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeStars : Cycle
{

    public GameObject starPrefab;
    public float upness  = 4;
    public Transform monolith;
    public MeshRenderer renderer;

    public GameObject[] starMarkers;

    public MaterialPropertyBlock mpb;



    public void DestroyMe(){
     // print( starMarkers.Length );
      for( int i = 0; i < starMarkers.Length; i++ ){
        if( starMarkers[i] ){
            DestroyImmediate(starMarkers[i]);
        }
      }

      Cycles.Clear();
    }



    public override void Create(){

   

   


      if( data.journey.monoSetters.Length != starMarkers.Length  || starMarkers == null ){


        DestroyMe();
        starMarkers = new GameObject[data.journey.monoSetters.Length];
        for( int i = 0; i < data.journey.monoSetters.Length; i++ ){
            starMarkers[i] = Instantiate( starPrefab);
            
            starMarkers[i].transform.parent = transform;
            starMarkers[i].transform.localPosition = Vector3.zero;
            starMarkers[i].transform.localPosition += (Vector3.right) *  data.journey.monoSetters[i].uv.x  / data.land.size; 
            starMarkers[i].transform.localPosition += (Vector3.forward) *  data.journey.monoSetters[i].uv.y / data.land.size; 
            starMarkers[i].transform.localPosition += (Vector3.up) *  data.land.height * upness;
            
        }
        }


      /*

        Setting the locations of the markers in the shader
        w
      */

      mpb = new MaterialPropertyBlock();

      Vector4[] positions = new Vector4[data.journey.monoSetters.Length ];
      for(int i = 0; i < positions.Length; i++ ){
        positions[i] = starMarkers[i].transform.position;
      }

      mpb.SetVectorArray("_StoryPositions" , positions );
      mpb.SetInt("_NumStories" , positions.Length );

      //renderer.SetPropertyBlock( mpb );
      


    }




}
