using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween : Cycle
{
   

  public delegate void TweenFunction(float v);
  public delegate void EndFunction();

  public delegate void idTweenFunction(float v,int id);
  public delegate void idEndFunction(int id);

  public struct tween{
    public TweenFunction tweenFunction;
    public EndFunction endFunction;
    public float startTime;
    public float length;
    public bool done;
  }

  public struct idTween{
    public idTweenFunction tweenFunction;
    public idEndFunction endFunction;
    public float startTime;
    public float length;
    public int id;
    public bool done;
  }

  public List<tween> tweens;
  public List<idTween> idTweens;


   public override void Create(){
    tweens = new List<tween>();
    idTweens = new List<idTween>();
  }

  public void RemoveTween( tween t ){

  }

  public void AddTween( float length , TweenFunction tweenFunction ){

   tween t = new tween();
   t.length = length;
   t.startTime = Time.time;
   t.tweenFunction = tweenFunction;
   t.done = false;

   tweens.Add(t);

  }

  public void AddTween( float length,int id, idTweenFunction tweenFunction ){

   idTween t = new idTween();
   t.length = length;
   t.startTime = Time.time;
   t.tweenFunction = tweenFunction;
   t.id = id;
   t.done = false;

   idTweens.Add(t);

  }


  public void AddTween( float length,int id, idTweenFunction tweenFunction , idEndFunction end){

   idTween t = new idTween();
   t.length = length;
   t.startTime = Time.time;
   t.tweenFunction = tweenFunction;
   t.id = id;
   t.done = false;
   t.endFunction = end;

   idTweens.Add(t);

  }



  public void AddTween( float length , TweenFunction tweenFunction , EndFunction end ){

   tween t = new tween();
   t.length = length;
   t.startTime = Time.time;
   t.tweenFunction = tweenFunction;
   t.done = false;
   t.endFunction = end;

   tweens.Add(t);

  }

  public override void WhileLiving( float f ){

    for( int i = tweens.Count-1; i >= 0; i-- ){

      float v = (Time.time - tweens[i].startTime) / tweens[i].length;

      if( v > 1 ){

        tweens[i].tweenFunction(1);
        if( tweens[i].endFunction != null ){ tweens[i].endFunction(); }
        tweens.Remove(tweens[i]);

      }else{
        tweens[i].tweenFunction(v);
      }
    }


  for( int i = idTweens.Count-1; i >= 0; i-- ){

      float v = (Time.time - idTweens[i].startTime) / idTweens[i].length;

      if( v > 1 ){

        idTweens[i].tweenFunction(1,idTweens[i].id);
        if( idTweens[i].endFunction != null ){ idTweens[i].endFunction(idTweens[i].id); }
        idTweens.Remove(idTweens[i]);

      }else{
        idTweens[i].tweenFunction(v,idTweens[i].id);
      }
    }


  }

  public void TestFunction( float v ){ print("WHATSSSS : " + v ); }

  public void TestEndFunction(){ print("TADAAAA"); }


}
