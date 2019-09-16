using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween : Cycle
{
   

  public delegate void TweenFunction(float v);
  public delegate void EndFunction();

  public struct tween{
    public TweenFunction tweenFunction;
    public EndFunction endFunction;
    public float startTime;
    public float length;
    public bool done;
  }



  public List<tween> tweens;

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


  public void AddTween( float length , TweenFunction tweenFunction , EndFunction end ){

   tween t = new tween();
   t.length = length;
   t.startTime = Time.time;
   t.tweenFunction = tweenFunction;
   t.done = false;
   t.endFunction = end;

   tweens.Add(t);

  }


  public override void Create(){
    tweens = new List<tween>();
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


    for( int i = tweens.Count-1; i >= 0; i-- ){

      //if( )
    }
  }

  public void TestFunction( float v ){ print("WHATSSSS : " + v ); }

  public void TestEndFunction(){ print("TADAAAA"); }


}
