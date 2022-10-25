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
        if (!isLoadingRoom)
        {
            if (GetComponent<RDG>().roomsQueue.Count != 0)
            {
                currLoadRoomPos = GetComponent<RDG>().roomsQueue.Dequeue();
                isLoadingRoom = true;
                StartCoroutine(LoadRoom(currLoadRoomPos));
            } else if (!doorsSet) {
                foreach (Room room in loadedRooms){
                    room.SetDoors();
                }
                doorsSet = true;
            }
        }
    }

    //async operation to load rooms additively (scenes)
    IEnumerator LoadRoom(Vector2Int pos)
    {   
        //determines type of room based on coordinate
        if (pos == Vector2Int.zero){
            currLoadRoomType = "Start";
        } else if (pos == GetComponent<RDG>().endRoomCoord){
            currLoadRoomType = "End";
        } else {
            currLoadRoomType = "Empty";
        }

        AsyncOperation loadRoomOp = SceneManager.LoadSceneAsync(currLoadRoomType, LoadSceneMode.Additive);

        while (loadRoomOp.isDone == false)
        {
            yield return null; //continues loading until the async operation is finished
        }
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

        //combines current scene into the main scene
        //comment the line below out if you want to look at the scenes being loaded
        SceneManager.MergeScenes(SceneManager.GetSceneByName(currLoadRoomType), SceneManager.GetSceneByName("RDG"));
    }

    public List<Room> getRooms(){
        return loadedRooms;
    }

}
