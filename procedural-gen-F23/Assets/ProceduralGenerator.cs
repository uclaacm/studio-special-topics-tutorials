using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour
{
    //array of all available 'chunks'
    [SerializeField] GameObject[] chunks;
    [SerializeField] Transform playerTransform;

    float highestChunk;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        while (playerTransform.position.y > highestChunk - 12)
        {
            highestChunk += 4;
            PlaceChunk(new Vector2(4, highestChunk));
            PlaceChunk(new Vector2(-4, highestChunk));
        }
    }

    //places a random chunk at 'position'
    void PlaceChunk(Vector2 position)
    {
        //pick a chunk
        GameObject selectedChunk = chunks[Random.Range(0, chunks.Length)];

        //place the chunk
        Instantiate(selectedChunk, position, Quaternion.identity);
    }
}
