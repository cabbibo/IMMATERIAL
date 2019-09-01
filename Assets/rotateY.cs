using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class rotateY : MonoBehaviour
{
    public float speed;
    public float size;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate( Vector3.up , Mathf.Sin(Time.time*speed) * Time.deltaTime * size);
    }
}
