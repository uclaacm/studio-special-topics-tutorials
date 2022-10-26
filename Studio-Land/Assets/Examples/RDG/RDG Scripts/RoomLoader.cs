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
        /* ==========
        YOUR CODE HERE (Slide 15)
        ========== */

        /* ==========
        YOUR CODE HERE (Slide 22)
        ========== */
    }

    //async operation to load rooms additively (scenes)
    IEnumerator LoadRoom(Vector2Int pos)
    {   

        //delete this line
        yield return null;

        /* ==========
        YOUR CODE HERE (Slide 15)
        ========== */

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
        /* ==========
        YOUR CODE HERE (Slide 18)
        ========== */

        //combines current scene into the main scene
        //comment the line below out if you want to look at the scenes being loaded
        SceneManager.MergeScenes(SceneManager.GetSceneByName(currLoadRoomType), SceneManager.GetSceneByName("RDG"));
    }

    public List<Room> getRooms(){
        return loadedRooms;
    }

}
