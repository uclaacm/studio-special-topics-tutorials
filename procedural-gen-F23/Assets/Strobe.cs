using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strobe : MonoBehaviour
{
    [SerializeReference] SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [SerializeField] float strobeRate;
    float strobe;
    Color color;
    void FixedUpdate()
    {
        strobe += strobeRate;

        color = spriteRenderer.color;
        color.a = Mathf.Sin(strobe) * .3f + .7f;

        spriteRenderer.color = color;
    }
}
