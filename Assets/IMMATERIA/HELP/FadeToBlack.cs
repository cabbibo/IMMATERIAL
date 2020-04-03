using UnityEngine;
 

[ExecuteInEditMode]
public class FadeToBlack : MonoBehaviour
{
    public AnimationCurve FadeCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.6f, 0.7f, -1.8f, -1.2f), new Keyframe(1, 0));
 
    private float _alpha = 1;
    private Texture2D _texture;
    private bool _done;
    private float _time;
    private Material _material;
 
    public void Reset()
    {
        _done = false;
        _alpha = 1;
        _time = 0;
    }
    public void OnEnable(){

        _material = GetComponent<Renderer>().sharedMaterial;

        _material.SetColor("_Color",new Color(0, 0, 0, 0));
        _done = true;
    }
    public void RedoFade()
    {
        Reset();
    }
 
    public void Update()
    {
        if (_done) return;


        
        
 
        _time += Time.deltaTime * .1f;
        _alpha = FadeCurve.Evaluate(Mathf.Clamp(_time-.5f , 0 , 1));

        //print(_alpha);

        _material.SetColor("_Color",new Color(0, 0, 0, 1-_alpha));
        //_material.SetColor("_Color",new Color(1, 0, 0, 1));
 
        if (_alpha <= 0) _done = true;
    }
}