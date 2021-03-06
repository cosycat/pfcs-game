using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpringScript : MonoBehaviour
{
    [FormerlySerializedAs("K")] public float k;
    private float _initialLength;
    public float weight;
    private float _speed;
    
    // Start is called before the first frame update
    void Start()
    {
        this._initialLength = this.transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        var transform1 = transform;
        var scale = transform1.localScale;
        var position = transform1.position;
        
        var federKraft = k * (_initialLength - scale.y) ;
        var gewichtsKraft = weight;
        
        var delta = federKraft + gewichtsKraft;
        _speed += delta * Time.fixedDeltaTime * 0.1f; 
        
        transform1.localScale = new Vector3(scale.x, scale.y + _speed, scale.z);
        transform1.position = new Vector3(position.x, position.y - _speed, position.z);
    }

}
