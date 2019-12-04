using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : Cycle{

  public int playID;
  public int oPlayID;
  public int numSources;

    public static AudioPlayer Instance { get; private set; }

    private static AudioPlayer _instance;

    private GameObject[] objects;
    private AudioSource[] sources;
    public Transform sourceTransform;

    public override void Create(){

        if( objects != null ){
            for( int i = 0; i < objects.Length; i++ ){
                Object.DestroyImmediate(objects[i]);//.Destroy();
            }
        }

        if( Instance == null ){
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }else{
            //Destroy( gameObject);
        }

        sources = new AudioSource[numSources];
        objects = new GameObject[numSources];

        for( int i = 0; i < numSources; i++){
            objects[i] = new GameObject();
            objects[i].transform.parent = sourceTransform;

            sources[i] = objects[i].AddComponent<AudioSource>() as AudioSource;
            sources[i].dopplerLevel = 0;
            sources[i].playOnAwake = false;
        }
    }

    public void Play( AudioClip clip ){

        sources[playID].clip = clip;
        sources[playID].Play();

        oPlayID = playID;
        playID ++;
        playID %= numSources;
    }


    public void Play( AudioClip clip , float pitch){

        sources[playID].volume = 1;
        sources[playID].pitch = pitch;
        Play(clip);
    }

    public void Play( AudioClip clip , float pitch , float volume){

        sources[playID].volume = volume;
        sources[playID].pitch = pitch;
        Play(clip);
    }

    public void Play( AudioClip clip , int step , float volume ){

        float p = Mathf.Pow( 1.05946f , (float)step );
        sources[playID].volume = volume;
        sources[playID].pitch = p;
        Play(clip);
    }

    public void Play(AudioClip clip , int step , float volume,float location){
        float p = Mathf.Pow( 1.05946f , (float)step );
        sources[playID].volume = volume;
        sources[playID].pitch = p;
        sources[playID].time = location;
        Play(clip);
    }

    public void Play(AudioClip clip , int step , float volume,float location,float length){
        float p = Mathf.Pow( 1.05946f , (float)step );
        sources[playID].volume = volume;
        sources[playID].pitch = p;
        sources[playID].time = location;
        sources[playID].SetScheduledEndTime( AudioSettings.dspTime +.25f );
        Play(clip);
    }

    public void Play(AudioClip clip , float pitch , float volume,float location,float length){
        
        sources[playID].volume = volume;
        sources[playID].pitch = pitch;
        sources[playID].time = location;

        Play(clip);

        sources[oPlayID].SetScheduledEndTime( AudioSettings.dspTime + length );
    }

        public void Play(AudioClip clip , float pitch , float volume,float location,float length, AudioMixer mixer, string group){
        
        sources[playID].volume = volume;
        sources[playID].pitch = pitch;
        sources[playID].time = location;
        sources[playID].outputAudioMixerGroup = mixer.FindMatchingGroups(group)[0];

        Play(clip);

        sources[oPlayID].SetScheduledEndTime( AudioSettings.dspTime + length );
    }


      public void Play( AudioClip clip , int step , float volume , Vector3 location , float falloff ){

        float p = Mathf.Pow( 1.05946f , (float)step );
        sources[playID].volume = volume;
        sources[playID].pitch = p;
        sources[playID].spatialize = true;
        sources[playID].spatialBlend = 1;
        sources[playID].maxDistance = falloff;
        sources[playID].minDistance = falloff/10;

        objects[playID].transform.position = location;
        Play(clip);
    }
}
