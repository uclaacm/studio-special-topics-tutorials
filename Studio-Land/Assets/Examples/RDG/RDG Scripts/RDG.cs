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
        /* ==========
        YOUR CODE HERE (Slide 10)
        ========== */

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
        /* ==========
        YOUR CODE HERE (Slide 11)
        ========== */

        return roomCoords;
    }

    //converts list into queue, finds the end room
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

