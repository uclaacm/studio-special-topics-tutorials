using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//for list manipulation
using System.Linq;

//class used to generate a randomly vector2list of connected coordinates
public class Crawler
{
    private Vector2Int origPos; //original position
    private Vector2Int currPos; //current position
    private int numSteps; //number of positions travelled (random)
    private List<Vector2Int> path = new List<Vector2Int>(); //list of coordinates

    //constructor
    public Crawler(int minSteps, int maxSteps)
    {
        origPos = Vector2Int.zero;
        currPos = origPos;
        numSteps = Random.Range(minSteps, maxSteps + 1); //random number of steps within range
    }

    //generates a vector2 coordinate adjacent to currPos
    public Vector2Int Step()
    {

        /* ====================
        
        SLIDE 10

        Task: 
        - Generate a random direction (up, down, left or right) each time Step() is called
        - Return the coordinate (in grid) of a where step this direction will go

        EXAMPLE CODE:

        int axis = Random.Range(0, 2); //0 = move horizontally, 1 = move vertically
        int dir = Random.Range(0, 2); //0 = -1 direction, 1 = +1 direction
        
        int step = 0;
        if (dir == 0) step = -1;
        else if (dir == 1) step = 1;

        //"steps" in a random location
        if (axis == 0) currPos += new Vector2Int(step, 0);
        else if (axis == 1) currPos += new Vector2Int(0, step);

        ==================== */ 

        return currPos;
    }

    public List<Vector2Int> Path()
    {
        path.Clear();
        for (int i = 0; i < numSteps; i++)
        {
            path.Add(Step());
        }
        return path;
    }
}

public class RDG : MonoBehaviour
{
    [SerializeField] private int numCrawlers = 2; //not single to make branching maps
    [SerializeField] private int minSteps = 6;
    [SerializeField] private int maxSteps = 12;

    private List<Vector2Int> roomCoords = new List<Vector2Int>(); //list of room coordinates
    public Queue<Vector2Int> roomsQueue = new Queue<Vector2Int>(); //queue of rooms to load, FIFO

    public Vector2Int endRoomCoord;
    private bool endRoomCreated;

    //concatenates room coordinates, cleans list by removing duplicate rooms etc.
    public List<Vector2Int> GenCoordList()
    {

        /* ====================

        SLIDE 11
        
        Task: 
        - Create crawlers and generate, by repeatedly calling Step(), a list of coordinates for each
        - Clean up list (remove duplicates), manually add starting room
        - Return the cleaned list of room coordinates

        -> We now have the connected coordinates of rooms in our dungeon!
        -> This list will be shoved into a queue (FIFO) by the method below
        -> Both methods called once in Start()

        EXAMPLE CODE:

        roomCoords.Clear();
        for (int i = 0; i < numCrawlers; i++)
        {
            Crawler crawler = new Crawler(minSteps, maxSteps);
            roomCoords = roomCoords.Concat(crawler.Path()).ToList();
            //note: Concat() returns two IEnumerable<T>s without modifying the two original lists, hence ToList() is needed
        }

        //cleans up the list of duplicate values
        roomCoords = roomCoords.Distinct().ToList();
        //removes starting room (0,0), this is added manually
        roomCoords.RemoveAll(pos => pos == Vector2Int.zero);
        
        ==================== */ 

        return roomCoords;
    }

    //converts list into queue (FIFO), finds the end room
    public void addRoomsToQueue()
    {
        roomsQueue.Clear();

        roomsQueue.Enqueue(Vector2Int.zero); //makes sure start room is loaded first
        foreach (Vector2Int pos in roomCoords)
        {
            roomsQueue.Enqueue(pos);
            if (pos == roomCoords[roomCoords.Count - 1] && pos != Vector2Int.zero && !endRoomCreated)
            {
                endRoomCoord = pos;
                endRoomCreated = true;
            }
        }
    }

    void Start()
    {
        roomCoords = GenCoordList();
        addRoomsToQueue();
    }
}

