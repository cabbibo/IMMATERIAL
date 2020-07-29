using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : Cycle{

  public int playID;
  public int oPlayID;
  public int numSources;
  public int numLoopSources;
  public int numGlobalLoopSources;

  public AudioMixer master;

    public static AudioPlayer Instance { get; private set; }

    private static AudioPlayer _instance;

    public GameObject[] objects;
    public GameObject[] loopObjects;
    public GameObject[] globalLoopObjects;
    public AudioSource[] sources;
    public SourcePlayer[] players;
    public AudioSource[] loopSources;
    public AudioSource[] globalLoopSources;

    public float loopBPM;
    public int loopBars;
    public int loopBPB;

    public Transform sourceTransform;
    public Transform loopTransform;
    public Transform globalLoopTransform;

    public GlobalLooper globalLooper;

    public GameObject sourcePrefab;

    public override void Create(){

        if( objects == null || objects.Length != numSources  || 
            loopSources.Length != numLoopSources || loopSources == null  || 
            globalLoopSources.Length != numGlobalLoopSources || globalLoopSources == null   ){
    

            if( objects != null ){
            for( int i = 0; i < objects.Length; i++ ){
                Object.DestroyImmediate(objects[i]);//.Destroy();
            }}

            if( loopObjects != null ){
            for( int i = 0; i < loopObjects.Length; i++ ){
                Object.DestroyImmediate(loopObjects[i]);//.Destroy();
            }}


             if( globalLoopObjects != null ){
            for( int i = 0; i < globalLoopObjects.Length; i++ ){
                Object.DestroyImmediate(globalLoopObjects[i]);//.Destroy();
            }}

        sources = new AudioSource[numSources];
        players = new SourcePlayer[numSources];
        objects = new GameObject[numSources];

        for( int i = 0; i < numSources; i++){
            objects[i] = Instantiate( sourcePrefab );
            objects[i].transform.parent = sourceTransform;

            sources[i] = objects[i].GetComponent<AudioSource>();
            sources[i].dopplerLevel = 0;
            sources[i].playOnAwake = false;

            players[i] = objects[i].GetComponent<SourcePlayer>();

        }

        loopSources = new AudioSource[numLoopSources];
        loopObjects = new GameObject[numLoopSources];
        for( int i = 0 ; i < numLoopSources; i++ ){ 

            loopObjects[i] = new GameObject();
            loopObjects[i].transform.parent = loopTransform;

            loopSources[i] = loopObjects[i].AddComponent<AudioSource>();
            loopSources[i].volume = 0;
            loopSources[i].dopplerLevel = 0;
            loopSources[i].playOnAwake = false;

            loopSources[i].outputAudioMixerGroup = master.FindMatchingGroups("Loops")[0];

        }

        print("WHA : " + numGlobalLoopSources);
        globalLoopSources = new AudioSource[numGlobalLoopSources];
        globalLoopObjects = new GameObject[numGlobalLoopSources];
        for( int i = 0 ; i < numGlobalLoopSources; i++ ){ 

            globalLoopObjects[i] = new GameObject();
            globalLoopObjects[i].transform.parent = globalLoopTransform;

            globalLoopSources[i] = globalLoopObjects[i].AddComponent<AudioSource>();
            globalLoopSources[i].volume = 0;
            globalLoopSources[i].dopplerLevel = 0;
            globalLoopSources[i].playOnAwake = false;
            globalLoopSources[i].loop = true;
            globalLoopSources[i].outputAudioMixerGroup = master.FindMatchingGroups("GlobalLoops")[0];

        }



        }



        SafeInsert(globalLooper);

    }


    public float loopStartTime = 0;

    public float loopTime{
        get{ return (loopBPM / 60) * loopBPB * loopBars; }
    }

    public float timeTilLoop{

        get{
            float fadeTime = ((loopStartTime + loopTime) - Time.time);
            return fadeTime;
        }
    }

    public void FadeLoop( int i , float v, float t ){
        StartCoroutine(Fade(loopSources[i],loopSources[i].volume,v,t));
    }

    public void FadeGlobalLoop( int i , float v, float t ){
        StartCoroutine(Fade(globalLoopSources[i],globalLoopSources[i].volume,v,t));
    }


     IEnumerator Fade( AudioSource a , float sv ,float v,  float time  ){
        for (float i = 0; i < time; i += Time.deltaTime ){
            a.volume = Mathf.SmoothStep( sv , v , (i/time));
            yield return null;
        }
    }

    IEnumerator FadeIn( AudioSource a  , float time ){
        for (float i = 0; i < time; i += Time.deltaTime ){
            a.volume = i/time;
            yield return null;
        }
    }
    IEnumerator FadeOut( AudioSource a , float time  ){
        for (float i = 0; i < time; i += Time.deltaTime ){
            a.volume = 1-(i/time);
            yield return null;
        }
    }

    public override void WhileLiving(float v){


        if( Time.time - loopStartTime > (loopBPM / 60) * loopBPB * loopBars ){
            NewLoop();
        }

    }

    public override void OnBirthed(){
        NewLoop();
        NewGlobalLoop();
    }

    public void NewLoop(){
        loopStartTime = Time.time;// + (loopBPM / 60) * loopBPB * loopBars;
        for( int i = 0; i < loopSources.Length; i++ ){
            if( loopSources[i].clip != null ){
                loopSources[i].Play();
            }
        
        }

    }

    public void NewGlobalLoop(){
        for( int i = 0; i < globalLoopSources.Length; i++ ){
            if( globalLoopSources[i].clip != null ){
                globalLoopSources[i].Play();
            }
        }
    }

    public void Play( AudioClip clip ){
//        print( clip);

        sources[playID].clip = clip;
        //sources[playID].time = 0;
        sources[playID].Play();

        oPlayID = playID;
        playID ++;
        playID %= numSources;
    }

    public void PlayOne( AudioClip clip ){
        players[playID].Play(clip);
        Next();
    }

    public void PlayGrain( AudioClip clip , float start , float length){
        players[playID].Play(clip , start, length);
        Next();
    }

     public void PlayGrain( AudioClip clip , float start , float length , AudioMixer mixer, string group){
        players[playID].Play(clip , start, length , mixer, group);
        Next();
    }

    public void Next(){
        oPlayID = playID;
        playID ++;
        playID %= numSources;
    }



    public void Play( AudioClip clip , float pitch){

        sources[playID].outputAudioMixerGroup = master.FindMatchingGroups("Default")[0];
        sources[playID].volume = 1;
        sources[playID].time = 0;
        sources[playID].pitch = pitch;
        Play(clip);
    }

    public void Play( AudioClip clip , float pitch , float volume){

        sources[playID].outputAudioMixerGroup = master.FindMatchingGroups("Default")[0];
        sources[playID].time = 0;
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

    public void Play( AudioClip clip , int step , float volume , float location , AudioMixer mixer, string group ){
        float p = Mathf.Pow( 1.05946f , (float)step );
        
        sources[playID].outputAudioMixerGroup = mixer.FindMatchingGroups(group)[0];
        sources[playID].volume = volume;
        sources[playID].time = location;
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
        

        sources[playID].clip = clip;
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
