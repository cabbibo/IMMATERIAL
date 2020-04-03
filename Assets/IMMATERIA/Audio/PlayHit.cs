using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


[ExecuteAlways]
public class PlayHit : MonoBehaviour
{

    public AudioClip clip;
    public AudioPlayer player;
    public AudioMixer mixer;
    public string group;

    public float startTime;
    public float length;

    public float speed;
    
    public float lastTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

      if( Time.time - lastTime > speed + Random.Range(0 , speed * .4f)){
        lastTime = Time.time;
        player.PlayGrain( clip , startTime + Random.Range( -startTime, startTime ) , length , mixer , group );
      }
    }
}
