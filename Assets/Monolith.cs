using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monolith : Cycle
{

    public GameObject storyMarkerPrefab;
    public Transform monolith;
    public Story story;

    public bool isBook;

    public GameObject[] storyMarkers;

    public float ratio;


    public void DestroyMe(){
     // print( storyMarkers.Length );
      for( int i = 0; i < storyMarkers.Length; i++ ){
        Cycles.Remove(storyMarkers[i].GetComponent<StoryMarker>());
        DestroyImmediate(storyMarkers[i]);

      }
    }

    public override void Create(){

      DestroyMe();

      ratio = (float)Screen.width / (float) Screen.height;

      ratio = 1 / ratio;

      //transform.position = story.transform.position;
      //transform.rotation = story.transform.rotation;

      float size = 2;
      monolith.localScale = new Vector3( size, size * ratio , size / 7 );
      monolith.localPosition = Vector3.up * size * ratio * .4f;


      storyMarkers = new GameObject[data.journey.stories.Length];
      for( int i = 0; i < data.journey.stories.Length; i++ ){
          storyMarkers[i] = Instantiate( storyMarkerPrefab);
          
          SafeInsert(storyMarkers[i].GetComponent<StoryMarker>());
          storyMarkers[i].GetComponent<StoryMarker>().text.text = data.journey.stories[i].gameObject.name;
          storyMarkers[i].GetComponent<StoryMarker>().id = i;
          storyMarkers[i].transform.parent = transform;
          storyMarkers[i].transform.localPosition = Vector3.zero;
          storyMarkers[i].transform.localPosition += monolith.localScale.x * monolith.right * ( (-.5f + data.journey.stories[i].uv.x) * .7f ); 
          storyMarkers[i].transform.localPosition += monolith.localScale.y * monolith.up * ( (data.journey.stories[i].uv.y) * .7f + .1f );
          storyMarkers[i].transform.localPosition -= monolith.localScale.z * monolith.forward * .5f;
      }


      MaterialPropertyBlock mpb = new MaterialPropertyBlock();

      Vector4[] positions = new Vector4[data.journey.stories.Length ];
      for(int i = 0; i < positions.Length; i++ ){
        positions[i] = storyMarkers[i].transform.position;
      }

      mpb.SetVectorArray("_StoryPositions" , positions );
      mpb.SetInt("_NumStories" , positions.Length );
      mpb.SetInt("_ThisStory" , story.id );
      monolith.GetComponent<MeshRenderer>().SetPropertyBlock( mpb );
      
      if( isBook ){ transform.rotation = Quaternion.AngleAxis(90,Vector3.right);}


    }


}
