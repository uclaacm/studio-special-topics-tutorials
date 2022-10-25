using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneSpawner : MonoBehaviour
{

    private int numRunes;

    [SerializeField] List<GameObject> runeTypes = new List<GameObject>();
    [SerializeField] public List<Room> runeRooms = new List<Room>();
    //[SerializeField] List<Vector3> takenLocations = new List<Vector3>();
    
    void Start(){
        numRunes = GameManager.instance.runeNum;
    }

    //sets a random room for each rune to be spawned in
    public void SpawnRunes(){

        for (int i = 0; i < numRunes; i++){
            int roomNum = Random.Range(1, gameObject.GetComponent<RoomLoader>().getRooms().Count - 1);
            runeRooms.Add(gameObject.GetComponent<RoomLoader>().getRooms()[roomNum]);
        }

        foreach (Room room in runeRooms){
            Vector3 location = new Vector3(Random.Range(-7, 7), Random.Range(-3, 3), 5) + room.gameObject.transform.position;
            Instantiate(runeTypes[Random.Range(0, runeTypes.Count)], location, Quaternion.identity, room.gameObject.transform);
            //location constraints so no two are too close to eachother
        }
    }
}
