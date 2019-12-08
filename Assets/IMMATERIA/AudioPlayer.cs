using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : Cycle{

  public int playID;
  public int oPlayID;
  public int numSources;
  public int numLoopSources;

    public static AudioPlayer Instance { get; private set; }

    private static AudioPlayer _instance;

    public GameObject[] objects;
    public GameObject[] loopObjects;
    public AudioSource[] sources;
    public SourcePlayer[] players;
    public AudioSource[] loopSources;

    public float loopBPM;
    public int loopBars;
    public int loopBPB;

    public Transform sourceTransform;
    public Transform loopTransform;

    public GameObject sourcePrefab;

    public override void Create(){

        if( objects == null || objects.Length != numSources  || loopSources.Length != numLoopSources || loopSources == null ){
    

            if( objects != null ){
            for( int i = 0; i < objects.Length; i++ ){
                Object.DestroyImmediate(objects[i]);//.Destroy();
            }}

            if( loopObjects != null ){
            for( int i = 0; i < loopObjects.Length; i++ ){
                Object.DestroyImmediate(loopObjects[i]);//.Destroy();
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

        }


        }



    }


    public float loopStartTime = 0;

    public float timeTilLoop(){

        float loopTime = (loopBPM / 60) * loopBPB * loopBars;
        float fadeTime = ((loopStartTime + loopTime) - Time.time);
        print(fadeTime);
     return fadeTime;
    }

    public void FadeLoop( int i , bool inOut){
       if(inOut){ FadeInLoop(i); }else{FadeOutLoop(i);}
    }

    public void FadeInLoop(int i){
        data.tween.AddTween( timeTilLoop() ,i , fadeInLoop );
    }

    public void FadeOutLoop( int i ){
        data.tween.AddTween( timeTilLoop(),i , fadeOutLoop );
    }

    public void fadeInLoop( float v , int id ){
        loopSources[id].volume = v;
    }

    public void fadeOutLoop( float v , int id ){
        loopSources[id].volume = 1-v;
    }

    public override void WhileLiving(float v){


        if( Time.time - loopStartTime > (loopBPM / 60) * loopBPB * loopBars ){
            NewLoop();
        }

    }

    public override void OnBirthed(){
        NewLoop();
    }

    public void NewLoop(){
        loopStartTime = Time.time;// + (loopBPM / 60) * loopBPB * loopBars;
        for( int i = 0; i < loopSources.Length; i++ ){
            if( loopSources[i].clip != null ){
                loopSources[i].Play();
            }
        
        }
    }

    public void Play( AudioClip clip ){

        sources[playID].clip = clip;
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

    public void Play( AudioClip clip , int step , float volume, AudioMixer mixer, string group ){
        float p = Mathf.Pow( 1.05946f , (float)step );
        
        sources[playID].outputAudioMixerGroup = mixer.FindMatchingGroups(group)[0];
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
