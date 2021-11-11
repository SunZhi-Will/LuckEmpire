using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBehaviour
{
    public Material _material;
    private Image _image;
    public float bl2 = 0f;
    public float size = 1f;
    public float _pcg = 0f;
    public float s = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
        _image = GetComponent<Image>();
        _image.material = new Material(_material);
        
    }

    // Update is called once per frame
    void Update()
    {
        _image.material.SetFloat("bl2", bl2);
        _image.material.SetFloat("size", size);
        _image.material.SetFloat("_pcg", _pcg);
        _image.material.SetFloat("s", s);
    }
}
