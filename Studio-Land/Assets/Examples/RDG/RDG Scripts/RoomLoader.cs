using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//for loading scenes
using UnityEngine.SceneManagement;

public class RoomLoader : MonoBehaviour
{
    //singleton manager
    public static RoomLoader instance { get; private set; }

    private void Awake()
    {
        //instatiating singleton instance
        if (instance == null)
        {
            Debug.Log("room loader instance created");
            instance = this; //sets it to this instance if script is running for the first time
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); //keeps only the first instance
        }
    }

    [SerializeField] private List<Room> loadedRooms = new List<Room>();

    private bool isLoadingRoom = false;
    private bool doorsSet = false;

    private Vector2Int currLoadRoomPos;
    private string currLoadRoomType;

    void Update()
    {

        /* =============

        SLIDE 16

        Task 1:
        - Load a room whenever RoomLoader is not currently loading a room (and if there are rooms left)
        
        SLIDE 22

        Task 2:
        - After loading ALL rooms, call SetDoors for each loaded room

        EXAMPLE CODE:

        if (!isLoadingRoom)
        {
            //TASK 1:
            if (GetComponent<RDG>().roomsQueue.Count != 0)
            {
                currLoadRoomPos = GetComponent<RDG>().roomsQueue.Dequeue();
                isLoadingRoom = true;
                StartCoroutine(LoadRoom(currLoadRoomPos));
            } 
            // TASK 2:
            else if (!doorsSet) {
                foreach (Room room in loadedRooms){
                    room.SetDoors();
                }
                doorsSet = true;
            }
        }
        ============= */

    }

    //async operation to load rooms additively (scenes)
    IEnumerator LoadRoom(Vector2Int pos)
    {   

        yield return null;

        /* =============

        SLIDE 15

        Task:
        - Delete the yield return line above
        - Load the rooms based on the coordinates (0,0) -> start, endRoomPos -> end, otherwise empty
        - Do this using an async operation that yield returns when complete

        EXAMPLE CODE:

        //determines type of room based on coordinate
        if (pos == Vector2Int.zero){
            currLoadRoomType = "Start";
        } else if (pos == GetComponent<RDG>().endRoomCoord){
            currLoadRoomType = "End";
        } else {
            currLoadRoomType = "Empty";
        }

        while (loadRoomOp.isDone == false)
        {
            yield return null; //continues loading until the async operation is finished
        }

        ==================== */ 

    }

    //returns true if coordinate is empty
    public bool IsCoordEmpty(Vector2Int coord)
    {
        return loadedRooms.Find(room => room.X == coord.x && room.Y == coord.y) == null;
    }

    //positions room correctly, adds room to list of loaded rooms
    //called within room class (attached to each room game object)
    //serves as a "constructor" for the rooms (since monobehavior scripts cannot have constructors)
    public void PositionRoom(Room room)
    {

        /* =============

        SLIDE 18

        Task:
        - Set transform position of each room to (grid) coordinate * size
        - Set the X and Y (grid) coordinates (see Room class) accordingly so we can find rooms easily later
        - Add room to list of LoadedRooms
        - Reset the flag isLoadingRoom to prepare to load the next room

        EXAMPLE CODE:

        if (IsCoordEmpty(currLoadRoomPos))
        {
            room.transform.position = new Vector3(currLoadRoomPos.x * room.width, currLoadRoomPos.y * room.height, 0);
            room.transform.parent = transform;
            room.name = currLoadRoomType +  " " + currLoadRoomPos.x + "," + currLoadRoomPos.y;
            
            room.X = currLoadRoomPos.x;
            room.Y = currLoadRoomPos.y;

            loadedRooms.Add(room);

            if (currLoadRoomType == "End") {
                gameObject.GetComponent<RuneSpawner>().SpawnRunes();
                Debug.Log("runes spawned");
            }
        } else
        {
            //room already exists
            Destroy(room.gameObject);
        }
    
        //ready to load the next room
        isLoadingRoom = false;

        ============= */

        //combines current scene into the main scene
        //comment the line below out if you want to look at the scenes being loaded
        SceneManager.MergeScenes(SceneManager.GetSceneByName(currLoadRoomType), SceneManager.GetSceneByName("RDG"));
    }

    public List<Room> getRooms(){
        return loadedRooms;
    }

}
