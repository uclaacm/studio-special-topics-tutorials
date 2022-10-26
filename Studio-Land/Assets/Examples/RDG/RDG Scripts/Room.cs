using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //Room size
    public float width = 15.7f;
    public float height = 8.7f;

    //Room position in grid
    public int X;
    public int Y;

    //array of doors
    Door[] doors;

    void Start()
    {
        if(RoomLoader.instance == null)
        {
            Debug.Log("wrong scene, press play in the main scene");
            return; //no room controller instance created yet
        }

        /* ==========
        YOUR CODE HERE (Slide 17)
        ========== */

        /* ==========
        YOUR CODE HERE (Slide 20)
        ========== */
    }

    public void SetDoors()
    {
        /* ==========
        YOUR CODE HERE (Slide 21)
        ========== */
    }

    public void Open(Door d)
    {
        d.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        //alternativley: 
        //d.gameObject.SetActive(false);

        d.openTile();
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        /* ==========
        YOUR CODE HERE (Slide 25)
        ========== */
    }

}
