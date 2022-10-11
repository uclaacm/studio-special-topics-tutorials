using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{

    public bool activated = false;

    [SerializeField] GameObject glow;

    void Start(){
        glow.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.tag == "Player" && !activated){
            glow.SetActive(true);
            activated = true;
            GameManager.instance.ActivateRune();
        }
    }
}
