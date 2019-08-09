using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Osscilate : MonoBehaviour {


	public float rand;
	// Use this for initialization
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {
    transform.Rotate( Vector3.up , 1.5f* Mathf.Sin(Time.time * 2 + Mathf.Sin(Time.time * 6 + Mathf.Sin(Time.time*20))));
		transform.Rotate( Vector3.left , .5f *Mathf.Sin(Time.time + Mathf.Sin(Time.time * 3)));
		transform.position += new Vector3( Mathf.Sin(Time.time*rand),Mathf.Sin(Time.time*rand*5),Mathf.Sin(Time.time*rand*10)) * .02f;
	}
}
