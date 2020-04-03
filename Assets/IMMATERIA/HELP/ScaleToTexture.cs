using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ScaleToTexture : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
      Texture t = GetComponent<Renderer>().sharedMaterial.GetTexture("_MainTex");

      Vector3 ls = transform.localScale;
      
      ///print( t.width );
      ///print( t.height );

      float ratio = (float)t.width / (float)t.height;
      //print( ratio );

      transform.localScale = new Vector3(ls.z * ratio , ls.z  , ls.z );
    }

}
