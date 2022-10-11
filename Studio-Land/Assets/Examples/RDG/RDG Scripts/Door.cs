using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    //door position
    public enum Type
    {
        top, bot, left, right
    }

    public Type type;

    [SerializeField] GameObject tilemap;

    public void openTile(){
        tilemap.SetActive(true);
    }

    void Awake(){
        tilemap.SetActive(false);
    }

}
