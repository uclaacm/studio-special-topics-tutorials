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

        //positions the room correctly (acts like a constructor)
        RoomLoader.instance.PositionRoom(this);

        doors = GetComponentsInChildren<Door>(); //list of all four doors in room
    }

    public void SetDoors()
    {
        foreach(Door d in doors)
        {
            switch (d.type)
            {
                case Door.Type.top:
                    if (!RoomLoader.instance.IsCoordEmpty(new Vector2Int(X, Y + 1))) Open(d);
                    break;
                case Door.Type.bot:
                    if (!RoomLoader.instance.IsCoordEmpty(new Vector2Int(X, Y - 1))) Open(d);
                    break;
                case Door.Type.left:
                    if (!RoomLoader.instance.IsCoordEmpty(new Vector2Int(X - 1, Y))) Open(d);
                    break;
                case Door.Type.right:
                    if (!RoomLoader.instance.IsCoordEmpty(new Vector2Int(X + 1, Y))) Open(d);
                    break;
            }
        }
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
        if (collider.tag == "Player")
        {
            CameraController.instance.currRoom = this;
        }
    }

}
