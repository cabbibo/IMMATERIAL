using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monolith : Cycle
{

    public GameObject storyMarkerPrefab;
    public Transform monolith;
    public Story story;

    public float ratio;

    public override void Create(){

      ratio = (float)Screen.width / (float) Screen.height;

      transform.position = story.transform.position;
      transform.rotation = story.transform.rotation;


      float size = 2;
      monolith.localScale = new Vector3( size, size * ratio , size / 7 );
      monolith.transform.localPosition = Vector3.up * size * ratio * .4f;



      for( int i = 0; i < data.journey.stories.Length; i++ ){

      }
    }


}
