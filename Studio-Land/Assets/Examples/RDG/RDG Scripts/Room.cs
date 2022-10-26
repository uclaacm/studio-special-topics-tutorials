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

        /* =============

        Task:
        - Call the "constructor" we created in RoomLoader, PositionRoom, for this room

        EXAMPLE CODE:

        //positions the room correctly (acts like a constructor)
        RoomLoader.instance.PositionRoom(this);

        ============= */

        /* =============

        Task:
        - get all door components (children of room game object) and store in a list

        EXAMPLE CODE:

        doors = GetComponentsInChildren<Door>(); //list of all four doors in room

        ============= */
    }

    public void SetDoors()
    {

    /* =============

    Task:
    - Iterate through top, bot, left, right to check if there is an adjacent room (use IsCoordEmpty in RoomLoader)
    - "Open" the door by setting the renderer transparent and disable the collider (use Open())

    EXAMPLE CODE:

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

    ============= */
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
        /* =============

        Task:
        - Set currRoom to this room if player enters the collider trigger of the room (already made in prefab)

        EXAMPLE CODE:

        if (collider.tag == "Player")
        {
            CameraController.instance.currRoom = this;
        }

        ============= */
    }

}
